using VeganLife.Views.Base;

namespace VeganLife.Views.MainPageFlyout;

public partial class NoteBookPage : BasePage<ReportPageViewModel>
{
    public NoteBookPage(ReportPageViewModel vm)
        : base(vm)
    {
        this.InitializeComponent();
    }
}