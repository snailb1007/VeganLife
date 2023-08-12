// <copyright file="ProfilePopupViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.ViewModels.PopupViewModels
{
    /// <summary>
    /// vm for  ProfilePopup.
    /// </summary>
    public partial class ProfilePopupViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string userName;
        [ObservableProperty]
        private string userAge;
        [ObservableProperty]
        private string userHeight;
        [ObservableProperty]
        private string userWeight;
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfilePopupViewModel"/> class.
        /// </summary>
        public ProfilePopupViewModel()
        {
        }

        [RelayCommand]
        private async Task CloseThisPopup()
        {
            if (CloseThisPopupCommand.IsRunning)
            {
                return;
            }

            await popupNaviService.PopAsync();
        }
    }
}
