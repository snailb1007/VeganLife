// <copyright file="BmiResultPopupViewmodel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.ViewModels.PopupViewModels
{
    using VeganLife.Helpers;
#if GPT
    using ChatGptNet;
    using ChatGptNet.Exceptions;
    using VeganLife.Resources.Translations;
#endif

    /// <summary>
    /// vm for  BmiResultPopup.
    /// </summary>
    public partial class BmiResultPopupViewmodel : BaseViewModel
    {
        [ObservableProperty]
        private string bmiResultText;

        [ObservableProperty]
        private Color bmiStatusColor;

        public string Message { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BmiResultPopupViewmodel"/> class.
        /// </summary>
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
#if GPT
                string sex = result.Sex;
                string openAIMess = string.Empty;
                try
                {
                    string query = string.Format(AppResources.queryBMI_bmiPopup, sex, result.Age, result.BMIResult);
                    await ServicesHelper.GetService<IChatGptClient>()
                        .AskAsync(System.Guid.NewGuid(), message: query)
                        .ContinueWith((t) =>
                    {
                        if (t.Result.IsSuccessful)
                        {
                            openAIMess = t.Result.GetMessage();
                            this.Message = openAIMess;
                        }
                    }).ConfigureAwait(true);
                }
                catch (ChatGptException chatEX)
                {
#if DEBUG
                    await Console.Out.WriteLineAsync("==> " + chatEX.Message);
#endif
                }
                catch (Exception)
                {
                }
#endif
            }

            return base.OnNavigatingTo(parameter);
        }
    }
}
