using System;
using Xamarin.Forms;
using BLEACR.Server;
using BLEACR.Client;

namespace BLEACR
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        void OnClientButtonClicked(object sender, EventArgs args)
        {
            SetUpClient serverActivity = new SetUpClient(this);
        }

        void OnServerButtonClicked(object sender, EventArgs args)
        {
            SetUpServer serverActivity = new SetUpServer(this);
        }

        public void ReceivedText(string write) { 

        Device.BeginInvokeOnMainThread(() =>
                {
                    WriteReceived.Text = write;
                });
        }

        public void DeviceFound(string name)
        {

            Test.Text = name;

        }

    }
}
