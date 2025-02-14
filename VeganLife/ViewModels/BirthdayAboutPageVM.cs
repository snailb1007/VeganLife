using PropertyChanged;
using VeganLife.Views.AboutYou;

namespace VeganLife.ViewModels
{
    public partial class BirthdayAboutPageVM : BaseViewModel
    {
        [ObservableProperty]
        private string _name;

        public DateTime MaximumDateOfBirth => DateTime.Now.AddDays(-30).Date;

        [ObservableProperty]
        private DateTime _selectedDate;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ContinueClickedCommand))]
        private bool _isFilledBirthday;

        public override Task ViewAppearingVM()
        {
            SelectedDate = MaximumDateOfBirth;
            return base.ViewAppearingVM();
        }

        public override Task OnNavigatingTo(object parameter)
        {
            if (parameter is string name)
            {
                Name = name;
            }

            return base.OnNavigatingTo(parameter);
        }

        [RelayCommand(CanExecute = nameof(IsFilledBirthday))]
        private async Task OnContinueClicked()
        {
            await this.navigationService.NavigateToPage<GenderAboutPage>(
                paramater: new InitAboutYouDataRecord(Name: Name, Birthday: SelectedDate, false));
        }

        [SuppressPropertyChangedWarnings]
        partial void OnSelectedDateChanged(DateTime value)
        {
            IsFilledBirthday = value < MaximumDateOfBirth && value > DateTime.MinValue;
        }
    }

    internal record InitAboutYouDataRecord(string Name, DateTime Birthday, bool IsMale);
}