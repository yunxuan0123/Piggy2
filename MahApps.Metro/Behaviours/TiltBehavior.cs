using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace MahApps.Metro.Behaviours
{
	public class TiltBehavior : Behavior<FrameworkElement>
	{
		public readonly static DependencyProperty KeepDraggingProperty;

		public readonly static DependencyProperty TiltFactorProperty;

		private bool isPressed;

		private Thickness originalMargin;

		private Panel originalPanel;

		private Size originalSize;

		private FrameworkElement attachedElement;

		private Point current;

		private int times;

		public bool KeepDragging
		{
			get
			{
				return (bool)base.GetValue(TiltBehavior.KeepDraggingProperty);
			}
			set
			{
				base.SetValue(TiltBehavior.KeepDraggingProperty, value);
			}
		}

		public Planerator RotatorParent
		{
			get;
			set;
		}

		public int TiltFactor
		{
			get
			{
				return (int)base.GetValue(TiltBehavior.TiltFactorProperty);
			}
			set
			{
				base.SetValue(TiltBehavior.TiltFactorProperty, value);
			}
		}

		static TiltBehavior()
		{
			Class6.yDnXvgqzyB5jw();
			TiltBehavior.KeepDraggingProperty = DependencyProperty.Register("KeepDragging", typeof(bool), typeof(TiltBehavior), new PropertyMetadata(true));
			TiltBehavior.TiltFactorProperty = DependencyProperty.Register("TiltFactor", typeof(int), typeof(TiltBehavior), new PropertyMetadata((object)20));
		}

		public TiltBehavior()
		{
			Class6.yDnXvgqzyB5jw();
			this.current = new Point(-99, -99);
			this.times = -1;
			base();
		}

		private void CompositionTargetRendering(object sender, EventArgs e)
		{
			if (this.KeepDragging)
			{
				this.current = Mouse.GetPosition(this.RotatorParent.Child);
				if (Mouse.LeftButton != MouseButtonState.Pressed)
				{
					this.RotatorParent.RotationY = (this.RotatorParent.RotationY - 5 < 0 ? 0 : this.RotatorParent.RotationY - 5);
					this.RotatorParent.RotationX = (this.RotatorParent.RotationX - 5 < 0 ? 0 : this.RotatorParent.RotationX - 5);
					return;
				}
				if (this.current.X > 0 && this.current.X < this.attachedElement.ActualWidth && this.current.Y > 0 && this.current.Y < this.attachedElement.ActualHeight)
				{
					this.RotatorParent.RotationY = (double)(-1 * this.TiltFactor) + this.current.X * 2 * (double)this.TiltFactor / this.attachedElement.ActualWidth;
					this.RotatorParent.RotationX = (double)(-1 * this.TiltFactor) + this.current.Y * 2 * (double)this.TiltFactor / this.attachedElement.ActualHeight;
					return;
				}
			}
			else if (Mouse.LeftButton != MouseButtonState.Pressed)
			{
				this.isPressed = false;
				this.times = -1;
				this.RotatorParent.RotationY = (this.RotatorParent.RotationY - 5 < 0 ? 0 : this.RotatorParent.RotationY - 5);
				this.RotatorParent.RotationX = (this.RotatorParent.RotationX - 5 < 0 ? 0 : this.RotatorParent.RotationX - 5);
			}
			else
			{
				if (!this.isPressed)
				{
					this.current = Mouse.GetPosition(this.RotatorParent.Child);
					if (this.current.X > 0 && this.current.X < this.attachedElement.ActualWidth && this.current.Y > 0 && this.current.Y < this.attachedElement.ActualHeight)
					{
						this.RotatorParent.RotationY = (double)(-1 * this.TiltFactor) + this.current.X * 2 * (double)this.TiltFactor / this.attachedElement.ActualWidth;
						this.RotatorParent.RotationX = (double)(-1 * this.TiltFactor) + this.current.Y * 2 * (double)this.TiltFactor / this.attachedElement.ActualHeight;
					}
					this.isPressed = true;
				}
				if (this.isPressed && this.times == 7)
				{
					this.RotatorParent.RotationY = (this.RotatorParent.RotationY - 5 < 0 ? 0 : this.RotatorParent.RotationY - 5);
					this.RotatorParent.RotationX = (this.RotatorParent.RotationX - 5 < 0 ? 0 : this.RotatorParent.RotationX - 5);
					return;
				}
				if (this.isPressed && this.times < 7)
				{
					this.times++;
					return;
				}
			}
		}

		private static Panel GetParentPanel(DependencyObject element)
		{
			DependencyObject parent = VisualTreeHelper.GetParent(element);
			object obj = parent as Panel;
			if (obj == null)
			{
				if (parent != null)
				{
					return TiltBehavior.GetParentPanel(parent);
				}
				obj = null;
			}
			return obj;
		}

		protected override void OnAttached()
		{
			this.attachedElement = base.AssociatedObject;
			if (this.attachedElement is ListBox)
			{
				return;
			}
			Panel panel = this.attachedElement as Panel;
			if (panel != null)
			{
				panel.Loaded += new RoutedEventHandler((object sender, RoutedEventArgs e) => panel.Children.Cast<UIElement>().ToList<UIElement>().ForEach((UIElement element) => Interaction.GetBehaviors(element).Add(new TiltBehavior()
				{
					KeepDragging = this.KeepDragging,
					TiltFactor = this.TiltFactor
				})));
				return;
			}
			this.originalPanel = this.attachedElement.Parent as Panel ?? TiltBehavior.GetParentPanel(this.attachedElement);
			this.originalMargin = this.attachedElement.Margin;
			this.originalSize = new Size(this.attachedElement.Width, this.attachedElement.Height);
			double left = Canvas.GetLeft(this.attachedElement);
			double right = Canvas.GetRight(this.attachedElement);
			double top = Canvas.GetTop(this.attachedElement);
			double bottom = Canvas.GetBottom(this.attachedElement);
			int zIndex = Panel.GetZIndex(this.attachedElement);
			VerticalAlignment verticalAlignment = this.attachedElement.VerticalAlignment;
			HorizontalAlignment horizontalAlignment = this.attachedElement.HorizontalAlignment;
			this.RotatorParent = new Planerator()
			{
				Margin = this.originalMargin,
				Width = this.originalSize.Width,
				Height = this.originalSize.Height,
				VerticalAlignment = verticalAlignment,
				HorizontalAlignment = horizontalAlignment
			};
			this.RotatorParent.SetValue(Canvas.LeftProperty, left);
			this.RotatorParent.SetValue(Canvas.RightProperty, right);
			this.RotatorParent.SetValue(Canvas.TopProperty, top);
			this.RotatorParent.SetValue(Canvas.BottomProperty, bottom);
			this.RotatorParent.SetValue(Panel.ZIndexProperty, zIndex);
			this.originalPanel.Children.Remove(this.attachedElement);
			this.attachedElement.Margin = new Thickness();
			this.attachedElement.Width = double.NaN;
			this.attachedElement.Height = double.NaN;
			this.originalPanel.Children.Add(this.RotatorParent);
			this.RotatorParent.Child = this.attachedElement;
			CompositionTarget.Rendering += new EventHandler(this.CompositionTargetRendering);
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			CompositionTarget.Rendering -= new EventHandler(this.CompositionTargetRendering);
		}
	}
}