using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BLEACR.Client
{
    public class ScanResults
    {

        public IDevice Device;
        public string Name;
        public bool IsConnected;
        public Guid Uuid;
        public int Rssi;
        public bool IsConnectable;
        public int ServiceCount;
        public string ManufacturerData;
        public string LocalName;
        public int TxPower;

        public ScanResults(IScanResult result)
        {


            if (Uuid == Guid.Empty)
            {
                Device = result.Device;
                Uuid = Device.Uuid;
            }

            try
            {
                if (Uuid == result.Device.Uuid)
                {

                    Name = result.Device.Name;
                    Rssi = result.Rssi;

                    var ad = result.AdvertisementData;
                    ServiceCount = ad.ServiceUuids?.Length ?? 0;
                    IsConnectable = ad.IsConnectable;
                    LocalName = ad.LocalName;
                    TxPower = ad.TxPower;
                    ManufacturerData = ad.ManufacturerData == null
                        ? null
                        : BitConverter.ToString(ad.ManufacturerData);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

    }
}
