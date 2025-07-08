using System;
using System.Collections.Generic;
using System.Text;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Widget;
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using Microsoft.Maui.Graphics;
using AView = Android.Views.View;
using AWebView = Android.Webkit.WebView;

namespace Microsoft.Maui.Platform
{
	public class MauiSwipeRefreshLayout : SwipeRefreshLayout, ICrossPlatformLayoutBacking
	{
		AView? _contentView;

		public MauiSwipeRefreshLayout(Context context) : base(context)
		{
			// This works around a bug in SwipeRefreshLayout
			// https://github.com/dotnet/maui/pull/17647#discussion_r1433358418
			// https://issuetracker.google.com/issues/110463864
			// It looks like this issue is fixed on the main branch of Android but it hasn't made its way into the packages yet
			SetProgressViewOffset(true, ProgressViewStartOffset, ProgressViewEndOffset - Math.Abs(ProgressViewStartOffset));
		}

#pragma warning disable RS0016 // Add public types and members to the declared API
		public ICrossPlatformLayout? CrossPlatformLayout { get; set; }
#pragma warning restore RS0016 // Add public types and members to the declared API

		Graphics.Size CrossPlatformMeasure(double widthConstraint, double heightConstraint)
		{
			return CrossPlatformLayout?.CrossPlatformMeasure(widthConstraint, heightConstraint) ?? Graphics.Size.Zero;
		}

		Graphics.Size CrossPlatformArrange(Graphics.Rect bounds)
		{
			return CrossPlatformLayout?.CrossPlatformArrange(bounds) ?? Graphics.Size.Zero;
		}

		public void UpdateContent(IView? content, IMauiContext? mauiContext)
		{
			_contentView?.RemoveFromParent();

			if (content != null && mauiContext != null)
			{
				_contentView = content.ToPlatform(mauiContext);
				var layoutParams = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
				AddView(_contentView, layoutParams);
			}
		}

		internal ImageView? CircleImageView
		{
			get
			{
				for (int i = 0; i < ChildCount; i++)
				{
					var child = GetChildAt(i);

					if (child is ImageView iv && child != _contentView)
						return iv;
				}

				return null;
			}
		}

#pragma warning disable RS0016 // Add public types and members to the declared API
		public override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
#pragma warning restore RS0016 // Add public types and members to the declared API
		{
			if (CrossPlatformMeasure == null)
			{
				base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
				return;
			}

			var context = Context;
			if (context == null)
			{
				base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
				return;
			}

			var deviceIndependentWidth = widthMeasureSpec.ToDouble(context);
			var deviceIndependentHeight = heightMeasureSpec.ToDouble(context);

			var widthMode = MeasureSpec.GetMode(widthMeasureSpec);
			var heightMode = MeasureSpec.GetMode(heightMeasureSpec);

			var measure = CrossPlatformMeasure(deviceIndependentWidth, deviceIndependentHeight);

			// If the measure spec was exact, we should return the explicit size value, even if the content
			// measure came out to a different size
			var width = widthMode == MeasureSpecMode.Exactly ? deviceIndependentWidth : measure.Width;
			var height = heightMode == MeasureSpecMode.Exactly ? deviceIndependentHeight : measure.Height;

			var platformWidth = context.ToPixels(width);
			var platformHeight = context.ToPixels(height);

			// Minimum values win over everything
			platformWidth = Math.Max(MinimumWidth, platformWidth);
			platformHeight = Math.Max(MinimumHeight, platformHeight);

			SetMeasuredDimension((int)platformWidth, (int)platformHeight);
		}

#pragma warning disable RS0016 // Add public types and members to the declared API
		protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
#pragma warning restore RS0016 // Add public types and members to the declared API
		{
			if (CrossPlatformArrange == null)
			{
				base.OnLayout(changed, left, top, right, bottom);
				return;
			}

			var context = Context;
			if (context == null)
			{
				base.OnLayout(changed, left, top, right, bottom);
				return;
			}

			var destination = context.ToCrossPlatformRectInReferenceFrame(left, top, right, bottom);
			CrossPlatformArrange(destination);
		}

		public override bool CanChildScrollUp()
		{
			if (ChildCount == 0)
				return base.CanChildScrollUp();

			return CanScrollUp(_contentView);
		}

		bool CanScrollUp(AView? view)
		{
			if (!(view is ViewGroup viewGroup))
				return base.CanChildScrollUp();

			if (!CanScrollUpViewByType(view))
				return false;

			for (int i = 0; i < viewGroup.ChildCount; i++)
			{
				var child = viewGroup.GetChildAt(i);

				if (!CanScrollUpViewByType(child))
					return false;

				if (child is SwipeRefreshLayout)
				{
					return CanScrollUp(child as ViewGroup);
				}
			}

			return true;
		}

		static bool CanScrollUpViewByType(AView? view)
		{
			if (view is AbsListView absListView)
			{
				if (absListView.FirstVisiblePosition == 0)
				{
					var subChild = absListView.GetChildAt(0);

					return subChild != null && subChild.Top != 0;
				}

				return true;
			}

			if (view is RecyclerView recyclerView)
				return recyclerView.ComputeVerticalScrollOffset() > 0;

#pragma warning disable XAOBS001 // Obsolete
			if (view is NestedScrollView nestedScrollView)
				return nestedScrollView.ComputeVerticalScrollOffset() > 0;
#pragma warning restore XAOBS001 // Obsolete

			if (view is AWebView webView)
				return webView.ScrollY > 0;

			return true;
		}
	}
}
