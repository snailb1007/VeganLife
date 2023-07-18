namespace VeganLife.ViewModels
{
    public partial class TutorialViewModel : BaseViewModel
    {
        [RelayCommand]
        void Skip()
        {
            Application.Current.MainPage = new AppShell();
        }
    }
}
