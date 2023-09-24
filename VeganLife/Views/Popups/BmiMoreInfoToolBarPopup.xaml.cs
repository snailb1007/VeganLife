using CommunityToolkit.Maui.Views;
using static VeganLife.Helpers.AppSetting.StaticHelper;

namespace VeganLife.Views.Popups;

public partial class BmiMoreInfoToolBarPopup : Popup
{
	public ObservableCollection<string> Documents { get; private set; }
	public BmiMoreInfoToolBarPopup()
	{
		InitializeComponent();
		mainGrid.WidthRequest = App.MainSize * 0.8;
		Documents = new ObservableCollection<string>();
		var document = HealthDiagnosisFirebaseDataModel.BMIModel?.Documents;
		Documents.Add(document.WikiVN);
		Documents.Add(document.WHO);
    }

    private void Close_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		this.Close();
    }
}