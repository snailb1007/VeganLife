using AsyncAwaitBestPractices;
using CommunityToolkit.Maui.Views;
using VeganLife.ViewModels.ToolsFlyoutViewModel;
using VeganLife.Views.Popups;

namespace VeganLife.Views.ToolFlyout;

public partial class BMICalculatorPage
{
    public BMICalculatorPage(BmiCalculatorViewModel vm)
        : base(vm)
    {
        InitializeComponent();
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        this.ShowPopupAsync(FFImageLoading.Helpers.ServiceHelper.GetService<BmiMoreInfoToolBarPopup>()).SafeFireAndForget();
    }
}