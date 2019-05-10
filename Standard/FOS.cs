using System;

namespace Standard
{
	[Flags]
	internal enum FOS : uint
	{
		OVERWRITEPROMPT = 2,
		STRICTFILETYPES = 4,
		NOCHANGEDIR = 8,
		PICKFOLDERS = 32,
		FORCEFILESYSTEM = 64,
		ALLNONSTORAGEITEMS = 128,
		NOVALIDATE = 256,
		ALLOWMULTISELECT = 512,
		PATHMUSTEXIST = 2048,
		FILEMUSTEXIST = 4096,
		CREATEPROMPT = 8192,
		SHAREAWARE = 16384,
		NOREADONLYRETURN = 32768,
		NOTESTFILECREATE = 65536,
		HIDEMRUPLACES = 131072,
		HIDEPINNEDPLACES = 262144,
		NODEREFERENCELINKS = 1048576,
		DONTADDTORECENT = 33554432,
		FORCESHOWHIDDEN = 268435456,
		DEFAULTNOMINIMODE = 536870912,
		FORCEPREVIEWPANEON = 1073741824
	}
}