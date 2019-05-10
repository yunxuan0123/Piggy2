using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MahApps.Metro
{
	[DebuggerDisplay("apptheme={Name}, res={Resources.Source}")]
	public class AppTheme
	{
		public string Name
		{
			get;
			private set;
		}

		public ResourceDictionary Resources
		{
			get;
			private set;
		}

		public AppTheme(string name, Uri resourceAddress)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			if (name == null)
			{
				throw new ArgumentException("name");
			}
			if (resourceAddress == null)
			{
				throw new ArgumentNullException("resourceAddress");
			}
			this.Name = name;
			this.Resources = new ResourceDictionary()
			{
				Source = resourceAddress
			};
		}
	}
}