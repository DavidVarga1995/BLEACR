using System;
using Xamarin.Forms;
using BLEACR.Server;

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
            var serverActivity = new SetUpServer();
            serverActivity.SetUpServerActivity(this);
        }

        void OnServerButtonClicked(object sender, EventArgs args)
        {
            var serverActivity = new SetUpServer();
            serverActivity.SetUpServerActivity(this);
        }

        public void ReceivedText(string write) { 

        Device.BeginInvokeOnMainThread(() =>
                {
                    WriteReceived.Text = write;
                });
        }


    }
}
