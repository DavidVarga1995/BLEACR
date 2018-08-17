using BLEACR.Pages;
using Plugin.BluetoothLE;
using Plugin.BluetoothLE.Server;
using System;
using System.Text;

namespace BLEACR.Server
{
    class SetUpServer
    {

        IAdapter adapter;
        IGattServer server;

        public SetUpServer(ClientPage clientPage)
        {

            adapter = CrossBleAdapter.Current;
            server = adapter.CreateGattServer();

            var service = server.CreateService(Guid.Parse("A495FF20-C5B1-4B44-B512-1370F02D74DE"), true);
            BuildCharacteristics(service, Guid.Parse("A495FF21-C5B1-4B44-B512-1370F02D74D1"), clientPage);
            BuildCharacteristics(service, Guid.Parse("A495FF22-C5B1-4B44-B512-1370F02D74D2"), clientPage );
            BuildCharacteristics(service, Guid.Parse("A495FF23-C5B1-4B44-B512-1370F02D74D3"), clientPage);
            BuildCharacteristics(service, Guid.Parse("A495FF24-C5B1-4B44-B512-1370F02D74D4"), clientPage);
            BuildCharacteristics(service, Guid.Parse("A495FF25-C5B1-4B44-B512-1370F02D74D5"), clientPage);
            server.AddService(service);

            var characteristic = service.AddCharacteristic
            (
                Guid.NewGuid(),
                CharacteristicProperties.Read | CharacteristicProperties.Write | CharacteristicProperties.WriteNoResponse,
                GattPermissions.Read | GattPermissions.Write
            );

            var notifyCharacteristic = service.AddCharacteristic
            (
                Guid.NewGuid(),
                CharacteristicProperties.Indicate | CharacteristicProperties.Notify,
                GattPermissions.Read | GattPermissions.Write
            );

            this.adapter.Advertiser.Start(new AdvertisementData
            {
                LocalName = "My GATT"
            });

        }

        void BuildCharacteristics(Plugin.BluetoothLE.Server.IGattService service, Guid characteristicId, ClientPage clientPage)
        {
            var characteristic = service.AddCharacteristic(
                characteristicId,
                CharacteristicProperties.Notify | CharacteristicProperties.Read | CharacteristicProperties.Write | CharacteristicProperties.WriteNoResponse,
                GattPermissions.Read | GattPermissions.Write
            );

            characteristic.WhenReadReceived().Subscribe(x =>
            {
                var write = "@@@@";
                if (string.IsNullOrWhiteSpace(write))
                {
                    write = "0000";
                }

                x.Value = Encoding.UTF8.GetBytes(write);
            });

            characteristic.WhenWriteReceived().Subscribe(x =>
            {
                var write = Encoding.UTF8.GetString(x.Value, 0, x.Value.Length);
                clientPage.ReceivedText(write);

            });

        }

    }
}
