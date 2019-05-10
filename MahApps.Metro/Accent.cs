using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MahApps.Metro
{
	[DebuggerDisplay("accent={Name}, res={Resources.Source}")]
	public class Accent
	{
		public ResourceDictionary Resources;

		public string Name
		{
			get;
			set;
		}

		public Accent()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public Accent(string name, Uri resourceAddress)
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