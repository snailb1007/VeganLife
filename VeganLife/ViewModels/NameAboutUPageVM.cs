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
        private string name;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(NextClickedCommand))]
        private bool isFilledName;

        public override Task ViewAppearingVM()
        {
            ServicesHelper.GetService<IDeviceService>().SetNavigationBarColor("#144d5a");
            return base.ViewAppearingVM();
        }

        [RelayCommand(CanExecute = nameof(IsFilledName))]
        public async Task OnNextClicked()
        {
            if (NextClickedCommand.IsRunning)
            {
                return;
            }

            await this.navigationService.NavigateToPage<BirthdayAboutPage>(paramater: Name);
        }

        [SuppressPropertyChangedWarnings]
        partial void OnNameChanged(string value)
        {
            IsFilledName = !string.IsNullOrEmpty(Name) && !string.IsNullOrWhiteSpace(Name);
        }
    }
}