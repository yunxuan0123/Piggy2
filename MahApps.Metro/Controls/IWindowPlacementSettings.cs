using MahApps.Metro.Native;
using System;

namespace MahApps.Metro.Controls
{
	public interface IWindowPlacementSettings
	{
		WINDOWPLACEMENT? Placement
		{
			get;
			set;
		}

		bool UpgradeSettings
		{
			get;
			set;
		}

		void Reload();

		void Save();

		void Upgrade();
	}
}