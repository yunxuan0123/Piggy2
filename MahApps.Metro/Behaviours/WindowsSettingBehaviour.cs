using MahApps.Metro.Controls;
using System;
using System.Windows.Interactivity;

namespace MahApps.Metro.Behaviours
{
	public class WindowsSettingBehaviour : Behavior<MetroWindow>
	{
		public WindowsSettingBehaviour()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		protected override void OnAttached()
		{
			if (base.AssociatedObject != null && base.AssociatedObject.SaveWindowPosition)
			{
				IWindowPlacementSettings windowPlacementSettings = base.AssociatedObject.GetWindowPlacementSettings();
				WindowSettings.SetSave(base.AssociatedObject, windowPlacementSettings);
			}
		}
	}
}