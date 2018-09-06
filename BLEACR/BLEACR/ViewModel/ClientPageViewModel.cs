using BLEACR.Client;
using System.Collections.ObjectModel;

namespace BLEACR.ClientPageViewModel
{
    public static class ClientPageViewModel
    {
        public static ObservableCollection<ScanResults> DeviceData { get; set; }

        static ClientPageViewModel()
        {
            DeviceData = SetUpClient.GetData();
        }
    }
}
