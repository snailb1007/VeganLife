namespace VeganLife.Views.ChatFlyout;

public partial class ConversationPage : ContentPage
{
	public ConversationPage(ConversationViewModel viewModel)
	{
		this.BindingContext = viewModel;
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
		await (this.BindingContext as ConversationViewModel)?.ViewAppearingVM()!;
    }
}