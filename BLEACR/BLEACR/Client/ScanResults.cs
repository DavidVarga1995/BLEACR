using Plugin.BluetoothLE;
using System;
using System.Diagnostics;

namespace BLEACR.Client
{
    public class ScanResults
    {

        public IDevice Device { get; set; }
        public string Name { get; set; }
        public Guid Uuid { get; set; }
        public int Rssi { get; set; }
        public bool IsConnectable { get; set; }
        public int ServiceCount { get; set; }
        public string ManufacturerData { get; set; }
        public string LocalName { get; set; }
        public int TxPower { get; set; }

        public ScanResults(IScanResult result)
        {

            Device = result.Device;
            Uuid = Device.Uuid;

            try
            {
                Name = Device.Name;
                Rssi = result.Rssi;

                IAdvertisementData ad = result.AdvertisementData;
                ServiceCount = ad.ServiceUuids?.Length ?? 0;
                IsConnectable = ad.IsConnectable;
                LocalName = ad.LocalName;
                TxPower = ad.TxPower;
                ManufacturerData = ad.ManufacturerData == null 
                    ? null
                    : BitConverter.ToString(ad.ManufacturerData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

    }
}
