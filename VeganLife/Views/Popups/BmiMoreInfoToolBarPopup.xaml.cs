using VeganLife.ViewModels.PopupViewModels;

namespace VeganLife.Views.Popups;

public partial class BmiMoreInfoToolBarPopup
{
    public BmiMoreInfoToolBarPopupVM ViewModel { get; private set; }
    public BmiMoreInfoToolBarPopup(BmiMoreInfoToolBarPopupVM vm)
    {
        InitializeComponent();
        mainGrid.WidthRequest = App.MainWidthSize * 0.8;
        this.BindingContext = ViewModel = vm;
    }

    private void Close_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        this.Close();
    }
}