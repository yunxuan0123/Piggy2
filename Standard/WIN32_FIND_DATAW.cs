using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard
{
	[BestFitMapping(false)]
	internal class WIN32_FIND_DATAW
	{
		public FileAttributes dwFileAttributes;

		public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;

		public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;

		public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;

		public int nFileSizeHigh;

		public int nFileSizeLow;

		public int dwReserved0;

		public int dwReserved1;

		public string cFileName;

		public string cAlternateFileName;

		public WIN32_FIND_DATAW()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}
	}
}