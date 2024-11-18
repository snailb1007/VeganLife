using VeganLife.ViewModels.ContentViewModels;

namespace VeganLife.Views.ContentViews;

public partial class DataGridNutritionFactCW
{
    public UsdaFoodFactDetailVM ViewModel { get; private set; }

    public DataGridNutritionFactCW()
    {
        InitializeComponent();
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName != nameof(ItemsSource))
        {
            return;
        }

        if (this.ItemsSource is null)
        {
            return;
        }

        unitNameColumn.PropertyName = this.ItemsSource is List<UndefinedFoodNutrient> ? "Unit" : "Nutrient.UnitName";
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        ViewModel ??= this.BindingContext as UsdaFoodFactDetailVM;
    }

    private void SelfDataGridNutritionFactCW_Refreshing(object sender, EventArgs e)
    {
        Task.Delay(200);
        this.IsRefreshing = false;
    }
}