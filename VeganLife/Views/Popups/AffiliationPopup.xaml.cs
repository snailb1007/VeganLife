using AsyncAwaitBestPractices;
using Mopups.Pages;
using Mopups.Services;
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

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var target = (sender as Label)?.BindingContext as AffiliationModel;
        if (target is null)
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