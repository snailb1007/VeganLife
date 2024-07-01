using Mopups.Pages;

namespace VeganLife.Views.Popups;

public partial class AffiliationPopup : PopupPage
{
    public List<AffiliationModel> Affiliations { get; set; }

    public AffiliationPopup(List<AffiliationModel> affiliations)
    {
        InitializeComponent();
        Affiliations = affiliations;
    }
}