namespace VeganLife.Helpers.Extensions
{
    internal static class PageExtension
    {
        public static void AnimateShellMenu(this Page page, Grid mainGridContent)
        {
            //_ = mainGridContent.RotateYTo(-2);
            //_ = mainGridContent.TranslateTo(mainGridContent.Width * 0.58, page.Height * 0.05, 800u, Easing.CubicIn);
            //await mainGridContent.ScaleTo(0.8, 800u);
            //_ = mainGridContent.FadeTo(0.8, 800u);
            var animation = new Animation(d =>
            {
                mainGridContent.Scale = 1 - ((1 - 0.8) * d);
                mainGridContent.TranslationX = mainGridContent.Width * 0.58 * d;
                mainGridContent.TranslationY = page.Height * 0.05 * d;
                mainGridContent.RotationY = -2 * d;
            });
            animation.Commit(mainGridContent, "OpenMenuAnimation", finished: (d, b) =>
            {
                mainGridContent.Scale = 0.8;
                mainGridContent.TranslationX = mainGridContent.Width * 0.58;
                mainGridContent.RotationY = -2;
            });
        }

        public static void AnimateCloseShellMenu(this Page page, Grid mainGridContent)
        {
            //_ = mainGridContent.RotateYTo(0, 100);
            //_ = mainGridContent.TranslateTo(0, 0, 500, Easing.CubicIn);
            //await mainGridContent.ScaleTo(1, 500);
            //_ = mainGridContent.FadeTo(1, 500);
            mainGridContent.AbortAnimation("OpenMenuAnimation");
            mainGridContent.AbortAnimation("CloseMenuAnimation");

            Animation animation = new Animation(d =>
            {
                mainGridContent.Scale = 0.8 + ((1 - 0.8) * d);
                mainGridContent.TranslationX = (mainGridContent.Width * 0.58) - ((mainGridContent.Width * 0.58) * d);
            });

            animation.Commit(mainGridContent, "CloseMenuAnimation", finished: (d, b) =>
            {
                mainGridContent.Scale = 1;
                mainGridContent.TranslationX = 0;
                mainGridContent.RotationY = 0;

                mainGridContent.Clip = null;
            });
        }
    }
}
