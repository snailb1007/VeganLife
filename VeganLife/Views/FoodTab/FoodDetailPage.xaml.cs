using VeganLife.ViewModels.ContentViewModels;
using VeganLife.Views.Base;

namespace VeganLife.Views.FoodTab;

public partial class FoodDetailPage : BasePage<FoodDetailViewModel>
{
    double _marginTopContent;
    public double MarginTopContent
    {
        get => _marginTopContent;
        set => SetProperty(ref _marginTopContent, value);
    }

    public FoodDetailPage(FoodDetailViewModel vm) : base(vm)
    {
        InitializeComponent();
    }

    double _imgHeight;
    private void Image_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName.Equals("Height"))
        {
            var img = sender as Image;
            if (img?.Height > 0)
            {
                _imgHeight = img.Height;
            }
        }
    }

    double _frameTitleHeight;
    private void Frame_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {

        if (e.PropertyName.Equals("Height"))
        {
            var frame = sender as Grid;
            if (frame?.Height > 0)
            {
                _frameTitleHeight = frame.Height;
                if (_imgHeight > 0)
                {
                    Console.WriteLine("==> Frame_PropertyChanged " + imgPreview.HeightRequest);
                    MarginTopContent = _imgHeight - _frameTitleHeight / 2f;
                }
            }
        }
    }

    private void VerticalStackLayout_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName.Equals("Height"))
        {
            var stack = sender as VerticalStackLayout;
            (stack as IView).InvalidateMeasure();
        }
    }
}