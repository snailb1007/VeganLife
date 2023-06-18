namespace VeganLife.Helpers
{
    public static class ServicesHelper
    {
        public static T GetService<T>() => MauiApplication.Current.Services.GetService<T>();
    }
}
