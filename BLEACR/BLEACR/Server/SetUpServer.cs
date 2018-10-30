using BLEACR.Pages;
using Plugin.BluetoothLE;
using Plugin.BluetoothLE.Server;
using System;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLEACR.Server
{
    public class SetUpServer
    {
        private enum CharacteristicsType
        {
            RX_Notify = 1,
            TX_Write = 2
        };

        private IAdapter adapter;
        private IGattServer server;

        public SetUpServer(ClientPage clientPage)
        {           
            BuildServer(clientPage).Wait();
        }

        private async Task BuildServer(ClientPage clientPage)
        {
            try
            {
                adapter = CrossBleAdapter.Current;
                server = await adapter.CreateGattServer();

                Plugin.BluetoothLE.Server.IGattService service = server.CreateService(Guid.Parse("569A1101-B87F-490C-92CB-11BA5EA5167C"), true);
                BuildCharacteristics(service, Guid.Parse("569A2003-B87F-490C-92CB-11BA5EA5167C"), clientPage, CharacteristicsType.TX_Write);
                BuildCharacteristics(service, Guid.Parse("569A2002-B87F-490C-92CB-11BA5EA5167C"), clientPage, CharacteristicsType.RX_Notify);
                BuildCharacteristics(service, Guid.Parse("569A2001-B87F-490C-92CB-11BA5EA5167C"), clientPage, CharacteristicsType.TX_Write);
                BuildCharacteristics(service, Guid.Parse("569A2000-B87F-490C-92CB-11BA5EA5167C"), clientPage, CharacteristicsType.RX_Notify);
                server.AddService(service);

                //Plugin.BluetoothLE.Server.IGattCharacteristic characteristic = service.AddCharacteristic
                //(
                //    Guid.NewGuid(),
                //    CharacteristicProperties.Read | CharacteristicProperties.Write | CharacteristicProperties.WriteNoResponse,
                //    GattPermissions.Read | GattPermissions.Write
                //);

                //Plugin.BluetoothLE.Server.IGattCharacteristic notifyCharacteristic = service.AddCharacteristic
                //(
                //    Guid.NewGuid(),
                //    CharacteristicProperties.Indicate | CharacteristicProperties.Notify,
                //    GattPermissions.Read | GattPermissions.Write
                //);

                adapter.Advertiser.Start(new AdvertisementData
                {
                    LocalName = "My GATT"
                });
            }
            catch (Exception ex){

                Exception exception = ex;
            }
        }

        private void BuildCharacteristics(Plugin.BluetoothLE.Server.IGattService service, Guid characteristicId, ClientPage clientPage, CharacteristicsType type)
        {

            //Plugin.BluetoothLE.Server.IGattCharacteristic characteristic = service.AddCharacteristic(characteristicId, 
            //    CharacteristicProperties.Notify | CharacteristicProperties.Read |
            //    CharacteristicProperties.Write | CharacteristicProperties.WriteNoResponse, 
            //    GattPermissions.Read | GattPermissions.Write );

            //characteristic.WhenReadReceived().Subscribe(x =>
            //{
            //    string write = "@@@@";
            //    if (string.IsNullOrWhiteSpace(write))
            //    {
            //        write = "0000";
            //    }

            //    x.Value = Encoding.UTF8.GetBytes(write);
            //});

            //characteristic.WhenWriteReceived().Subscribe(x =>
            //{
            //    string write = Encoding.UTF8.GetString(x.Value, 0, x.Value.Length);
            //    clientPage.ReceivedText(write);
            //});

            if(type == CharacteristicsType.RX_Notify)
            {
                Plugin.BluetoothLE.Server.IGattCharacteristic RX_NotifyCharacteristic = 
                service.AddCharacteristic(characteristicId, CharacteristicProperties.Notify, GattPermissions.Read);

                RX_NotifyCharacteristic.WhenDeviceSubscriptionChanged().Subscribe(e => {

                    RX_NotifyCharacteristic.Broadcast(Encoding.UTF8.GetBytes("Device successfully subscribed"));

                });


                //IDisposable notifyBroadcast = null;
                //notifyCharacteristic.WhenDeviceSubscriptionChanged().Subscribe(e =>
                //{
                //    var @event = e.IsSubscribed ? "Subscribed" : "Unsubcribed";

                //    if (notifyBroadcast == null)
                //    {
                //        this.notifyBroadcast = Observable
                //            .Interval(TimeSpan.FromSeconds(1))
                //            .Where(x => notifyCharacteristic.SubscribedDevices.Count > 0)
                //            .Subscribe(_ =>
                //            {
                //                Debug.WriteLine("Sending Broadcast");
                //                var dt = DateTime.Now.ToString("g");
                //                var bytes = Encoding.UTF8.GetBytes(dt);
                //                notifyCharacteristic.Broadcast(bytes);
                //            });
                //    }
                //});
            }

            if (type == CharacteristicsType.TX_Write)
            {
                Plugin.BluetoothLE.Server.IGattCharacteristic TX_WriteCharacteristic =
                service.AddCharacteristic(characteristicId, CharacteristicProperties.Write, GattPermissions.Write);

                TX_WriteCharacteristic.WhenWriteReceived().Subscribe(x =>
                {
                    string write = Encoding.UTF8.GetString(x.Value, 0, x.Value.Length);
                    clientPage.ReceivedText(write);
                });
            }
        }

    }
}
