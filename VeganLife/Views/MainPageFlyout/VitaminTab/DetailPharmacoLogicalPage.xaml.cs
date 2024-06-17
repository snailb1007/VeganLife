using VeganLife.Views.Base;

namespace VeganLife.Views.MainPageFlyout.VitaminTab;

public partial class DetailPharmacoLogicalPage : BasePage<DetailPharmacoLogicalPageVM>
{
    public DetailPharmacoLogicalPage(DetailPharmacoLogicalPageVM viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}