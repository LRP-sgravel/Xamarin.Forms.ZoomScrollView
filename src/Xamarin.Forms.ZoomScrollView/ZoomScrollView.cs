namespace Xamarin.Forms.ZoomScrollView
{
    public class ZoomScrollView : ScrollView
    {
        public static readonly BindableProperty MinimumZoomScaleProperty = BindableProperty.Create(nameof(MinimumZoomScale), typeof(float), typeof(ZoomScrollView), 1.0f);
        public static readonly BindableProperty MaximumZoomScaleProperty = BindableProperty.Create(nameof(MaximumZoomScale), typeof(float), typeof(ZoomScrollView), 1.0f);

        public float MinimumZoomScale
        {
            get => (float)GetValue(MinimumZoomScaleProperty);
            set => SetValue(MinimumZoomScaleProperty, value);
        }

        public float MaximumZoomScale
        {
            get => (float)GetValue(MaximumZoomScaleProperty);
            set => SetValue(MaximumZoomScaleProperty, value);
        }

        public ZoomScrollView()
        {
        }
    }
}
