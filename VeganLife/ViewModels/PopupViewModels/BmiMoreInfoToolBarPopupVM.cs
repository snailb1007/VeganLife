using VeganLife.Helpers;
using static VeganLife.Helpers.AppSetting.StaticHelper;

namespace VeganLife.ViewModels.PopupViewModels
{
    public partial class BmiMoreInfoToolBarPopupVM : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<string> _documents;

        public BmiMoreInfoToolBarPopupVM()
            : base()
        {
            this.Documents = new ObservableCollection<string>();
            var document = HealthDiagnosisFirebaseDataModel.BMIModel?.Documents;
            if (document is null)
            {
                return;
            }

            Documents.Add(document.WikiVN);
            Documents.Add(document.WHO);
        }

        [RelayCommand]
        private async Task HyperlinkClicked(object parameter)
        {
            var data = parameter as string;
            if (!string.IsNullOrEmpty(data) && !HyperlinkClickedCommand.IsRunning)
            {
                var link = StringProcessHelper.GetHyperLink(data);
                if (!string.IsNullOrEmpty(link))
                {
                    await Browser.Default.OpenAsync(link);
                    parameter = null!;
                }
            }
        }
    }
}
