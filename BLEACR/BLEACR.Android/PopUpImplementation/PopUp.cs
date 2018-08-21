using Android.App;
using Android.Support.Design.Widget;
using Android.Widget;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using BLEACR.PopUpInterface;

[assembly: Dependency(typeof(BLEACR.Droid.PopUpImplementation.PopUp))]
namespace BLEACR.Droid.PopUpImplementation
{
    class PopUp : IPopUp
    {
        public void ShowSnackbar(string message, int duration)
        {
            Activity activity = CrossCurrentActivity.Current.Activity;
            Android.Views.View activityRootView = activity.FindViewById(Android.Resource.Id.Content);
            Snackbar.Make(activityRootView, message, duration)
                .SetAction("OK", e => {}).Show();
        }

        public void ShowToast(string message)
        {
            Activity activity = CrossCurrentActivity.Current.Activity;
            Toast.MakeText(activity, message, ToastLength.Long).Show();
        }
    }
}