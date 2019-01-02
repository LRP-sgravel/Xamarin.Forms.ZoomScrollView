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
using System;
using Android.Content;
using Com.Otaliastudios.Zoom;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.ZoomScrollView;
using Xamarin.Forms.ZoomScrollView.Platforms.Android;
using AndroidView = Android.Views.View;
using FrameLayout = Android.Widget.FrameLayout;
using FormsPlatform = Xamarin.Forms.Platform.Android.Platform;

[assembly: ExportRenderer(typeof(ZoomScrollView), typeof(ZoomScrollViewRenderer))]
namespace Xamarin.Forms.ZoomScrollView.Platforms.Android
{
    public class ZoomScrollViewRenderer : ViewRenderer<ZoomScrollView, ZoomLayout>
    {
        private ZoomLayout _ZoomLayout { get; set; }
        private AndroidView _content;
        private FrameLayout _container;

        private ZoomScrollView _ZoomScrollView => Element as ZoomScrollView;

        public ZoomScrollViewRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ZoomScrollView> args)
        {
            base.OnElementChanged(args);

            _ZoomLayout = GetOrCreateZoomLayout();
            _ZoomLayout.SetTransformation(ZoomLayout.InterfaceConsts.TransformationNone, 0);
            _ZoomLayout.RemoveAllViews();
            SetNativeControl(_ZoomLayout);

            _content = CreateScrollViewContent();
            if(_content != null)
            {
                _container = new FrameLayout(Context);
                _container.AddView(_content);
                _ZoomLayout.AddView(_container);

                _ZoomScrollView.Content.PropertyChanged += OnContentPropertyChanged;
			}

            UpdateMinMaxScale();
            UpdateScrollbars();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

            if (args.PropertyName == nameof(ZoomScrollView.MinimumZoomScale) ||
                args.PropertyName == nameof(ZoomScrollView.MaximumZoomScale))
            {
                UpdateMinMaxScale();
            }
        }

        private void OnContentPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if ((args.PropertyName == nameof(View.Width) ||
                 args.PropertyName == nameof(View.Height)) &&
                _ZoomScrollView.Content.Width > 0 &&
                _ZoomScrollView.Content.Height > 0)
            {
                double density = Resources.DisplayMetrics.Density;
                double availableWidth = _ZoomScrollView.Content.Width * density;
                double availableHeight = _ZoomScrollView.Content.Height * density;
                SizeRequest size = _ZoomScrollView.Content.Measure(availableWidth, availableHeight, MeasureFlags.None);
                double measuredWidth = size.Request.Width * density;
                double measuredHeight = size.Request.Height * density;
                double effectiveWidth = Math.Max(measuredWidth, availableWidth);
                double effectiveHeight = Math.Max(measuredHeight, availableHeight);

                _content.LayoutParameters = new FrameLayout.LayoutParams((int)measuredWidth, (int)measuredHeight);
                _container.LayoutParameters = new FrameLayout.LayoutParams((int)effectiveWidth, (int)effectiveHeight);
            }
        }

        private ZoomLayout GetOrCreateZoomLayout()
        {
            return GetChildOfType<ZoomLayout>() ?? new ZoomLayout(Context);
        }

        private TView GetChildOfType<TView>() where TView : AndroidView
        {
            for(int i = 0; i < ChildCount; ++i)
            {
                AndroidView child = GetChildAt(i);

                if(child is TView)
                {
                    return child as TView;
                }
            }

            return null;
        }

        private AndroidView CreateScrollViewContent()
        {
            View content = _ZoomScrollView.Content;

            if(content != null)
            {
                IVisualElementRenderer renderer = FormsPlatform.GetRenderer(content);

                if (renderer == null)
                {
                    renderer = FormsPlatform.CreateRendererWithContext(content, Context);
                    FormsPlatform.SetRenderer(content, renderer);
                }

                if(renderer.View.Parent != null)
                {
                    renderer.View.RemoveFromParent();
                }

                return renderer.View;
            }

            return null;
        }

        private void UpdateMinMaxScale()
        {
            if (_ZoomLayout != null && _ZoomScrollView != null)
            {
                _ZoomLayout.SetMinZoom(_ZoomScrollView.MinimumZoomScale, ZoomLayout.InterfaceConsts.TypeZoom);
                _ZoomLayout.SetMaxZoom(_ZoomScrollView.MaximumZoomScale, ZoomLayout.InterfaceConsts.TypeZoom);
            }
        }

        private void UpdateScrollbars()
        {
            if (_ZoomLayout != null && _ZoomScrollView != null)
            {
                _ZoomLayout.SetHorizontalPanEnabled(_ZoomScrollView.Orientation == ScrollOrientation.Horizontal ||
                    _ZoomScrollView.Orientation == ScrollOrientation.Both);
                _ZoomLayout.SetVerticalPanEnabled(_ZoomScrollView.Orientation == ScrollOrientation.Vertical ||
                    _ZoomScrollView.Orientation == ScrollOrientation.Both);
            }
        }

        protected override void UpdateBackgroundColor()
        {
            _ZoomLayout.SetBackgroundColor(Element.BackgroundColor.ToAndroid(Color.Transparent));
        }
    }
}
