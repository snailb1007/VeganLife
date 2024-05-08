using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace VeganLife.Views.Controls
{
    public class RatingControl : SKCanvasView
    {
        private const string Star = "M9 11.3l3.71 2.7-1.42-4.36L15 7h-4.55L9 2.5 7.55 7H3l3.71 2.64L5.29 14z";

        private float _itemWidth;
        private float _itemHeight;
        private float _canvasScale;
        private SKColor _sKColorOn = SKColor.Parse("#F09235");
        private SKColor _sKOutlineOnColor = SKColors.Transparent;
        private SKColor _sKOutlineOffColor = SKColors.Gray;
        private SKColor _sKOutlineOffWhiteColor = SKColors.White;

        public RatingControl()
        {
            this.BackgroundColor = Colors.Transparent;
            this.PaintSurface += Handle_PaintSurface;
        }

        public SKColor CanvasBackgroundColor { get; set; } = SKColors.Transparent;

        public float StrokeWidth { get; set; } = 0.1f;

        public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(RatingControl), default(double), propertyChanged: OnValueChanged);
        public static readonly BindableProperty PathProperty = BindableProperty.Create(nameof(Path), typeof(string), typeof(RatingControl), Star);
        public static readonly BindableProperty CountProperty = BindableProperty.Create(nameof(Count), typeof(int), typeof(RatingControl), 5);
        public static readonly BindableProperty ColorOnProperty = BindableProperty.Create(nameof(ColorOn), typeof(Color), typeof(RatingControl), Colors.Yellow, propertyChanged: ColorOnChanged);
        public static readonly BindableProperty OutlineOnColorProperty = BindableProperty.Create(nameof(OutlineOnColor), typeof(Color), typeof(RatingControl), Colors.Gray);
        public static readonly BindableProperty OutlineOffColorProperty = BindableProperty.Create(nameof(OutlineOffColor), typeof(Color), typeof(RatingControl), Colors.Gray);
        public static readonly BindableProperty RatingTypeProperty = BindableProperty.Create(nameof(RatingFillType), typeof(RatingType), typeof(RatingControl), RatingType.Floating, propertyChanged: OnPropertyChanged);
        public static readonly BindableProperty SpacingProperty = BindableProperty.Create(nameof(Spacing), typeof(int), typeof(RatingControl), 5, propertyChanged: OnPropertyChanged);
        public static readonly BindableProperty IsWhiteStarProperty = BindableProperty.Create(nameof(IsWhiteStar), typeof(bool), typeof(RatingControl), false);

        public int Spacing
        {
            get => (int)GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, this.ClampValue(value)); }
        }

        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }

        public Color ColorOn
        {
            get { return (Color)GetValue(ColorOnProperty); }
            set { SetValue(ColorOnProperty, value); }
        }

        public Color OutlineOnColor
        {
            get { return (Color)GetValue(OutlineOnColorProperty); }
            set { SetValue(OutlineOnColorProperty, value); }
        }

        public Color OutlineOffColor
        {
            get { return (Color)GetValue(OutlineOffColorProperty); }
            set { SetValue(OutlineOffColorProperty, value); }
        }

        public RatingType RatingFillType
        {
            get { return (RatingType)GetValue(RatingTypeProperty); }
            set { SetValue(RatingTypeProperty, value); }
        }

        public enum RatingType
        {
            Full,
            Half,
            Floating,
        }

        public bool IsWhiteStar
        {
            get { return (bool)GetValue(IsWhiteStarProperty); }
            set { SetValue(ValueProperty, IsWhiteStarProperty); }
        }

        private void Handle_PaintSurface(object? sender, SKPaintSurfaceEventArgs e)
        {
            this.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
        }

        private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as RatingControl;
            view?.InvalidateSurface();
        }

        private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as RatingControl;
            if (view is null)
            {
                return;
            }

            view.Value = view.ClampValue((double)newValue);
            OnPropertyChanged(bindable, oldValue, newValue);
        }

        private static void ColorOnChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as RatingControl;
            if (view is null)
            {
                return;
            }

            view._sKColorOn = ((Color)newValue).ToSKColor();
            OnPropertyChanged(bindable, oldValue, newValue);
        }

        private double CalculateValue(double x)
        {
            if (x < this._itemWidth)
            {
                return (double)x / this._itemWidth;
            }
            else if (x < this._itemWidth + this.Spacing)
            {
                return 1;
            }
            else
            {
                return 1 + CalculateValue(x - (this._itemWidth + this.Spacing));
            }
        }

        public double ClampValue(double val)
        {
            if (val < 0)
            {
                return 0;
            }
            else if (val > this.Count)
            {
                return this.Count;
            }
            else
            {
                return val;
            }
        }

        public void SetValue(double x, double y)
        {
            var val = this.CalculateValue(x);
            switch (this.RatingFillType)
            {
                case RatingType.Full:
                    this.Value = ClampValue((double)Math.Ceiling(val));
                    break;
                case RatingType.Half:
                    this.Value = ClampValue((double)Math.Round(val * 2) / 2);
                    break;
                case RatingType.Floating:
                    this.Value = ClampValue(val);
                    break;
            }
        }

        public void Draw(SKCanvas canvas, int width, int height)
        {
            canvas.Clear(this.CanvasBackgroundColor);

            var path = SKPath.ParseSvgPathData(this.Path);

            var itemWidth = (width - ((this.Count - 1) * this.Spacing)) / this.Count;
            var scaleX = itemWidth / path.Bounds.Width;
            scaleX = (itemWidth - (scaleX * this.StrokeWidth)) / path.Bounds.Width;

            this._itemHeight = height;
            var scaleY = this._itemHeight / path.Bounds.Height;
            scaleY = (this._itemHeight - (scaleY * this.StrokeWidth)) / path.Bounds.Height;

            this._canvasScale = Math.Min(scaleX, scaleY);
            this._itemWidth = path.Bounds.Width * this._canvasScale;

            canvas.Scale(this._canvasScale);
            canvas.Translate(this.StrokeWidth / 2, this.StrokeWidth / 2);
            canvas.Translate(-path.Bounds.Left, 0);
            canvas.Translate(0, -path.Bounds.Top);

            var strokeFillPaintConfig = new SKPaint();
            var fillPaintConfig = new SKPaint();

            if (this.IsWhiteStar)
            {
                strokeFillPaintConfig.Style = SKPaintStyle.Stroke;
                strokeFillPaintConfig.Color = this._sKOutlineOffColor;
                strokeFillPaintConfig.StrokeWidth = 0.5f;
                strokeFillPaintConfig.StrokeJoin = SKStrokeJoin.Round;
                strokeFillPaintConfig.IsAntialias = true;

                fillPaintConfig.Style = SKPaintStyle.Fill;
                fillPaintConfig.Color = this._sKColorOn;
                fillPaintConfig.IsAntialias = true;
            }
            else
            {
                strokeFillPaintConfig.Style = SKPaintStyle.StrokeAndFill;
                strokeFillPaintConfig.Color = this._sKOutlineOnColor;
                strokeFillPaintConfig.StrokeWidth = this.StrokeWidth;
                strokeFillPaintConfig.StrokeJoin = SKStrokeJoin.Round;
                strokeFillPaintConfig.IsAntialias = true;

                fillPaintConfig.Style = SKPaintStyle.Fill;
                fillPaintConfig.Color = this._sKColorOn;
                fillPaintConfig.IsAntialias = true;
            }

            using (var strokeFillPaint = strokeFillPaintConfig)
            using (var fillPaint = fillPaintConfig)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    // Full
                    if (i <= this.Value - 1)
                    {
                        canvas.DrawPath(path, fillPaint);
                        canvas.DrawPath(path, strokeFillPaint);
                    }

                    // Partial
                    else if (i < this.Value)
                    {
                        float filledPercentage = (float)(this.Value - Math.Truncate(this.Value));
                        if (this.IsWhiteStar)
                        {
                            fillPaint.Color = this._sKOutlineOffWhiteColor;
                            canvas.DrawPath(path, fillPaint);
                        }

                        strokeFillPaint.Color = this._sKOutlineOffColor;
                        canvas.DrawPath(path, strokeFillPaint);

                        using (var rectPath = new SKPath())
                        {
                            var rect = SKRect.Create(path.Bounds.Left + (path.Bounds.Width * filledPercentage), path.Bounds.Top, path.Bounds.Width * (1 - filledPercentage), this._itemHeight);
                            rectPath.AddRect(rect);
                            canvas.ClipPath(rectPath, SKClipOperation.Difference);
                            canvas.DrawPath(path, fillPaint);
                        }
                    }

                    // Empty
                    else
                    {
                        if (this.IsWhiteStar)
                        {
                            fillPaint.Color = this._sKOutlineOffWhiteColor;
                            canvas.DrawPath(path, fillPaint);
                        }

                        strokeFillPaint.Color = this._sKOutlineOffColor;
                        canvas.DrawPath(path, strokeFillPaint);
                    }

                    canvas.Translate((this._itemWidth + this.Spacing) / this._canvasScale, 0);
                }
            }
        }
    }
}
