namespace VeganLife.Views.Base
{
    internal interface IBaseRootPage
    {
        bool IsAnimated { get; set; }

        Task OnOpenedShellFlyout();

        Task OnClosedShellFlyout();
    }
}
