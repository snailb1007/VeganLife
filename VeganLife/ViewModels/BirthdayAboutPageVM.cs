using VeganLife.Views.AboutYou;

namespace VeganLife.ViewModels
{
    public partial class BirthdayAboutPageVM : BaseViewModel
    {
        private string _name;

        public DateTime MaximumDateOfBirth => DateTime.Now.AddDays(-30).Date;

        [ObservableProperty]
        private DateTime selectedDate;

        public bool IsFilledBirthday => SelectedDate < MaximumDateOfBirth && SelectedDate > DateTime.MinValue;

        public override Task ViewAppearingVM()
        {
            SelectedDate = MaximumDateOfBirth;
            return base.ViewAppearingVM();
        }

        public override Task OnNavigatingTo(object? parameter)
        {
            if (parameter is string name)
            {
                _name = name;
            }

            return base.OnNavigatingTo(parameter);
        }

        [RelayCommand]
        public async Task OnContinueClicked()
        {
            await this.navigationService.NavigateToPage<GenderAboutPage>(
                new InitAboutYouDataRecord(Name: _name, Birthday: SelectedDate, false));
        }
    }

    internal record InitAboutYouDataRecord(string Name, DateTime Birthday, bool IsMale);
}