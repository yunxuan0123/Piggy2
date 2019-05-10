using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MahApps.Metro.Controls
{
	public class TextBoxHelper
	{
		public readonly static DependencyProperty IsMonitoringProperty;

		public readonly static DependencyProperty WatermarkProperty;

		public readonly static DependencyProperty UseFloatingWatermarkProperty;

		public readonly static DependencyProperty TextLengthProperty;

		public readonly static DependencyProperty ClearTextButtonProperty;

		public readonly static DependencyProperty ButtonsAlignmentProperty;

		public readonly static DependencyProperty IsClearTextButtonBehaviorEnabledProperty;

		public readonly static DependencyProperty ButtonCommandProperty;

		public readonly static DependencyProperty ButtonCommandParameterProperty;

		public readonly static DependencyProperty ButtonContentProperty;

		public readonly static DependencyProperty ButtonTemplateProperty;

		public readonly static DependencyProperty ButtonFontFamilyProperty;

		public readonly static DependencyProperty SelectAllOnFocusProperty;

		public readonly static DependencyProperty IsWaitingForDataProperty;

		public readonly static DependencyProperty HasTextProperty;

		public readonly static DependencyProperty IsSpellCheckContextMenuEnabledProperty;

		static TextBoxHelper()
		{
			Class6.yDnXvgqzyB5jw();
			TextBoxHelper.IsMonitoringProperty = DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(TextBoxHelper), new UIPropertyMetadata(false, new PropertyChangedCallback(TextBoxHelper.OnIsMonitoringChanged)));
			TextBoxHelper.WatermarkProperty = DependencyProperty.RegisterAttached("Watermark", typeof(string), typeof(TextBoxHelper), new UIPropertyMetadata(string.Empty));
			TextBoxHelper.UseFloatingWatermarkProperty = DependencyProperty.RegisterAttached("UseFloatingWatermark", typeof(bool), typeof(TextBoxHelper), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(TextBoxHelper.ButtonCommandOrClearTextChanged)));
			TextBoxHelper.TextLengthProperty = DependencyProperty.RegisterAttached("TextLength", typeof(int), typeof(TextBoxHelper), new UIPropertyMetadata((object)0));
			TextBoxHelper.ClearTextButtonProperty = DependencyProperty.RegisterAttached("ClearTextButton", typeof(bool), typeof(TextBoxHelper), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(TextBoxHelper.ButtonCommandOrClearTextChanged)));
			TextBoxHelper.ButtonsAlignmentProperty = DependencyProperty.RegisterAttached("ButtonsAlignment", typeof(ButtonsAlignment), typeof(TextBoxHelper), new FrameworkPropertyMetadata((object)ButtonsAlignment.Right, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));
			TextBoxHelper.IsClearTextButtonBehaviorEnabledProperty = DependencyProperty.RegisterAttached("IsClearTextButtonBehaviorEnabled", typeof(bool), typeof(TextBoxHelper), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(TextBoxHelper.IsClearTextButtonBehaviorEnabledChanged)));
			TextBoxHelper.ButtonCommandProperty = DependencyProperty.RegisterAttached("ButtonCommand", typeof(ICommand), typeof(TextBoxHelper), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(TextBoxHelper.ButtonCommandOrClearTextChanged)));
			TextBoxHelper.ButtonCommandParameterProperty = DependencyProperty.RegisterAttached("ButtonCommandParameter", typeof(object), typeof(TextBoxHelper), new FrameworkPropertyMetadata(null));
			TextBoxHelper.ButtonContentProperty = DependencyProperty.RegisterAttached("ButtonContent", typeof(object), typeof(TextBoxHelper), new FrameworkPropertyMetadata("r"));
			TextBoxHelper.ButtonTemplateProperty = DependencyProperty.RegisterAttached("ButtonTemplate", typeof(ControlTemplate), typeof(TextBoxHelper), new FrameworkPropertyMetadata(null));
			TextBoxHelper.ButtonFontFamilyProperty = DependencyProperty.RegisterAttached("ButtonFontFamily", typeof(FontFamily), typeof(TextBoxHelper), new FrameworkPropertyMetadata((new FontFamilyConverter()).ConvertFromString("Marlett")));
			TextBoxHelper.SelectAllOnFocusProperty = DependencyProperty.RegisterAttached("SelectAllOnFocus", typeof(bool), typeof(TextBoxHelper), new FrameworkPropertyMetadata(false));
			TextBoxHelper.IsWaitingForDataProperty = DependencyProperty.RegisterAttached("IsWaitingForData", typeof(bool), typeof(TextBoxHelper), new UIPropertyMetadata(false));
			TextBoxHelper.HasTextProperty = DependencyProperty.RegisterAttached("HasText", typeof(bool), typeof(TextBoxHelper), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender));
			TextBoxHelper.IsSpellCheckContextMenuEnabledProperty = DependencyProperty.RegisterAttached("IsSpellCheckContextMenuEnabled", typeof(bool), typeof(TextBoxHelper), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(TextBoxHelper.UseSpellCheckContextMenuChanged)));
		}

		public TextBoxHelper()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public static void ButtonClicked(Button sender, RoutedEventArgs e)
		{
			DependencyObject parent = VisualTreeHelper.GetParent((Button)sender);
			while (!(parent is TextBox) && !(parent is PasswordBox) && !(parent is ComboBox))
			{
				parent = VisualTreeHelper.GetParent(parent);
			}
			ICommand buttonCommand = TextBoxHelper.GetButtonCommand(parent);
			if (buttonCommand != null && buttonCommand.CanExecute(parent))
			{
				object buttonCommandParameter = TextBoxHelper.GetButtonCommandParameter(parent);
				ICommand command = buttonCommand;
				object obj = buttonCommandParameter;
				if (obj == null)
				{
					obj = parent;
				}
				command.Execute(obj);
			}
			if (TextBoxHelper.GetClearTextButton(parent))
			{
				if (parent is TextBox)
				{
					((TextBox)parent).Clear();
					return;
				}
				if (parent is PasswordBox)
				{
					((PasswordBox)parent).Clear();
					return;
				}
				if (parent is ComboBox)
				{
					if (((ComboBox)parent).IsEditable)
					{
						((ComboBox)parent).Text = string.Empty;
					}
					((ComboBox)parent).SelectedItem = null;
				}
			}
		}

		private static void ButtonCommandOrClearTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBox textBox = d as TextBox;
			if (textBox != null)
			{
				textBox.Loaded -= new RoutedEventHandler(TextBoxHelper.TextChanged);
				textBox.Loaded += new RoutedEventHandler(TextBoxHelper.TextChanged);
				if (textBox.IsLoaded)
				{
					TextBoxHelper.TextChanged(textBox, new RoutedEventArgs());
				}
			}
			PasswordBox passwordBox = d as PasswordBox;
			if (passwordBox != null)
			{
				passwordBox.Loaded -= new RoutedEventHandler(TextBoxHelper.PasswordChanged);
				passwordBox.Loaded += new RoutedEventHandler(TextBoxHelper.PasswordChanged);
				if (passwordBox.IsLoaded)
				{
					TextBoxHelper.PasswordChanged(passwordBox, new RoutedEventArgs());
				}
			}
			ComboBox comboBox = d as ComboBox;
			if (comboBox != null)
			{
				comboBox.Loaded -= new RoutedEventHandler(TextBoxHelper.ComboBoxLoaded);
				comboBox.Loaded += new RoutedEventHandler(TextBoxHelper.ComboBoxLoaded);
				if (comboBox.IsLoaded)
				{
					TextBoxHelper.ComboBoxLoaded(comboBox, new RoutedEventArgs());
				}
			}
		}

		private static void ComboBoxLoaded(object sender, RoutedEventArgs e)
		{
			ComboBox comboBox = sender as ComboBox;
			if (comboBox != null)
			{
				comboBox.SetValue(TextBoxHelper.HasTextProperty, (!string.IsNullOrWhiteSpace(comboBox.Text) ? true : comboBox.SelectedItem != null));
			}
		}

		private static void ControlGotFocus<TDependencyObject>(TDependencyObject sender, Action<TDependencyObject> action)
		where TDependencyObject : DependencyObject
		{
			if (sender != null && TextBoxHelper.GetSelectAllOnFocus(sender))
			{
				sender.Dispatcher.BeginInvoke(action, new object[] { sender });
			}
		}

		[Category("MahApps.Metro")]
		public static ICommand GetButtonCommand(DependencyObject d)
		{
			return (ICommand)d.GetValue(TextBoxHelper.ButtonCommandProperty);
		}

		[Category("MahApps.Metro")]
		public static object GetButtonCommandParameter(DependencyObject d)
		{
			return d.GetValue(TextBoxHelper.ButtonCommandParameterProperty);
		}

		[Category("MahApps.Metro")]
		public static object GetButtonContent(DependencyObject d)
		{
			return d.GetValue(TextBoxHelper.ButtonContentProperty);
		}

		[Category("MahApps.Metro")]
		public static FontFamily GetButtonFontFamily(DependencyObject d)
		{
			return (FontFamily)d.GetValue(TextBoxHelper.ButtonFontFamilyProperty);
		}

		[Category("MahApps.Metro")]
		public static ButtonsAlignment GetButtonsAlignment(DependencyObject d)
		{
			return (ButtonsAlignment)d.GetValue(TextBoxHelper.ButtonsAlignmentProperty);
		}

		[Category("MahApps.Metro")]
		public static ControlTemplate GetButtonTemplate(DependencyObject d)
		{
			return (ControlTemplate)d.GetValue(TextBoxHelper.ButtonTemplateProperty);
		}

		[Category("MahApps.Metro")]
		public static bool GetClearTextButton(DependencyObject d)
		{
			return (bool)d.GetValue(TextBoxHelper.ClearTextButtonProperty);
		}

		private static ContextMenu GetDefaultTextBoxBaseContextMenu()
		{
			ContextMenu contextMenu = new ContextMenu();
			MenuItem menuItem = new MenuItem()
			{
				Command = ApplicationCommands.Cut
			};
			menuItem.SetResourceReference(FrameworkElement.StyleProperty, "MetroMenuItem");
			MenuItem menuItem1 = new MenuItem()
			{
				Command = ApplicationCommands.Copy
			};
			menuItem1.SetResourceReference(FrameworkElement.StyleProperty, "MetroMenuItem");
			MenuItem menuItem2 = new MenuItem()
			{
				Command = ApplicationCommands.Paste
			};
			menuItem2.SetResourceReference(FrameworkElement.StyleProperty, "MetroMenuItem");
			contextMenu.Items.Add(menuItem);
			contextMenu.Items.Add(menuItem1);
			contextMenu.Items.Add(menuItem2);
			return contextMenu;
		}

		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		[AttachedPropertyBrowsableForType(typeof(DatePicker))]
		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		[AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
		[Category("MahApps.Metro")]
		public static bool GetHasText(DependencyObject obj)
		{
			return (bool)obj.GetValue(TextBoxHelper.HasTextProperty);
		}

		[AttachedPropertyBrowsableForType(typeof(Button))]
		[Category("MahApps.Metro")]
		public static bool GetIsClearTextButtonBehaviorEnabled(Button d)
		{
			return (bool)d.GetValue(TextBoxHelper.IsClearTextButtonBehaviorEnabledProperty);
		}

		[AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
		[Category("MahApps.Metro")]
		public static bool GetIsSpellCheckContextMenuEnabled(UIElement element)
		{
			return (bool)element.GetValue(TextBoxHelper.IsSpellCheckContextMenuEnabledProperty);
		}

		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		[AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
		[Category("MahApps.Metro")]
		public static bool GetIsWaitingForData(DependencyObject obj)
		{
			return (bool)obj.GetValue(TextBoxHelper.IsWaitingForDataProperty);
		}

		public static bool GetSelectAllOnFocus(DependencyObject obj)
		{
			return (bool)obj.GetValue(TextBoxHelper.SelectAllOnFocusProperty);
		}

		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		[AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
		[Category("MahApps.Metro")]
		public static bool GetUseFloatingWatermark(DependencyObject obj)
		{
			return (bool)obj.GetValue(TextBoxHelper.UseFloatingWatermarkProperty);
		}

		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		[AttachedPropertyBrowsableForType(typeof(DatePicker))]
		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		[AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
		[Category("MahApps.Metro")]
		public static string GetWatermark(DependencyObject obj)
		{
			return (string)obj.GetValue(TextBoxHelper.WatermarkProperty);
		}

		private static void IsClearTextButtonBehaviorEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Button button = d as Button;
			if (e.OldValue != e.NewValue && button != null)
			{
				button.Click -= new RoutedEventHandler(TextBoxHelper.ButtonClicked);
				if ((bool)e.NewValue)
				{
					button.Click += new RoutedEventHandler(TextBoxHelper.ButtonClicked);
				}
			}
		}

		private static void NumericUpDownGotFocus(object sender, RoutedEventArgs e)
		{
			TextBoxHelper.ControlGotFocus<NumericUpDown>(sender as NumericUpDown, (NumericUpDown numericUpDown) => numericUpDown.SelectAll());
		}

		private static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is TextBox)
			{
				TextBox textBox = d as TextBox;
				if (!(bool)e.NewValue)
				{
					textBox.TextChanged -= new TextChangedEventHandler(TextBoxHelper.TextChanged);
					textBox.GotFocus -= new RoutedEventHandler(TextBoxHelper.TextBoxGotFocus);
					return;
				}
				textBox.TextChanged += new TextChangedEventHandler(TextBoxHelper.TextChanged);
				textBox.GotFocus += new RoutedEventHandler(TextBoxHelper.TextBoxGotFocus);
				textBox.Dispatcher.BeginInvoke(new Action(() => TextBoxHelper.TextChanged(textBox, new TextChangedEventArgs(TextBoxBase.TextChangedEvent, UndoAction.None))), new object[0]);
				return;
			}
			if (!(d is PasswordBox))
			{
				if (d is NumericUpDown)
				{
					NumericUpDown newValue = d as NumericUpDown;
					newValue.SelectAllOnFocus = (bool)e.NewValue;
					if ((bool)e.NewValue)
					{
						newValue.ValueChanged += new RoutedPropertyChangedEventHandler<double?>(TextBoxHelper.OnNumericUpDownValueChaged);
						newValue.GotFocus += new RoutedEventHandler(TextBoxHelper.NumericUpDownGotFocus);
						return;
					}
					newValue.ValueChanged -= new RoutedPropertyChangedEventHandler<double?>(TextBoxHelper.OnNumericUpDownValueChaged);
					newValue.GotFocus -= new RoutedEventHandler(TextBoxHelper.NumericUpDownGotFocus);
				}
				return;
			}
			PasswordBox passwordBox = d as PasswordBox;
			if (!(bool)e.NewValue)
			{
				passwordBox.PasswordChanged -= new RoutedEventHandler(TextBoxHelper.PasswordChanged);
				passwordBox.GotFocus -= new RoutedEventHandler(TextBoxHelper.PasswordGotFocus);
				return;
			}
			passwordBox.PasswordChanged += new RoutedEventHandler(TextBoxHelper.PasswordChanged);
			passwordBox.GotFocus += new RoutedEventHandler(TextBoxHelper.PasswordGotFocus);
			passwordBox.Dispatcher.BeginInvoke(new Action(() => TextBoxHelper.PasswordChanged(passwordBox, new RoutedEventArgs(PasswordBox.PasswordChangedEvent, passwordBox))), new object[0]);
		}

		private static void OnNumericUpDownValueChaged(object sender, RoutedEventArgs e)
		{
			TextBoxHelper.SetTextLength<NumericUpDown>(sender as NumericUpDown, (NumericUpDown numericUpDown) => {
				if (!numericUpDown.Value.HasValue)
				{
					return 0;
				}
				return 1;
			});
		}

		private static void PasswordChanged(object sender, RoutedEventArgs e)
		{
			TextBoxHelper.SetTextLength<PasswordBox>(sender as PasswordBox, (PasswordBox passwordBox) => passwordBox.Password.Length);
		}

		private static void PasswordGotFocus(object sender, RoutedEventArgs e)
		{
			TextBoxHelper.ControlGotFocus<PasswordBox>(sender as PasswordBox, (PasswordBox passwordBox) => passwordBox.SelectAll());
		}

		public static void SetButtonCommand(DependencyObject obj, ICommand value)
		{
			obj.SetValue(TextBoxHelper.ButtonCommandProperty, value);
		}

		public static void SetButtonCommandParameter(DependencyObject obj, object value)
		{
			obj.SetValue(TextBoxHelper.ButtonCommandParameterProperty, value);
		}

		public static void SetButtonContent(DependencyObject obj, object value)
		{
			obj.SetValue(TextBoxHelper.ButtonContentProperty, value);
		}

		public static void SetButtonFontFamily(DependencyObject obj, FontFamily value)
		{
			obj.SetValue(TextBoxHelper.ButtonFontFamilyProperty, value);
		}

		public static void SetButtonsAlignment(DependencyObject obj, ButtonsAlignment value)
		{
			obj.SetValue(TextBoxHelper.ButtonsAlignmentProperty, value);
		}

		public static void SetButtonTemplate(DependencyObject obj, ControlTemplate value)
		{
			obj.SetValue(TextBoxHelper.ButtonTemplateProperty, value);
		}

		public static void SetClearTextButton(DependencyObject obj, bool value)
		{
			obj.SetValue(TextBoxHelper.ClearTextButtonProperty, value);
		}

		public static void SetHasText(DependencyObject obj, bool value)
		{
			obj.SetValue(TextBoxHelper.HasTextProperty, value);
		}

		[AttachedPropertyBrowsableForType(typeof(Button))]
		public static void SetIsClearTextButtonBehaviorEnabled(Button obj, bool value)
		{
			obj.SetValue(TextBoxHelper.IsClearTextButtonBehaviorEnabledProperty, value);
		}

		public static void SetIsMonitoring(DependencyObject obj, bool value)
		{
			obj.SetValue(TextBoxHelper.IsMonitoringProperty, value);
		}

		[AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
		public static void SetIsSpellCheckContextMenuEnabled(UIElement element, bool value)
		{
			element.SetValue(TextBoxHelper.IsSpellCheckContextMenuEnabledProperty, value);
		}

		public static void SetIsWaitingForData(DependencyObject obj, bool value)
		{
			obj.SetValue(TextBoxHelper.IsWaitingForDataProperty, value);
		}

		public static void SetSelectAllOnFocus(DependencyObject obj, bool value)
		{
			obj.SetValue(TextBoxHelper.SelectAllOnFocusProperty, value);
		}

		private static void SetTextLength<TDependencyObject>(TDependencyObject sender, Func<TDependencyObject, int> funcTextLength)
		where TDependencyObject : DependencyObject
		{
			if (sender != null)
			{
				int num = funcTextLength(sender);
				sender.SetValue(TextBoxHelper.TextLengthProperty, num);
				sender.SetValue(TextBoxHelper.HasTextProperty, num >= 1);
			}
		}

		public static void SetUseFloatingWatermark(DependencyObject obj, bool value)
		{
			obj.SetValue(TextBoxHelper.UseFloatingWatermarkProperty, value);
		}

		public static void SetWatermark(DependencyObject obj, string value)
		{
			obj.SetValue(TextBoxHelper.WatermarkProperty, value);
		}

		private static void TextBoxBaseContextMenuOpening(TextBoxBase sender, ContextMenuEventArgs e)
		{
			SpellingError spellingError;
			TextBoxBase defaultTextBoxBaseContextMenu = (TextBoxBase)sender;
			TextBox textBox = defaultTextBoxBaseContextMenu as TextBox;
			RichTextBox richTextBox = defaultTextBoxBaseContextMenu as RichTextBox;
			defaultTextBoxBaseContextMenu.ContextMenu = TextBoxHelper.GetDefaultTextBoxBaseContextMenu();
			int num = 0;
			if (textBox != null)
			{
				spellingError = textBox.GetSpellingError(textBox.CaretIndex);
			}
			else if (richTextBox != null)
			{
				spellingError = richTextBox.GetSpellingError(richTextBox.CaretPosition);
			}
			else
			{
				spellingError = null;
			}
			SpellingError spellingError1 = spellingError;
			if (spellingError1 != null)
			{
				IEnumerable<string> suggestions = spellingError1.Suggestions;
				if (suggestions.Any<string>())
				{
					foreach (string suggestion in suggestions)
					{
						MenuItem menuItem = new MenuItem()
						{
							Header = suggestion,
							FontWeight = FontWeights.Bold,
							Command = EditingCommands.CorrectSpellingError,
							CommandParameter = suggestion,
							CommandTarget = defaultTextBoxBaseContextMenu
						};
						menuItem.SetResourceReference(FrameworkElement.StyleProperty, "MetroMenuItem");
						defaultTextBoxBaseContextMenu.ContextMenu.Items.Insert(num, menuItem);
						num++;
					}
					defaultTextBoxBaseContextMenu.ContextMenu.Items.Insert(num, new Separator());
					num++;
				}
				MenuItem menuItem1 = new MenuItem()
				{
					Header = "Ignore All",
					Command = EditingCommands.IgnoreSpellingError,
					CommandTarget = defaultTextBoxBaseContextMenu
				};
				menuItem1.SetResourceReference(FrameworkElement.StyleProperty, "MetroMenuItem");
				defaultTextBoxBaseContextMenu.ContextMenu.Items.Insert(num, menuItem1);
				num++;
				Separator separator = new Separator();
				defaultTextBoxBaseContextMenu.ContextMenu.Items.Insert(num, separator);
			}
		}

		private static void TextBoxGotFocus(object sender, RoutedEventArgs e)
		{
			TextBoxHelper.ControlGotFocus<TextBox>(sender as TextBox, (TextBox textBox) => textBox.SelectAll());
		}

		private static void TextChanged(object sender, RoutedEventArgs e)
		{
			TextBoxHelper.SetTextLength<TextBox>(sender as TextBox, (TextBox textBox) => textBox.Text.Length);
		}

		private static void UseSpellCheckContextMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBoxBase defaultTextBoxBaseContextMenu = d as TextBoxBase;
			if (defaultTextBoxBaseContextMenu == null)
			{
				throw new InvalidOperationException("The property 'IsSpellCheckContextMenuEnabled' may only be set on TextBoxBase elements.");
			}
			if ((bool)e.NewValue)
			{
				defaultTextBoxBaseContextMenu.SetValue(SpellCheck.IsEnabledProperty, (object)true);
				defaultTextBoxBaseContextMenu.ContextMenu = TextBoxHelper.GetDefaultTextBoxBaseContextMenu();
				defaultTextBoxBaseContextMenu.ContextMenuOpening += new ContextMenuEventHandler(TextBoxHelper.TextBoxBaseContextMenuOpening);
				return;
			}
			defaultTextBoxBaseContextMenu.SetValue(SpellCheck.IsEnabledProperty, (object)false);
			defaultTextBoxBaseContextMenu.ContextMenu = TextBoxHelper.GetDefaultTextBoxBaseContextMenu();
			defaultTextBoxBaseContextMenu.ContextMenuOpening -= new ContextMenuEventHandler(TextBoxHelper.TextBoxBaseContextMenuOpening);
		}
	}
}