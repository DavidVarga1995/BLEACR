namespace BLEACR.PopUpInterface
{
    public interface IPopUp
    {
        void ShowToast(string message);
        void ShowSnackbar(string message, int duration);
    }
}
