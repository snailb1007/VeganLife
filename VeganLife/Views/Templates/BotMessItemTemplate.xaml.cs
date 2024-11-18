namespace VeganLife.Views.Templates;

public partial class BotMessItemTemplate
{
    private const string _uriAvatar = "https://firebasestorage.googleapis.com/v0/b/vegan-life-d1c9b.appspot.com/o/bot_girl.jpg?alt=media&token=c67d9b16-4ebd-45da-9066-7590c586b8bb";

    public BotMessItemTemplate()
    {
        InitializeComponent();
        img.Source = _uriAvatar;
    }
}