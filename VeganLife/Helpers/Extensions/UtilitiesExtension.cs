namespace VeganLife.Helpers.Extensions
{
    public static class UtilitiesExtension
    {
        public static void TearDown(this IVisualTreeElement vte)
        {
            TearDownImpl(vte, true);
            return;

            void TearDownImpl(IVisualTreeElement vte, bool isRoot)
            {
                if (vte is not BindableObject bindableObject)
                {
                    return;
                }

                foreach (var childElement in vte.GetVisualChildren())
                {
                    TearDownImpl(childElement, false);
                }

                if (vte is VisualElement visualElement)
                {
                    // First, clear the BindingContext
                    visualElement.BindingContext = null;

                    // Next, isolate the element.
                    visualElement.Parent = null;

                    if (vte is ListView listView)
                    {
                        listView.ItemsSource = null;
                    }
                    else if (vte is ContentView contentView)
                    {
                        contentView.Content = null;
                    }
                    else if (vte is Border border)
                    {
                        border.Content = null;
                    }
                    else if (vte is ContentPage contentPage)
                    {
                        contentPage.Content = null;
                    }
                    else if (vte is ScrollView scrollView)
                    {
                        scrollView.Content = null;
                    }

                    visualElement.ClearLogicalChildren();

                    // The _last_ thing we want to do is disconnect the handler.
                    if (visualElement.Handler != null)
                    {
                        if (visualElement.Handler is IDisposable disposableHandler)
                        {
                            disposableHandler.Dispose();
                        }

                        visualElement.Handler?.DisconnectHandler();
                    }

                    visualElement.Resources = null;
                }
                else if (vte is Element element)
                {
                    element.BindingContext = null;
                    element.Parent = null;

                    element.ClearLogicalChildren();
                    if (element.Handler != null)
                    {
#if IOS
                    // Fixes issue specific to ListView on iOS, where RealCell is not nulled out.
                    if (element is ViewCell && element.Handler.PlatformView is IDisposable disposablePlatformView)
                        disposablePlatformView.Dispose();
#endif
                        if (element.Handler is IDisposable disposableElementHandler)
                        {
                            disposableElementHandler.Dispose();
                        }

                        element.Handler.DisconnectHandler();
                    }
                }
            }
        }

        public static void LogError(this Exception e)
        {
#if DEBUG
            Console.Out.WriteLineAsync($"Error: {e.Message}");
#endif
        }
    }
}
