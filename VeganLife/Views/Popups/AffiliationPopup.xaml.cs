using Mopups.Pages;
using VeganLife.Helpers;

namespace VeganLife.Views.Popups;

public partial class AffiliationPopup : PopupPage
{
    public List<AffiliationModel> Affiliations { get; set; }

    public AffiliationPopup(List<AffiliationModel> affiliations)
    {
        InitializeComponent();
        Affiliations = affiliations;
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var target = (sender as Label)?.BindingContext as AffiliationModel;
        if (target is null)
        {
            return;
        }

        await ServicesHelper.OpenViaBrowserAsync(target.Link);
    }
}