using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace MahApps.Metro.Controls
{
	[DefaultEvent("RangeSelectionChanged")]
	[TemplatePart(Name="PART_Container", Type=typeof(StackPanel))]
	[TemplatePart(Name="PART_LeftEdge", Type=typeof(RepeatButton))]
	[TemplatePart(Name="PART_LeftThumb", Type=typeof(Thumb))]
	[TemplatePart(Name="PART_MiddleThumb", Type=typeof(Thumb))]
	[TemplatePart(Name="PART_PART_BottomTick", Type=typeof(TickBar))]
	[TemplatePart(Name="PART_PART_TopTick", Type=typeof(TickBar))]
	[TemplatePart(Name="PART_RangeSliderContainer", Type=typeof(StackPanel))]
	[TemplatePart(Name="PART_RightEdge", Type=typeof(RepeatButton))]
	[TemplatePart(Name="PART_RightThumb", Type=typeof(Thumb))]
	public class RangeSlider : RangeBase
	{
		public static RoutedUICommand MoveBack;

		public static RoutedUICommand MoveForward;

		public static RoutedUICommand MoveAllForward;

		public static RoutedUICommand MoveAllBack;

		public readonly static RoutedEvent RangeSelectionChangedEvent;

		public readonly static RoutedEvent LowerValueChangedEvent;

		public readonly static RoutedEvent UpperValueChangedEvent;

		public readonly static RoutedEvent LowerThumbDragStartedEvent;

		public readonly static RoutedEvent LowerThumbDragCompletedEvent;

		public readonly static RoutedEvent UpperThumbDragStartedEvent;

		public readonly static RoutedEvent UpperThumbDragCompletedEvent;

		public readonly static RoutedEvent CentralThumbDragStartedEvent;

		public readonly static RoutedEvent CentralThumbDragCompletedEvent;

		public readonly static RoutedEvent LowerThumbDragDeltaEvent;

		public readonly static RoutedEvent UpperThumbDragDeltaEvent;

		public readonly static RoutedEvent CentralThumbDragDeltaEvent;

		public readonly static DependencyProperty UpperValueProperty;

		public readonly static DependencyProperty LowerValueProperty;

		public readonly static DependencyProperty MinRangeProperty;

		public readonly static DependencyProperty MinRangeWidthProperty;

		public readonly static DependencyProperty MoveWholeRangeProperty;

		public readonly static DependencyProperty ExtendedModeProperty;

		public readonly static DependencyProperty IsSnapToTickEnabledProperty;

		public readonly static DependencyProperty OrientationProperty;

		public readonly static DependencyProperty TickFrequencyProperty;

		public readonly static DependencyProperty IsMoveToPointEnabledProperty;

		public readonly static DependencyProperty TickPlacementProperty;

		public readonly static DependencyProperty AutoToolTipPlacementProperty;

		public readonly static DependencyProperty AutoToolTipPrecisionProperty;

		public readonly static DependencyProperty AutoToolTipTextConverterProperty;

		public readonly static DependencyProperty IntervalProperty;

		private bool _internalUpdate;

		private Thumb _centerThumb;

		private Thumb _leftThumb;

		private Thumb _rightThumb;

		private RepeatButton _leftButton;

		private RepeatButton _rightButton;

		private StackPanel _visualElementsContainer;

		private StackPanel _container;

		private double _movableWidth;

		private readonly DispatcherTimer _timer;

		private uint _tickCount;

		private double _currentpoint;

		private bool _isInsideRange;

		private bool _centerThumbBlocked;

		private RangeSlider.Direction _direction;

		private RangeSlider.ButtonType _bType;

		private Point _position;

		private Point _basePoint;

		private double _currenValue;

		private double _density;

		private System.Windows.Controls.ToolTip _autoToolTip;

		private double _oldLower;

		private double _oldUpper;

		private bool _isMoved;

		private bool _roundToPrecision;

		private int _precision;

		private readonly PropertyChangeNotifier actualWidthPropertyChangeNotifier;

		private readonly PropertyChangeNotifier actualHeightPropertyChangeNotifier;

		[Bindable(true)]
		[Category("Behavior")]
		public System.Windows.Controls.Primitives.AutoToolTipPlacement AutoToolTipPlacement
		{
			get
			{
				return (System.Windows.Controls.Primitives.AutoToolTipPlacement)base.GetValue(RangeSlider.AutoToolTipPlacementProperty);
			}
			set
			{
				base.SetValue(RangeSlider.AutoToolTipPlacementProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		public int AutoToolTipPrecision
		{
			get
			{
				return (int)base.GetValue(RangeSlider.AutoToolTipPrecisionProperty);
			}
			set
			{
				base.SetValue(RangeSlider.AutoToolTipPrecisionProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Behavior")]
		public IValueConverter AutoToolTipTextConverter
		{
			get
			{
				return (IValueConverter)base.GetValue(RangeSlider.AutoToolTipTextConverterProperty);
			}
			set
			{
				base.SetValue(RangeSlider.AutoToolTipTextConverterProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Behavior")]
		public bool ExtendedMode
		{
			get
			{
				return (bool)base.GetValue(RangeSlider.ExtendedModeProperty);
			}
			set
			{
				base.SetValue(RangeSlider.ExtendedModeProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Behavior")]
		public int Interval
		{
			get
			{
				return (int)base.GetValue(RangeSlider.IntervalProperty);
			}
			set
			{
				base.SetValue(RangeSlider.IntervalProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Common")]
		public bool IsMoveToPointEnabled
		{
			get
			{
				return (bool)base.GetValue(RangeSlider.IsMoveToPointEnabledProperty);
			}
			set
			{
				base.SetValue(RangeSlider.IsMoveToPointEnabledProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		public bool IsSnapToTickEnabled
		{
			get
			{
				return (bool)base.GetValue(RangeSlider.IsSnapToTickEnabledProperty);
			}
			set
			{
				base.SetValue(RangeSlider.IsSnapToTickEnabledProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Common")]
		public double LowerValue
		{
			get
			{
				return (double)base.GetValue(RangeSlider.LowerValueProperty);
			}
			set
			{
				base.SetValue(RangeSlider.LowerValueProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Common")]
		public double MinRange
		{
			get
			{
				return (double)base.GetValue(RangeSlider.MinRangeProperty);
			}
			set
			{
				base.SetValue(RangeSlider.MinRangeProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Common")]
		public double MinRangeWidth
		{
			get
			{
				return (double)base.GetValue(RangeSlider.MinRangeWidthProperty);
			}
			set
			{
				base.SetValue(RangeSlider.MinRangeWidthProperty, value);
			}
		}

		public double MovableRange
		{
			get
			{
				return base.Maximum - base.Minimum - this.MinRange;
			}
		}

		[Bindable(true)]
		[Category("Behavior")]
		public bool MoveWholeRange
		{
			get
			{
				return (bool)base.GetValue(RangeSlider.MoveWholeRangeProperty);
			}
			set
			{
				base.SetValue(RangeSlider.MoveWholeRangeProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Common")]
		public System.Windows.Controls.Orientation Orientation
		{
			get
			{
				return (System.Windows.Controls.Orientation)base.GetValue(RangeSlider.OrientationProperty);
			}
			set
			{
				base.SetValue(RangeSlider.OrientationProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Common")]
		public double TickFrequency
		{
			get
			{
				return (double)base.GetValue(RangeSlider.TickFrequencyProperty);
			}
			set
			{
				base.SetValue(RangeSlider.TickFrequencyProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Common")]
		public System.Windows.Controls.Primitives.TickPlacement TickPlacement
		{
			get
			{
				return (System.Windows.Controls.Primitives.TickPlacement)base.GetValue(RangeSlider.TickPlacementProperty);
			}
			set
			{
				base.SetValue(RangeSlider.TickPlacementProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Common")]
		public double UpperValue
		{
			get
			{
				return (double)base.GetValue(RangeSlider.UpperValueProperty);
			}
			set
			{
				base.SetValue(RangeSlider.UpperValueProperty, value);
			}
		}

		static RangeSlider()
		{
			Class6.yDnXvgqzyB5jw();
			RangeSlider.MoveBack = new RoutedUICommand("MoveBack", "MoveBack", typeof(RangeSlider), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.B, ModifierKeys.Control) }));
			RangeSlider.MoveForward = new RoutedUICommand("MoveForward", "MoveForward", typeof(RangeSlider), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.F, ModifierKeys.Control) }));
			RangeSlider.MoveAllForward = new RoutedUICommand("MoveAllForward", "MoveAllForward", typeof(RangeSlider), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.F, ModifierKeys.Alt) }));
			RangeSlider.MoveAllBack = new RoutedUICommand("MoveAllBack", "MoveAllBack", typeof(RangeSlider), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.B, ModifierKeys.Alt) }));
			RangeSlider.RangeSelectionChangedEvent = EventManager.RegisterRoutedEvent("RangeSelectionChanged", RoutingStrategy.Bubble, typeof(RangeSelectionChangedEventHandler), typeof(RangeSlider));
			RangeSlider.LowerValueChangedEvent = EventManager.RegisterRoutedEvent("LowerValueChanged", RoutingStrategy.Bubble, typeof(RangeParameterChangedEventHandler), typeof(RangeSlider));
			RangeSlider.UpperValueChangedEvent = EventManager.RegisterRoutedEvent("UpperValueChanged", RoutingStrategy.Bubble, typeof(RangeParameterChangedEventHandler), typeof(RangeSlider));
			RangeSlider.LowerThumbDragStartedEvent = EventManager.RegisterRoutedEvent("LowerThumbDragStarted", RoutingStrategy.Bubble, typeof(DragStartedEventHandler), typeof(RangeSlider));
			RangeSlider.LowerThumbDragCompletedEvent = EventManager.RegisterRoutedEvent("LowerThumbDragCompleted", RoutingStrategy.Bubble, typeof(DragCompletedEventHandler), typeof(RangeSlider));
			RangeSlider.UpperThumbDragStartedEvent = EventManager.RegisterRoutedEvent("UpperThumbDragStarted", RoutingStrategy.Bubble, typeof(DragStartedEventHandler), typeof(RangeSlider));
			RangeSlider.UpperThumbDragCompletedEvent = EventManager.RegisterRoutedEvent("UpperThumbDragCompleted", RoutingStrategy.Bubble, typeof(DragCompletedEventHandler), typeof(RangeSlider));
			RangeSlider.CentralThumbDragStartedEvent = EventManager.RegisterRoutedEvent("CentralThumbDragStarted", RoutingStrategy.Bubble, typeof(DragStartedEventHandler), typeof(RangeSlider));
			RangeSlider.CentralThumbDragCompletedEvent = EventManager.RegisterRoutedEvent("CentralThumbDragCompleted", RoutingStrategy.Bubble, typeof(DragCompletedEventHandler), typeof(RangeSlider));
			RangeSlider.LowerThumbDragDeltaEvent = EventManager.RegisterRoutedEvent("LowerThumbDragDelta", RoutingStrategy.Bubble, typeof(DragDeltaEventHandler), typeof(RangeSlider));
			RangeSlider.UpperThumbDragDeltaEvent = EventManager.RegisterRoutedEvent("UpperThumbDragDelta", RoutingStrategy.Bubble, typeof(DragDeltaEventHandler), typeof(RangeSlider));
			RangeSlider.CentralThumbDragDeltaEvent = EventManager.RegisterRoutedEvent("CentralThumbDragDelta", RoutingStrategy.Bubble, typeof(DragDeltaEventHandler), typeof(RangeSlider));
			RangeSlider.UpperValueProperty = DependencyProperty.Register("UpperValue", typeof(double), typeof(RangeSlider), new FrameworkPropertyMetadata((object)0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(RangeSlider.RangesChanged), new CoerceValueCallback(RangeSlider.CoerceUpperValue)));
			RangeSlider.LowerValueProperty = DependencyProperty.Register("LowerValue", typeof(double), typeof(RangeSlider), new FrameworkPropertyMetadata((object)0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(RangeSlider.RangesChanged), new CoerceValueCallback(RangeSlider.CoerceLowerValue)));
			RangeSlider.MinRangeProperty = DependencyProperty.Register("MinRange", typeof(double), typeof(RangeSlider), new FrameworkPropertyMetadata((object)0, new PropertyChangedCallback(RangeSlider.MinRangeChanged), new CoerceValueCallback(RangeSlider.CoerceMinRange)), new ValidateValueCallback(RangeSlider.IsValidMinRange));
			RangeSlider.MinRangeWidthProperty = DependencyProperty.Register("MinRangeWidth", typeof(double), typeof(RangeSlider), new FrameworkPropertyMetadata((object)30, new PropertyChangedCallback(RangeSlider.MinRangeWidthChanged), new CoerceValueCallback(RangeSlider.CoerceMinRangeWidth)), new ValidateValueCallback(RangeSlider.IsValidMinRange));
			RangeSlider.MoveWholeRangeProperty = DependencyProperty.Register("MoveWholeRange", typeof(bool), typeof(RangeSlider), new PropertyMetadata(false));
			RangeSlider.ExtendedModeProperty = DependencyProperty.Register("ExtendedMode", typeof(bool), typeof(RangeSlider), new PropertyMetadata(false));
			RangeSlider.IsSnapToTickEnabledProperty = DependencyProperty.Register("IsSnapToTickEnabled", typeof(bool), typeof(RangeSlider), new PropertyMetadata(false));
			RangeSlider.OrientationProperty = DependencyProperty.Register("Orientation", typeof(System.Windows.Controls.Orientation), typeof(RangeSlider), new FrameworkPropertyMetadata((object)System.Windows.Controls.Orientation.Horizontal));
			RangeSlider.TickFrequencyProperty = DependencyProperty.Register("TickFrequency", typeof(double), typeof(RangeSlider), new FrameworkPropertyMetadata((object)1), new ValidateValueCallback(RangeSlider.IsValidTickFrequency));
			RangeSlider.IsMoveToPointEnabledProperty = DependencyProperty.Register("IsMoveToPointEnabled", typeof(bool), typeof(RangeSlider), new PropertyMetadata(false));
			RangeSlider.TickPlacementProperty = DependencyProperty.Register("TickPlacement", typeof(System.Windows.Controls.Primitives.TickPlacement), typeof(RangeSlider), new FrameworkPropertyMetadata((object)System.Windows.Controls.Primitives.TickPlacement.None));
			RangeSlider.AutoToolTipPlacementProperty = DependencyProperty.Register("AutoToolTipPlacement", typeof(System.Windows.Controls.Primitives.AutoToolTipPlacement), typeof(RangeSlider), new FrameworkPropertyMetadata((object)System.Windows.Controls.Primitives.AutoToolTipPlacement.None));
			RangeSlider.AutoToolTipPrecisionProperty = DependencyProperty.Register("AutoToolTipPrecision", typeof(int), typeof(RangeSlider), new FrameworkPropertyMetadata((object)0), new ValidateValueCallback(RangeSlider.IsValidPrecision));
			RangeSlider.AutoToolTipTextConverterProperty = DependencyProperty.Register("AutoToolTipTextConverter", typeof(IValueConverter), typeof(RangeSlider), new FrameworkPropertyMetadata(null));
			RangeSlider.IntervalProperty = DependencyProperty.Register("Interval", typeof(int), typeof(RangeSlider), new FrameworkPropertyMetadata((object)100, new PropertyChangedCallback(RangeSlider.IntervalChangedCallback)), new ValidateValueCallback(RangeSlider.IsValidPrecision));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeSlider), new FrameworkPropertyMetadata(typeof(RangeSlider)));
			RangeBase.MinimumProperty.OverrideMetadata(typeof(RangeSlider), new FrameworkPropertyMetadata((object)0, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(RangeSlider.MinPropertyChangedCallback), new CoerceValueCallback(RangeSlider.CoerceMinimum)));
			RangeBase.MaximumProperty.OverrideMetadata(typeof(RangeSlider), new FrameworkPropertyMetadata((object)100, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(RangeSlider.MaxPropertyChangedCallback), new CoerceValueCallback(RangeSlider.CoerceMaximum)));
		}

		public RangeSlider()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			base.CommandBindings.Add(new CommandBinding(RangeSlider.MoveBack, new ExecutedRoutedEventHandler(this.MoveBackHandler)));
			base.CommandBindings.Add(new CommandBinding(RangeSlider.MoveForward, new ExecutedRoutedEventHandler(this.MoveForwardHandler)));
			base.CommandBindings.Add(new CommandBinding(RangeSlider.MoveAllForward, new ExecutedRoutedEventHandler(this.MoveAllForwardHandler)));
			base.CommandBindings.Add(new CommandBinding(RangeSlider.MoveAllBack, new ExecutedRoutedEventHandler(this.MoveAllBackHandler)));
			this.actualWidthPropertyChangeNotifier = new PropertyChangeNotifier(this, FrameworkElement.ActualWidthProperty);
			this.actualWidthPropertyChangeNotifier.ValueChanged += new EventHandler((object sender, EventArgs e) => this.ReCalculateSize());
			this.actualHeightPropertyChangeNotifier = new PropertyChangeNotifier(this, FrameworkElement.ActualHeightProperty);
			this.actualHeightPropertyChangeNotifier.ValueChanged += new EventHandler((object sender, EventArgs e) => this.ReCalculateSize());
			this._timer = new DispatcherTimer();
			this._timer.Tick += new EventHandler(this.MoveToNextValue);
			this._timer.Interval = TimeSpan.FromMilliseconds((double)this.Interval);
		}

		private bool ApproximatelyEquals(double value1, double value2)
		{
			return Math.Abs(value1 - value2) <= 1.53E-06;
		}

		private double CalculateNextTick(RangeSlider.Direction direction, double checkingValue, double distance, bool moveDirectlyToNextTick)
		{
			double num;
			double num1 = checkingValue - base.Minimum;
			if (!this.IsMoveToPointEnabled)
			{
				double num2 = (num1 + distance) / this.TickFrequency;
				if (!this.IsDoubleCloseToInt(num2))
				{
					distance = this.TickFrequency * (double)((int)num2);
					if (direction == RangeSlider.Direction.Increase)
					{
						distance += this.TickFrequency;
					}
					distance -= Math.Abs(num1);
					this._currenValue = 0;
					return Math.Abs(distance);
				}
			}
			if (!moveDirectlyToNextTick)
			{
				double num3 = num1 + distance / this._density;
				double tickFrequency = num3 / this.TickFrequency;
				if (direction != RangeSlider.Direction.Increase)
				{
					num = (tickFrequency.ToString(CultureInfo.InvariantCulture).ToLower().Contains("e+") ? tickFrequency * this.TickFrequency : (double)((int)tickFrequency) * this.TickFrequency);
					distance = Math.Abs(num1) - num;
				}
				else
				{
					distance = (tickFrequency.ToString(CultureInfo.InvariantCulture).ToLower().Contains("e+") ? tickFrequency * this.TickFrequency + this.TickFrequency : (double)((int)tickFrequency) * this.TickFrequency + this.TickFrequency) - Math.Abs(num1);
				}
			}
			else
			{
				distance = this.TickFrequency;
			}
			return Math.Abs(distance);
		}

		private void CenterThumbDragCompleted(object sender, DragCompletedEventArgs e)
		{
			if (this._autoToolTip != null)
			{
				this._autoToolTip.IsOpen = false;
				this._autoToolTip = null;
			}
			e.RoutedEvent = RangeSlider.CentralThumbDragCompletedEvent;
			base.RaiseEvent(e);
		}

		private void CenterThumbDragDelta(object sender, DragDeltaEventArgs e)
		{
			RangeSlider.Direction direction;
			if (!this._centerThumbBlocked)
			{
				double num = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? e.HorizontalChange : e.VerticalChange);
				if (this.IsSnapToTickEnabled)
				{
					Point position = Mouse.GetPosition(this._container);
					if (this.Orientation == System.Windows.Controls.Orientation.Horizontal)
					{
						if (position.X >= 0 && position.X < this._container.ActualWidth)
						{
							direction = (position.X > this._basePoint.X ? RangeSlider.Direction.Increase : RangeSlider.Direction.Decrease);
							this.JumpToNextTick(direction, RangeSlider.ButtonType.Both, num, (direction == RangeSlider.Direction.Increase ? this.UpperValue : this.LowerValue), false);
						}
					}
					else if (position.Y >= 0 && position.Y < this._container.ActualHeight)
					{
						direction = (position.Y < this._basePoint.Y ? RangeSlider.Direction.Increase : RangeSlider.Direction.Decrease);
						this.JumpToNextTick(direction, RangeSlider.ButtonType.Both, -num, (direction == RangeSlider.Direction.Increase ? this.UpperValue : this.LowerValue), false);
					}
				}
				else
				{
					RangeSlider.MoveThumb(this._leftButton, this._rightButton, num, this.Orientation, out this._direction);
					this.ReCalculateRangeSelected(true, true, this._direction);
				}
				this._basePoint = Mouse.GetPosition(this._container);
				if (this.AutoToolTipPlacement != System.Windows.Controls.Primitives.AutoToolTipPlacement.None)
				{
					this._autoToolTip.Content = string.Concat(this.GetLowerToolTipNumber(), " ; ", this.GetUpperToolTipNumber());
					this.RelocateAutoToolTip();
				}
			}
			e.RoutedEvent = RangeSlider.CentralThumbDragDeltaEvent;
			base.RaiseEvent(e);
		}

		private void CenterThumbDragStarted(object sender, DragStartedEventArgs e)
		{
			this._isMoved = true;
			if (this.AutoToolTipPlacement != System.Windows.Controls.Primitives.AutoToolTipPlacement.None)
			{
				if (this._autoToolTip == null)
				{
					this._autoToolTip = new System.Windows.Controls.ToolTip()
					{
						Placement = PlacementMode.Custom,
						CustomPopupPlacementCallback = new CustomPopupPlacementCallback(this.PopupPlacementCallback)
					};
				}
				this._autoToolTip.Content = string.Concat(this.GetLowerToolTipNumber(), " ; ", this.GetUpperToolTipNumber());
				this._autoToolTip.PlacementTarget = this._centerThumb;
				this._autoToolTip.IsOpen = true;
			}
			this._basePoint = Mouse.GetPosition(this._container);
			e.RoutedEvent = RangeSlider.CentralThumbDragStartedEvent;
			base.RaiseEvent(e);
		}

		private void CentralThumbMouseDown()
		{
			if (this.ExtendedMode)
			{
				if (Mouse.LeftButton == MouseButtonState.Pressed && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
				{
					this._centerThumbBlocked = true;
					Point position = Mouse.GetPosition(this._visualElementsContainer);
					double num = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? position.X + this._leftThumb.ActualWidth / 2 - (this._leftButton.ActualWidth + this._leftThumb.ActualWidth) : -(base.ActualHeight - (position.Y + this._leftThumb.ActualHeight / 2 + this._leftButton.ActualHeight)));
					if (!this.IsSnapToTickEnabled)
					{
						if (this.IsMoveToPointEnabled && !this.MoveWholeRange)
						{
							RangeSlider.MoveThumb(this._leftButton, this._centerThumb, num, this.Orientation, out this._direction);
							this.ReCalculateRangeSelected(true, false, this._direction);
						}
						else if (this.IsMoveToPointEnabled && this.MoveWholeRange)
						{
							RangeSlider.MoveThumb(this._leftButton, this._rightButton, num, this.Orientation, out this._direction);
							this.ReCalculateRangeSelected(true, true, this._direction);
						}
					}
					else if (this.IsMoveToPointEnabled && !this.MoveWholeRange)
					{
						this.JumpToNextTick(RangeSlider.Direction.Increase, RangeSlider.ButtonType.BottomLeft, num, this.LowerValue, true);
					}
					else if (this.IsMoveToPointEnabled && this.MoveWholeRange)
					{
						this.JumpToNextTick(RangeSlider.Direction.Increase, RangeSlider.ButtonType.Both, num, this.LowerValue, true);
					}
					if (!this.IsMoveToPointEnabled)
					{
						this._position = Mouse.GetPosition(this._visualElementsContainer);
						this._bType = (this.MoveWholeRange ? RangeSlider.ButtonType.Both : RangeSlider.ButtonType.BottomLeft);
						this._currentpoint = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? this._position.X : this._position.Y);
						this._currenValue = this.LowerValue;
						this._direction = RangeSlider.Direction.Increase;
						this._isInsideRange = true;
						this._timer.Start();
						return;
					}
				}
				else if (Mouse.RightButton == MouseButtonState.Pressed && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
				{
					this._centerThumbBlocked = true;
					Point point = Mouse.GetPosition(this._visualElementsContainer);
					double num1 = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? base.ActualWidth - (point.X + this._rightThumb.ActualWidth / 2 + this._rightButton.ActualWidth) : -(point.Y + this._rightThumb.ActualHeight / 2 - (this._rightButton.ActualHeight + this._rightThumb.ActualHeight)));
					if (!this.IsSnapToTickEnabled)
					{
						if (this.IsMoveToPointEnabled && !this.MoveWholeRange)
						{
							RangeSlider.MoveThumb(this._centerThumb, this._rightButton, -num1, this.Orientation, out this._direction);
							this.ReCalculateRangeSelected(false, true, this._direction);
						}
						else if (this.IsMoveToPointEnabled && this.MoveWholeRange)
						{
							RangeSlider.MoveThumb(this._leftButton, this._rightButton, -num1, this.Orientation, out this._direction);
							this.ReCalculateRangeSelected(true, true, this._direction);
						}
					}
					else if (this.IsMoveToPointEnabled && !this.MoveWholeRange)
					{
						this.JumpToNextTick(RangeSlider.Direction.Decrease, RangeSlider.ButtonType.TopRight, -num1, this.UpperValue, true);
					}
					else if (this.IsMoveToPointEnabled && this.MoveWholeRange)
					{
						this.JumpToNextTick(RangeSlider.Direction.Decrease, RangeSlider.ButtonType.Both, -num1, this.UpperValue, true);
					}
					if (!this.IsMoveToPointEnabled)
					{
						this._position = Mouse.GetPosition(this._visualElementsContainer);
						this._bType = (this.MoveWholeRange ? RangeSlider.ButtonType.Both : RangeSlider.ButtonType.TopRight);
						this._currentpoint = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? this._position.X : this._position.Y);
						this._currenValue = this.UpperValue;
						this._direction = RangeSlider.Direction.Decrease;
						this._isInsideRange = true;
						this._timer.Start();
					}
				}
			}
		}

		private static object CoerceLowerValue(DependencyObject d, object basevalue)
		{
			RangeSlider rangeSlider = (RangeSlider)d;
			double num = (double)basevalue;
			if (num < rangeSlider.Minimum || rangeSlider.UpperValue - rangeSlider.MinRange < rangeSlider.Minimum)
			{
				return rangeSlider.Minimum;
			}
			if (num <= rangeSlider.UpperValue - rangeSlider.MinRange)
			{
				return basevalue;
			}
			return rangeSlider.UpperValue - rangeSlider.MinRange;
		}

		private static object CoerceMaximum(DependencyObject d, object basevalue)
		{
			RangeSlider rangeSlider = (RangeSlider)d;
			if ((double)basevalue >= rangeSlider.Minimum)
			{
				return basevalue;
			}
			return rangeSlider.Minimum;
		}

		private static object CoerceMinimum(DependencyObject d, object basevalue)
		{
			RangeSlider rangeSlider = (RangeSlider)d;
			if ((double)basevalue <= rangeSlider.Maximum)
			{
				return basevalue;
			}
			return rangeSlider.Maximum;
		}

		private static object CoerceMinRange(DependencyObject d, object basevalue)
		{
			RangeSlider rangeSlider = (RangeSlider)d;
			double num = (double)basevalue;
			if (rangeSlider.LowerValue + num <= rangeSlider.Maximum)
			{
				return basevalue;
			}
			return rangeSlider.Maximum - rangeSlider.LowerValue;
		}

		private static object CoerceMinRangeWidth(DependencyObject d, object basevalue)
		{
			double num;
			RangeSlider rangeSlider = (RangeSlider)d;
			if (rangeSlider._leftThumb == null || rangeSlider._rightThumb == null)
			{
				return basevalue;
			}
			num = (rangeSlider.Orientation != System.Windows.Controls.Orientation.Horizontal ? rangeSlider.ActualHeight - rangeSlider._leftThumb.ActualHeight - rangeSlider._rightThumb.ActualHeight : rangeSlider.ActualWidth - rangeSlider._leftThumb.ActualWidth - rangeSlider._rightThumb.ActualWidth);
			return ((double)basevalue > num / 2 ? num / 2 : (double)basevalue);
		}

		private static object CoerceUpperValue(DependencyObject d, object basevalue)
		{
			RangeSlider rangeSlider = (RangeSlider)d;
			double num = (double)basevalue;
			if (num > rangeSlider.Maximum || rangeSlider.LowerValue + rangeSlider.MinRange > rangeSlider.Maximum)
			{
				return rangeSlider.Maximum;
			}
			if (num >= rangeSlider.LowerValue + rangeSlider.MinRange)
			{
				return basevalue;
			}
			return rangeSlider.LowerValue + rangeSlider.MinRange;
		}

		private T EnforceInstance<T>(string partName)
		where T : FrameworkElement, new()
		{
			T templateChild = (T)(base.GetTemplateChild(partName) as T);
			if (templateChild == null)
			{
				templateChild = Activator.CreateInstance<T>();
			}
			return templateChild;
		}

		private static double GetChangeKeepPositive(double width, double increment)
		{
			return Math.Max(width + increment, 0) - width;
		}

		private string GetLowerToolTipNumber()
		{
			return this.GetToolTipNumber(this.LowerValue);
		}

		private bool GetResult(double currentPoint, double endPoint, RangeSlider.Direction direction)
		{
			if (direction == RangeSlider.Direction.Increase)
			{
				if (this.Orientation == System.Windows.Controls.Orientation.Horizontal && currentPoint > endPoint)
				{
					return true;
				}
				if (this.Orientation != System.Windows.Controls.Orientation.Vertical)
				{
					return false;
				}
				return currentPoint < endPoint;
			}
			if (this.Orientation == System.Windows.Controls.Orientation.Horizontal && currentPoint < endPoint)
			{
				return true;
			}
			if (this.Orientation != System.Windows.Controls.Orientation.Vertical)
			{
				return false;
			}
			return currentPoint > endPoint;
		}

		private string GetToolTipNumber(double value)
		{
			IValueConverter autoToolTipTextConverter = this.AutoToolTipTextConverter;
			if (autoToolTipTextConverter != null)
			{
				object obj = autoToolTipTextConverter.Convert(value, typeof(string), null, CultureInfo.InvariantCulture);
				if (obj != null)
				{
					return obj.ToString();
				}
			}
			NumberFormatInfo autoToolTipPrecision = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
			autoToolTipPrecision.NumberDecimalDigits = this.AutoToolTipPrecision;
			return value.ToString("N", autoToolTipPrecision);
		}

		private string GetUpperToolTipNumber()
		{
			return this.GetToolTipNumber(this.UpperValue);
		}

		private void InitializeVisualElementsContainer()
		{
			this._leftThumb.DragCompleted += new DragCompletedEventHandler(this.LeftThumbDragComplete);
			this._rightThumb.DragCompleted += new DragCompletedEventHandler(this.RightThumbDragComplete);
			this._leftThumb.DragStarted += new DragStartedEventHandler(this.LeftThumbDragStart);
			this._rightThumb.DragStarted += new DragStartedEventHandler(this.RightThumbDragStart);
			this._centerThumb.DragStarted += new DragStartedEventHandler(this.CenterThumbDragStarted);
			this._centerThumb.DragCompleted += new DragCompletedEventHandler(this.CenterThumbDragCompleted);
			this._centerThumb.DragDelta += new DragDeltaEventHandler(this.CenterThumbDragDelta);
			this._leftThumb.DragDelta += new DragDeltaEventHandler(this.LeftThumbDragDelta);
			this._rightThumb.DragDelta += new DragDeltaEventHandler(this.RightThumbDragDelta);
			this._visualElementsContainer.PreviewMouseDown += new MouseButtonEventHandler(this.VisualElementsContainerPreviewMouseDown);
			this._visualElementsContainer.PreviewMouseUp += new MouseButtonEventHandler(this.VisualElementsContainerPreviewMouseUp);
			this._visualElementsContainer.MouseLeave += new MouseEventHandler(this.VisualElementsContainerMouseLeave);
			this._visualElementsContainer.MouseDown += new MouseButtonEventHandler(this.VisualElementsContainerMouseDown);
		}

		private static void IntervalChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			RangeSlider rangeSlider = (RangeSlider)dependencyObject;
			rangeSlider._timer.Interval = TimeSpan.FromMilliseconds((double)((int)e.NewValue));
		}

		private bool IsDoubleCloseToInt(double val)
		{
			return this.ApproximatelyEquals(Math.Abs(val - Math.Round(val)), 0);
		}

		private bool IsValidDouble(double d)
		{
			if (!double.IsNaN(d) && !double.IsInfinity(d))
			{
				return true;
			}
			return false;
		}

		private static bool IsValidMinRange(object value)
		{
			double num = (double)value;
			if (num >= 0 && !double.IsInfinity(num) && !double.IsNaN(num))
			{
				return true;
			}
			return false;
		}

		private static bool IsValidPrecision(object value)
		{
			return (int)value >= 0;
		}

		private static bool IsValidTickFrequency(object value)
		{
			double num = (double)value;
			if (num > 0 && !double.IsInfinity(num) && !double.IsNaN(num))
			{
				return true;
			}
			return false;
		}

		private void JumpToNextTick(RangeSlider.Direction direction, RangeSlider.ButtonType type, double distance, double checkingValue, bool jumpDirectlyToTick)
		{
			double num = this.CalculateNextTick(direction, checkingValue, distance, false);
			Point position = Mouse.GetPosition(this._visualElementsContainer);
			double num1 = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? position.X : position.Y);
			double num2 = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? base.ActualWidth : base.ActualHeight);
			double num3 = (direction == RangeSlider.Direction.Increase ? this.TickFrequency * this._density : -this.TickFrequency * this._density);
			if (jumpDirectlyToTick)
			{
				this.SnapToTickHandle(type, direction, num);
				return;
			}
			if (direction == RangeSlider.Direction.Increase)
			{
				if (!this.IsDoubleCloseToInt(checkingValue / this.TickFrequency))
				{
					if (distance > num * this._density / 2 || distance >= num2 - num1 || distance >= num1)
					{
						this.SnapToTickHandle(type, direction, num);
						return;
					}
				}
				else if (distance > num3 / 2 || distance >= num2 - num1 || distance >= num1)
				{
					this.SnapToTickHandle(type, direction, num);
					return;
				}
			}
			else if (!this.IsDoubleCloseToInt(checkingValue / this.TickFrequency))
			{
				if (distance <= -(num * this._density) / 2 || this.UpperValue - this.LowerValue < num)
				{
					this.SnapToTickHandle(type, direction, num);
					return;
				}
			}
			else if (distance < num3 / 2 || this.UpperValue - this.LowerValue < num)
			{
				this.SnapToTickHandle(type, direction, num);
			}
		}

		private void LeftButtonMouseDown()
		{
			if (Mouse.LeftButton == MouseButtonState.Pressed)
			{
				Point position = Mouse.GetPosition(this._visualElementsContainer);
				double num = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? this._leftButton.ActualWidth - position.X + this._leftThumb.ActualWidth / 2 : -(this._leftButton.ActualHeight - (base.ActualHeight - (position.Y + this._leftThumb.ActualHeight / 2))));
				if (!this.IsSnapToTickEnabled)
				{
					if (this.IsMoveToPointEnabled && !this.MoveWholeRange)
					{
						RangeSlider.MoveThumb(this._leftButton, this._centerThumb, -num, this.Orientation, out this._direction);
						this.ReCalculateRangeSelected(true, false, this._direction);
					}
					else if (this.IsMoveToPointEnabled && this.MoveWholeRange)
					{
						RangeSlider.MoveThumb(this._leftButton, this._rightButton, -num, this.Orientation, out this._direction);
						this.ReCalculateRangeSelected(true, true, this._direction);
					}
				}
				else if (this.IsMoveToPointEnabled && !this.MoveWholeRange)
				{
					this.JumpToNextTick(RangeSlider.Direction.Decrease, RangeSlider.ButtonType.BottomLeft, -num, this.LowerValue, true);
				}
				else if (this.IsMoveToPointEnabled && this.MoveWholeRange)
				{
					this.JumpToNextTick(RangeSlider.Direction.Decrease, RangeSlider.ButtonType.Both, -num, this.LowerValue, true);
				}
				if (!this.IsMoveToPointEnabled)
				{
					this._position = Mouse.GetPosition(this._visualElementsContainer);
					this._bType = (this.MoveWholeRange ? RangeSlider.ButtonType.Both : RangeSlider.ButtonType.BottomLeft);
					this._currentpoint = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? this._position.X : this._position.Y);
					this._currenValue = this.LowerValue;
					this._isInsideRange = false;
					this._direction = RangeSlider.Direction.Decrease;
					this._timer.Start();
				}
			}
		}

		private void LeftThumbDragComplete(object sender, DragCompletedEventArgs e)
		{
			if (this._autoToolTip != null)
			{
				this._autoToolTip.IsOpen = false;
				this._autoToolTip = null;
			}
			e.RoutedEvent = RangeSlider.LowerThumbDragCompletedEvent;
			base.RaiseEvent(e);
		}

		private void LeftThumbDragDelta(object sender, DragDeltaEventArgs e)
		{
			double num = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? e.HorizontalChange : e.VerticalChange);
			if (this.IsSnapToTickEnabled)
			{
				Point position = Mouse.GetPosition(this._container);
				if (this.Orientation == System.Windows.Controls.Orientation.Horizontal)
				{
					if (position.X >= 0 && position.X < this._container.ActualWidth - (this._rightButton.ActualWidth + this._rightThumb.ActualWidth + this._centerThumb.MinWidth))
					{
						this.JumpToNextTick((position.X > this._basePoint.X ? RangeSlider.Direction.Increase : RangeSlider.Direction.Decrease), RangeSlider.ButtonType.BottomLeft, num, this.LowerValue, false);
					}
				}
				else if (position.Y <= this._container.ActualHeight && position.Y > this._rightButton.ActualHeight + this._rightThumb.ActualHeight + this._centerThumb.MinHeight)
				{
					RangeSlider.Direction direction = (position.Y < this._basePoint.Y ? RangeSlider.Direction.Increase : RangeSlider.Direction.Decrease);
					this.JumpToNextTick(direction, RangeSlider.ButtonType.BottomLeft, -num, this.LowerValue, false);
				}
			}
			else
			{
				RangeSlider.MoveThumb(this._leftButton, this._centerThumb, num, this.Orientation, out this._direction);
				this.ReCalculateRangeSelected(true, false, this._direction);
			}
			this._basePoint = Mouse.GetPosition(this._container);
			if (this.AutoToolTipPlacement != System.Windows.Controls.Primitives.AutoToolTipPlacement.None)
			{
				this._autoToolTip.Content = this.GetLowerToolTipNumber();
				this.RelocateAutoToolTip();
			}
			e.RoutedEvent = RangeSlider.LowerThumbDragDeltaEvent;
			base.RaiseEvent(e);
		}

		private void LeftThumbDragStart(object sender, DragStartedEventArgs e)
		{
			this._isMoved = true;
			if (this.AutoToolTipPlacement != System.Windows.Controls.Primitives.AutoToolTipPlacement.None)
			{
				if (this._autoToolTip == null)
				{
					this._autoToolTip = new System.Windows.Controls.ToolTip()
					{
						Placement = PlacementMode.Custom,
						CustomPopupPlacementCallback = new CustomPopupPlacementCallback(this.PopupPlacementCallback)
					};
				}
				this._autoToolTip.Content = this.GetLowerToolTipNumber();
				this._autoToolTip.PlacementTarget = this._leftThumb;
				this._autoToolTip.IsOpen = true;
			}
			this._basePoint = Mouse.GetPosition(this._container);
			e.RoutedEvent = RangeSlider.LowerThumbDragStartedEvent;
			base.RaiseEvent(e);
		}

		private static void MaxPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			dependencyObject.CoerceValue(RangeBase.MaximumProperty);
			dependencyObject.CoerceValue(RangeBase.MinimumProperty);
			dependencyObject.CoerceValue(RangeSlider.UpperValueProperty);
		}

		private static void MinPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			dependencyObject.CoerceValue(RangeBase.MinimumProperty);
			dependencyObject.CoerceValue(RangeBase.MaximumProperty);
			dependencyObject.CoerceValue(RangeSlider.LowerValueProperty);
		}

		private static void MinRangeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			double newValue = (double)e.NewValue;
			if (newValue < 0)
			{
				newValue = 0;
			}
			RangeSlider lowerValue = (RangeSlider)dependencyObject;
			dependencyObject.CoerceValue(RangeSlider.MinRangeProperty);
			lowerValue._internalUpdate = true;
			lowerValue.UpperValue = Math.Max(lowerValue.UpperValue, lowerValue.LowerValue + newValue);
			lowerValue.UpperValue = Math.Min(lowerValue.UpperValue, lowerValue.Maximum);
			lowerValue._internalUpdate = false;
			lowerValue.CoerceValue(RangeSlider.UpperValueProperty);
			RangeSlider.RaiseValueChangedEvents(dependencyObject, true, true);
			lowerValue._oldLower = lowerValue.LowerValue;
			lowerValue._oldUpper = lowerValue.UpperValue;
			lowerValue.ReCalculateSize();
		}

		private static void MinRangeWidthChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			((RangeSlider)sender).ReCalculateSize();
		}

		private void MoveAllBackHandler(object sender, ExecutedRoutedEventArgs e)
		{
			this.ResetSelection(true);
		}

		private void MoveAllForwardHandler(object sender, ExecutedRoutedEventArgs e)
		{
			this.ResetSelection(false);
		}

		private void MoveBackHandler(object sender, ExecutedRoutedEventArgs e)
		{
			this.MoveSelection(true);
		}

		private void MoveForwardHandler(object sender, ExecutedRoutedEventArgs e)
		{
			this.MoveSelection(false);
		}

		public void MoveSelection(bool isLeft)
		{
			double smallChange = base.SmallChange * (this.UpperValue - this.LowerValue) * this._movableWidth / this.MovableRange;
			smallChange = (isLeft ? -smallChange : smallChange);
			RangeSlider.MoveThumb(this._leftButton, this._rightButton, smallChange, this.Orientation, out this._direction);
			this.ReCalculateRangeSelected(true, true, this._direction);
		}

		private static void MoveThumb(FrameworkElement x, FrameworkElement y, double change, System.Windows.Controls.Orientation orientation)
		{
			RangeSlider.Direction direction = RangeSlider.Direction.Increase;
			RangeSlider.MoveThumb(x, y, change, orientation, out direction);
		}

		private static void MoveThumb(FrameworkElement x, FrameworkElement y, double change, System.Windows.Controls.Orientation orientation, out RangeSlider.Direction direction)
		{
			direction = RangeSlider.Direction.Increase;
			if (orientation == System.Windows.Controls.Orientation.Horizontal)
			{
				direction = (change < 0 ? RangeSlider.Direction.Decrease : RangeSlider.Direction.Increase);
				RangeSlider.MoveThumbHorizontal(x, y, change);
				return;
			}
			if (orientation == System.Windows.Controls.Orientation.Vertical)
			{
				direction = (change < 0 ? RangeSlider.Direction.Increase : RangeSlider.Direction.Decrease);
				RangeSlider.MoveThumbVertical(x, y, change);
			}
		}

		private static void MoveThumbHorizontal(FrameworkElement x, FrameworkElement y, double horizonalChange)
		{
			if (!double.IsNaN(x.Width) && !double.IsNaN(y.Width))
			{
				if (horizonalChange < 0)
				{
					double changeKeepPositive = RangeSlider.GetChangeKeepPositive(x.Width, horizonalChange);
					if (x.Name != "PART_MiddleThumb")
					{
						FrameworkElement width = x;
						width.Width = width.Width + changeKeepPositive;
						FrameworkElement frameworkElement = y;
						frameworkElement.Width = frameworkElement.Width - changeKeepPositive;
						return;
					}
					if (x.Width > x.MinWidth)
					{
						if (x.Width + changeKeepPositive >= x.MinWidth)
						{
							FrameworkElement width1 = x;
							width1.Width = width1.Width + changeKeepPositive;
							FrameworkElement frameworkElement1 = y;
							frameworkElement1.Width = frameworkElement1.Width - changeKeepPositive;
							return;
						}
						double num = x.Width - x.MinWidth;
						x.Width = x.MinWidth;
						FrameworkElement width2 = y;
						width2.Width = width2.Width + num;
						return;
					}
				}
				else if (horizonalChange > 0)
				{
					double changeKeepPositive1 = -RangeSlider.GetChangeKeepPositive(y.Width, -horizonalChange);
					if (y.Name != "PART_MiddleThumb")
					{
						FrameworkElement frameworkElement2 = x;
						frameworkElement2.Width = frameworkElement2.Width + changeKeepPositive1;
						FrameworkElement width3 = y;
						width3.Width = width3.Width - changeKeepPositive1;
					}
					else if (y.Width > y.MinWidth)
					{
						if (y.Width - changeKeepPositive1 >= y.MinWidth)
						{
							FrameworkElement frameworkElement3 = x;
							frameworkElement3.Width = frameworkElement3.Width + changeKeepPositive1;
							FrameworkElement width4 = y;
							width4.Width = width4.Width - changeKeepPositive1;
							return;
						}
						double num1 = y.Width - y.MinWidth;
						y.Width = y.MinWidth;
						FrameworkElement frameworkElement4 = x;
						frameworkElement4.Width = frameworkElement4.Width + num1;
						return;
					}
				}
			}
		}

		private static void MoveThumbVertical(FrameworkElement x, FrameworkElement y, double verticalChange)
		{
			if (!double.IsNaN(x.Height) && !double.IsNaN(y.Height))
			{
				if (verticalChange < 0)
				{
					double changeKeepPositive = -RangeSlider.GetChangeKeepPositive(y.Height, verticalChange);
					if (y.Name != "PART_MiddleThumb")
					{
						FrameworkElement height = x;
						height.Height = height.Height + changeKeepPositive;
						FrameworkElement frameworkElement = y;
						frameworkElement.Height = frameworkElement.Height - changeKeepPositive;
						return;
					}
					if (y.Height > y.MinHeight)
					{
						if (y.Height - changeKeepPositive >= y.MinHeight)
						{
							FrameworkElement height1 = x;
							height1.Height = height1.Height + changeKeepPositive;
							FrameworkElement frameworkElement1 = y;
							frameworkElement1.Height = frameworkElement1.Height - changeKeepPositive;
							return;
						}
						double num = y.Height - y.MinHeight;
						y.Height = y.MinHeight;
						FrameworkElement height2 = x;
						height2.Height = height2.Height + num;
						return;
					}
				}
				else if (verticalChange > 0)
				{
					double changeKeepPositive1 = RangeSlider.GetChangeKeepPositive(x.Height, -verticalChange);
					if (x.Name != "PART_MiddleThumb")
					{
						FrameworkElement frameworkElement2 = x;
						frameworkElement2.Height = frameworkElement2.Height + changeKeepPositive1;
						FrameworkElement height3 = y;
						height3.Height = height3.Height - changeKeepPositive1;
					}
					else if (x.Height > y.MinHeight)
					{
						if (x.Height + changeKeepPositive1 >= x.MinHeight)
						{
							FrameworkElement frameworkElement3 = x;
							frameworkElement3.Height = frameworkElement3.Height + changeKeepPositive1;
							FrameworkElement height4 = y;
							height4.Height = height4.Height - changeKeepPositive1;
							return;
						}
						double num1 = x.Height - x.MinHeight;
						x.Height = x.MinHeight;
						FrameworkElement frameworkElement4 = y;
						frameworkElement4.Height = frameworkElement4.Height + num1;
						return;
					}
				}
			}
		}

		private void MoveToNextValue(object sender, EventArgs e)
		{
			double smallChange;
			this._position = Mouse.GetPosition(this._visualElementsContainer);
			this._currentpoint = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? this._position.X : this._position.Y);
			double num = this.UpdateEndPoint(this._bType, this._direction);
			bool result = this.GetResult(this._currentpoint, num, this._direction);
			if (this.IsSnapToTickEnabled)
			{
				smallChange = this.CalculateNextTick(this._direction, this._currenValue, 0, true);
				double num1 = smallChange;
				smallChange = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? smallChange : -smallChange);
				if (this._direction == RangeSlider.Direction.Increase)
				{
					if (result)
					{
						switch (this._bType)
						{
							case RangeSlider.ButtonType.BottomLeft:
							{
								RangeSlider.MoveThumb(this._leftButton, this._centerThumb, smallChange * this._density, this.Orientation);
								this.ReCalculateRangeSelected(true, false, this.LowerValue + num1, this._direction);
								break;
							}
							case RangeSlider.ButtonType.TopRight:
							{
								RangeSlider.MoveThumb(this._centerThumb, this._rightButton, smallChange * this._density, this.Orientation);
								this.ReCalculateRangeSelected(false, true, this.UpperValue + num1, this._direction);
								break;
							}
							case RangeSlider.ButtonType.Both:
							{
								RangeSlider.MoveThumb(this._leftButton, this._rightButton, smallChange * this._density, this.Orientation);
								this.ReCalculateRangeSelected(this.LowerValue + num1, this.UpperValue + num1, this._direction);
								break;
							}
						}
					}
				}
				else if (this._direction == RangeSlider.Direction.Decrease && result)
				{
					switch (this._bType)
					{
						case RangeSlider.ButtonType.BottomLeft:
						{
							RangeSlider.MoveThumb(this._leftButton, this._centerThumb, -smallChange * this._density, this.Orientation);
							this.ReCalculateRangeSelected(true, false, this.LowerValue - num1, this._direction);
							break;
						}
						case RangeSlider.ButtonType.TopRight:
						{
							RangeSlider.MoveThumb(this._centerThumb, this._rightButton, -smallChange * this._density, this.Orientation);
							this.ReCalculateRangeSelected(false, true, this.UpperValue - num1, this._direction);
							break;
						}
						case RangeSlider.ButtonType.Both:
						{
							RangeSlider.MoveThumb(this._leftButton, this._rightButton, -smallChange * this._density, this.Orientation);
							this.ReCalculateRangeSelected(this.LowerValue - num1, this.UpperValue - num1, this._direction);
							break;
						}
					}
				}
			}
			else
			{
				smallChange = base.SmallChange;
				if (this._tickCount > 5)
				{
					smallChange = base.LargeChange;
				}
				this._roundToPrecision = true;
				if (smallChange.ToString(CultureInfo.InvariantCulture).ToLower().Contains("e") || !smallChange.ToString(CultureInfo.InvariantCulture).Contains("."))
				{
					this._precision = 0;
				}
				else
				{
					string[] strArrays = smallChange.ToString(CultureInfo.InvariantCulture).Split(new char[] { '.' });
					this._precision = strArrays[1].Length;
				}
				smallChange = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? smallChange : -smallChange);
				smallChange = (this._direction == RangeSlider.Direction.Increase ? smallChange : -smallChange);
				if (result)
				{
					switch (this._bType)
					{
						case RangeSlider.ButtonType.BottomLeft:
						{
							RangeSlider.MoveThumb(this._leftButton, this._centerThumb, smallChange * this._density, this.Orientation, out this._direction);
							this.ReCalculateRangeSelected(true, false, this._direction);
							break;
						}
						case RangeSlider.ButtonType.TopRight:
						{
							RangeSlider.MoveThumb(this._centerThumb, this._rightButton, smallChange * this._density, this.Orientation, out this._direction);
							this.ReCalculateRangeSelected(false, true, this._direction);
							break;
						}
						case RangeSlider.ButtonType.Both:
						{
							RangeSlider.MoveThumb(this._leftButton, this._rightButton, smallChange * this._density, this.Orientation, out this._direction);
							this.ReCalculateRangeSelected(true, true, this._direction);
							break;
						}
					}
				}
			}
			this._tickCount++;
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this._container = this.EnforceInstance<StackPanel>("PART_Container");
			this._visualElementsContainer = this.EnforceInstance<StackPanel>("PART_RangeSliderContainer");
			this._centerThumb = this.EnforceInstance<Thumb>("PART_MiddleThumb");
			this._leftButton = this.EnforceInstance<RepeatButton>("PART_LeftEdge");
			this._rightButton = this.EnforceInstance<RepeatButton>("PART_RightEdge");
			this._leftThumb = this.EnforceInstance<Thumb>("PART_LeftThumb");
			this._rightThumb = this.EnforceInstance<Thumb>("PART_RightThumb");
			this.InitializeVisualElementsContainer();
			this.ReCalculateSize();
		}

		protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
		{
			this.ReCalculateSize();
		}

		protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
		{
			this.ReCalculateSize();
		}

		private void OnRangeParameterChanged(RangeParameterChangedEventArgs e, RoutedEvent Event)
		{
			e.RoutedEvent = Event;
			base.RaiseEvent(e);
		}

		private void OnRangeSelectionChanged(RangeSelectionChangedEventArgs e)
		{
			e.RoutedEvent = RangeSlider.RangeSelectionChangedEvent;
			base.RaiseEvent(e);
		}

		private CustomPopupPlacement[] PopupPlacementCallback(Size popupSize, Size targetSize, Point offset)
		{
			System.Windows.Controls.Primitives.AutoToolTipPlacement autoToolTipPlacement = this.AutoToolTipPlacement;
			if (autoToolTipPlacement == System.Windows.Controls.Primitives.AutoToolTipPlacement.TopLeft)
			{
				if (this.Orientation == System.Windows.Controls.Orientation.Horizontal)
				{
					return new CustomPopupPlacement[] { new CustomPopupPlacement(new Point((targetSize.Width - popupSize.Width) * 0.5, -popupSize.Height), PopupPrimaryAxis.Horizontal) };
				}
				return new CustomPopupPlacement[] { new CustomPopupPlacement(new Point(-popupSize.Width, (targetSize.Height - popupSize.Height) * 0.5), PopupPrimaryAxis.Vertical) };
			}
			if (autoToolTipPlacement != System.Windows.Controls.Primitives.AutoToolTipPlacement.BottomRight)
			{
				return new CustomPopupPlacement[0];
			}
			if (this.Orientation == System.Windows.Controls.Orientation.Horizontal)
			{
				return new CustomPopupPlacement[] { new CustomPopupPlacement(new Point((targetSize.Width - popupSize.Width) * 0.5, targetSize.Height), PopupPrimaryAxis.Horizontal) };
			}
			return new CustomPopupPlacement[] { new CustomPopupPlacement(new Point(targetSize.Width, (targetSize.Height - popupSize.Height) * 0.5), PopupPrimaryAxis.Vertical) };
		}

		private static void RaiseValueChangedEvents(DependencyObject dependencyObject, bool lowerValueReCalculated = true, bool upperValueReCalculated = true)
		{
			RangeSlider rangeSlider = (RangeSlider)dependencyObject;
			bool flag = object.Equals(rangeSlider._oldLower, rangeSlider.LowerValue);
			bool flag1 = object.Equals(rangeSlider._oldUpper, rangeSlider.UpperValue);
			if (lowerValueReCalculated | upperValueReCalculated && (!flag || !flag1))
			{
				rangeSlider.OnRangeSelectionChanged(new RangeSelectionChangedEventArgs(rangeSlider.LowerValue, rangeSlider.UpperValue, rangeSlider._oldLower, rangeSlider._oldUpper));
			}
			if (lowerValueReCalculated && !flag)
			{
				rangeSlider.OnRangeParameterChanged(new RangeParameterChangedEventArgs(RangeParameterChangeType.Lower, rangeSlider._oldLower, rangeSlider.LowerValue), RangeSlider.LowerValueChangedEvent);
			}
			if (upperValueReCalculated && !flag1)
			{
				rangeSlider.OnRangeParameterChanged(new RangeParameterChangedEventArgs(RangeParameterChangeType.Upper, rangeSlider._oldUpper, rangeSlider.UpperValue), RangeSlider.UpperValueChangedEvent);
			}
		}

		private static void RangesChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			RangeSlider lowerValue = (RangeSlider)dependencyObject;
			if (lowerValue._internalUpdate)
			{
				return;
			}
			dependencyObject.CoerceValue(RangeSlider.UpperValueProperty);
			dependencyObject.CoerceValue(RangeSlider.LowerValueProperty);
			RangeSlider.RaiseValueChangedEvents(dependencyObject, true, true);
			lowerValue._oldLower = lowerValue.LowerValue;
			lowerValue._oldUpper = lowerValue.UpperValue;
			lowerValue.ReCalculateSize();
		}

		private void ReCalculateRangeSelected(bool reCalculateLowerValue, bool reCalculateUpperValue, RangeSlider.Direction direction)
		{
			double num;
			double num1;
			double num2;
			double num3;
			this._internalUpdate = true;
			if (direction != RangeSlider.Direction.Increase)
			{
				if (reCalculateLowerValue)
				{
					this._oldLower = this.LowerValue;
					double num4 = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? this._leftButton.Width : this._leftButton.Height);
					if (this.IsValidDouble(num4))
					{
						double num5 = (object.Equals(num4, 0) ? base.Minimum : Math.Max(base.Minimum, base.Minimum + this.MovableRange * num4 / this._movableWidth));
						if (this._isMoved)
						{
							num1 = num5;
						}
						else
						{
							num1 = (this._roundToPrecision ? Math.Round(num5, this._precision) : num5);
						}
						this.LowerValue = num1;
					}
				}
				if (reCalculateUpperValue)
				{
					this._oldUpper = this.UpperValue;
					double num6 = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? this._rightButton.Width : this._rightButton.Height);
					if (this.IsValidDouble(num6))
					{
						double num7 = (object.Equals(num6, 0) ? base.Maximum : Math.Min(base.Maximum, base.Maximum - this.MovableRange * num6 / this._movableWidth));
						if (this._isMoved)
						{
							num = num7;
						}
						else
						{
							num = (this._roundToPrecision ? Math.Round(num7, this._precision) : num7);
						}
						this.UpperValue = num;
					}
				}
			}
			else
			{
				if (reCalculateUpperValue)
				{
					this._oldUpper = this.UpperValue;
					double num8 = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? this._rightButton.Width : this._rightButton.Height);
					if (this.IsValidDouble(num8))
					{
						double num9 = (object.Equals(num8, 0) ? base.Maximum : Math.Min(base.Maximum, base.Maximum - this.MovableRange * num8 / this._movableWidth));
						if (this._isMoved)
						{
							num3 = num9;
						}
						else
						{
							num3 = (this._roundToPrecision ? Math.Round(num9, this._precision) : num9);
						}
						this.UpperValue = num3;
					}
				}
				if (reCalculateLowerValue)
				{
					this._oldLower = this.LowerValue;
					double num10 = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? this._leftButton.Width : this._leftButton.Height);
					if (this.IsValidDouble(num10))
					{
						double num11 = (object.Equals(num10, 0) ? base.Minimum : Math.Max(base.Minimum, base.Minimum + this.MovableRange * num10 / this._movableWidth));
						if (this._isMoved)
						{
							num2 = num11;
						}
						else
						{
							num2 = (this._roundToPrecision ? Math.Round(num11, this._precision) : num11);
						}
						this.LowerValue = num2;
					}
				}
			}
			this._roundToPrecision = false;
			this._internalUpdate = false;
			RangeSlider.RaiseValueChangedEvents(this, reCalculateLowerValue, reCalculateUpperValue);
		}

		private void ReCalculateRangeSelected(bool reCalculateLowerValue, bool reCalculateUpperValue, double value, RangeSlider.Direction direction)
		{
			this._internalUpdate = true;
			string str = this.TickFrequency.ToString(CultureInfo.InvariantCulture);
			if (reCalculateLowerValue)
			{
				this._oldLower = this.LowerValue;
				double num = 0;
				if (this.IsSnapToTickEnabled)
				{
					num = (direction == RangeSlider.Direction.Increase ? Math.Min(this.UpperValue - this.MinRange, value) : Math.Max(base.Minimum, value));
				}
				if (str.ToLower().Contains("e+") || !str.Contains("."))
				{
					this.LowerValue = num;
				}
				else
				{
					string[] strArrays = str.Split(new char[] { '.' });
					this.LowerValue = Math.Round(num, strArrays[1].Length, MidpointRounding.AwayFromZero);
				}
			}
			if (reCalculateUpperValue)
			{
				this._oldUpper = this.UpperValue;
				double num1 = 0;
				if (this.IsSnapToTickEnabled)
				{
					num1 = (direction == RangeSlider.Direction.Increase ? Math.Min(value, base.Maximum) : Math.Max(this.LowerValue + this.MinRange, value));
				}
				if (str.ToLower().Contains("e+") || !str.Contains("."))
				{
					this.UpperValue = num1;
				}
				else
				{
					string[] strArrays1 = str.Split(new char[] { '.' });
					this.UpperValue = Math.Round(num1, strArrays1[1].Length, MidpointRounding.AwayFromZero);
				}
			}
			this._internalUpdate = false;
			RangeSlider.RaiseValueChangedEvents(this, reCalculateLowerValue, reCalculateUpperValue);
		}

		private void ReCalculateRangeSelected(double newLower, double newUpper, RangeSlider.Direction direction)
		{
			double num = 0;
			double num1 = 0;
			this._internalUpdate = true;
			this._oldLower = this.LowerValue;
			this._oldUpper = this.UpperValue;
			if (this.IsSnapToTickEnabled)
			{
				if (direction != RangeSlider.Direction.Increase)
				{
					num = Math.Max(newLower, base.Minimum);
					num1 = Math.Max(base.Minimum + (this.UpperValue - this.LowerValue), newUpper);
				}
				else
				{
					num = Math.Min(newLower, base.Maximum - (this.UpperValue - this.LowerValue));
					num1 = Math.Min(newUpper, base.Maximum);
				}
				string str = this.TickFrequency.ToString(CultureInfo.InvariantCulture);
				if (!str.ToLower().Contains("e+") && str.Contains("."))
				{
					string[] strArrays = str.Split(new char[] { '.' });
					if (direction != RangeSlider.Direction.Decrease)
					{
						this.UpperValue = Math.Round(num1, strArrays[1].Length, MidpointRounding.AwayFromZero);
						this.LowerValue = Math.Round(num, strArrays[1].Length, MidpointRounding.AwayFromZero);
					}
					else
					{
						this.LowerValue = Math.Round(num, strArrays[1].Length, MidpointRounding.AwayFromZero);
						this.UpperValue = Math.Round(num1, strArrays[1].Length, MidpointRounding.AwayFromZero);
					}
				}
				else if (direction != RangeSlider.Direction.Decrease)
				{
					this.UpperValue = num1;
					this.LowerValue = num;
				}
				else
				{
					this.LowerValue = num;
					this.UpperValue = num1;
				}
			}
			this._internalUpdate = false;
			RangeSlider.RaiseValueChangedEvents(this, true, true);
		}

		private void ReCalculateSize()
		{
			if (this._leftButton != null && this._rightButton != null && this._centerThumb != null)
			{
				if (this.Orientation == System.Windows.Controls.Orientation.Horizontal)
				{
					this._movableWidth = Math.Max(base.ActualWidth - this._rightThumb.ActualWidth - this._leftThumb.ActualWidth - this.MinRangeWidth, 1);
					if (this.MovableRange > 0)
					{
						this._leftButton.Width = Math.Max(this._movableWidth * (this.LowerValue - base.Minimum) / this.MovableRange, 0);
						this._rightButton.Width = Math.Max(this._movableWidth * (base.Maximum - this.UpperValue) / this.MovableRange, 0);
					}
					else
					{
						this._leftButton.Width = double.NaN;
						this._rightButton.Width = double.NaN;
					}
					if (!this.IsValidDouble(this._rightButton.Width) || !this.IsValidDouble(this._leftButton.Width))
					{
						this._centerThumb.Width = Math.Max(base.ActualWidth - (this._rightThumb.ActualWidth + this._leftThumb.ActualWidth), 0);
					}
					else
					{
						this._centerThumb.Width = Math.Max(base.ActualWidth - (this._leftButton.Width + this._rightButton.Width + this._rightThumb.ActualWidth + this._leftThumb.ActualWidth), 0);
					}
				}
				else if (this.Orientation == System.Windows.Controls.Orientation.Vertical)
				{
					this._movableWidth = Math.Max(base.ActualHeight - this._rightThumb.ActualHeight - this._leftThumb.ActualHeight - this.MinRangeWidth, 1);
					if (this.MovableRange > 0)
					{
						this._leftButton.Height = Math.Max(this._movableWidth * (this.LowerValue - base.Minimum) / this.MovableRange, 0);
						this._rightButton.Height = Math.Max(this._movableWidth * (base.Maximum - this.UpperValue) / this.MovableRange, 0);
					}
					else
					{
						this._leftButton.Height = double.NaN;
						this._rightButton.Height = double.NaN;
					}
					if (!this.IsValidDouble(this._rightButton.Height) || !this.IsValidDouble(this._leftButton.Height))
					{
						this._centerThumb.Height = Math.Max(base.ActualHeight - (this._rightThumb.ActualHeight + this._leftThumb.ActualHeight), 0);
					}
					else
					{
						this._centerThumb.Height = Math.Max(base.ActualHeight - (this._leftButton.Height + this._rightButton.Height + this._rightThumb.ActualHeight + this._leftThumb.ActualHeight), 0);
					}
				}
				this._density = this._movableWidth / this.MovableRange;
			}
		}

		private void RelocateAutoToolTip()
		{
			double horizontalOffset = this._autoToolTip.HorizontalOffset;
			this._autoToolTip.HorizontalOffset = horizontalOffset + 0.001;
			this._autoToolTip.HorizontalOffset = horizontalOffset;
		}

		public void ResetSelection(bool isStart)
		{
			double maximum = base.Maximum - base.Minimum;
			maximum = (isStart ? -maximum : maximum);
			RangeSlider.MoveThumb(this._leftButton, this._rightButton, maximum, this.Orientation, out this._direction);
			this.ReCalculateRangeSelected(true, true, this._direction);
		}

		private void RightButtonMouseDown()
		{
			if (Mouse.LeftButton == MouseButtonState.Pressed)
			{
				Point position = Mouse.GetPosition(this._visualElementsContainer);
				double num = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? this._rightButton.ActualWidth - (base.ActualWidth - (position.X + this._rightThumb.ActualWidth / 2)) : -(this._rightButton.ActualHeight - (position.Y - this._rightThumb.ActualHeight / 2)));
				if (!this.IsSnapToTickEnabled)
				{
					if (this.IsMoveToPointEnabled && !this.MoveWholeRange)
					{
						RangeSlider.MoveThumb(this._centerThumb, this._rightButton, num, this.Orientation, out this._direction);
						this.ReCalculateRangeSelected(false, true, this._direction);
					}
					else if (this.IsMoveToPointEnabled && this.MoveWholeRange)
					{
						RangeSlider.MoveThumb(this._leftButton, this._rightButton, num, this.Orientation, out this._direction);
						this.ReCalculateRangeSelected(true, true, this._direction);
					}
				}
				else if (this.IsMoveToPointEnabled && !this.MoveWholeRange)
				{
					this.JumpToNextTick(RangeSlider.Direction.Increase, RangeSlider.ButtonType.TopRight, num, this.UpperValue, true);
				}
				else if (this.IsMoveToPointEnabled && this.MoveWholeRange)
				{
					this.JumpToNextTick(RangeSlider.Direction.Increase, RangeSlider.ButtonType.Both, num, this.UpperValue, true);
				}
				if (!this.IsMoveToPointEnabled)
				{
					this._position = Mouse.GetPosition(this._visualElementsContainer);
					this._bType = (this.MoveWholeRange ? RangeSlider.ButtonType.Both : RangeSlider.ButtonType.TopRight);
					this._currentpoint = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? this._position.X : this._position.Y);
					this._currenValue = this.UpperValue;
					this._direction = RangeSlider.Direction.Increase;
					this._isInsideRange = false;
					this._timer.Start();
				}
			}
		}

		private void RightThumbDragComplete(object sender, DragCompletedEventArgs e)
		{
			if (this._autoToolTip != null)
			{
				this._autoToolTip.IsOpen = false;
				this._autoToolTip = null;
			}
			e.RoutedEvent = RangeSlider.UpperThumbDragCompletedEvent;
			base.RaiseEvent(e);
		}

		private void RightThumbDragDelta(object sender, DragDeltaEventArgs e)
		{
			double num = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? e.HorizontalChange : e.VerticalChange);
			if (this.IsSnapToTickEnabled)
			{
				Point position = Mouse.GetPosition(this._container);
				if (this.Orientation == System.Windows.Controls.Orientation.Horizontal)
				{
					if (position.X < this._container.ActualWidth && position.X > this._leftButton.ActualWidth + this._leftThumb.ActualWidth + this._centerThumb.MinWidth)
					{
						this.JumpToNextTick((position.X > this._basePoint.X ? RangeSlider.Direction.Increase : RangeSlider.Direction.Decrease), RangeSlider.ButtonType.TopRight, num, this.UpperValue, false);
					}
				}
				else if (position.Y >= 0 && position.Y < this._container.ActualHeight - (this._leftButton.ActualHeight + this._leftThumb.ActualHeight + this._centerThumb.MinHeight))
				{
					RangeSlider.Direction direction = (position.Y < this._basePoint.Y ? RangeSlider.Direction.Increase : RangeSlider.Direction.Decrease);
					this.JumpToNextTick(direction, RangeSlider.ButtonType.TopRight, -num, this.UpperValue, false);
				}
				this._basePoint = Mouse.GetPosition(this._container);
			}
			else
			{
				RangeSlider.MoveThumb(this._centerThumb, this._rightButton, num, this.Orientation, out this._direction);
				this.ReCalculateRangeSelected(false, true, this._direction);
			}
			if (this.AutoToolTipPlacement != System.Windows.Controls.Primitives.AutoToolTipPlacement.None)
			{
				this._autoToolTip.Content = this.GetUpperToolTipNumber();
				this.RelocateAutoToolTip();
			}
			e.RoutedEvent = RangeSlider.UpperThumbDragDeltaEvent;
			base.RaiseEvent(e);
		}

		private void RightThumbDragStart(object sender, DragStartedEventArgs e)
		{
			this._isMoved = true;
			if (this.AutoToolTipPlacement != System.Windows.Controls.Primitives.AutoToolTipPlacement.None)
			{
				if (this._autoToolTip == null)
				{
					this._autoToolTip = new System.Windows.Controls.ToolTip()
					{
						Placement = PlacementMode.Custom,
						CustomPopupPlacementCallback = new CustomPopupPlacementCallback(this.PopupPlacementCallback)
					};
				}
				this._autoToolTip.Content = this.GetUpperToolTipNumber();
				this._autoToolTip.PlacementTarget = this._rightThumb;
				this._autoToolTip.IsOpen = true;
			}
			this._basePoint = Mouse.GetPosition(this._container);
			e.RoutedEvent = RangeSlider.UpperThumbDragStartedEvent;
			base.RaiseEvent(e);
		}

		private void SnapToTickHandle(RangeSlider.ButtonType type, RangeSlider.Direction direction, double difference)
		{
			double num = difference;
			difference = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? difference : -difference);
			if (direction != RangeSlider.Direction.Increase)
			{
				switch (type)
				{
					case RangeSlider.ButtonType.BottomLeft:
					{
						if (this.LowerValue <= base.Minimum)
						{
							break;
						}
						RangeSlider.MoveThumb(this._leftButton, this._centerThumb, -difference * this._density, this.Orientation);
						this.ReCalculateRangeSelected(true, false, this.LowerValue - num, direction);
						return;
					}
					case RangeSlider.ButtonType.TopRight:
					{
						if (this.UpperValue <= this.LowerValue + this.MinRange)
						{
							break;
						}
						RangeSlider.MoveThumb(this._centerThumb, this._rightButton, -difference * this._density, this.Orientation);
						this.ReCalculateRangeSelected(false, true, this.UpperValue - num, direction);
						return;
					}
					case RangeSlider.ButtonType.Both:
					{
						if (this.LowerValue <= base.Minimum)
						{
							break;
						}
						RangeSlider.MoveThumb(this._leftButton, this._rightButton, -difference * this._density, this.Orientation);
						this.ReCalculateRangeSelected(this.LowerValue - num, this.UpperValue - num, direction);
						break;
					}
					default:
					{
						return;
					}
				}
			}
			else
			{
				switch (type)
				{
					case RangeSlider.ButtonType.BottomLeft:
					{
						if (this.LowerValue >= this.UpperValue - this.MinRange)
						{
							break;
						}
						RangeSlider.MoveThumb(this._leftButton, this._centerThumb, difference * this._density, this.Orientation);
						this.ReCalculateRangeSelected(true, false, this.LowerValue + num, direction);
						return;
					}
					case RangeSlider.ButtonType.TopRight:
					{
						if (this.UpperValue >= base.Maximum)
						{
							break;
						}
						RangeSlider.MoveThumb(this._centerThumb, this._rightButton, difference * this._density, this.Orientation);
						this.ReCalculateRangeSelected(false, true, this.UpperValue + num, direction);
						return;
					}
					case RangeSlider.ButtonType.Both:
					{
						if (this.UpperValue >= base.Maximum)
						{
							break;
						}
						RangeSlider.MoveThumb(this._leftButton, this._rightButton, difference * this._density, this.Orientation);
						this.ReCalculateRangeSelected(this.LowerValue + num, this.UpperValue + num, direction);
						return;
					}
					default:
					{
						return;
					}
				}
			}
		}

		private double UpdateEndPoint(RangeSlider.ButtonType type, RangeSlider.Direction dir)
		{
			double num = 0;
			if (dir != RangeSlider.Direction.Increase)
			{
				if (dir == RangeSlider.Direction.Decrease)
				{
					if (type == RangeSlider.ButtonType.BottomLeft || type == RangeSlider.ButtonType.Both && !this._isInsideRange)
					{
						num = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? this._leftButton.ActualWidth : base.ActualHeight - this._leftButton.ActualHeight);
					}
					else if (type == RangeSlider.ButtonType.TopRight || type == RangeSlider.ButtonType.Both && this._isInsideRange)
					{
						num = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? base.ActualWidth - this._rightButton.ActualWidth - this._rightThumb.ActualWidth : this._rightButton.ActualHeight + this._rightThumb.ActualHeight);
					}
				}
			}
			else if (type == RangeSlider.ButtonType.BottomLeft || type == RangeSlider.ButtonType.Both && this._isInsideRange)
			{
				num = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? this._leftButton.ActualWidth + this._leftThumb.ActualWidth : base.ActualHeight - (this._leftButton.ActualHeight + this._leftThumb.ActualHeight));
			}
			else if (type == RangeSlider.ButtonType.TopRight || type == RangeSlider.ButtonType.Both && !this._isInsideRange)
			{
				num = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? base.ActualWidth - this._rightButton.ActualWidth : this._rightButton.ActualHeight);
			}
			return num;
		}

		private void VisualElementsContainerMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.MiddleButton == MouseButtonState.Pressed)
			{
				this.MoveWholeRange = !this.MoveWholeRange;
			}
		}

		private void VisualElementsContainerMouseLeave(object sender, MouseEventArgs e)
		{
			this._tickCount = 0;
			this._timer.Stop();
		}

		private void VisualElementsContainerPreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			Point position = Mouse.GetPosition(this._visualElementsContainer);
			if (this.Orientation != System.Windows.Controls.Orientation.Horizontal)
			{
				if (position.Y > base.ActualHeight - this._leftButton.ActualHeight)
				{
					this.LeftButtonMouseDown();
					return;
				}
				if (position.Y < this._rightButton.ActualHeight)
				{
					this.RightButtonMouseDown();
					return;
				}
				if (position.Y > this._rightButton.ActualHeight + this._rightButton.ActualHeight && position.Y < base.ActualHeight - (this._leftButton.ActualHeight + this._leftThumb.ActualHeight))
				{
					this.CentralThumbMouseDown();
				}
			}
			else
			{
				if (position.X < this._leftButton.ActualWidth)
				{
					this.LeftButtonMouseDown();
					return;
				}
				if (position.X > base.ActualWidth - this._rightButton.ActualWidth)
				{
					this.RightButtonMouseDown();
					return;
				}
				if (position.X > this._leftButton.ActualWidth + this._leftThumb.ActualWidth && position.X < base.ActualWidth - (this._rightButton.ActualWidth + this._rightThumb.ActualWidth))
				{
					this.CentralThumbMouseDown();
					return;
				}
			}
		}

		private void VisualElementsContainerPreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			this._tickCount = 0;
			this._timer.Stop();
			this._centerThumbBlocked = false;
		}

		public event DragCompletedEventHandler CentralThumbDragCompleted
		{
			add
			{
				base.AddHandler(RangeSlider.CentralThumbDragCompletedEvent, value);
			}
			remove
			{
				base.RemoveHandler(RangeSlider.CentralThumbDragCompletedEvent, value);
			}
		}

		public event DragDeltaEventHandler CentralThumbDragDelta
		{
			add
			{
				base.AddHandler(RangeSlider.CentralThumbDragDeltaEvent, value);
			}
			remove
			{
				base.RemoveHandler(RangeSlider.CentralThumbDragDeltaEvent, value);
			}
		}

		public event DragStartedEventHandler CentralThumbDragStarted
		{
			add
			{
				base.AddHandler(RangeSlider.CentralThumbDragStartedEvent, value);
			}
			remove
			{
				base.RemoveHandler(RangeSlider.CentralThumbDragStartedEvent, value);
			}
		}

		public event DragCompletedEventHandler LowerThumbDragCompleted
		{
			add
			{
				base.AddHandler(RangeSlider.LowerThumbDragCompletedEvent, value);
			}
			remove
			{
				base.RemoveHandler(RangeSlider.LowerThumbDragCompletedEvent, value);
			}
		}

		public event DragDeltaEventHandler LowerThumbDragDelta
		{
			add
			{
				base.AddHandler(RangeSlider.LowerThumbDragDeltaEvent, value);
			}
			remove
			{
				base.RemoveHandler(RangeSlider.LowerThumbDragDeltaEvent, value);
			}
		}

		public event DragStartedEventHandler LowerThumbDragStarted
		{
			add
			{
				base.AddHandler(RangeSlider.LowerThumbDragStartedEvent, value);
			}
			remove
			{
				base.RemoveHandler(RangeSlider.LowerThumbDragStartedEvent, value);
			}
		}

		public event RangeParameterChangedEventHandler LowerValueChanged
		{
			add
			{
				base.AddHandler(RangeSlider.LowerValueChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(RangeSlider.LowerValueChangedEvent, value);
			}
		}

		public event RangeSelectionChangedEventHandler RangeSelectionChanged
		{
			add
			{
				base.AddHandler(RangeSlider.RangeSelectionChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(RangeSlider.RangeSelectionChangedEvent, value);
			}
		}

		public event DragCompletedEventHandler UpperThumbDragCompleted
		{
			add
			{
				base.AddHandler(RangeSlider.UpperThumbDragCompletedEvent, value);
			}
			remove
			{
				base.RemoveHandler(RangeSlider.UpperThumbDragCompletedEvent, value);
			}
		}

		public event DragDeltaEventHandler UpperThumbDragDelta
		{
			add
			{
				base.AddHandler(RangeSlider.UpperThumbDragDeltaEvent, value);
			}
			remove
			{
				base.RemoveHandler(RangeSlider.UpperThumbDragDeltaEvent, value);
			}
		}

		public event DragStartedEventHandler UpperThumbDragStarted
		{
			add
			{
				base.AddHandler(RangeSlider.UpperThumbDragStartedEvent, value);
			}
			remove
			{
				base.RemoveHandler(RangeSlider.UpperThumbDragStartedEvent, value);
			}
		}

		public event RangeParameterChangedEventHandler UpperValueChanged
		{
			add
			{
				base.AddHandler(RangeSlider.UpperValueChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(RangeSlider.UpperValueChangedEvent, value);
			}
		}

		private enum ButtonType
		{
			BottomLeft,
			TopRight,
			Both
		}

		private enum Direction
		{
			Increase,
			Decrease
		}
	}
}