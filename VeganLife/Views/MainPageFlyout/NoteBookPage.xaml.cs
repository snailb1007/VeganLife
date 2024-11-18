using VeganLife.Helpers.Extensions;
using VeganLife.Views.Base;

namespace VeganLife.Views.MainPageFlyout;

public partial class NoteBookPage : IBaseRootPage
{
    public NoteBookPage(NoteBookPageViewModel vm)
        : base(vm)
    {
        this.InitializeComponent();
    }

    public bool IsAnimated { get; set; }

    public void OnOpenedShellFlyout()
    {
        this.AnimateShellMenu(this.mainGridContent);
    }

    public void OnClosedShellFlyout()
    {
        this.AnimateCloseShellMenu(this.mainGridContent);
    }
}