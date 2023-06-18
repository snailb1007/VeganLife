using VeganLife.ViewModels.ContentViewModels;
using VeganLife.Views.Base;

namespace VeganLife.Views.ContentViews;

public partial class VitaminAndMineralPage : BasePage<VitaminAndMineralViewModel>
{
    readonly VitaminAndMineralViewModel _viewModel;

    public VitaminAndMineralPage(VitaminAndMineralViewModel vm) : base(vm)
    {
        InitializeComponent();
        _viewModel = vm;
    }

    private void vitaminsCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }
}