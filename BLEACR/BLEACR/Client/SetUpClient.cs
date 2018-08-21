using BLEACR.Pages;
using BLEACR.PopUpInterface;
using Plugin.BluetoothLE;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Xamarin.Forms;

namespace BLEACR.Client
{
    class SetUpClient
    {
        IAdapter adapter;
        IDisposable scan;

        List<ScanResults> devices = new List<ScanResults>();

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
            devices.Add(resultData);
            clientPage.DeviceFound(resultData.Name);
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

