using Acr.UserDialogs;
using BLEACR.Pages;
using Plugin.BluetoothLE;
using System;
using System.Text;

namespace BLEACR.Client
{
    public class SelectedDevice
    {
        private ScanResults scanResult;
        private string notificationRecieved = "NONE";
        private static IGattCharacteristic gattCharacteristic;

        public SelectedDevice(ScanResults scanResult)
        {
            this.scanResult = scanResult;
            EnableBLENotifications();
        }

        public void EnableBLENotifications()
        {
            Guid characteristicGuid = new Guid("569a2000-b87f-490c-92cb-11ba5ea5167c");
            Guid testGuid = new Guid("569a2001-b87f-490c-92cb-11ba5ea5167c");

            scanResult.Device.Connect();

            scanResult.Device.WhenStatusChanged().Subscribe(status =>
            {
                if (status == ConnectionStatus.Connected)
                {
                    scanResult.IsRunning = false;
                    scanResult.IsVisible = false;
                    UserDialogs.Instance.Toast("Connection was established.");
                    scanResult.Device.WhenAnyCharacteristicDiscovered().Subscribe(characteristic =>
                    {
                        if (characteristic.Uuid == characteristicGuid)
                        {
                            characteristic.EnableNotifications(true);
                            characteristic.RegisterAndNotify().Subscribe(result =>
                            {
                                notificationRecieved = Encoding.UTF8.GetString(result.Data, 0, result.Data.Length);
                                UserDialogs.Instance.Toast(notificationRecieved);
                            });

                            string testCode = "1111";

                            gattCharacteristic.Write(Encoding.UTF8.GetBytes(testCode)).Subscribe(
                            x => UserDialogs.Instance.Toast("Write Complete"),
                            ex => UserDialogs.Instance.Alert(ex.ToString()));;
                        }

                        if (characteristic.Uuid == testGuid)
                        {
                            gattCharacteristic = characteristic;
                        }
                    });
                }
            });
        }
    }
}
