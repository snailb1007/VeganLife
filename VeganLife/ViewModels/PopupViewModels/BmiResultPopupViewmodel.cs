// <copyright file="BmiResultPopupViewmodel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.ViewModels.PopupViewModels
{
    using VeganLife.Helpers;

    public partial class BmiResultPopupViewmodel : BaseViewModel
    {
        [ObservableProperty]
        private string bmiResultText;

        [ObservableProperty]
        private Color bmiStatusColor;

        public BmiResultPopupViewmodel()
            : base()
        {
        }

        /// <inheritdoc/>
        public override Task OnNavigatingTo(object parameter)
        {
            if (parameter != null)
            {
                var result = parameter as BMIResultModel;
                this.BmiResultText = result?.BMIResult.ToString();
                if (short.TryParse(result.Age, out var age))
                {
                    this.BmiStatusColor = BMICalculateHelper.GetWeightStatusCategory(age, result.Sex, result.BMIResult);
                }
            }

            return base.OnNavigatingTo(parameter);
        }
    }
}
