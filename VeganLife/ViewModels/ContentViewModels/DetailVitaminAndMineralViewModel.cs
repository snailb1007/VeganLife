// <copyright file="DetailVitaminAndMineralViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>


namespace VeganLife.ViewModels.ContentViewModels
{
    /// <summary>
    /// vm of DetailVitaminAndMineralPage.
    /// </summary>
    public partial class DetailVitaminAndMineralViewModel : BaseViewModel
    {
        [ObservableProperty]
        private VitaminModel vitamin;
        /// <summary>
        /// Initializes a new instance of the <see cref="DetailVitaminAndMineralViewModel"/> class.
        /// </summary>
        public DetailVitaminAndMineralViewModel()
            : base()
        {
        }

        public override Task OnNavigatingTo(object? parameter)
        {
            if (parameter is VitaminModel vitamin)
            {
                this.Vitamin = vitamin;
            }

            return base.OnNavigatingTo(parameter);
        }
    }
}
