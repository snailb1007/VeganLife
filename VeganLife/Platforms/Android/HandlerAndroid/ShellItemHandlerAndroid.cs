using Microsoft.Maui.Controls.Platform.Compatibility;
using VeganLife.Helpers;

namespace VeganLife.Platforms.Android.HandlerAndroid
{
    public class ShellItemHandlerAndroid : ShellItemRenderer
    {
        public ShellItemHandlerAndroid(IShellContext shellContext) : base(shellContext)
        {
        }

        protected override void OnTabReselected(ShellSection shellSection)
        {
            base.OnTabReselected(shellSection);
            var navi = ServicesHelper.GetService<INavigationService>();
            if (navi.GetStackCount() == 1)
                return;
            navi.PopToRootAsync();
        }
    }
}
