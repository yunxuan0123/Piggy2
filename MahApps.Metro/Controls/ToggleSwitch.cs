using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name="Switch", Type=typeof(ToggleButton))]
	[TemplateVisualState(Name="Disabled", GroupName="CommonStates")]
	[TemplateVisualState(Name="Normal", GroupName="CommonStates")]
	public class ToggleSwitch : ContentControl
	{
		private ToggleButton _toggleButton;

		public readonly static DependencyProperty OnLabelProperty;

		public readonly static DependencyProperty OffLabelProperty;

		public readonly static DependencyProperty HeaderProperty;

		public readonly static DependencyProperty HeaderTemplateProperty;

		[Obsolete("This property will be deleted in the next release. You should use OnSwitchBrush and OffSwitchBrush to change the switch's brushes.")]
		public readonly static DependencyProperty SwitchForegroundProperty;

		public readonly static DependencyProperty OnSwitchBrushProperty;

		public readonly static DependencyProperty OffSwitchBrushProperty;

		public readonly static DependencyProperty ThumbIndicatorBrushProperty;

		public readonly static DependencyProperty ThumbIndicatorDisabledBrushProperty;

		public readonly static DependencyProperty ThumbIndicatorWidthProperty;

		public readonly static DependencyProperty IsCheckedProperty;

		public readonly static DependencyProperty CheckChangedCommandProperty;

		public readonly static DependencyProperty CheckedCommandProperty;

		public readonly static DependencyProperty UnCheckedCommandProperty;

		public readonly static DependencyProperty CheckChangedCommandParameterProperty;

		public readonly static DependencyProperty CheckedCommandParameterProperty;

		public readonly static DependencyProperty UnCheckedCommandParameterProperty;

		public readonly static DependencyProperty ContentDirectionProperty;

		public readonly static DependencyProperty ToggleSwitchButtonStyleProperty;

		public ICommand CheckChangedCommand
		{
			get
			{
				return (ICommand)base.GetValue(ToggleSwitch.CheckChangedCommandProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.CheckChangedCommandProperty, value);
			}
		}

		public object CheckChangedCommandParameter
		{
			get
			{
				return base.GetValue(ToggleSwitch.CheckChangedCommandParameterProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.CheckChangedCommandParameterProperty, value);
			}
		}

		public ICommand CheckedCommand
		{
			get
			{
				return (ICommand)base.GetValue(ToggleSwitch.CheckedCommandProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.CheckedCommandProperty, value);
			}
		}

		public object CheckedCommandParameter
		{
			get
			{
				return base.GetValue(ToggleSwitch.CheckedCommandParameterProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.CheckedCommandParameterProperty, value);
			}
		}

		public System.Windows.FlowDirection ContentDirection
		{
			get
			{
				return (System.Windows.FlowDirection)base.GetValue(ToggleSwitch.ContentDirectionProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.ContentDirectionProperty, value);
			}
		}

		public object Header
		{
			get
			{
				return base.GetValue(ToggleSwitch.HeaderProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.HeaderProperty, value);
			}
		}

		public DataTemplate HeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(ToggleSwitch.HeaderTemplateProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.HeaderTemplateProperty, value);
			}
		}

		[TypeConverter(typeof(NullableBoolConverter))]
		public bool? IsChecked
		{
			get
			{
				return (bool?)base.GetValue(ToggleSwitch.IsCheckedProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.IsCheckedProperty, value);
			}
		}

		public string OffLabel
		{
			get
			{
				return (string)base.GetValue(ToggleSwitch.OffLabelProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.OffLabelProperty, value);
			}
		}

		public Brush OffSwitchBrush
		{
			get
			{
				return (Brush)base.GetValue(ToggleSwitch.OffSwitchBrushProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.OffSwitchBrushProperty, value);
			}
		}

		public string OnLabel
		{
			get
			{
				return (string)base.GetValue(ToggleSwitch.OnLabelProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.OnLabelProperty, value);
			}
		}

		public Brush OnSwitchBrush
		{
			get
			{
				return (Brush)base.GetValue(ToggleSwitch.OnSwitchBrushProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.OnSwitchBrushProperty, value);
			}
		}

		[Obsolete("This property will be deleted in the next release. You should use OnSwitchBrush and OffSwitchBrush to change the switch's brushes.")]
		public Brush SwitchForeground
		{
			get
			{
				return (Brush)base.GetValue(ToggleSwitch.SwitchForegroundProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.SwitchForegroundProperty, value);
			}
		}

		public Brush ThumbIndicatorBrush
		{
			get
			{
				return (Brush)base.GetValue(ToggleSwitch.ThumbIndicatorBrushProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.ThumbIndicatorBrushProperty, value);
			}
		}

		public Brush ThumbIndicatorDisabledBrush
		{
			get
			{
				return (Brush)base.GetValue(ToggleSwitch.ThumbIndicatorDisabledBrushProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.ThumbIndicatorDisabledBrushProperty, value);
			}
		}

		public double ThumbIndicatorWidth
		{
			get
			{
				return (double)base.GetValue(ToggleSwitch.ThumbIndicatorWidthProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.ThumbIndicatorWidthProperty, value);
			}
		}

		public System.Windows.Style ToggleSwitchButtonStyle
		{
			get
			{
				return (System.Windows.Style)base.GetValue(ToggleSwitch.ToggleSwitchButtonStyleProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.ToggleSwitchButtonStyleProperty, value);
			}
		}

		public ICommand UnCheckedCommand
		{
			get
			{
				return (ICommand)base.GetValue(ToggleSwitch.UnCheckedCommandProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.UnCheckedCommandProperty, value);
			}
		}

		public object UnCheckedCommandParameter
		{
			get
			{
				return base.GetValue(ToggleSwitch.UnCheckedCommandParameterProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.UnCheckedCommandParameterProperty, value);
			}
		}

		static ToggleSwitch()
		{
			Class6.yDnXvgqzyB5jw();
			ToggleSwitch.OnLabelProperty = DependencyProperty.Register("OnLabel", typeof(string), typeof(ToggleSwitch), new PropertyMetadata("On"));
			ToggleSwitch.OffLabelProperty = DependencyProperty.Register("OffLabel", typeof(string), typeof(ToggleSwitch), new PropertyMetadata("Off"));
			ToggleSwitch.HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(ToggleSwitch), new PropertyMetadata(null));
			ToggleSwitch.HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(ToggleSwitch), new PropertyMetadata(null));
			ToggleSwitch.SwitchForegroundProperty = DependencyProperty.Register("SwitchForeground", typeof(Brush), typeof(ToggleSwitch), new PropertyMetadata(null, (DependencyObject o, DependencyPropertyChangedEventArgs e) => ((ToggleSwitch)o).OnSwitchBrush = e.NewValue as Brush));
			ToggleSwitch.OnSwitchBrushProperty = DependencyProperty.Register("OnSwitchBrush", typeof(Brush), typeof(ToggleSwitch), null);
			ToggleSwitch.OffSwitchBrushProperty = DependencyProperty.Register("OffSwitchBrush", typeof(Brush), typeof(ToggleSwitch), null);
			ToggleSwitch.ThumbIndicatorBrushProperty = DependencyProperty.Register("ThumbIndicatorBrush", typeof(Brush), typeof(ToggleSwitch), null);
			ToggleSwitch.ThumbIndicatorDisabledBrushProperty = DependencyProperty.Register("ThumbIndicatorDisabledBrush", typeof(Brush), typeof(ToggleSwitch), null);
			ToggleSwitch.ThumbIndicatorWidthProperty = DependencyProperty.Register("ThumbIndicatorWidth", typeof(double), typeof(ToggleSwitch), new PropertyMetadata((object)13));
			ToggleSwitch.IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool?), typeof(ToggleSwitch), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(ToggleSwitch.OnIsCheckedChanged)));
			ToggleSwitch.CheckChangedCommandProperty = DependencyProperty.Register("CheckChangedCommand", typeof(ICommand), typeof(ToggleSwitch), new PropertyMetadata(null));
			ToggleSwitch.CheckedCommandProperty = DependencyProperty.Register("CheckedCommand", typeof(ICommand), typeof(ToggleSwitch), new PropertyMetadata(null));
			ToggleSwitch.UnCheckedCommandProperty = DependencyProperty.Register("UnCheckedCommand", typeof(ICommand), typeof(ToggleSwitch), new PropertyMetadata(null));
			ToggleSwitch.CheckChangedCommandParameterProperty = DependencyProperty.Register("CheckChangedCommandParameter", typeof(object), typeof(ToggleSwitch), new PropertyMetadata(null));
			ToggleSwitch.CheckedCommandParameterProperty = DependencyProperty.Register("CheckedCommandParameter", typeof(object), typeof(ToggleSwitch), new PropertyMetadata(null));
			ToggleSwitch.UnCheckedCommandParameterProperty = DependencyProperty.Register("UnCheckedCommandParameter", typeof(object), typeof(ToggleSwitch), new PropertyMetadata(null));
			ToggleSwitch.ContentDirectionProperty = DependencyProperty.Register("ContentDirection", typeof(System.Windows.FlowDirection), typeof(ToggleSwitch), new PropertyMetadata((object)System.Windows.FlowDirection.LeftToRight));
			ToggleSwitch.ToggleSwitchButtonStyleProperty = DependencyProperty.Register("ToggleSwitchButtonStyle", typeof(System.Windows.Style), typeof(ToggleSwitch), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));
		}

		public ToggleSwitch()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			base.DefaultStyleKey = typeof(ToggleSwitch);
			base.PreviewKeyUp += new KeyEventHandler(this.ToggleSwitch_PreviewKeyUp);
			base.MouseUp += new MouseButtonEventHandler((object sender, MouseButtonEventArgs e) => Keyboard.Focus(this));
		}

		private new void ChangeVisualState(bool useTransitions)
		{
			VisualStateManager.GoToState(this, (base.IsEnabled ? "Normal" : "Disabled"), useTransitions);
		}

		private void CheckedHandler(object sender, RoutedEventArgs e)
		{
			ICommand checkedCommand = this.CheckedCommand;
			object checkedCommandParameter = this.CheckedCommandParameter;
			if (checkedCommandParameter == null)
			{
				checkedCommandParameter = this;
			}
			object obj = checkedCommandParameter;
			if (checkedCommand != null && checkedCommand.CanExecute(obj))
			{
				checkedCommand.Execute(obj);
			}
			SafeRaise.Raise<RoutedEventArgs>(this.Checked, this, e);
		}

		private void ClickHandler(object sender, RoutedEventArgs e)
		{
			SafeRaise.Raise<RoutedEventArgs>(this.Click, this, e);
		}

		private void IndeterminateHandler(object sender, RoutedEventArgs e)
		{
			SafeRaise.Raise<RoutedEventArgs>(this.Indeterminate, this, e);
		}

		private void IsEnabledHandler(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.ChangeVisualState(false);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (this._toggleButton != null)
			{
				this._toggleButton.Checked -= new RoutedEventHandler(this.CheckedHandler);
				this._toggleButton.Unchecked -= new RoutedEventHandler(this.UncheckedHandler);
				this._toggleButton.Indeterminate -= new RoutedEventHandler(this.IndeterminateHandler);
				this._toggleButton.Click -= new RoutedEventHandler(this.ClickHandler);
				BindingOperations.ClearBinding(this._toggleButton, ToggleButton.IsCheckedProperty);
				this._toggleButton.IsEnabledChanged -= new DependencyPropertyChangedEventHandler(this.IsEnabledHandler);
				this._toggleButton.PreviewMouseUp -= new MouseButtonEventHandler(this.ToggleButtonPreviewMouseUp);
			}
			this._toggleButton = base.GetTemplateChild("Switch") as ToggleButton;
			if (this._toggleButton != null)
			{
				this._toggleButton.Checked += new RoutedEventHandler(this.CheckedHandler);
				this._toggleButton.Unchecked += new RoutedEventHandler(this.UncheckedHandler);
				this._toggleButton.Indeterminate += new RoutedEventHandler(this.IndeterminateHandler);
				this._toggleButton.Click += new RoutedEventHandler(this.ClickHandler);
				Binding binding = new Binding("IsChecked")
				{
					Source = this
				};
				this._toggleButton.SetBinding(ToggleButton.IsCheckedProperty, binding);
				this._toggleButton.IsEnabledChanged += new DependencyPropertyChangedEventHandler(this.IsEnabledHandler);
				this._toggleButton.PreviewMouseUp += new MouseButtonEventHandler(this.ToggleButtonPreviewMouseUp);
			}
			this.ChangeVisualState(false);
		}

		private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ToggleSwitch toggleSwitch = (ToggleSwitch)d;
			if (toggleSwitch._toggleButton != null)
			{
				bool? oldValue = (bool?)e.OldValue;
				bool? newValue = (bool?)e.NewValue;
				bool? nullable = oldValue;
				bool? nullable1 = newValue;
				if ((nullable.GetValueOrDefault() == nullable1.GetValueOrDefault() ? nullable.HasValue != nullable1.HasValue : true))
				{
					ICommand checkChangedCommand = toggleSwitch.CheckChangedCommand;
					object checkChangedCommandParameter = toggleSwitch.CheckChangedCommandParameter;
					if (checkChangedCommandParameter == null)
					{
						checkChangedCommandParameter = toggleSwitch;
					}
					object obj = checkChangedCommandParameter;
					if (checkChangedCommand != null && checkChangedCommand.CanExecute(obj))
					{
						checkChangedCommand.Execute(obj);
					}
					EventHandler eventHandler = toggleSwitch.IsCheckedChanged;
					if (eventHandler != null)
					{
						eventHandler(toggleSwitch, EventArgs.Empty);
					}
				}
			}
		}

		private void ToggleButtonPreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			Keyboard.Focus(this);
		}

		private void ToggleSwitch_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			bool? nullable;
			if (e.Key == Key.Space && e.OriginalSource == sender)
			{
				bool? isChecked = this.IsChecked;
				if (isChecked.HasValue)
				{
					nullable = new bool?(!isChecked.GetValueOrDefault());
				}
				else
				{
					nullable = null;
				}
				this.IsChecked = nullable;
			}
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{{ToggleSwitch IsChecked={0}, Content={1}}}", new object[] { this.IsChecked, base.Content });
		}

		private void UncheckedHandler(object sender, RoutedEventArgs e)
		{
			ICommand unCheckedCommand = this.UnCheckedCommand;
			object unCheckedCommandParameter = this.UnCheckedCommandParameter;
			if (unCheckedCommandParameter == null)
			{
				unCheckedCommandParameter = this;
			}
			object obj = unCheckedCommandParameter;
			if (unCheckedCommand != null && unCheckedCommand.CanExecute(obj))
			{
				unCheckedCommand.Execute(obj);
			}
			SafeRaise.Raise<RoutedEventArgs>(this.Unchecked, this, e);
		}

		public event EventHandler<RoutedEventArgs> Checked;

		public event EventHandler<RoutedEventArgs> Click;

		public event EventHandler<RoutedEventArgs> Indeterminate;

		public event EventHandler IsCheckedChanged;

		public event EventHandler<RoutedEventArgs> Unchecked;
	}
}