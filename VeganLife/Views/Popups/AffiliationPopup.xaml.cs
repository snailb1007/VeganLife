using AsyncAwaitBestPractices;
using Mopups.Pages;
using Mopups.Services;
using VeganLife.Helpers;

namespace VeganLife.Views.Popups;

public partial class AffiliationPopup
{
    public List<AffiliationModel> Affiliations { get; set; }

    public AffiliationPopup(List<AffiliationModel> affiliations)
    {
        InitializeComponent();
        Affiliations = affiliations;
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if ((sender as Label)?.BindingContext is not AffiliationModel target)
        {
            return;
        }

        ServicesHelper.OpenViaBrowserAsync(target.Link).SafeFireAndForget();
    }

    private void CloseBtn_Clicked(object sender, EventArgs e)
    {
        MopupService.Instance.PopAsync().SafeFireAndForget();
    }
}