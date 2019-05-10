using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name="PART_NumericDown", Type=typeof(RepeatButton))]
	[TemplatePart(Name="PART_NumericUp", Type=typeof(RepeatButton))]
	[TemplatePart(Name="PART_TextBox", Type=typeof(TextBox))]
	public class NumericUpDown : Control
	{
		public readonly static RoutedEvent ValueIncrementedEvent;

		public readonly static RoutedEvent ValueDecrementedEvent;

		public readonly static RoutedEvent DelayChangedEvent;

		public readonly static RoutedEvent MaximumReachedEvent;

		public readonly static RoutedEvent MinimumReachedEvent;

		public readonly static RoutedEvent ValueChangedEvent;

		public readonly static DependencyProperty DelayProperty;

		public readonly static DependencyProperty TextAlignmentProperty;

		public readonly static DependencyProperty SpeedupProperty;

		public readonly static DependencyProperty IsReadOnlyProperty;

		public readonly static DependencyProperty StringFormatProperty;

		public readonly static DependencyProperty InterceptArrowKeysProperty;

		public readonly static DependencyProperty ValueProperty;

		public readonly static DependencyProperty ButtonsAlignmentProperty;

		public readonly static DependencyProperty MinimumProperty;

		public readonly static DependencyProperty MaximumProperty;

		public readonly static DependencyProperty IntervalProperty;

		public readonly static DependencyProperty InterceptMouseWheelProperty;

		public readonly static DependencyProperty TrackMouseWheelWhenMouseOverProperty;

		public readonly static DependencyProperty HideUpDownButtonsProperty;

		public readonly static DependencyProperty UpDownButtonsWidthProperty;

		public readonly static DependencyProperty InterceptManualEnterProperty;

		public readonly static DependencyProperty CultureProperty;

		public readonly static DependencyProperty SelectAllOnFocusProperty;

		public readonly static DependencyProperty HasDecimalsProperty;

		private readonly static Regex RegexStringFormatHexadecimal;

		private Tuple<string, string> _removeFromText;

		private Lazy<PropertyInfo> _handlesMouseWheelScrolling;

		private double _internalIntervalMultiplierForCalculation;

		private double _internalLargeChange;

		private double _intervalValueSinceReset;

		private bool _manualChange;

		private RepeatButton _repeatDown;

		private RepeatButton _repeatUp;

		private TextBox _valueTextBox;

		private ScrollViewer _scrollViewer;

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(MahApps.Metro.Controls.ButtonsAlignment.Right)]
		public MahApps.Metro.Controls.ButtonsAlignment ButtonsAlignment
		{
			get
			{
				return (MahApps.Metro.Controls.ButtonsAlignment)base.GetValue(NumericUpDown.ButtonsAlignmentProperty);
			}
			set
			{
				base.SetValue(NumericUpDown.ButtonsAlignmentProperty, value);
			}
		}

		[Category("Behavior")]
		[DefaultValue(null)]
		public CultureInfo Culture
		{
			get
			{
				return (CultureInfo)base.GetValue(NumericUpDown.CultureProperty);
			}
			set
			{
				base.SetValue(NumericUpDown.CultureProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Behavior")]
		[DefaultValue(500)]
		public int Delay
		{
			get
			{
				return (int)base.GetValue(NumericUpDown.DelayProperty);
			}
			set
			{
				base.SetValue(NumericUpDown.DelayProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Common")]
		[DefaultValue(true)]
		public bool HasDecimals
		{
			get
			{
				return (bool)base.GetValue(NumericUpDown.HasDecimalsProperty);
			}
			set
			{
				base.SetValue(NumericUpDown.HasDecimalsProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		public bool HideUpDownButtons
		{
			get
			{
				return (bool)base.GetValue(NumericUpDown.HideUpDownButtonsProperty);
			}
			set
			{
				base.SetValue(NumericUpDown.HideUpDownButtonsProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Behavior")]
		[DefaultValue(true)]
		public bool InterceptArrowKeys
		{
			get
			{
				return (bool)base.GetValue(NumericUpDown.InterceptArrowKeysProperty);
			}
			set
			{
				base.SetValue(NumericUpDown.InterceptArrowKeysProperty, value);
			}
		}

		[Category("Behavior")]
		[DefaultValue(true)]
		public bool InterceptManualEnter
		{
			get
			{
				return (bool)base.GetValue(NumericUpDown.InterceptManualEnterProperty);
			}
			set
			{
				base.SetValue(NumericUpDown.InterceptManualEnterProperty, value);
			}
		}

		[Category("Behavior")]
		[DefaultValue(true)]
		public bool InterceptMouseWheel
		{
			get
			{
				return (bool)base.GetValue(NumericUpDown.InterceptMouseWheelProperty);
			}
			set
			{
				base.SetValue(NumericUpDown.InterceptMouseWheelProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Behavior")]
		[DefaultValue(1)]
		public double Interval
		{
			get
			{
				return (double)base.GetValue(NumericUpDown.IntervalProperty);
			}
			set
			{
				base.SetValue(NumericUpDown.IntervalProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		public bool IsReadOnly
		{
			get
			{
				return (bool)base.GetValue(NumericUpDown.IsReadOnlyProperty);
			}
			set
			{
				base.SetValue(NumericUpDown.IsReadOnlyProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Common")]
		[DefaultValue(double.MaxValue)]
		public double Maximum
		{
			get
			{
				return (double)base.GetValue(NumericUpDown.MaximumProperty);
			}
			set
			{
				base.SetValue(NumericUpDown.MaximumProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Common")]
		[DefaultValue(double.MinValue)]
		public double Minimum
		{
			get
			{
				return (double)base.GetValue(NumericUpDown.MinimumProperty);
			}
			set
			{
				base.SetValue(NumericUpDown.MinimumProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Behavior")]
		[DefaultValue(true)]
		public bool SelectAllOnFocus
		{
			get
			{
				return (bool)base.GetValue(NumericUpDown.SelectAllOnFocusProperty);
			}
			set
			{
				base.SetValue(NumericUpDown.SelectAllOnFocusProperty, value);
			}
		}

		private CultureInfo SpecificCultureInfo
		{
			get
			{
				return this.Culture ?? base.Language.GetSpecificCulture();
			}
		}

		[Category("Common")]
		[DefaultValue(true)]
		public bool Speedup
		{
			get
			{
				return (bool)base.GetValue(NumericUpDown.SpeedupProperty);
			}
			set
			{
				base.SetValue(NumericUpDown.SpeedupProperty, value);
			}
		}

		[Category("Common")]
		public string StringFormat
		{
			get
			{
				return (string)base.GetValue(NumericUpDown.StringFormatProperty);
			}
			set
			{
				base.SetValue(NumericUpDown.StringFormatProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Common")]
		[DefaultValue(System.Windows.TextAlignment.Right)]
		public System.Windows.TextAlignment TextAlignment
		{
			get
			{
				return (System.Windows.TextAlignment)base.GetValue(NumericUpDown.TextAlignmentProperty);
			}
			set
			{
				base.SetValue(NumericUpDown.TextAlignmentProperty, value);
			}
		}

		[Category("Behavior")]
		[DefaultValue(false)]
		public bool TrackMouseWheelWhenMouseOver
		{
			get
			{
				return (bool)base.GetValue(NumericUpDown.TrackMouseWheelWhenMouseOverProperty);
			}
			set
			{
				base.SetValue(NumericUpDown.TrackMouseWheelWhenMouseOverProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(20)]
		public double UpDownButtonsWidth
		{
			get
			{
				return (double)base.GetValue(NumericUpDown.UpDownButtonsWidthProperty);
			}
			set
			{
				base.SetValue(NumericUpDown.UpDownButtonsWidthProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Common")]
		[DefaultValue(null)]
		public double? Value
		{
			get
			{
				return (double?)base.GetValue(NumericUpDown.ValueProperty);
			}
			set
			{
				base.SetValue(NumericUpDown.ValueProperty, value);
			}
		}

		static NumericUpDown()
		{
			Class6.yDnXvgqzyB5jw();
			NumericUpDown.ValueIncrementedEvent = EventManager.RegisterRoutedEvent("ValueIncremented", RoutingStrategy.Bubble, typeof(NumericUpDownChangedRoutedEventHandler), typeof(NumericUpDown));
			NumericUpDown.ValueDecrementedEvent = EventManager.RegisterRoutedEvent("ValueDecremented", RoutingStrategy.Bubble, typeof(NumericUpDownChangedRoutedEventHandler), typeof(NumericUpDown));
			NumericUpDown.DelayChangedEvent = EventManager.RegisterRoutedEvent("DelayChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumericUpDown));
			NumericUpDown.MaximumReachedEvent = EventManager.RegisterRoutedEvent("MaximumReached", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumericUpDown));
			NumericUpDown.MinimumReachedEvent = EventManager.RegisterRoutedEvent("MinimumReached", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumericUpDown));
			NumericUpDown.ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double?>), typeof(NumericUpDown));
			NumericUpDown.DelayProperty = DependencyProperty.Register("Delay", typeof(int), typeof(NumericUpDown), new FrameworkPropertyMetadata((object)500, new PropertyChangedCallback(NumericUpDown.OnDelayChanged)), new ValidateValueCallback(NumericUpDown.ValidateDelay));
			NumericUpDown.TextAlignmentProperty = TextBox.TextAlignmentProperty.AddOwner(typeof(NumericUpDown));
			NumericUpDown.SpeedupProperty = DependencyProperty.Register("Speedup", typeof(bool), typeof(NumericUpDown), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(NumericUpDown.OnSpeedupChanged)));
			NumericUpDown.IsReadOnlyProperty = TextBoxBase.IsReadOnlyProperty.AddOwner(typeof(NumericUpDown), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(NumericUpDown.IsReadOnlyPropertyChangedCallback)));
			NumericUpDown.StringFormatProperty = DependencyProperty.Register("StringFormat", typeof(string), typeof(NumericUpDown), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(NumericUpDown.OnStringFormatChanged), new CoerceValueCallback(NumericUpDown.CoerceStringFormat)));
			NumericUpDown.InterceptArrowKeysProperty = DependencyProperty.Register("InterceptArrowKeys", typeof(bool), typeof(NumericUpDown), new FrameworkPropertyMetadata(true));
			NumericUpDown.ValueProperty = DependencyProperty.Register("Value", typeof(double?), typeof(NumericUpDown), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(NumericUpDown.OnValueChanged), new CoerceValueCallback(NumericUpDown.CoerceValue)));
			NumericUpDown.ButtonsAlignmentProperty = DependencyProperty.Register("ButtonsAlignment", typeof(MahApps.Metro.Controls.ButtonsAlignment), typeof(NumericUpDown), new FrameworkPropertyMetadata((object)MahApps.Metro.Controls.ButtonsAlignment.Right, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));
			NumericUpDown.MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata((object)double.MinValue, new PropertyChangedCallback(NumericUpDown.OnMinimumChanged)));
			NumericUpDown.MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata((object)double.MaxValue, new PropertyChangedCallback(NumericUpDown.OnMaximumChanged), new CoerceValueCallback(NumericUpDown.CoerceMaximum)));
			NumericUpDown.IntervalProperty = DependencyProperty.Register("Interval", typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata((object)1, new PropertyChangedCallback(NumericUpDown.IntervalChanged)));
			NumericUpDown.InterceptMouseWheelProperty = DependencyProperty.Register("InterceptMouseWheel", typeof(bool), typeof(NumericUpDown), new FrameworkPropertyMetadata(true));
			NumericUpDown.TrackMouseWheelWhenMouseOverProperty = DependencyProperty.Register("TrackMouseWheelWhenMouseOver", typeof(bool), typeof(NumericUpDown), new FrameworkPropertyMetadata(false));
			NumericUpDown.HideUpDownButtonsProperty = DependencyProperty.Register("HideUpDownButtons", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(false));
			NumericUpDown.UpDownButtonsWidthProperty = DependencyProperty.Register("UpDownButtonsWidth", typeof(double), typeof(NumericUpDown), new PropertyMetadata((object)20));
			NumericUpDown.InterceptManualEnterProperty = DependencyProperty.Register("InterceptManualEnter", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(true, new PropertyChangedCallback(NumericUpDown.InterceptManualEnterChangedCallback)));
			NumericUpDown.CultureProperty = DependencyProperty.Register("Culture", typeof(CultureInfo), typeof(NumericUpDown), new PropertyMetadata(null, (DependencyObject o, DependencyPropertyChangedEventArgs e) => {
				if (e.NewValue != e.OldValue)
				{
					NumericUpDown numericUpDown = (NumericUpDown)o;
					numericUpDown.OnValueChanged(numericUpDown.Value, numericUpDown.Value);
				}
			}));
			NumericUpDown.SelectAllOnFocusProperty = DependencyProperty.Register("SelectAllOnFocus", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(true));
			NumericUpDown.HasDecimalsProperty = DependencyProperty.Register("HasDecimals", typeof(bool), typeof(NumericUpDown), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(NumericUpDown.OnHasDecimalsChanged)));
			NumericUpDown.RegexStringFormatHexadecimal = new Regex("^(?<complexHEX>.*{\\d:X\\d+}.*)?(?<simpleHEX>X\\d+)?$", RegexOptions.Compiled);
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));
			Control.VerticalContentAlignmentProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata((object)System.Windows.VerticalAlignment.Center));
			Control.HorizontalContentAlignmentProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata((object)System.Windows.HorizontalAlignment.Right));
			EventManager.RegisterClassHandler(typeof(NumericUpDown), UIElement.GotFocusEvent, new RoutedEventHandler(NumericUpDown.OnGotFocus));
		}

		public NumericUpDown()
		{
			Class6.yDnXvgqzyB5jw();
			this._removeFromText = new Tuple<string, string>(string.Empty, string.Empty);
			this._handlesMouseWheelScrolling = new Lazy<PropertyInfo>();
			this._internalIntervalMultiplierForCalculation = 1;
			this._internalLargeChange = 100;
			base();
		}

		private void ChangeValueBy(double difference)
		{
			double valueOrDefault = this.Value.GetValueOrDefault() + difference;
			this.Value = new double?((double)NumericUpDown.CoerceValue(this, valueOrDefault));
		}

		private void ChangeValueInternal(bool addInterval)
		{
			this.ChangeValueInternal((addInterval ? this.Interval : -this.Interval));
		}

		private void ChangeValueInternal(double interval)
		{
			if (this.IsReadOnly)
			{
				return;
			}
			NumericUpDownChangedRoutedEventArgs numericUpDownChangedRoutedEventArg = (interval > 0 ? new NumericUpDownChangedRoutedEventArgs(NumericUpDown.ValueIncrementedEvent, interval) : new NumericUpDownChangedRoutedEventArgs(NumericUpDown.ValueDecrementedEvent, interval));
			base.RaiseEvent(numericUpDownChangedRoutedEventArg);
			if (!numericUpDownChangedRoutedEventArg.Handled)
			{
				this.ChangeValueBy(numericUpDownChangedRoutedEventArg.Interval);
				this._valueTextBox.CaretIndex = this._valueTextBox.Text.Length;
			}
		}

		private void ChangeValueWithSpeedUp(bool toPositive)
		{
			if (this.IsReadOnly)
			{
				return;
			}
			double num = (double)((toPositive ? 1 : -1));
			if (!this.Speedup)
			{
				this.ChangeValueInternal(num * this.Interval);
				return;
			}
			double interval = this.Interval * this._internalLargeChange;
			double interval1 = this._intervalValueSinceReset + this.Interval * this._internalIntervalMultiplierForCalculation;
			double num1 = interval1;
			this._intervalValueSinceReset = interval1;
			if (num1 > interval)
			{
				this._internalLargeChange *= 10;
				this._internalIntervalMultiplierForCalculation *= 10;
			}
			this.ChangeValueInternal(num * this._internalIntervalMultiplierForCalculation);
		}

		private static object CoerceMaximum(DependencyObject d, object value)
		{
			double minimum = ((NumericUpDown)d).Minimum;
			double num = (double)value;
			return (num < minimum ? minimum : num);
		}

		private static object CoerceStringFormat(DependencyObject d, object basevalue)
		{
			object empty = basevalue;
			if (empty == null)
			{
				empty = string.Empty;
			}
			return empty;
		}

		private static object CoerceValue(DependencyObject d, object value)
		{
			if (value == null)
			{
				return null;
			}
			NumericUpDown numericUpDown = (NumericUpDown)d;
			double num = ((double?)value).Value;
			if (!numericUpDown.HasDecimals)
			{
				num = Math.Truncate(num);
			}
			if (num < numericUpDown.Minimum)
			{
				return numericUpDown.Minimum;
			}
			if (num <= numericUpDown.Maximum)
			{
				return num;
			}
			return numericUpDown.Maximum;
		}

		private void EnableDisableDown()
		{
			if (this._repeatDown != null)
			{
				RepeatButton repeatButton = this._repeatDown;
				double? value = this.Value;
				double minimum = this.Minimum;
				repeatButton.IsEnabled = (value.GetValueOrDefault() > minimum ? value.HasValue : false);
			}
		}

		private void EnableDisableUp()
		{
			if (this._repeatUp != null)
			{
				RepeatButton repeatButton = this._repeatUp;
				double? value = this.Value;
				double maximum = this.Maximum;
				repeatButton.IsEnabled = (value.GetValueOrDefault() < maximum ? value.HasValue : false);
			}
		}

		private void EnableDisableUpDown()
		{
			this.EnableDisableUp();
			this.EnableDisableDown();
		}

		private void FormatValue(double? newValue, CultureInfo culture)
		{
			Match match = NumericUpDown.RegexStringFormatHexadecimal.Match(this.StringFormat);
			if (!match.Success)
			{
				if (!this.StringFormat.Contains("{"))
				{
					TextBox str = this._valueTextBox;
					double value = newValue.Value;
					str.Text = value.ToString(this.StringFormat, culture);
					return;
				}
				this._valueTextBox.Text = string.Format(culture, this.StringFormat, new object[] { newValue.Value });
			}
			else
			{
				if (match.Groups["simpleHEX"].Success)
				{
					TextBox textBox = this._valueTextBox;
					int num = (int)newValue.Value;
					textBox.Text = num.ToString(match.Groups["simpleHEX"].Value, culture);
					return;
				}
				if (match.Groups["complexHEX"].Success)
				{
					this._valueTextBox.Text = string.Format(culture, match.Groups["complexHEX"].Value, new object[] { (int)newValue.Value });
					return;
				}
			}
		}

		private static void InterceptManualEnterChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			if (e.OldValue != e.NewValue && e.NewValue != null)
			{
				NumericUpDown numericUpDown = (NumericUpDown)dependencyObject;
				numericUpDown.ToggleReadOnlyMode(!(bool)e.NewValue | numericUpDown.IsReadOnly);
			}
		}

		private void InternalSetText(double? newValue)
		{
			if (!newValue.HasValue)
			{
				this._valueTextBox.Text = null;
				return;
			}
			CultureInfo specificCultureInfo = this.SpecificCultureInfo;
			if (!string.IsNullOrEmpty(this.StringFormat))
			{
				this.FormatValue(newValue, specificCultureInfo);
			}
			else
			{
				this._valueTextBox.Text = newValue.Value.ToString(specificCultureInfo);
			}
			if ((bool)base.GetValue(TextBoxHelper.IsMonitoringProperty))
			{
				base.SetValue(TextBoxHelper.TextLengthProperty, this._valueTextBox.Text.Length);
			}
		}

		private static void IntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((NumericUpDown)d).ResetInternal();
		}

		private static void IsReadOnlyPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			if (e.OldValue != e.NewValue && e.NewValue != null)
			{
				NumericUpDown numericUpDown = (NumericUpDown)dependencyObject;
				numericUpDown.ToggleReadOnlyMode((bool)e.NewValue | !numericUpDown.InterceptManualEnter);
			}
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this._repeatUp = base.GetTemplateChild("PART_NumericUp") as RepeatButton;
			this._repeatDown = base.GetTemplateChild("PART_NumericDown") as RepeatButton;
			this._valueTextBox = base.GetTemplateChild("PART_TextBox") as TextBox;
			if (this._repeatUp == null || this._repeatDown == null || this._valueTextBox == null)
			{
				throw new InvalidOperationException(string.Format("You have missed to specify {0}, {1} or {2} in your template", "PART_NumericUp", "PART_NumericDown", "PART_TextBox"));
			}
			this.ToggleReadOnlyMode(this.IsReadOnly | !this.InterceptManualEnter);
			this._repeatUp.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) => this.ChangeValueWithSpeedUp(true));
			this._repeatDown.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) => this.ChangeValueWithSpeedUp(false));
			this._repeatUp.PreviewMouseUp += new MouseButtonEventHandler((object sender, MouseButtonEventArgs e) => this.ResetInternal());
			this._repeatDown.PreviewMouseUp += new MouseButtonEventHandler((object sender, MouseButtonEventArgs e) => this.ResetInternal());
			this.OnValueChanged(this.Value, this.Value);
			this._scrollViewer = this.TryFindScrollViewer();
		}

		protected virtual void OnDelayChanged(int oldDelay, int newDelay)
		{
			if (oldDelay != newDelay)
			{
				if (this._repeatDown != null)
				{
					this._repeatDown.Delay = newDelay;
				}
				if (this._repeatUp != null)
				{
					this._repeatUp.Delay = newDelay;
				}
			}
		}

		private static void OnDelayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			NumericUpDown numericUpDown = (NumericUpDown)d;
			numericUpDown.RaiseChangeDelay();
			numericUpDown.OnDelayChanged((int)e.OldValue, (int)e.NewValue);
		}

		private static void OnGotFocus(NumericUpDown sender, RoutedEventArgs e)
		{
			NumericUpDown numericUpDown = (NumericUpDown)sender;
			if (!e.Handled && (numericUpDown.InterceptManualEnter || numericUpDown.IsReadOnly) && numericUpDown._valueTextBox != null)
			{
				if (e.OriginalSource == numericUpDown)
				{
					numericUpDown._valueTextBox.Focus();
					e.Handled = true;
					return;
				}
				if (e.OriginalSource == numericUpDown._valueTextBox && numericUpDown.SelectAllOnFocus)
				{
					numericUpDown._valueTextBox.SelectAll();
				}
			}
		}

		private static void OnHasDecimalsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			NumericUpDown nullable = (NumericUpDown)d;
			double? value = nullable.Value;
			if (!(bool)e.NewValue && nullable.Value.HasValue)
			{
				double? value1 = nullable.Value;
				nullable.Value = new double?(Math.Truncate(value1.GetValueOrDefault()));
			}
		}

		protected virtual void OnMaximumChanged(double oldMaximum, double newMaximum)
		{
		}

		private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			NumericUpDown numericUpDown = (NumericUpDown)d;
			numericUpDown.CoerceValue(NumericUpDown.ValueProperty);
			numericUpDown.Value = (double?)NumericUpDown.CoerceValue(numericUpDown, numericUpDown.Value);
			numericUpDown.OnMaximumChanged((double)e.OldValue, (double)e.NewValue);
			numericUpDown.EnableDisableUpDown();
		}

		protected virtual void OnMinimumChanged(double oldMinimum, double newMinimum)
		{
		}

		private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			NumericUpDown numericUpDown = (NumericUpDown)d;
			numericUpDown.CoerceValue(NumericUpDown.ValueProperty);
			numericUpDown.CoerceValue(NumericUpDown.MaximumProperty);
			numericUpDown.Value = (double?)NumericUpDown.CoerceValue(numericUpDown, numericUpDown.Value);
			numericUpDown.OnMinimumChanged((double)e.OldValue, (double)e.NewValue);
			numericUpDown.EnableDisableUpDown();
		}

		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			base.OnPreviewKeyDown(e);
			if (!this.InterceptArrowKeys)
			{
				return;
			}
			Key key = e.Key;
			if (key == Key.Up)
			{
				this.ChangeValueWithSpeedUp(true);
				e.Handled = true;
			}
			else if (key == Key.Down)
			{
				this.ChangeValueWithSpeedUp(false);
				e.Handled = true;
			}
			if (e.Handled)
			{
				this._manualChange = false;
				this.InternalSetText(this.Value);
			}
		}

		protected override void OnPreviewKeyUp(KeyEventArgs e)
		{
			base.OnPreviewKeyUp(e);
			if (e.Key == Key.Down || e.Key == Key.Up)
			{
				this.ResetInternal();
			}
		}

		protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
		{
			base.OnPreviewMouseWheel(e);
			if (this.InterceptMouseWheel && (base.IsFocused || this._valueTextBox.IsFocused || this.TrackMouseWheelWhenMouseOver))
			{
				bool delta = e.Delta > 0;
				this._manualChange = false;
				this.ChangeValueInternal(delta);
			}
			if (this._scrollViewer != null && this._handlesMouseWheelScrolling.Value != null)
			{
				if (this.TrackMouseWheelWhenMouseOver)
				{
					this._handlesMouseWheelScrolling.Value.SetValue(this._scrollViewer, true, null);
					return;
				}
				if (this.InterceptMouseWheel)
				{
					this._handlesMouseWheelScrolling.Value.SetValue(this._scrollViewer, this._valueTextBox.IsFocused, null);
					return;
				}
				this._handlesMouseWheelScrolling.Value.SetValue(this._scrollViewer, true, null);
			}
		}

		protected void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = true;
			if (!string.IsNullOrWhiteSpace(e.Text))
			{
				if (e.Text.Length == 1)
				{
					string text = e.Text;
					if (char.IsDigit(text[0]))
					{
						e.Handled = false;
						return;
					}
					CultureInfo specificCultureInfo = this.SpecificCultureInfo;
					NumberFormatInfo numberFormat = specificCultureInfo.NumberFormat;
					TextBox textBox = (TextBox)sender;
					bool selectedText = textBox.SelectedText == textBox.Text;
					if (numberFormat.NumberDecimalSeparator == text)
					{
						if (textBox.Text.All<char>((char i) => i.ToString(specificCultureInfo) != numberFormat.NumberDecimalSeparator) | selectedText && this.HasDecimals)
						{
							e.Handled = false;
							return;
						}
					}
					else if (numberFormat.NegativeSign != text && text != numberFormat.PositiveSign)
					{
						if (text.Equals("E", StringComparison.InvariantCultureIgnoreCase) && textBox.SelectionStart > 0 && !textBox.Text.Any<char>((char i) => i.ToString(specificCultureInfo).Equals("E", StringComparison.InvariantCultureIgnoreCase)))
						{
							e.Handled = false;
						}
					}
					else if (textBox.SelectionStart == 0)
					{
						if (textBox.Text.Length <= 1)
						{
							e.Handled = false;
							return;
						}
						if (selectedText || !textBox.Text.StartsWith(numberFormat.NegativeSign, StringComparison.InvariantCultureIgnoreCase) && !textBox.Text.StartsWith(numberFormat.PositiveSign, StringComparison.InvariantCultureIgnoreCase))
						{
							e.Handled = false;
							return;
						}
					}
					else if (textBox.SelectionStart > 0 && textBox.Text.ElementAt<char>(textBox.SelectionStart - 1).ToString(specificCultureInfo).Equals("E", StringComparison.InvariantCultureIgnoreCase))
					{
						e.Handled = false;
						return;
					}
					return;
				}
			}
		}

		protected virtual void OnSpeedupChanged(bool oldSpeedup, bool newSpeedup)
		{
		}

		private static void OnSpeedupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			NumericUpDown numericUpDown = (NumericUpDown)d;
			numericUpDown.OnSpeedupChanged((bool)e.OldValue, (bool)e.NewValue);
		}

		private static void OnStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			NumericUpDown numericUpDown = (NumericUpDown)d;
			numericUpDown.SetRemoveStringFormatFromText((string)e.NewValue);
			if (numericUpDown._valueTextBox != null && numericUpDown.Value.HasValue)
			{
				numericUpDown.InternalSetText(numericUpDown.Value);
			}
			numericUpDown.HasDecimals = !NumericUpDown.RegexStringFormatHexadecimal.IsMatch((string)e.NewValue);
		}

		private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
		{
			this._manualChange = true;
			if (e.Key == Key.Decimal || e.Key == Key.OemPeriod)
			{
				TextBox textBox = sender as TextBox;
				if (!textBox.Text.Contains(this.SpecificCultureInfo.NumberFormat.NumberDecimalSeparator))
				{
					int caretIndex = textBox.CaretIndex;
					textBox.Text = textBox.Text.Insert(caretIndex, this.SpecificCultureInfo.NumberFormat.CurrencyDecimalSeparator);
					textBox.CaretIndex = caretIndex + 1;
				}
				e.Handled = true;
			}
		}

		private void OnTextBoxLostFocus(object sender, RoutedEventArgs e)
		{
			double num;
			double maximum;
			if (!this.InterceptManualEnter)
			{
				return;
			}
			TextBox textBox = (TextBox)sender;
			this._manualChange = false;
			if (!this.ValidateText(textBox.Text, out num))
			{
				this.OnValueChanged(this.Value, this.Value);
				return;
			}
			double? value = this.Value;
			if ((value.GetValueOrDefault() == num ? value.HasValue : false))
			{
				this.OnValueChanged(this.Value, this.Value);
			}
			if (num > this.Maximum)
			{
				value = this.Value;
				maximum = this.Maximum;
				if ((value.GetValueOrDefault() == maximum ? value.HasValue : false))
				{
					this.OnValueChanged(this.Value, this.Value);
					return;
				}
				base.SetValue(NumericUpDown.ValueProperty, this.Maximum);
				return;
			}
			if (num >= this.Minimum)
			{
				base.SetValue(NumericUpDown.ValueProperty, num);
				return;
			}
			value = this.Value;
			maximum = this.Minimum;
			if ((value.GetValueOrDefault() == maximum ? value.HasValue : false))
			{
				this.OnValueChanged(this.Value, this.Value);
				return;
			}
			base.SetValue(NumericUpDown.ValueProperty, this.Minimum);
		}

		private void OnTextChanged(object sender, TextChangedEventArgs e)
		{
			double num;
			if (string.IsNullOrEmpty(((TextBox)sender).Text))
			{
				this.Value = null;
				return;
			}
			if (this._manualChange && this.ValidateText(((TextBox)sender).Text, out num))
			{
				this.Value = (double?)NumericUpDown.CoerceValue(this, num);
				e.Handled = true;
			}
		}

		protected virtual void OnValueChanged(double? oldValue, double? newValue)
		{
			double? nullable;
			double? nullable1;
			if (!this._manualChange)
			{
				if (!newValue.HasValue)
				{
					if (this._valueTextBox != null)
					{
						this._valueTextBox.Text = null;
					}
					nullable = oldValue;
					nullable1 = newValue;
					if ((nullable.GetValueOrDefault() == nullable1.GetValueOrDefault() ? nullable.HasValue != nullable1.HasValue : true))
					{
						base.RaiseEvent(new RoutedPropertyChangedEventArgs<double?>(oldValue, newValue, NumericUpDown.ValueChangedEvent));
					}
					return;
				}
				if (this._repeatUp != null && !this._repeatUp.IsEnabled)
				{
					this._repeatUp.IsEnabled = true;
				}
				if (this._repeatDown != null && !this._repeatDown.IsEnabled)
				{
					this._repeatDown.IsEnabled = true;
				}
				nullable1 = newValue;
				double minimum = this.Minimum;
				if ((nullable1.GetValueOrDefault() <= minimum ? nullable1.HasValue : false))
				{
					if (this._repeatDown != null)
					{
						this._repeatDown.IsEnabled = false;
					}
					this.ResetInternal();
					if (base.IsLoaded)
					{
						base.RaiseEvent(new RoutedEventArgs(NumericUpDown.MinimumReachedEvent));
					}
				}
				nullable1 = newValue;
				minimum = this.Maximum;
				if ((nullable1.GetValueOrDefault() >= minimum ? nullable1.HasValue : false))
				{
					if (this._repeatUp != null)
					{
						this._repeatUp.IsEnabled = false;
					}
					this.ResetInternal();
					if (base.IsLoaded)
					{
						base.RaiseEvent(new RoutedEventArgs(NumericUpDown.MaximumReachedEvent));
					}
				}
				if (this._valueTextBox != null)
				{
					this.InternalSetText(newValue);
				}
			}
			nullable1 = oldValue;
			nullable = newValue;
			if ((nullable1.GetValueOrDefault() == nullable.GetValueOrDefault() ? nullable1.HasValue != nullable.HasValue : true))
			{
				base.RaiseEvent(new RoutedPropertyChangedEventArgs<double?>(oldValue, newValue, NumericUpDown.ValueChangedEvent));
			}
		}

		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			NumericUpDown numericUpDown = (NumericUpDown)d;
			numericUpDown.OnValueChanged((double?)e.OldValue, (double?)e.NewValue);
		}

		private void OnValueTextBoxPaste(object sender, DataObjectPastingEventArgs e)
		{
			double num;
			TextBox textBox = (TextBox)sender;
			string text = textBox.Text;
			if (!e.SourceDataObject.GetDataPresent(DataFormats.Text, true))
			{
				return;
			}
			string data = e.SourceDataObject.GetData(DataFormats.Text) as string;
			if (!this.ValidateText(string.Concat(text.Substring(0, textBox.SelectionStart), data, text.Substring(textBox.SelectionStart)), out num))
			{
				e.CancelCommand();
			}
		}

		private void RaiseChangeDelay()
		{
			base.RaiseEvent(new RoutedEventArgs(NumericUpDown.DelayChangedEvent));
		}

		private string RemoveStringFormatFromText(string text)
		{
			if (!string.IsNullOrEmpty(this._removeFromText.Item1))
			{
				text = text.Replace(this._removeFromText.Item1, string.Empty);
			}
			if (!string.IsNullOrEmpty(this._removeFromText.Item2))
			{
				text = text.Replace(this._removeFromText.Item2, string.Empty);
			}
			return text;
		}

		private void ResetInternal()
		{
			if (this.IsReadOnly)
			{
				return;
			}
			this._internalLargeChange = 100 * this.Interval;
			this._internalIntervalMultiplierForCalculation = this.Interval;
			this._intervalValueSinceReset = 0;
		}

		public void SelectAll()
		{
			if (this._valueTextBox != null)
			{
				this._valueTextBox.SelectAll();
			}
		}

		private void SetRemoveStringFormatFromText(string stringFormat)
		{
			string empty = string.Empty;
			string str = string.Empty;
			string str1 = stringFormat;
			int num = str1.IndexOf("{", StringComparison.InvariantCultureIgnoreCase);
			if (num > -1)
			{
				if (num > 0)
				{
					empty = str1.Substring(0, num);
				}
				str = (new string(str1.SkipWhile<char>((char i) => i != '}').Skip<char>(1).ToArray<char>())).Trim();
			}
			this._removeFromText = new Tuple<string, string>(empty, str);
		}

		private void ToggleReadOnlyMode(bool isReadOnly)
		{
			if (this._repeatUp == null || this._repeatDown == null || this._valueTextBox == null)
			{
				return;
			}
			if (isReadOnly)
			{
				this._valueTextBox.LostFocus -= new RoutedEventHandler(this.OnTextBoxLostFocus);
				this._valueTextBox.PreviewTextInput -= new TextCompositionEventHandler(this.OnPreviewTextInput);
				this._valueTextBox.PreviewKeyDown -= new KeyEventHandler(this.OnTextBoxKeyDown);
				this._valueTextBox.TextChanged -= new TextChangedEventHandler(this.OnTextChanged);
				DataObject.RemovePastingHandler(this._valueTextBox, new DataObjectPastingEventHandler(this.OnValueTextBoxPaste));
				return;
			}
			this._valueTextBox.LostFocus += new RoutedEventHandler(this.OnTextBoxLostFocus);
			this._valueTextBox.PreviewTextInput += new TextCompositionEventHandler(this.OnPreviewTextInput);
			this._valueTextBox.PreviewKeyDown += new KeyEventHandler(this.OnTextBoxKeyDown);
			this._valueTextBox.TextChanged += new TextChangedEventHandler(this.OnTextChanged);
			DataObject.AddPastingHandler(this._valueTextBox, new DataObjectPastingEventHandler(this.OnValueTextBoxPaste));
		}

		private ScrollViewer TryFindScrollViewer()
		{
			this._valueTextBox.ApplyTemplate();
			ScrollViewer scrollViewer = this._valueTextBox.Template.FindName("PART_ContentHost", this._valueTextBox) as ScrollViewer;
			if (scrollViewer != null)
			{
				this._handlesMouseWheelScrolling = new Lazy<PropertyInfo>(() => ((IEnumerable<PropertyInfo>)this._scrollViewer.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic)).SingleOrDefault<PropertyInfo>((PropertyInfo i) => i.Name == "HandlesMouseWheelScrolling"));
			}
			return scrollViewer;
		}

		private static bool ValidateDelay(object value)
		{
			return Convert.ToInt32(value) >= 0;
		}

		private bool ValidateText(string text, out double convertedValue)
		{
			text = this.RemoveStringFormatFromText(text);
			return double.TryParse(text, NumberStyles.Any, this.SpecificCultureInfo, out convertedValue);
		}

		public event RoutedEventHandler DelayChanged
		{
			add
			{
				base.AddHandler(NumericUpDown.DelayChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(NumericUpDown.DelayChangedEvent, value);
			}
		}

		public event RoutedEventHandler MaximumReached
		{
			add
			{
				base.AddHandler(NumericUpDown.MaximumReachedEvent, value);
			}
			remove
			{
				base.RemoveHandler(NumericUpDown.MaximumReachedEvent, value);
			}
		}

		public event RoutedEventHandler MinimumReached
		{
			add
			{
				base.AddHandler(NumericUpDown.MinimumReachedEvent, value);
			}
			remove
			{
				base.RemoveHandler(NumericUpDown.MinimumReachedEvent, value);
			}
		}

		public event RoutedPropertyChangedEventHandler<double?> ValueChanged
		{
			add
			{
				base.AddHandler(NumericUpDown.ValueChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(NumericUpDown.ValueChangedEvent, value);
			}
		}

		public event NumericUpDownChangedRoutedEventHandler ValueDecremented
		{
			add
			{
				base.AddHandler(NumericUpDown.ValueDecrementedEvent, value);
			}
			remove
			{
				base.RemoveHandler(NumericUpDown.ValueDecrementedEvent, value);
			}
		}

		public event NumericUpDownChangedRoutedEventHandler ValueIncremented
		{
			add
			{
				base.AddHandler(NumericUpDown.ValueIncrementedEvent, value);
			}
			remove
			{
				base.RemoveHandler(NumericUpDown.ValueIncrementedEvent, value);
			}
		}
	}
}