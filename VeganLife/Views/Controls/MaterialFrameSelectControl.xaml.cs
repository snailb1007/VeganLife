using Sharpnado.MaterialFrame;

namespace VeganLife.Views.Controls;

public partial class MaterialFrameSelectControl
{
    public static BindableProperty IsSelectedProperty = BindableProperty.Create(
                propertyName: nameof(IsSelected),
                declaringType: typeof(MaterialFrameSelectControl),
                defaultValue: false,
                returnType: typeof(bool));

    public bool IsSelected
    {
        get => (bool)this.GetValue(IsSelectedProperty);
        set => this.SetValue(IsSelectedProperty, value);
    }

    public static BindableProperty TitleProperty = BindableProperty.Create(
                propertyName: nameof(Title),
                declaringType: typeof(MaterialFrameSelectControl),
                defaultValue: string.Empty,
                returnType: typeof(string));

    public string Title
    {
        get => (string)this.GetValue(TitleProperty);
        set => this.SetValue(TitleProperty, value);
    }

    public MaterialFrameSelectControl()
    {
        InitializeComponent();
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        switch (propertyName)
        {
            case nameof(IsSelected):
                this.Opacity = IsSelected ? 1 : 0.5;
                break;
            case nameof(Title):
                lbTitle.Text = Title;
                break;
        }

        base.OnPropertyChanged(propertyName);
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        IsSelected = !IsSelected;
    }
}