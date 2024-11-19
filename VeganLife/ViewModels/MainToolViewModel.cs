using CommunityToolkit.Mvvm.Messaging;
using PropertyChanged;
using VeganLife.Data;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Messages;
using VeganLife.Views.Popups;
using VeganLife.Views.ToolFlyout;

namespace VeganLife.ViewModels
{
    public partial class MainToolViewModel : BaseViewModel, IRecipient<BmiResultSelectedOptionMessage>
    {
        private readonly BaseDataStore<UserInfo> _infoDataStoreServie;

        private BMIResultModel _bmiResultData;
        private UserInfo _localUserInfor;

        public string WeightBmiRegexPattern { get; } = @"^(?:[1-9]\d*|0)+(?:\.(\d)?(\d)?)?$";

        public string AgeBmiRegexPattern { get; } = @"^\d+$";

        private float _weight;

        // private short age;
        [ObservableProperty]
        private int _height;

        [ObservableProperty]
        private bool _isDisplayedSexDetail;

        [ObservableProperty]
        private bool _isMale;

        [ObservableProperty]
        private bool _isEnableSubmit;

        [ObservableProperty]
        private string _weightValue;

        [ObservableProperty]
        private int _ageValue;

        [ObservableProperty]
        private string _backgroundImg;

        [ObservableProperty]
        private string _weightErrMess;

        [ObservableProperty]
        private string _ageErrMess;

        [ObservableProperty]
        private string _generalError;

        [ObservableProperty]
        private double _bmiResult;

        public MainToolViewModel(LocalDataStoreFactory localDataStore)
            : base()
        {
            _infoDataStoreServie = localDataStore.GetDataStore<UserInfo>();
        }

        public override async Task<Task> ViewAppearingVM()
        {
            await _infoDataStoreServie.GetItemsAsync().ContinueWith(t =>
            {
                this._localUserInfor = t?.Result?.FirstOrDefault() ?? new UserInfo();

                // fill data from user service
                if (string.IsNullOrEmpty(_localUserInfor.Name))
                {
                    return;
                }

                this.IsMale = _localUserInfor.IsMale;
                this.AgeValue = _localUserInfor.Age;
                this.Height = _localUserInfor.Height;
                this.WeightValue = _localUserInfor.Weight.ToString(CultureInfo.InvariantCulture);
            }).ConfigureAwait(false);
            WeakReferenceMessenger.Default.Register<BmiResultSelectedOptionMessage>(this);
            return base.ViewAppearingVM();
        }

        public override Task ViewDisappearingVM()
        {
            WeakReferenceMessenger.Default.Unregister<BmiResultSelectedOptionMessage>(this);
            return base.ViewDisappearingVM();
        }

        [RelayCommand]
        private async Task CalculateBmi()
        {
            this.BmiResult = BMICalculateHelper.Calculate(this._weight, this.Height / 100f);
            _bmiResultData = new BMIResultModel()
            {
                BMIResult = (float)this.BmiResult,
                IsMale = this.IsMale,
                Age = this.AgeValue,
            };
            await ServicesHelper.GetService<IPopupNaviService>().PushAsync<BmiResultPopup>(_bmiResultData);
        }

        [RelayCommand]
        private void HelpSexDetail(string parameter)
        {
            if (!string.IsNullOrEmpty(parameter) && parameter.Equals("closeSexDetail"))
            {
                this.IsDisplayedSexDetail = false;
            }
            else
            {
                this.IsDisplayedSexDetail = !this.IsDisplayedSexDetail;
            }
        }

        [RelayCommand]
        private void UnFocus(object obj)
        {
            var view = (MainTool)obj;
            if (view == null)
            {
                return;
            }

            if (view.FindByName("entryWeight") is Entry { IsFocused: true } entryWeight)
            {
                this.deviceService.HideKeyboard();
                entryWeight.Unfocus();
                return;
            }

            if (view.FindByName("entryAge") is not Entry { IsFocused: true } entryAge)
            {
                return;
            }

            this.deviceService.HideKeyboard();
            entryAge.Unfocus();
        }

        [RelayCommand]
        private void SelectGender(string parameter)
        {
            this.IsMale = !this.IsMale;
        }

        [SuppressPropertyChangedWarnings]
        partial void OnWeightValueChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                this.WeightErrMess = string.Empty;
                this.IsEnableSubmit = false;
                return;
            }

            this._weight = float.TryParse(value, provider: CultureInfo.InvariantCulture.NumberFormat, out var outValue) ? outValue : 0;
            this.WeightErrMess = _weight switch
            {
                < 2 => Resources.Translations.AppResources.wrongWeight_tooLow_bmiCalculatePage,
                > 635 => Resources.Translations.AppResources.wrongWeight_tooHigh_bmiCalculatePage,
                _ => string.Empty
            };

            this.IsEnableSubmit = CheckEnableButtonCalculate();
        }

        [SuppressPropertyChangedWarnings]
        partial void OnAgeValueChanged(int value)
        {
            switch (value)
            {
                case <= 0:
                    this.AgeErrMess = string.Empty;
                    this.IsEnableSubmit = false;
                    return;
                case <= 1:
                    this.AgeErrMess = Resources.Translations.AppResources.wrongAge_tooLow_bmiCalculatePage;
                    break;
                case >= 140:
                    this.AgeErrMess = Resources.Translations.AppResources.wrongAge_tooHigh_bmiCalculatePage;
                    break;
                default:
                    this.AgeErrMess = string.Empty;
                    break;
            }

            this.IsEnableSubmit = CheckEnableButtonCalculate();
        }

        [SuppressPropertyChangedWarnings]
        partial void OnHeightChanged(int value)
        {
            this.Height = (int)Height;
        }

        private bool CheckEnableButtonCalculate()
        {
            return string.IsNullOrEmpty(this.WeightErrMess) && string.IsNullOrEmpty(this.AgeErrMess)
            && !string.IsNullOrEmpty(this.WeightValue) && (AgeValue >= 1);
        }

        [RelayCommand]
        private void Increase(object data)
        {
            if (data is null)
            {
                return;
            }

            if (data.ToString() == "weight")
            {
                WeightValue ??= "0";

                if (int.TryParse(WeightValue, out var weightNumber))
                {
                    WeightValue = (++weightNumber).ToString();
                }
            }
            else
            {
                AgeValue++;
            }
        }

        [RelayCommand]
        private void Decrease(object data)
        {
            if (data is null)
            {
                return;
            }

            switch (data.ToString())
            {
                case "weight":
                {
                    WeightValue ??= "0";

                    if (int.TryParse(WeightValue, out int weightNumber) && weightNumber >= 1)
                    {
                        WeightValue = (--weightNumber).ToString();
                    }

                    break;
                }
                case "age":
                    AgeValue--;
                    break;
            }
        }

        public void Receive(BmiResultSelectedOptionMessage message)
        {
            if (message is null)
            {
                return;
            }

            var param = message.Value;
            if (param == 0)
            {
                Height = 0;
                WeightValue = "0";
                AgeValue = 0;
            }
            else if (Application.Current?.Windows[0]?.Page is AppShell currentShell)
            {
                MainThread.BeginInvokeOnMainThread(() => currentShell.SwitchShellContentToolsTab(param));
            }
        }
    }
}
