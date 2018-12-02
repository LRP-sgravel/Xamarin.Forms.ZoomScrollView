using Xamarin.Forms;

namespace ZoomScrollSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void PinchGestureRecognizer_OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            Scroller.Content.Scale += (e.Scale - 1);
        }
    }
}
