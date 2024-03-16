namespace VeganLife.Views.Controls
{
    public class FALabel : Label
    {
        public static readonly BindableProperty IconTypeProperty = BindableProperty.Create(nameof(IconType), typeof(IconType), typeof(FALabel), IconType.Solid, propertyChanged: OnIconTypeChanged);

        public IconType IconType
        {
            get => (IconType)GetValue(IconTypeProperty);
            set => SetValue(IconTypeProperty, value);
        }

        private static void OnIconTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (FALabel)bindable;
            switch (control.IconType)
            {
                case IconType.Solid:
                    control.FontFamily = "FASolid";
                    break;
                case IconType.Regular:
                    control.FontFamily = "FARegular";
                    break;
                case IconType.Thin:
                    control.FontFamily = "FAThin";
                    break;
                case IconType.Light:
                    control.FontFamily = "FALight";
                    break;
            }
        }
    }

    public enum IconType
    {
        Solid,
        Regular,
        Thin,
        Light
    }
}
