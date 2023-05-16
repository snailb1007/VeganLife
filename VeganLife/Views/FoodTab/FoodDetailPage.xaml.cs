using VeganLife.ViewModels.ContentViewModels;

namespace VeganLife.Views.FoodTab;

public partial class FoodDetailPage : ContentPage, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler CustomPropertyChanged;
    Thickness _marginTopContent;
    public Thickness MarginTopContent
    {
        get => _marginTopContent;
        set
        {
            _marginTopContent = value;
            OnPropertyChanged(nameof(MarginTopContent));
        }
    }

	public FoodDetailPage(FoodDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        CustomPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
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
            var frame = sender as Frame;
            if (frame?.Height > 0)
            {
                _frameTitleHeight = frame.Height;
                if (_imgHeight > 0)
                {
                    MarginTopContent = new Thickness(0, (int)(_imgHeight - _frameTitleHeight), 0, 0);
                    (frame as IView).InvalidateMeasure();
                }
            }
        }
    }
}