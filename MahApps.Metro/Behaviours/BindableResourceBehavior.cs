using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Shapes;

namespace MahApps.Metro.Behaviours
{
	public class BindableResourceBehavior : Behavior<Shape>
	{
		public readonly static DependencyProperty ResourceNameProperty;

		public readonly static DependencyProperty PropertyProperty;

		public DependencyProperty Property
		{
			get
			{
				return (DependencyProperty)base.GetValue(BindableResourceBehavior.PropertyProperty);
			}
			set
			{
				base.SetValue(BindableResourceBehavior.PropertyProperty, value);
			}
		}

		public string ResourceName
		{
			get
			{
				return (string)base.GetValue(BindableResourceBehavior.ResourceNameProperty);
			}
			set
			{
				base.SetValue(BindableResourceBehavior.ResourceNameProperty, value);
			}
		}

		static BindableResourceBehavior()
		{
			Class6.yDnXvgqzyB5jw();
			BindableResourceBehavior.ResourceNameProperty = DependencyProperty.Register("ResourceName", typeof(string), typeof(BindableResourceBehavior), new PropertyMetadata(null));
			BindableResourceBehavior.PropertyProperty = DependencyProperty.Register("Property", typeof(DependencyProperty), typeof(BindableResourceBehavior), new PropertyMetadata(null));
		}

		public BindableResourceBehavior()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		protected override void OnAttached()
		{
			base.AssociatedObject.SetResourceReference(this.Property, this.ResourceName);
			base.OnAttached();
		}
	}
}