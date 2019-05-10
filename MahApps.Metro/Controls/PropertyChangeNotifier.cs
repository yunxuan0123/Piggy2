using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Data;

namespace MahApps.Metro.Controls
{
	internal sealed class PropertyChangeNotifier : DependencyObject, IDisposable
	{
		private WeakReference _propertySource;

		public readonly static DependencyProperty ValueProperty;

		public DependencyObject PropertySource
		{
			get
			{
				DependencyObject dependencyObject;
				DependencyObject target;
				try
				{
					if (this._propertySource.IsAlive)
					{
						target = this._propertySource.Target as DependencyObject;
					}
					else
					{
						target = null;
					}
					dependencyObject = target;
				}
				catch
				{
					dependencyObject = null;
				}
				return dependencyObject;
			}
		}

		[Bindable(true)]
		[Category("Behavior")]
		[Description("Returns/sets the value of the property")]
		public object Value
		{
			get
			{
				return base.GetValue(PropertyChangeNotifier.ValueProperty);
			}
			set
			{
				base.SetValue(PropertyChangeNotifier.ValueProperty, value);
			}
		}

		static PropertyChangeNotifier()
		{
			Class6.yDnXvgqzyB5jw();
			PropertyChangeNotifier.ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(PropertyChangeNotifier), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(PropertyChangeNotifier.OnPropertyChanged)));
		}

		public PropertyChangeNotifier(DependencyObject propertySource, string path)
		{
			Class6.yDnXvgqzyB5jw();
			this(propertySource, new PropertyPath(path, new object[0]));
		}

		public PropertyChangeNotifier(DependencyObject propertySource, DependencyProperty property)
		{
			Class6.yDnXvgqzyB5jw();
			this(propertySource, new PropertyPath(property));
		}

		public PropertyChangeNotifier(DependencyObject propertySource, PropertyPath property)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			if (propertySource == null)
			{
				throw new ArgumentNullException("propertySource");
			}
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			this._propertySource = new WeakReference(propertySource);
			Binding binding = new Binding()
			{
				Path = property,
				Mode = BindingMode.OneWay,
				Source = propertySource
			};
			BindingOperations.SetBinding(this, PropertyChangeNotifier.ValueProperty, binding);
		}

		public void Dispose()
		{
			BindingOperations.ClearBinding(this, PropertyChangeNotifier.ValueProperty);
		}

		private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PropertyChangeNotifier propertyChangeNotifier = (PropertyChangeNotifier)d;
			if (propertyChangeNotifier.ValueChanged != null)
			{
				propertyChangeNotifier.ValueChanged(propertyChangeNotifier.PropertySource, EventArgs.Empty);
			}
		}

		public event EventHandler ValueChanged;
	}
}