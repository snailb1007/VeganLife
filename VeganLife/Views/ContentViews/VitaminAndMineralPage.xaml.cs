using VeganLife.ViewModels.ContentViewModels;

namespace VeganLife.Views.ContentViews;

[QueryProperty(nameof(Name), "name")]
public partial class VitaminAndMineralPage : ContentPage
{
    VitaminAndMineralViewModel _viewModel;

    public string Name
    {
        set => LoadAnimal(value);
    }

    public VitaminAndMineralPage(VitaminAndMineralViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
        _viewModel = vm;
	}

    void LoadAnimal(string name)
    {
        try
        {
            var vitamin = _viewModel.Vitamins.FirstOrDefault(a => a.Name == name);
        }
        catch (Exception)
        {
            Console.WriteLine("Failed to load.");
        }
    }
}