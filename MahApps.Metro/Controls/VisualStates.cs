using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	internal static class VisualStates
	{
		public const string GroupCommon = "CommonStates";

		public const string StateNormal = "Normal";

		public const string StateReadOnly = "ReadOnly";

		public const string StateMouseOver = "MouseOver";

		public const string StatePressed = "Pressed";

		public const string StateDisabled = "Disabled";

		public const string GroupFocus = "FocusStates";

		public const string StateUnfocused = "Unfocused";

		public const string StateFocused = "Focused";

		public const string GroupSelection = "SelectionStates";

		public const string StateSelected = "Selected";

		public const string StateUnselected = "Unselected";

		public const string StateSelectedInactive = "SelectedInactive";

		public const string GroupExpansion = "ExpansionStates";

		public const string StateExpanded = "Expanded";

		public const string StateCollapsed = "Collapsed";

		public const string GroupPopup = "PopupStates";

		public const string StatePopupOpened = "PopupOpened";

		public const string StatePopupClosed = "PopupClosed";

		public const string GroupValidation = "ValidationStates";

		public const string StateValid = "Valid";

		public const string StateInvalidFocused = "InvalidFocused";

		public const string StateInvalidUnfocused = "InvalidUnfocused";

		public const string GroupExpandDirection = "ExpandDirectionStates";

		public const string StateExpandDown = "ExpandDown";

		public const string StateExpandUp = "ExpandUp";

		public const string StateExpandLeft = "ExpandLeft";

		public const string StateExpandRight = "ExpandRight";

		public const string GroupHasItems = "HasItemsStates";

		public const string StateHasItems = "HasItems";

		public const string StateNoItems = "NoItems";

		public const string GroupIncrease = "IncreaseStates";

		public const string StateIncreaseEnabled = "IncreaseEnabled";

		public const string StateIncreaseDisabled = "IncreaseDisabled";

		public const string GroupDecrease = "DecreaseStates";

		public const string StateDecreaseEnabled = "DecreaseEnabled";

		public const string StateDecreaseDisabled = "DecreaseDisabled";

		public const string GroupInteractionMode = "InteractionModeStates";

		public const string StateEdit = "Edit";

		public const string StateDisplay = "Display";

		public const string GroupLocked = "LockedStates";

		public const string StateLocked = "Locked";

		public const string StateUnlocked = "Unlocked";

		public const string StateActive = "Active";

		public const string StateInactive = "Inactive";

		public const string GroupActive = "ActiveStates";

		public const string StateUnwatermarked = "Unwatermarked";

		public const string StateWatermarked = "Watermarked";

		public const string GroupWatermark = "WatermarkStates";

		public const string StateCalendarButtonUnfocused = "CalendarButtonUnfocused";

		public const string StateCalendarButtonFocused = "CalendarButtonFocused";

		public const string GroupCalendarButtonFocus = "CalendarButtonFocusStates";

		public const string StateBusy = "Busy";

		public const string StateIdle = "Idle";

		public const string GroupBusyStatus = "BusyStatusStates";

		public const string StateVisible = "Visible";

		public const string StateHidden = "Hidden";

		public const string GroupVisibility = "VisibilityStates";

		public static FrameworkElement GetImplementationRoot(DependencyObject dependencyObject)
		{
			if (VisualTreeHelper.GetChildrenCount(dependencyObject) != 1)
			{
				return null;
			}
			return VisualTreeHelper.GetChild(dependencyObject, 0) as FrameworkElement;
		}

		public static void GoToState(Control control, bool useTransitions, params string[] stateNames)
		{
			string[] strArrays = stateNames;
			int num = 0;
			while (num < (int)strArrays.Length && !VisualStateManager.GoToState(control, strArrays[num], useTransitions))
			{
				num++;
			}
		}

		public static VisualStateGroup TryGetVisualStateGroup(DependencyObject dependencyObject, string groupName)
		{
			FrameworkElement implementationRoot = MahApps.Metro.Controls.VisualStates.GetImplementationRoot(dependencyObject);
			if (implementationRoot == null)
			{
				return null;
			}
			return VisualStateManager.GetVisualStateGroups(implementationRoot).OfType<VisualStateGroup>().FirstOrDefault<VisualStateGroup>((VisualStateGroup group) => string.CompareOrdinal(groupName, group.Name) == 0);
		}
	}
}