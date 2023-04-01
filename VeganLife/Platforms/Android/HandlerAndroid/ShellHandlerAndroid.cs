using Microsoft.Maui.Controls.Platform.Compatibility;
using VeganLife.Platforms.Android.HandlerAndroid;

namespace VeganLife.Handlers
{
    public partial class ShellHandler
    {
        protected override IShellItemRenderer CreateShellItemRenderer(ShellItem shellItem)
        {
            return new ShellItemHandlerAndroid(this);
        }
    }
}
