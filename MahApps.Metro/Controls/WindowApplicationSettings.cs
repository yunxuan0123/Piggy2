using MahApps.Metro;
using MahApps.Metro.Native;
using System;
using System.Configuration;
using System.Windows;

namespace MahApps.Metro.Controls
{
	internal class WindowApplicationSettings : ApplicationSettingsBase, IWindowPlacementSettings
	{
		[UserScopedSetting]
		public WINDOWPLACEMENT? Placement
		{
			get
			{
				if (this["Placement"] == null)
				{
					return null;
				}
				return new WINDOWPLACEMENT?((WINDOWPLACEMENT)this["Placement"]);
			}
			set
			{
				this["Placement"] = value;
			}
		}

		[UserScopedSetting]
		public bool UpgradeSettings
		{
			get
			{
				bool item;
				try
				{
					if (this["UpgradeSettings"] == null)
					{
						return true;
					}
					else
					{
						item = (bool)this["UpgradeSettings"];
					}
				}
				catch (ConfigurationErrorsException configurationErrorsException)
				{
					ConfigurationErrorsException innerException = configurationErrorsException;
					string str = null;
					while (innerException != null)
					{
						string filename = innerException.Filename;
						str = filename;
						if (filename != null)
						{
							break;
						}
						innerException = innerException.InnerException as ConfigurationErrorsException;
					}
					throw new MahAppsException(string.Format("The settings file '{0}' seems to be corrupted", str ?? "<unknown>"), innerException);
				}
				return item;
			}
			set
			{
				this["UpgradeSettings"] = value;
			}
		}

		public WindowApplicationSettings(Window window)
		{
			Class6.yDnXvgqzyB5jw();
			base(window.GetType().FullName);
		}

		void MahApps.Metro.Controls.IWindowPlacementSettings.Reload()
		{
			base.Reload();
		}
	}
}