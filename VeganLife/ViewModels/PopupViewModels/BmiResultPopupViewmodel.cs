// <copyright file="BmiResultPopupViewmodel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.ViewModels.PopupViewModels
{
    using CommunityToolkit.Mvvm.Messaging;
    using Mopups.Services;
    using VeganLife.Helpers;
    using VeganLife.messages;
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

        [ObservableProperty]
        private string classifyLabel;
        [ObservableProperty]
        private string note;
        [ObservableProperty]
        private bool isReCalculateSelected;
        [ObservableProperty]
        private bool isGoAnalysisPageSelected = true;
        [ObservableProperty]
        private bool isSaveSelected;

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
                    var healthDiagnosis = BMICalculateHelper.GetWeightStatusCategory(age, result.Sex, result.BMIResult);
                    this.BmiStatusColor = healthDiagnosis.StatusColor;
                    this.ClassifyLabel = healthDiagnosis.Classify;
                    this.Note = healthDiagnosis.Note;
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

        [RelayCommand]
        private void SelectButton(string option)
        {
            if (option.Equals("0"))
            {
                if (this.IsReCalculateSelected)
                {
                    ClosePopupCommand.Execute(null);
                    return;
                }

                IsReCalculateSelected = true;
                IsGoAnalysisPageSelected = false;
                IsSaveSelected = false;
            }
            else if (option.Equals("1"))
            {
                if (this.IsGoAnalysisPageSelected)
                {
                    ClosePopupCommand.Execute(null);
                    WeakReferenceMessenger.Default.Send(new BmiResultSelectedOptionMessage(1));
                    return;
                }

                IsReCalculateSelected = false;
                IsGoAnalysisPageSelected = true;
                IsSaveSelected = false;
            }
            else
            {
                IsReCalculateSelected = false;
                IsGoAnalysisPageSelected = false;
                IsSaveSelected = true;
            }
        }

        [RelayCommand]
        private async Task ClosePopup()
        {
            if (ClosePopupCommand.IsRunning)
            {
                return;
            }

            await MopupService.Instance.PopAsync();
        }
    }
}
