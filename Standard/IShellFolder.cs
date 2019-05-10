using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard
{
	[Guid("000214E6-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IShellFolder
	{
		object BindToObject([In] IntPtr pidl, [In] IBindCtx pbc, [In] ref Guid riid);

		object BindToStorage([In] IntPtr pidl, [In] IBindCtx pbc, [In] ref Guid riid);

		object CreateViewObject([In] IntPtr hwndOwner, [In] ref Guid riid);

		IEnumIDList EnumObjects([In] IntPtr hwnd, [In] SHCONTF grfFlags);

		void GetAttributesOf([In] uint cidl, [In] IntPtr apidl, [In][Out] ref SFGAO rgfInOut);

		void GetDisplayNameOf([In] IntPtr pidl, [In] SHGDN uFlags, out IntPtr pName);

		object GetUIObjectOf([In] IntPtr hwndOwner, [In] uint cidl, [In] IntPtr apidl, [In] ref Guid riid, [In][Out] ref uint rgfReserved);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT imethod_0([In] IntPtr lParam, [In] IntPtr pidl1, [In] IntPtr pidl2);

		void ParseDisplayName([In] IntPtr hwnd, [In] IBindCtx pbc, [In] string pszDisplayName, [In][Out] ref int pchEaten, out IntPtr ppidl, [In][Out] ref uint pdwAttributes);

		void SetNameOf([In] IntPtr hwnd, [In] IntPtr pidl, [In] string pszName, [In] SHGDN uFlags, out IntPtr ppidlOut);
	}
}