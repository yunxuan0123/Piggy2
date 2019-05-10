using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Standard
{
	[Guid("000214F9-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IShellLinkW
	{
		void GetArguments([Out] StringBuilder pszArgs, int cchMaxPath);

		void GetDescription([Out] StringBuilder pszFile, int cchMaxName);

		short GetHotKey();

		void GetIconLocation([Out] StringBuilder pszIconPath, int cchIconPath, out int piIcon);

		void GetPath([Out] StringBuilder pszFile, int cchMaxPath, [In][Out] WIN32_FIND_DATAW pfd, SLGP fFlags);

		uint GetShowCmd();

		void GetWorkingDirectory([Out] StringBuilder pszDir, int cchMaxPath);

		void imethod_0(out IntPtr ppidl);

		void imethod_1(IntPtr pidl);

		void Resolve(IntPtr hwnd, uint fFlags);

		void SetArguments(string pszArgs);

		void SetDescription(string pszName);

		void SetHotKey(short wHotKey);

		void SetIconLocation(string pszIconPath, int iIcon);

		void SetPath(string pszFile);

		void SetRelativePath(string pszPathRel, uint dwReserved);

		void SetShowCmd(uint iShowCmd);

		void SetWorkingDirectory(string pszDir);
	}
}