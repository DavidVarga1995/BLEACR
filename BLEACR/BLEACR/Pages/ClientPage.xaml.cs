using BLEACR.Client;
using BLEACR.Server;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BLEACR.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ClientPage : ContentPage
	{
        public ClientPage ()
		{
			InitializeComponent ();
            Content = ClientContent;
        }

        private void OnClientButtonClicked(object sender, EventArgs args)
        {
            SetUpClient serverActivity = new SetUpClient(this);
        }

        private void OnServerButtonClicked(object sender, EventArgs args)
        {
            SetUpServer serverActivity = new SetUpServer(this);
        }

        public void ReceivedText(string write)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                WriteReceived.Text = write;
            });
        }

        private void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }

            ScanResults selectedScanResult = (ScanResults)e.SelectedItem;

            //DisplayAlert("Item Selected", selectedScanResult.GuiName, "Ok");
            ((ListView)sender).SelectedItem = null;
            selectedScanResult.IsRunning = true;
            selectedScanResult.IsVisible = true;
            SelectedDevice selectedDevice = new SelectedDevice(selectedScanResult);
        }

        public void DisplayScanIsRunningError()
        {
            DisplayAlert("Error", "Scan was running - scan has been stopped.", "Ok");
        }
    }
}