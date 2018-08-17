using BLEACR.Pages;
using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BLEACR.Client
{
    class SetUpClient
    {
        IAdapter adapter;
        IDisposable scan;

        List<ScanResults> devices = new List<ScanResults>();

        public SetUpClient(ClientPage clientPage)
        {

            if (MainPage.SDKNumber) { }

            ScanConfig sc = new ScanConfig
            {
                ScanType = BleScanType.LowLatency
            };

            scan?.Dispose();
            adapter = CrossBleAdapter.Current;

            if(adapter.Status == AdapterStatus.PoweredOn)
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

        void OnScanResult(IScanResult result, ClientPage clientPage)
        {
            ScanResults resultData = new ScanResults(result);
            devices.Add(resultData);
            clientPage.DeviceFound(resultData.Name);
        }

    }
}

