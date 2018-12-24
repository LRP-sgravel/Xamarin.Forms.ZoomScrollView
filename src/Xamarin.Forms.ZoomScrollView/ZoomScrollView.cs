using System;

namespace Xamarin.Forms.ZoomScrollView
{
    public class ZoomScrollView : ScrollView
    {
        public static readonly BindableProperty MinimumZoomScaleProperty = BindableProperty.Create(nameof(MinimumZoomScale), typeof(float), typeof(ZoomScrollView), 1.0f);
        public static readonly BindableProperty MaximumZoomScaleProperty = BindableProperty.Create(nameof(MaximumZoomScale), typeof(float), typeof(ZoomScrollView), 1.0f);

        PinchGestureRecognizer pinch = new PinchGestureRecognizer();

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

        public ZoomScrollView() : base()
        {
            pinch.PinchUpdated += HandlePinchZoom;
        }

        protected override void OnPropertyChanging(string propertyName = null)
        {
            base.OnPropertyChanging(propertyName);

            if (propertyName == nameof(Content))
            {
                Content?.GestureRecognizers.Remove(pinch);
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(Content))
            {
                //Content?.GestureRecognizers.Add(pinch);
            }
        }

        private void HandlePinchZoom(object sender, PinchGestureUpdatedEventArgs e)
        {
            Console.WriteLine($"New zoom = {e.Scale} | ContentSize = {ContentSize}");
            Content.Scale += (e.Scale - 1);
        }
    }
}
