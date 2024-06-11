using VeganLife.Views.Base;

namespace VeganLife.Views.MainPageFlyout
{
    public partial class VisionPage : BasePage<VisionPageVM>
    {
        public VisionPage(VisionPageVM vm)
            : base(vm)
        {
            InitializeComponent();
        }
    }
}