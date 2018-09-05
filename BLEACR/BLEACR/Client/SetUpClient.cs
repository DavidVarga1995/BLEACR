using BLEACR.Pages;
using BLEACR.PopUpInterface;
using Plugin.BluetoothLE;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Xamarin.Forms;

namespace BLEACR.Client
{
    class SetUpClient
    {
        private static ObservableCollection<ScanResults> devices = new ObservableCollection<ScanResults>();

        public static ObservableCollection<ScanResults> GetData()
        {
            return devices;
        }

        IAdapter adapter;
        IDisposable scan;
        public int status = 0;

        public SetUpClient(ClientPage clientPage)
        {
            RequestPermission(this);

            if (status == (int)PermissionStatus.Granted)
            {
                ScanConfig sc = new ScanConfig
                {
                    ScanType = BleScanType.LowLatency
                };

                scan?.Dispose();
                adapter = CrossBleAdapter.Current;

                if (adapter.Status == AdapterStatus.PoweredOn)
                {
                    Device.BeginInvokeOnMainThread(() =>
                {
                    scan = adapter
                       .Scan(sc)
                       .Subscribe(scanResults =>
                       {
                           OnScanResult(scanResults, clientPage);
                       });
                });
                }
            }
        }

        void OnScanResult(IScanResult result, ClientPage clientPage)
        {
            ScanResults resultData = new ScanResults(result);
            bool deviceIsOnTheList = false;

            foreach (ScanResults scanResult in devices)
            {
                if (resultData.Uuid == scanResult.Uuid)
                {
                    deviceIsOnTheList = true;
                }              
            }

            if (deviceIsOnTheList == false)
            {
                devices.Add(resultData);
            }

            //foreach (ScanResults sc in devices)
            //{
            //    System.Diagnostics.Debug.WriteLine("{0} {1} {2}", sc.Name ?? "", sc.Uuid.ToString() ?? "", sc.Rssi.ToString() ?? "");
            //}
            //System.Diagnostics.Debug.WriteLine("___________________");
        }

        async void RequestPermission(SetUpClient setUpClient)
        {
            setUpClient.status = (int)await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (setUpClient.status != (int)PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                {
                    DependencyService.Get<IPopUp>().ShowSnackbar("Application requires coarse location permission to perform a bluetooth scan.", 5000);
                }
            }
            setUpClient.status = (int)await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
        }
    }
}