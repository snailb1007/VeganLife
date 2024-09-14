using Maui.DataGrid;

namespace VeganLife.Views.ContentViews;

public partial class DataGridNutritionFactCW : DataGrid
{
    public DataGridNutritionFactCW()
    {
        InitializeComponent();
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == nameof(ItemsSource))
        {
            if (this.ItemsSource is not null)
            {
                if (this.ItemsSource is List<UndefinedFoodNutrient>)
                {
                    unitNameColumn.PropertyName = "Unit";
                }
                else
                {
                    unitNameColumn.PropertyName = "Nutrient.UnitName";
                }
            }
        }
    }

    private void SelfDataGridNutritionFactCW_Refreshing(object sender, EventArgs e)
    {
        Task.Delay(200);
        this.IsRefreshing = false;
    }
}