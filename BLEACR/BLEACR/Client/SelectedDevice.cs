using Acr.UserDialogs;
using BLEACR.Pages;
using Plugin.BluetoothLE;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;

namespace BLEACR.Client
{
    public class SelectedDevice
    {
        private ScanResults scanResult;
        private PromptResult message;
        private bool deviceIsConnected = false;
        //private string notificationRecieved = "NONE";
        //private static IGattCharacteristic gattCharacteristic;
        //private string test = null;

        public SelectedDevice(ScanResults scanResult)
        {
            if (true)
            {
                this.scanResult = scanResult;

                Plugin.BLE.Abstractions.Contracts.IAdapter adapter = Plugin.BLE.CrossBluetoothLE.Current.Adapter;
                StartSearchingDevices(adapter);
            }
            else
            {

            }
        }

        private async void StartSearchingDevices(Plugin.BLE.Abstractions.Contracts.IAdapter adapter)
        {
            await ReadUserInput();

            adapter.DeviceDiscovered += (s, a) => {
                if(a.Device.Name == scanResult.Name)
                {
                    Connecttodiv(adapter, a.Device);
                }
            };
            await adapter.StartScanningForDevicesAsync();
        }

        private async void Connecttodiv(Plugin.BLE.Abstractions.Contracts.IAdapter adapter,
            Plugin.BLE.Abstractions.Contracts.IDevice bledevice)
        {
            try
            {
                await adapter.ConnectToDeviceAsync(bledevice);
                if(bledevice != null)
                {
                    GetService(bledevice, adapter);
                }
            }
            catch (Exception e)
            {
                Exception ex = e;
            }
        }

        private async void GetService (Plugin.BLE.Abstractions.Contracts.IDevice bledevice,
            Plugin.BLE.Abstractions.Contracts.IAdapter adapter)
        {
            Plugin.BLE.Abstractions.Contracts.IService service = await bledevice.GetServiceAsync(Guid.Parse("569a1101-b87f-490c-92cb-11ba5ea5167c"));
            if(service != null)
            {
                GetCharacteristic(service, bledevice, adapter);
                GetNotifications(service);
            }
        }

        private async void GetCharacteristic(Plugin.BLE.Abstractions.Contracts.IService service,
            Plugin.BLE.Abstractions.Contracts.IDevice bledevice,
            Plugin.BLE.Abstractions.Contracts.IAdapter adapter)
        {
            Plugin.BLE.Abstractions.Contracts.ICharacteristic characteristic = null;

            try
            {
                characteristic = await service.GetCharacteristicAsync(Guid.Parse("569a2001-b87f-490c-92cb-11ba5ea5167c"));
            }
            catch(Exception e) {
                UserDialogs.Instance.Toast(e.Message);
            }

            if (characteristic != null)
            {
                WriteCharacteristic(characteristic, bledevice, adapter);
            }
        }

        private async void GetNotifications(Plugin.BLE.Abstractions.Contracts.IService service)
        {
            Plugin.BLE.Abstractions.Contracts.ICharacteristic characteristic = await service.GetCharacteristicAsync(Guid.Parse("569a2000-b87f-490c-92cb-11ba5ea5167c"));
            if (characteristic != null)
            {
                characteristic.ValueUpdated += (o, args) =>
                {
                    byte[] bytes = args.Characteristic.Value;
                    UserDialogs.Instance.Toast(Encoding.UTF8.GetString(bytes, 0, bytes.Length));
                };

                await characteristic.StartUpdatesAsync();
            }
        }

        private async void WriteCharacteristic(Plugin.BLE.Abstractions.Contracts.ICharacteristic characteristic,
            Plugin.BLE.Abstractions.Contracts.IDevice bledevice,
            Plugin.BLE.Abstractions.Contracts.IAdapter adapter)
        {
            try
            {
                byte[] data = Encoding.Default.GetBytes(message.Text);

                while (true)
                {
                    if (message.Ok)
                    {
                        await characteristic.WriteAsync(data);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                UserDialogs.Instance.Toast(e.Message);
            }

            try
            {
                await adapter.DisconnectDeviceAsync(bledevice);
            }
            catch (Exception e)
            {
                Exception ex = e;
            }

        }

        private async Task ReadUserInput()
        {
            PromptConfig userInput = new PromptConfig
            {
                Message = "Please enter a write value"
            };

            message = await UserDialogs.Instance.PromptAsync(userInput);
        }

        private static byte[] GetBytes(string text)
        {
            return text.Split(' ').Where(token => !string.IsNullOrEmpty(token)).Select(token => Convert.ToByte(token, 16)).ToArray();
        }

        //public void EnableBLENotifications()
        //{
        //    Guid characteristicGuid = new Guid("569a2000-b87f-490c-92cb-11ba5ea5167c");
        //    Guid testGuid = new Guid("569a2001-b87f-490c-92cb-11ba5ea5167c");

        //    scanResult.Device.Connect();

        //    scanResult.Device.WhenConnectionFailed().Subscribe(x => {
        //        UserDialogs.Instance.Toast("Connection faild.");
        //        scanResult.Device.Connect();
        //    });

        //    scanResult.Device.WhenStatusChanged().Subscribe(status =>
        //    {
        //        if (status == ConnectionStatus.Connected)
        //        {
        //            scanResult.IsRunning = false;
        //            scanResult.IsVisible = false;
        //            UserDialogs.Instance.Toast("Connection was established.");
        //            scanResult.Device.WhenAnyCharacteristicDiscovered().Subscribe(characteristic =>
        //            {
        //                if (characteristic.Uuid == characteristicGuid)
        //                {
        //                    characteristic.EnableNotifications(true);
        //                    characteristic.RegisterAndNotify().Subscribe(result =>
        //                    {
        //                        notificationRecieved = Encoding.UTF8.GetString(result.Data, 0, result.Data.Length);
        //                        UserDialogs.Instance.Toast(notificationRecieved);
        //                    });

        //                    string testCode = "1111";

        //                    gattCharacteristic.Write(Encoding.UTF8.GetBytes(testCode)).Subscribe(
        //                    x => UserDialogs.Instance.Toast("Write Complete"),
        //                    ex => UserDialogs.Instance.Alert(ex.ToString()));;
        //                }

        //                if (characteristic.Uuid == testGuid)
        //                {
        //                    gattCharacteristic = characteristic;
        //                }
        //            });
        //        }
        //    });
        //}
    }
}
