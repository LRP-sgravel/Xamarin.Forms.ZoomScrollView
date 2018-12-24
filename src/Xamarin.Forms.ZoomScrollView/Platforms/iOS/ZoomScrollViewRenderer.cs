// **********************************************************************
// 
//   ZoomScrollViewRenderer.cs
//   
//   This file is subject to the terms and conditions defined in
//   file 'LICENSE.txt', which is part of this source code package.
//   
//   Copyright (c) 2018, Sylvain Gravel
// 
// ***********************************************************************

using System.ComponentModel;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.ZoomScrollView;
using Xamarin.Forms.ZoomScrollView.Platforms.iOS;

[assembly: ExportRenderer(typeof(ZoomScrollView), typeof(ZoomScrollViewRenderer))]
namespace Xamarin.Forms.ZoomScrollView.Platforms.iOS
{
    public class ZoomScrollViewRenderer : ScrollViewRenderer
    {
        private ZoomScrollView _ZoomScrollView => Element as ZoomScrollView;

        protected override void OnElementChanged(VisualElementChangedEventArgs args)
        {
            if (args.OldElement != null)
            {
                args.OldElement.PropertyChanged -= OnElementPropertyChanged;
            }

            base.OnElementChanged(args);

            if (args.NewElement != null)
            {
                args.NewElement.PropertyChanged += OnElementPropertyChanged;
            }

            ViewForZoomingInScrollView = GetZoomSubView;
            UpdateMinMaxScale();
        }

        private void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(MinimumZoomScale) ||
                args.PropertyName == nameof(MaximumZoomScale))
            {
                UpdateMinMaxScale();
            }
        }

        private UIView GetZoomSubView(UIScrollView scrollView)
        {
            return scrollView.Subviews?.FirstOrDefault();
        }

        private void UpdateMinMaxScale()
        {
            if (_ZoomScrollView != null)
            {
                MinimumZoomScale = _ZoomScrollView.MinimumZoomScale;
                MaximumZoomScale = _ZoomScrollView.MaximumZoomScale;
            }
        }
    }
}
