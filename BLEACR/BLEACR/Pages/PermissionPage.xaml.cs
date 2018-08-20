using BLEACR.PopUpInterface;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BLEACR.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PermissionPage : ContentPage
	{
		public PermissionPage ()
		{
			InitializeComponent ();
            Content = PermissionPageContent;
            DependencyService.Get<IPopUp>().ShowSnackbar("Application can not access coarse location.");
            RequestPermission();
        }

        public void RequestPermission() {

            MainPage mainPage = new MainPage();
        }
	}
}