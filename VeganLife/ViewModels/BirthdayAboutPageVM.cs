namespace VeganLife.ViewModels
{
    public partial class BirthdayAboutPageVM : BaseViewModel
    {
        public DateTime MaximumDateOfBirth => DateTime.Now.AddDays(-30);
    }
}