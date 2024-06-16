using VeganLife.Helpers.Extensions;
using VeganLife.Views.Base;

namespace VeganLife.Views.MainPageFlyout;

public partial class NoteBookPage : BasePage<NoteBookPageViewModel>, IBaseRootPage
{
    public NoteBookPage(NoteBookPageViewModel vm)
        : base(vm)
    {
        this.InitializeComponent();
    }

    public bool IsAnimated { get; set; }

    public async Task OnOpenedShellFlyout()
    {
        await this.AnimateShellMenu(this.mainGridContent);
    }

    public async Task OnClosedShellFlyout()
    {
        await this.AnimateCloseShellMenu(this.mainGridContent);
    }
}