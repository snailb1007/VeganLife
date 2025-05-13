using FFImageLoading.Helpers;

namespace VeganLife.Helpers
{
    internal class AppHelpers
    {
        public static Page CurrentMainPage
        {
            get
            {
                try
                {
                    var result = App.Current?.Windows.FirstOrDefault()?.Page;
                    return result ?? throw new InvalidOperationException("No main page found. Ensure the app has valid main page");
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error get the main page: " + e.Message);
                    return null;
                }
            }
        }

        public static void SetMainPage(Page newPage)
        {
            if (newPage == null)
                throw new ArgumentNullException(nameof(newPage), "New MainPage cannot be null.");

            var app = Application.Current ?? throw new InvalidOperationException("Current Application instance available.");
            if (app.Windows.Count == 0)
                throw new InvalidOperationException("No Windows available in the current Application.");

            app.Windows[0].Page = newPage;
        }
    }
}
