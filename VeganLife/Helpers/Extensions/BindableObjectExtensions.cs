namespace VeganLife.Helpers.Extensions
{
    internal static class BindableObjectExtensions
    {
        public static TViewModel GetViewModel<TViewModel>(this BindableObject element)
            where TViewModel : class
        {
            return element.BindingContext as TViewModel;
        }
    }
}
