using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Aries.Lib
{
	[CompilerGenerated]
	[DebuggerNonUserCode]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	internal class Resource
	{
		private static System.Resources.ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			set
			{
				Resource.resourceCulture = value;
			}
		}

		internal static byte[] devcon_x64
		{
			get
			{
				return (byte[])Resource.ResourceManager.GetObject("devcon_x64", Resource.resourceCulture);
			}
		}

		internal static byte[] devcon_x86
		{
			get
			{
				return (byte[])Resource.ResourceManager.GetObject("devcon_x86", Resource.resourceCulture);
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static System.Resources.ResourceManager ResourceManager
		{
			get
			{
				if (Resource.resourceMan == null)
				{
					Resource.resourceMan = new System.Resources.ResourceManager("Aries.Lib.Resource", typeof(Resource).Assembly);
				}
				return Resource.resourceMan;
			}
		}

		internal Resource()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}
	}
}