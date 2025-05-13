using CommunityToolkit.Maui.Views;

namespace VeganLife.Views.Popups;

public partial class EditMealPopup : Popup
{
    public class EditMealResult
    {
        public bool IsDeleted { get; set; }
        public bool IsAmountChanged { get; set; }
        public int NewAmount { get; set; }
    }

    private readonly NutritionMealLogModel _mealLog;

    public EditMealPopup(NutritionMealLogModel mealLog)
    {
        InitializeComponent();
        _mealLog = mealLog;

        // Set up the initial values
        MealNameLabel.Text = mealLog.Name;
        AmountEntry.Text = mealLog.Amount.ToString();
    }

    private void OnSaveClicked(object sender, EventArgs e)
    {
        if (int.TryParse(AmountEntry.Text, out int newAmount) && newAmount > 0)
        {
            Close(new EditMealResult
            {
                IsAmountChanged = newAmount != _mealLog.Amount,
                NewAmount = newAmount
            });
        }
        else
        {
            // Show validation error
            AmountEntry.PlaceholderColor = Colors.Red;
            AmountEntry.Placeholder = "Enter a valid amount";
            AmountEntry.Text = string.Empty;
        }
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        Close(new EditMealResult { IsDeleted = true });
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close();
    }
}