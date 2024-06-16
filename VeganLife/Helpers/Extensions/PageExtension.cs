using VeganLife.Views.Base;

namespace VeganLife.Helpers.Extensions
{
    internal static class PageExtension
    {
        public static async Task AnimateShellMenu(this Page page, Grid mainGridContent)
        {
            _ = mainGridContent.RotateYTo(-2);
            _ = mainGridContent.TranslateTo(mainGridContent.Width * 0.58, page.Height * 0.1, 800u, Easing.CubicIn);
            await mainGridContent.ScaleTo(0.8, 800u);
            _ = mainGridContent.FadeTo(0.8, 800u);
        }

        public static async Task AnimateCloseShellMenu(this Page page, Grid mainGridContent)
        {
            _ = mainGridContent.RotateYTo(0, 100);
            _ = mainGridContent.TranslateTo(0, 0, 500, Easing.CubicIn);
            await mainGridContent.ScaleTo(1, 500);
            _ = mainGridContent.FadeTo(1, 500);
        }
    }
}
