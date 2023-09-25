using VeganLife.Helpers;
using static VeganLife.Helpers.AppSetting.StaticHelper;

namespace VeganLife.ViewModels.PopupViewModels
{
    public partial class BmiMoreInfoToolBarPopupVM : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<string> documents;
        public BmiMoreInfoToolBarPopupVM()
            : base()
        {
            this.Documents = new ObservableCollection<string>();
            var document = HealthDiagnosisFirebaseDataModel.BMIModel?.Documents;
            Documents.Add(document.WikiVN);
            Documents.Add(document.WHO);
        }

        [RelayCommand]
        private async Task HyperlinkClicked(string parameter)
        {
            var data = parameter as string;
            if (!string.IsNullOrEmpty(parameter) && !HyperlinkClickedCommand.IsRunning)
            {
                await Browser.Default.OpenAsync(StringProcessHelper.GetHyperlink(parameter));
            }
        }
    }
}
