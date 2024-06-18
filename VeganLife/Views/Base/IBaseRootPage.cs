namespace VeganLife.Views.Base
{
    internal interface IBaseRootPage
    {
        bool IsAnimated { get; set; }

        void OnOpenedShellFlyout();

        void OnClosedShellFlyout();
    }
}
