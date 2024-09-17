using AsyncAwaitBestPractices;
using Microsoft.Maui.Controls.Shapes;

namespace VeganLife.Helpers.Extensions
{
    internal static class PageExtension
    {
        private const string _openMenuAnimation = "OpenMenuAnimation";
        private const string _closeMenuAnimation = "CloseMenuAnimation";

        public static void AnimateShellMenu(this Page page, Grid mainGridContent)
        {
            mainGridContent.AbortAnimation(_openMenuAnimation);
            mainGridContent.AbortAnimation(_closeMenuAnimation);
            mainGridContent.FadeTo(0.5, 150).SafeFireAndForget();
            mainGridContent.Clip = new RoundRectangleGeometry(new CornerRadius(30), new Rect(0, 0, mainGridContent.Width, mainGridContent.Height));
            var animation = new Animation(d =>
            {
                mainGridContent.Scale = 1 - ((1 - 0.8) * d);
                mainGridContent.TranslationX = mainGridContent.Width * 0.58 * d;
                mainGridContent.TranslationY = page.Height * 0.02 * d;
                mainGridContent.RotationY = -2 * d;
            });
            animation.Commit(mainGridContent, _openMenuAnimation, finished: (d, b) =>
            {
                mainGridContent.Scale = 0.8;
                mainGridContent.TranslationX = mainGridContent.Width * 0.58;
                mainGridContent.RotationY = -2;
            });
        }

        public static void AnimateCloseShellMenu(this Page page, Grid mainGridContent)
        {
            mainGridContent.AbortAnimation(_openMenuAnimation);
            mainGridContent.AbortAnimation(_closeMenuAnimation);
            Animation animation = new Animation(d =>
            {
                mainGridContent.Scale = 0.8 + ((1 - 0.8) * d);
                mainGridContent.TranslationX = (mainGridContent.Width * 0.58) - ((mainGridContent.Width * 0.58) * d);
            });
            animation.Commit(mainGridContent, _closeMenuAnimation, length: 150, finished: (d, b) =>
            {
                mainGridContent.Scale = 1;
                mainGridContent.TranslationY = 0;
                mainGridContent.TranslationX = 0;
                mainGridContent.RotationY = 0;
                mainGridContent.Clip = null;
            });
            mainGridContent.FadeTo(1, 150).SafeFireAndForget();
        }
    }
}
