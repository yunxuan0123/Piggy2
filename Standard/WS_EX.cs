using System;

namespace Standard
{
	[Flags]
	internal enum WS_EX : uint
	{
		LEFT = 0,
		LTRREADING = 0,
		None = 0,
		RIGHTSCROLLBAR = 0,
		DLGMODALFRAME = 1,
		NOPARENTNOTIFY = 4,
		TOPMOST = 8,
		ACCEPTFILES = 16,
		TRANSPARENT = 32,
		MDICHILD = 64,
		TOOLWINDOW = 128,
		WINDOWEDGE = 256,
		PALETTEWINDOW = 392,
		CLIENTEDGE = 512,
		OVERLAPPEDWINDOW = 768,
		CONTEXTHELP = 1024,
		RIGHT = 4096,
		RTLREADING = 8192,
		LEFTSCROLLBAR = 16384,
		CONTROLPARENT = 65536,
		STATICEDGE = 131072,
		APPWINDOW = 262144,
		LAYERED = 524288,
		NOINHERITLAYOUT = 1048576,
		LAYOUTRTL = 4194304,
		COMPOSITED = 33554432,
		NOACTIVATE = 134217728
	}
}