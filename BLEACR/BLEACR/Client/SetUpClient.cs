using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace BLEACR.Client
{
    class SetUpClient
    {
        IDisposable scan;

        List<ScanResults> devices = new List<ScanResults>();

        public SetUpClient(MainPage mainPage)
        {

           scan = CrossBleAdapter.Current
                            .Scan()
                            .Subscribe(results =>
                            {
                                OnScanResult(results, mainPage);
                            });
        }

        void OnScanResult(IScanResult result, MainPage mainPage)
        {
            ScanResults resultData = new ScanResults(result);
            devices.Add(resultData);
            mainPage.DeviceFound(resultData.Name);
        }

    }
}

