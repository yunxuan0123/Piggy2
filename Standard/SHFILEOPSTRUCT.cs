using System;

namespace Standard
{
	internal struct SHFILEOPSTRUCT
	{
		public IntPtr hwnd;

		public FO wFunc;

		public string pFrom;

		public string pTo;

		public FOF fFlags;

		public int fAnyOperationsAborted;

		public IntPtr hNameMappings;

		public string lpszProgressTitle;
	}
}