using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;

namespace Standard
{
	internal sealed class MessageWindow : DispatcherObject, IDisposable
	{
		private readonly static WndProc s_WndProc;

		private readonly static Dictionary<IntPtr, MessageWindow> s_windowLookup;

		private WndProc _wndProcCallback;

		private string _className;

		private bool _isDisposed;

		public IntPtr Handle
		{
			get;
			private set;
		}

		static MessageWindow()
		{
			Class6.yDnXvgqzyB5jw();
			MessageWindow.s_WndProc = new WndProc(MessageWindow._WndProc);
			MessageWindow.s_windowLookup = new Dictionary<IntPtr, MessageWindow>();
		}

		public MessageWindow(CS classStyle, WS style, WS_EX exStyle, Rect location, string name, WndProc callback)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this._wndProcCallback = callback;
			Guid guid = Guid.NewGuid();
			this._className = string.Concat("MessageWindowClass+", guid.ToString());
			WNDCLASSEX wNDCLASSEX = new WNDCLASSEX()
			{
				cbSize = Marshal.SizeOf(typeof(WNDCLASSEX)),
				style = classStyle,
				lpfnWndProc = MessageWindow.s_WndProc,
				hInstance = Standard.NativeMethods.GetModuleHandle(null),
				hbrBackground = Standard.NativeMethods.GetStockObject(StockObject.NULL_BRUSH),
				lpszMenuName = "",
				lpszClassName = this._className
			};
			WNDCLASSEX wNDCLASSEX1 = wNDCLASSEX;
			Standard.NativeMethods.RegisterClassEx(ref wNDCLASSEX1);
			GCHandle gCHandle = new GCHandle();
			try
			{
				gCHandle = GCHandle.Alloc(this);
				IntPtr intPtr = (IntPtr)gCHandle;
				this.Handle = Standard.NativeMethods.CreateWindowEx(exStyle, this._className, name, style, (int)location.X, (int)location.Y, (int)location.Width, (int)location.Height, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, intPtr);
			}
			finally
			{
				gCHandle.Free();
			}
		}

		private static object _DestroyWindow(IntPtr hwnd, string className)
		{
			Utility.SafeDestroyWindow(ref hwnd);
			Standard.NativeMethods.UnregisterClass(className, Standard.NativeMethods.GetModuleHandle(null));
			return null;
		}

		private void _Dispose(bool disposing, bool isHwndBeingDestroyed)
		{
			if (this._isDisposed)
			{
				return;
			}
			this._isDisposed = true;
			IntPtr handle = this.Handle;
			string str = this._className;
			if (isHwndBeingDestroyed)
			{
				base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback((object arg) => MessageWindow._DestroyWindow(IntPtr.Zero, str)));
			}
			else if (this.Handle != IntPtr.Zero)
			{
				if (!base.CheckAccess())
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback((object arg) => MessageWindow._DestroyWindow(handle, str)));
				}
				else
				{
					MessageWindow._DestroyWindow(handle, str);
				}
			}
			MessageWindow.s_windowLookup.Remove(handle);
			this._className = null;
			this.Handle = IntPtr.Zero;
		}

		private static IntPtr _WndProc(IntPtr hwnd, WM msg, IntPtr wParam, IntPtr lParam)
		{
			IntPtr zero = IntPtr.Zero;
			MessageWindow target = null;
			if (msg == WM.CREATE)
			{
				CREATESTRUCT structure = (CREATESTRUCT)Marshal.PtrToStructure(lParam, typeof(CREATESTRUCT));
				target = (MessageWindow)GCHandle.FromIntPtr(structure.lpCreateParams).Target;
				MessageWindow.s_windowLookup.Add(hwnd, target);
			}
			else if (!MessageWindow.s_windowLookup.TryGetValue(hwnd, out target))
			{
				return Standard.NativeMethods.DefWindowProcW(hwnd, msg, wParam, lParam);
			}
			WndProc wndProc = target._wndProcCallback;
			zero = (wndProc == null ? Standard.NativeMethods.DefWindowProcW(hwnd, msg, wParam, lParam) : wndProc(hwnd, msg, wParam, lParam));
			if (msg == WM.NCDESTROY)
			{
				target._Dispose(true, true);
				GC.SuppressFinalize(target);
			}
			return zero;
		}

		public void Dispose()
		{
			this._Dispose(true, false);
			GC.SuppressFinalize(this);
		}

		protected override void Finalize()
		{
			try
			{
				this._Dispose(false, false);
			}
			finally
			{
				base.Finalize();
			}
		}
	}
}