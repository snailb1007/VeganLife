namespace VeganLife.ViewModels
{
    public partial class DetailPharmacoLogicalPageVM : BaseViewModel
    {
        [ObservableProperty]
        private PharmacoLogicalModel _pharmacoLogicalModel;

        public DetailPharmacoLogicalPageVM()
            : base()
        {
        }

        public override Task OnNavigatingTo(object? parameter)
        {
            if (parameter is PharmacoLogicalModel model)
            {
                this.PharmacoLogicalModel = model;
            }

            return base.OnNavigatingTo(parameter);
        }
    }
}
