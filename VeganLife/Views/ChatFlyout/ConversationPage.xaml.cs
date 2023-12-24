namespace VeganLife.Views.ChatFlyout;

public partial class ConversationPage : ContentPage
{
	public ConversationPage(ConversationViewModel viewModel)
	{
		this.BindingContext = viewModel;
		InitializeComponent();
	}
}