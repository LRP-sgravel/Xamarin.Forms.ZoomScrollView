﻿// **********************************************************************
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
using Android.Views;
using Com.Otaliastudios.Zoom;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.ZoomScrollView;
using Xamarin.Forms.ZoomScrollView.Platforms.Android;
using AndroidView = Android.Views.View;
using FormsPlatform = Xamarin.Forms.Platform.Android.Platform;

[assembly: ExportRenderer(typeof(ZoomScrollView), typeof(ZoomScrollViewRenderer))]
namespace Xamarin.Forms.ZoomScrollView.Platforms.Android
{
    public class ZoomScrollViewRenderer : ViewRenderer<ZoomScrollView, ZoomLayout>
    {
        private ZoomLayout _zoomLayout;
        private AndroidView _content;
        private VisualElementTracker _contentTracker;

        private ZoomScrollView _ZoomScrollView => Element as ZoomScrollView;

        public ZoomScrollViewRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ZoomScrollView> args)
        {
            base.OnElementChanged(args);

            _zoomLayout = GetOrCreateZoomLayout();
            _zoomLayout.SetTransformation(ZoomLayout.InterfaceConsts.TransformationNone, (int)(GravityFlags.Top | GravityFlags.Left));
            _zoomLayout.SetSmallerPolicy(ZoomLayout.InterfaceConsts.SmallerPolicyFromTransformation);
            SetNativeControl(_zoomLayout);

            _content = CreateScrollViewContent();
            if(_content != null)
            {
                _zoomLayout.AddView(_content);
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

        private ZoomLayout GetOrCreateZoomLayout()
        {
            ZoomLayout result = GetChildOfType<ZoomLayout>() ?? new ZoomLayout(Context);

            result.RemoveAllViews();

            return result;
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

                _contentTracker = new VisualElementTracker(renderer);
                _contentTracker.UpdateLayout();

                return renderer.View;
            }

            return null;
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            _contentTracker?.UpdateLayout();
        }

        private void UpdateMinMaxScale()
        {
            if (_zoomLayout != null && _ZoomScrollView != null)
            {
                _zoomLayout.SetMinZoom(_ZoomScrollView.MinimumZoomScale, ZoomLayout.InterfaceConsts.TypeZoom);
                _zoomLayout.SetMaxZoom(_ZoomScrollView.MaximumZoomScale, ZoomLayout.InterfaceConsts.TypeZoom);
            }
        }

        private void UpdateScrollbars()
        {
            if (_zoomLayout != null && _ZoomScrollView != null)
            {
                _zoomLayout.SetHorizontalPanEnabled(_ZoomScrollView.Orientation == ScrollOrientation.Horizontal ||
                    _ZoomScrollView.Orientation == ScrollOrientation.Both);
                _zoomLayout.SetVerticalPanEnabled(_ZoomScrollView.Orientation == ScrollOrientation.Vertical ||
                    _ZoomScrollView.Orientation == ScrollOrientation.Both);
            }
        }
    }
}
