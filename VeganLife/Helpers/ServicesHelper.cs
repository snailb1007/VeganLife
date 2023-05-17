namespace VeganLife.Helpers
{
    public static class ServicesHelper
    {
        public static IServiceProvider CurrentServices => MauiApplication.Current.Services;

        public static T GetService<T>() => CurrentServices.GetService<T>();
    }
}
