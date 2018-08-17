using Android.App;
using Android.Content.PM;
using Android.OS;
using BLEACR.Pages;

namespace BLEACR.Droid
{
    [Activity(Label = "BLEACR", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
  
            LoadApplication(new App());
        }

        static int ReturnSDKNum()
        {
            if ((int)Build.VERSION.SdkInt < 23) {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}