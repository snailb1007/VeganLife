// <copyright file="NameAboutUPageVM.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using PropertyChanged;
using VeganLife.Helpers;
using VeganLife.Views.AboutYou;

namespace VeganLife.ViewModels
{
    public partial class NameAboutUPageVM : BaseViewModel
    {
        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(NextClickedCommand))]
        private bool _isFilledName;

        public override Task ViewAppearingVM()
        {
            ServicesHelper.GetService<IDeviceService>().SetNavigationBarColor("#144d5a");
            return base.ViewAppearingVM();
        }

        [RelayCommand(CanExecute = nameof(IsFilledName))]
        private async Task OnNextClicked()
        {
            if (NextClickedCommand.IsRunning)
            {
                return;
            }

#if DEV
            var initDateOfBithday = DateTimeHelper.GetDateTime("1999-01-01").Date;
            await this.navigationService.NavigateToPage<GenderAboutPage>(
                paramater: new InitAboutYouDataRecord(Name: Name, Birthday: initDateOfBithday, false));
#else
            await this.navigationService.NavigateToPage<BirthdayAboutPage>(paramater: Name);
#endif
        }

        [SuppressPropertyChangedWarnings]
        partial void OnNameChanged(string value)
        {
            IsFilledName = !string.IsNullOrEmpty(Name) && !string.IsNullOrWhiteSpace(Name);
        }
    }
}