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

        void OnClientButtonClicked(object sender, EventArgs args)
        {
            SetUpClient serverActivity = new SetUpClient(this);
        }

        void OnServerButtonClicked(object sender, EventArgs args)
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

        //public void DeviceFound(string name)
        //{
        //    Test.Text = name;
        //}
    }
}