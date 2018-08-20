using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace BLEACR.Droid
{
    [Activity(Label = "BLEACR", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        const int permissionNeeded = 1;
        const string permission = Manifest.Permission.AccessCoarseLocation;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            if ((int)Build.VERSION.SdkInt > 22)
            {
                if (CheckSelfPermission(permission) == (int)Permission.Granted)
                {
                    LoadApplication(new App());
                }
                else
                {
                    LoadApplication(new App(permissionNeeded));
                }
            }
            else
            {
                LoadApplication(new App());
            }
        }
    }
}