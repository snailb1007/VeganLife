using VeganLife.Views.Base;

namespace VeganLife.Views.MainPageFlyout;

public partial class NoteBookPage : BasePage<NoteBookPageViewModel>
{
    public NoteBookPage(NoteBookPageViewModel vm)
        : base(vm)
    {
        this.InitializeComponent();
    }
}