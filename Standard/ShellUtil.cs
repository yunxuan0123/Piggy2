using System;

namespace Standard
{
	internal static class ShellUtil
	{
		public static string GetPathFromShellItem(IShellItem item)
		{
			return item.GetDisplayName(SIGDN.DESKTOPABSOLUTEPARSING);
		}

		public static IShellItem2 GetShellItemForPath(string path)
		{
			object obj;
			if (string.IsNullOrEmpty(path))
			{
				return null;
			}
			Guid guid = new Guid("7e9fb0d3-919f-4307-ab2e-9b1860310c93");
			HRESULT sOK = NativeMethods.SHCreateItemFromParsingName(path, null, ref guid, out obj);
			if (sOK == (HRESULT)Win32Error.ERROR_FILE_NOT_FOUND || sOK == (HRESULT)Win32Error.ERROR_PATH_NOT_FOUND)
			{
				sOK = HRESULT.S_OK;
				obj = null;
			}
			sOK.ThrowIfFailed();
			return (IShellItem2)obj;
		}
	}
}