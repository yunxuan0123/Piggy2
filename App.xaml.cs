using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;

namespace Aries
{
	public partial class App : Application
	{
		public App()
		{
		}

		private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
		{
			Assembly assembly;
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			AssemblyName name = executingAssembly.GetName();
			string str = string.Concat(name.Name, ".resources");
			AssemblyName assemblyName = new AssemblyName(args.Name);
			string str1 = "";
			if (str != assemblyName.Name)
			{
				str1 = string.Concat(assemblyName.Name, ".dll");
				if (!assemblyName.CultureInfo.Equals(CultureInfo.InvariantCulture))
				{
					str1 = string.Format("{0}\\{1}", assemblyName.CultureInfo, str1);
				}
			}
			else
			{
				str1 = string.Concat(name.Name, ".g.resources");
			}
			using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(str1))
			{
				if (manifestResourceStream != null)
				{
					byte[] numArray = new byte[checked((IntPtr)manifestResourceStream.Length)];
					manifestResourceStream.Read(numArray, 0, (int)numArray.Length);
					assembly = Assembly.Load(numArray);
				}
				else
				{
					assembly = null;
				}
			}
			return assembly;
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(App.OnResolveAssembly);
		}
	}
}