using Acr.UserDialogs;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Extensions;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BLEACR.Client
{
    public class SelectedDevice
    {
        public ScanResults scanResult;
        private PromptResult message;
        private IAdapter adapter;
        public IDevice bledevice;
        private IService service;
        private ICharacteristic Tx;
        private CancellationTokenSource _cancellationTokenSource;

        public SelectedDevice(ScanResults scanResult)
        {
            this.scanResult = scanResult;

            adapter = Plugin.BLE.CrossBluetoothLE.Current.Adapter;
            adapter.ScanTimeoutElapsed += Adapter_ScanTimeoutElapsed;
            StartSearchingDevices();
        }

        private async void StartSearchingDevices()
        {
            await ReadUserInput();

            adapter.DeviceDiscovered += (s, a) => {
                if(a.Device.Name == scanResult.Name)
                {
                    bledevice = a.Device;
                    Connecttodiv();
                }
            };

            _cancellationTokenSource = new CancellationTokenSource();
            adapter.ScanMode = ScanMode.LowLatency;
            await adapter.StartScanningForDevicesAsync(_cancellationTokenSource.Token);
        }

        private void Adapter_ScanTimeoutElapsed(object sender, EventArgs e)
        {
            CleanupCancellationToken();
        }

        private void CleanupCancellationToken()
        {
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }

        private async void Connecttodiv()
        {
            try
            {
                if(bledevice.State == Plugin.BLE.Abstractions.DeviceState.Disconnected)
                {
                    await adapter.ConnectToDeviceAsync(bledevice);
                }

                if (bledevice != null)
                {
                    GetService();
                }
            }
            catch (Exception e)
            {
                Exception ex = e;
            }
        }

        private async void GetService ()
        {
            try
            {
                service = await bledevice.GetServiceAsync(Guid.Parse("569a1101-b87f-490c-92cb-11ba5ea5167c"));
            }
            catch(Exception e)
            {
                Exception ex = e;
            }

            if(service != null)
            {
                await GetNotifications();
                await GetCharacteristic();
            }
        }

        private async Task GetNotifications()
        {
            ICharacteristic characteristic = await service.GetCharacteristicAsync(Guid.Parse("569a2000-b87f-490c-92cb-11ba5ea5167c"));
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

        private async Task GetCharacteristic()
        {
            ICharacteristic characteristic = null;

            try
            {
                characteristic = await service.GetCharacteristicAsync(Guid.Parse("569a2001-b87f-490c-92cb-11ba5ea5167c"));
            }
            catch (Exception e)
            {
                UserDialogs.Instance.Toast(e.Message);
            }

            if (characteristic != null)
            {
                Tx = characteristic;
                WriteCharacteristic();
            }
        }

        private void WriteCharacteristic()
        {
            try
            {
                byte[] data = Encoding.Default.GetBytes(message.Text);

                while (true)
                {
                    if (message.Ok)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            try
                            {
                                await Tx.WriteAsync(data);
                            }
                            catch (Exception e)
                            {
                                Exception ex = e;
                            }

                            //try
                            //{
                            //    await adapter.DisconnectDeviceAsync(bledevice);
                            //}
                            //catch (Exception e)
                            //{
                            //    Exception ex = e;
                            //}

                        });

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                UserDialogs.Instance.Toast(e.Message);
            }
        }

        public async void SendDataToConnectedDevice()
        {
            await ReadUserInput();
            WriteCharacteristic();
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
    }
}
