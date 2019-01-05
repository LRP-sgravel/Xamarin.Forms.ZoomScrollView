using System;
using Xamarin.Forms;

namespace ZoomScrollSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnImageButtonClicked(object sender, EventArgs e)
        {
            this.ImageScrollView.IsVisible = true;
            this.LabelScrollView.IsVisible = false;
            this.StackScrollView.IsVisible = false;
        }

        private void OnLabelButtonClicked(object sender, EventArgs e)
        {
            this.ImageScrollView.IsVisible = false;
            this.LabelScrollView.IsVisible = true;
            this.StackScrollView.IsVisible = false;
        }

        private void OnStackButtonClicked(object sender, EventArgs e)
        {
            this.ImageScrollView.IsVisible = false;
            this.LabelScrollView.IsVisible = false;
            this.StackScrollView.IsVisible = true;
        }

        private void OnScrollToTop(object sender, EventArgs e)
        {
            this.StackScrollView.ScrollToAsync(0, 0, true);
        }
    }
}
