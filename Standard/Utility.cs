using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Standard
{
	internal static class Utility
	{
		private readonly static Random _randomNumberGenerator;

		private readonly static Version _osVersion;

		private static int s_bitDepth;

		public static bool IsOSVistaOrNewer
		{
			get
			{
				return Utility._osVersion >= new Version(6, 0);
			}
		}

		public static bool IsOSWindows7OrNewer
		{
			get
			{
				return Utility._osVersion >= new Version(6, 1);
			}
		}

		static Utility()
		{
			Class6.yDnXvgqzyB5jw();
			Utility._randomNumberGenerator = new Random();
			Utility._osVersion = Environment.OSVersion.Version;
		}

		private static BitmapFrame _GetBestMatch(IList<BitmapFrame> frames, int bitDepth, int width, int height)
		{
			PixelFormat format;
			int bitsPerPixel;
			int num = 2147483647;
			int num1 = 0;
			int num2 = 0;
			bool decoder = frames[0].Decoder is IconBitmapDecoder;
			for (int i = 0; i < frames.Count && num != 0; i++)
			{
				if (decoder)
				{
					format = frames[i].Thumbnail.Format;
					bitsPerPixel = format.BitsPerPixel;
				}
				else
				{
					format = frames[i].Format;
					bitsPerPixel = format.BitsPerPixel;
				}
				int num3 = bitsPerPixel;
				if (num3 == 0)
				{
					num3 = 8;
				}
				int num4 = Utility._MatchImage(frames[i], bitDepth, width, height, num3);
				if (num4 < num)
				{
					num2 = i;
					num1 = num3;
					num = num4;
				}
				else if (num4 == num && num1 < num3)
				{
					num2 = i;
					num1 = num3;
				}
			}
			return frames[num2];
		}

		private static int _GetBitDepth()
		{
			if (Utility.s_bitDepth == 0)
			{
				using (SafeDC desktop = SafeDC.GetDesktop())
				{
					Utility.s_bitDepth = Standard.NativeMethods.GetDeviceCaps(desktop, DeviceCap.BITSPIXEL) * Standard.NativeMethods.GetDeviceCaps(desktop, DeviceCap.PLANES);
				}
			}
			return Utility.s_bitDepth;
		}

		private static int _HexToInt(char h)
		{
			if (h >= '0' && h <= '9')
			{
				return h - 48;
			}
			if (h >= 'a' && h <= 'f')
			{
				return h - 97 + 10;
			}
			if (h < 'A' || h > 'F')
			{
				return -1;
			}
			return h - 65 + 10;
		}

		private static byte _IntToHex(int n)
		{
			if (n <= 9)
			{
				return (byte)(n + 48);
			}
			return (byte)(n - 10 + 65);
		}

		private static bool _IsAsciiAlphaNumeric(byte b)
		{
			if (b >= 97 && b <= 122 || b >= 65 && b <= 90)
			{
				return true;
			}
			if (b < 48)
			{
				return false;
			}
			return b <= 57;
		}

		private static int _MatchImage(BitmapFrame frame, int bitDepth, int width, int height, int bpp)
		{
			int num = 2 * Utility._WeightedAbs(bpp, bitDepth, false) + Utility._WeightedAbs(frame.PixelWidth, width, true) + Utility._WeightedAbs(frame.PixelHeight, height, true);
			return num;
		}

		private static bool _MemCmp(IntPtr left, IntPtr right, long cb)
		{
			int i;
			for (i = 0; (long)i < cb - 8L; i += 8)
			{
				if (Marshal.ReadInt64(left, i) != Marshal.ReadInt64(right, i))
				{
					return false;
				}
			}
			while ((long)i < cb)
			{
				if (Marshal.ReadByte(left, i) != Marshal.ReadByte(right, i))
				{
					return false;
				}
				i++;
			}
			return true;
		}

		private static bool _UrlEncodeIsSafe(byte b)
		{
			if (Utility._IsAsciiAlphaNumeric(b))
			{
				return true;
			}
			char chr = (char)b;
			if (chr != '!')
			{
				switch (chr)
				{
					case '\'':
					case '(':
					case ')':
					case '*':
					case '-':
					case '.':
					{
						break;
					}
					case '+':
					case ',':
					{
						return false;
					}
					default:
					{
						if (chr != '\u005F')
						{
							return false;
						}
						break;
					}
				}
			}
			return true;
		}

		private static int _WeightedAbs(int valueHave, int valueWant, bool fPunish)
		{
			int num = valueHave - valueWant;
			if (num < 0)
			{
				num = (fPunish ? -2 : -1) * num;
			}
			return num;
		}

		public static void AddDependencyPropertyChangeListener(object component, DependencyProperty property, EventHandler listener)
		{
			if (component == null)
			{
				return;
			}
			DependencyPropertyDescriptor dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(property, component.GetType());
			dependencyPropertyDescriptor.AddValueChanged(component, listener);
		}

		public static int AlphaRGB(Color c)
		{
			return c.B | c.G << 8 | c.R << 16 | c.A << 24;
		}

		public static bool AreImageSourcesEqual(ImageSource left, ImageSource right)
		{
			if (left == null)
			{
				return right == null;
			}
			if (right == null)
			{
				return false;
			}
			BitmapSource bitmapSource = Utility.GenerateBitmapSource(left);
			BitmapSource bitmapSource1 = Utility.GenerateBitmapSource(right);
			byte[] bytesFromBitmapSource = Utility.GetBytesFromBitmapSource(bitmapSource);
			byte[] numArray = Utility.GetBytesFromBitmapSource(bitmapSource1);
			if ((int)bytesFromBitmapSource.Length != (int)numArray.Length)
			{
				return false;
			}
			return Utility.MemCmp(bytesFromBitmapSource, numArray, (int)bytesFromBitmapSource.Length);
		}

		public static bool AreStreamsEqual(Stream left, Stream right)
		{
			bool flag;
			if (left == null)
			{
				return right == null;
			}
			if (right == null)
			{
				return false;
			}
			if (!left.CanRead || !right.CanRead)
			{
				throw new NotSupportedException("The streams can't be read for comparison");
			}
			if (left.Length != right.Length)
			{
				return false;
			}
			int length = (int)left.Length;
			left.Position = 0L;
			right.Position = 0L;
			int num = 0;
			int num1 = 0;
			int num2 = 0;
			int num3 = 0;
			byte[] numArray = new byte[512];
			byte[] numArray1 = new byte[512];
			GCHandle gCHandle = GCHandle.Alloc(numArray, GCHandleType.Pinned);
			IntPtr intPtr = gCHandle.AddrOfPinnedObject();
			GCHandle gCHandle1 = GCHandle.Alloc(numArray1, GCHandleType.Pinned);
			IntPtr intPtr1 = gCHandle1.AddrOfPinnedObject();
			try
			{
				while (num < length)
				{
					num2 = left.Read(numArray, 0, (int)numArray.Length);
					num3 = right.Read(numArray1, 0, (int)numArray1.Length);
					if (num2 != num3)
					{
						flag = false;
						return flag;
					}
					else if (!Utility._MemCmp(intPtr, intPtr1, (long)num2))
					{
						flag = false;
						return flag;
					}
					else
					{
						num += num2;
						num1 += num3;
					}
				}
				flag = true;
			}
			finally
			{
				gCHandle.Free();
				gCHandle1.Free();
			}
			return flag;
		}

		public static Color ColorFromArgbDword(uint color)
		{
			return Color.FromArgb((byte)((color & -16777216) >> 24), (byte)((color & 16711680) >> 16), (byte)((color & 65280) >> 8), (byte)(color & 255));
		}

		public static void CopyStream(Stream destination, Stream source)
		{
			int num;
			destination.Position = 0L;
			if (source.CanSeek)
			{
				source.Position = 0L;
				destination.SetLength(source.Length);
			}
			byte[] numArray = new byte[4096];
			do
			{
				num = source.Read(numArray, 0, (int)numArray.Length);
				if (num == 0)
				{
					continue;
				}
				destination.Write(numArray, 0, num);
			}
			while ((int)numArray.Length == num);
			destination.Position = 0L;
		}

		public static void EnsureDirectory(string path)
		{
			if (!path.EndsWith("\\", StringComparison.Ordinal))
			{
				path = string.Concat(path, "\\");
			}
			path = Path.GetDirectoryName(path);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}

		public static Exception FailableFunction<T>(Func<T> function, out T result)
		{
			return Utility.FailableFunction<T>(5, function, out result);
		}

		public static T FailableFunction<T>(Func<T> function)
		{
			T t;
			Exception exception = Utility.FailableFunction<T>(function, out t);
			if (exception != null)
			{
				throw exception;
			}
			return t;
		}

		public static T FailableFunction<T>(int maxRetries, Func<T> function)
		{
			T t;
			Exception exception = Utility.FailableFunction<T>(maxRetries, function, out t);
			if (exception != null)
			{
				throw exception;
			}
			return t;
		}

		public static Exception FailableFunction<T>(int maxRetries, Func<T> function, out T result)
		{
			Exception exception;
			int num = 0;
			while (true)
			{
				try
				{
					result = function();
					exception = null;
					break;
				}
				catch (Exception exception2)
				{
					Exception exception1 = exception2;
					if (num == maxRetries)
					{
						result = default(T);
						exception = exception1;
						break;
					}
				}
				num++;
			}
			return exception;
		}

		public static BitmapSource GenerateBitmapSource(ImageSource img)
		{
			return Utility.GenerateBitmapSource(img, img.Width, img.Height);
		}

		public static BitmapSource GenerateBitmapSource(ImageSource img, double renderWidth, double renderHeight)
		{
			DrawingVisual drawingVisual = new DrawingVisual();
			using (DrawingContext drawingContext = drawingVisual.RenderOpen())
			{
				drawingContext.DrawImage(img, new Rect(0, 0, renderWidth, renderHeight));
			}
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)renderWidth, (int)renderHeight, 96, 96, PixelFormats.Pbgra32);
			renderTargetBitmap.Render(drawingVisual);
			return renderTargetBitmap;
		}

		public static BitmapSource GenerateBitmapSource(UIElement element, double renderWidth, double renderHeight, bool performLayout)
		{
			if (performLayout)
			{
				element.Measure(new Size(renderWidth, renderHeight));
				element.Arrange(new Rect(new Size(renderWidth, renderHeight)));
			}
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)renderWidth, (int)renderHeight, 96, 96, PixelFormats.Pbgra32);
			DrawingVisual drawingVisual = new DrawingVisual();
			using (DrawingContext drawingContext = drawingVisual.RenderOpen())
			{
				drawingContext.DrawRectangle(new VisualBrush(element), null, new Rect(0, 0, renderWidth, renderHeight));
			}
			renderTargetBitmap.Render(drawingVisual);
			return renderTargetBitmap;
		}

		public static IEnumerable<string> GenerateFileNames(string directory, string primaryFileName, string extension)
		{
			Verify.IsNeitherNullNorEmpty(directory, "directory");
			Verify.IsNeitherNullNorEmpty(primaryFileName, "primaryFileName");
			primaryFileName = Utility.MakeValidFileName(primaryFileName);
			for (int i = 0; i <= 50; i++)
			{
				if (i == 0)
				{
					yield return string.Concat(Path.Combine(directory, primaryFileName), extension);
				}
				else if (40 < i)
				{
					object[] objArray = new object[] { Path.Combine(directory, primaryFileName), " (", Utility._randomNumberGenerator.Next(41, 9999), ")", extension };
					yield return string.Concat(objArray);
				}
				else
				{
					string[] strArrays = new string[] { Path.Combine(directory, primaryFileName), " (", i.ToString((IFormatProvider)null), ")", extension };
					yield return string.Concat(strArrays);
				}
			}
		}

		public static IntPtr GenerateHICON(ImageSource image, Size dimensions)
		{
			IntPtr intPtr;
			IntPtr zero;
			if (image == null)
			{
				return IntPtr.Zero;
			}
			BitmapFrame bestMatch = image as BitmapFrame;
			if (bestMatch == null)
			{
				Rect rect = new Rect(0, 0, dimensions.Width, dimensions.Height);
				double width = dimensions.Width / dimensions.Height;
				double num = image.Width / image.Height;
				if (image.Width <= dimensions.Width && image.Height <= dimensions.Height)
				{
					rect = new Rect((dimensions.Width - image.Width) / 2, (dimensions.Height - image.Height) / 2, image.Width, image.Height);
				}
				else if (width > num)
				{
					double width1 = image.Width / image.Height * dimensions.Width;
					rect = new Rect((dimensions.Width - width1) / 2, 0, width1, dimensions.Height);
				}
				else if (width < num)
				{
					double height = image.Height / image.Width * dimensions.Height;
					rect = new Rect(0, (dimensions.Height - height) / 2, dimensions.Width, height);
				}
				DrawingVisual drawingVisual = new DrawingVisual();
				DrawingContext drawingContext = drawingVisual.RenderOpen();
				drawingContext.DrawImage(image, rect);
				drawingContext.Close();
				RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)dimensions.Width, (int)dimensions.Height, 96, 96, PixelFormats.Pbgra32);
				renderTargetBitmap.Render(drawingVisual);
				bestMatch = BitmapFrame.Create(renderTargetBitmap);
			}
			else
			{
				bestMatch = Utility.GetBestMatch(bestMatch.Decoder.Frames, (int)dimensions.Width, (int)dimensions.Height);
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
				BitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
				pngBitmapEncoder.Frames.Add(bestMatch);
				pngBitmapEncoder.Save(memoryStream);
				using (ManagedIStream managedIStream = new ManagedIStream(memoryStream))
				{
					IntPtr zero1 = IntPtr.Zero;
					try
					{
						if (Standard.NativeMethods.GdipCreateBitmapFromStream(managedIStream, out zero1) == Status.Ok)
						{
							zero = (Standard.NativeMethods.GdipCreateHICONFromBitmap(zero1, out intPtr) == Status.Ok ? intPtr : IntPtr.Zero);
						}
						else
						{
							zero = IntPtr.Zero;
						}
					}
					finally
					{
						Utility.SafeDisposeImage(ref zero1);
					}
				}
			}
			return zero;
		}

		public static void GeneratePropertyString(StringBuilder source, string propertyName, string value)
		{
			if (source.Length != 0)
			{
				source.Append(' ');
			}
			source.Append(propertyName);
			source.Append(": ");
			if (string.IsNullOrEmpty(value))
			{
				source.Append("<null>");
				return;
			}
			source.Append('\"');
			source.Append(value);
			source.Append('\"');
		}

		[Obsolete]
		public static string GenerateToString<T>(T @object)
		where T : struct
		{
			StringBuilder stringBuilder = new StringBuilder();
			PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
			for (int i = 0; i < (int)properties.Length; i++)
			{
				PropertyInfo propertyInfo = properties[i];
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(", ");
				}
				object value = propertyInfo.GetValue(@object, null);
				stringBuilder.AppendFormat((value == null ? "{0}: <null>" : "{0}: \"{1}\""), propertyInfo.Name, value);
			}
			return stringBuilder.ToString();
		}

		public static int GET_X_LPARAM(IntPtr lParam)
		{
			return Utility.LOWORD(lParam.ToInt32());
		}

		public static int GET_Y_LPARAM(IntPtr lParam)
		{
			return Utility.HIWORD(lParam.ToInt32());
		}

		public static BitmapFrame GetBestMatch(IList<BitmapFrame> frames, int width, int height)
		{
			return Utility._GetBestMatch(frames, Utility._GetBitDepth(), width, height);
		}

		public static byte[] GetBytesFromBitmapSource(BitmapSource bmp)
		{
			int pixelWidth = bmp.PixelWidth;
			int pixelHeight = bmp.PixelHeight;
			PixelFormat format = bmp.Format;
			int bitsPerPixel = pixelWidth * ((format.BitsPerPixel + 7) / 8);
			byte[] numArray = new byte[pixelHeight * bitsPerPixel];
			bmp.CopyPixels(numArray, bitsPerPixel, 0);
			return numArray;
		}

		public static string GetHashString(string value)
		{
			string str;
			using (MD5 mD5 = MD5.Create())
			{
				byte[] numArray = mD5.ComputeHash(Encoding.UTF8.GetBytes(value));
				byte[] numArray1 = numArray;
				StringBuilder stringBuilder = new StringBuilder();
				string str1 = ((IEnumerable<byte>)numArray1).Aggregate<byte, StringBuilder>(stringBuilder, (StringBuilder sb, byte b) => sb.Append(b.ToString("x2", CultureInfo.InvariantCulture))).ToString();
				str = str1;
			}
			return str;
		}

		public static bool GuidTryParse(string guidString, out Guid guid)
		{
			Verify.IsNeitherNullNorEmpty(guidString, "guidString");
			try
			{
				guid = new Guid(guidString);
				return true;
			}
			catch (FormatException formatException)
			{
			}
			catch (OverflowException overflowException)
			{
			}
			guid = new Guid();
			return false;
		}

		public static string HashStreamMD5(Stream stm)
		{
			stm.Position = 0L;
			StringBuilder stringBuilder = new StringBuilder();
			using (MD5 mD5 = MD5.Create())
			{
				byte[] numArray = mD5.ComputeHash(stm);
				for (int i = 0; i < (int)numArray.Length; i++)
				{
					byte num = numArray[i];
					stringBuilder.Append(num.ToString("x2", CultureInfo.InvariantCulture));
				}
			}
			return stringBuilder.ToString();
		}

		public static int HIWORD(int i)
		{
			return (short)(i >> 16);
		}

		public static bool IsFlagSet(int value, int mask)
		{
			return (value & mask) != 0;
		}

		public static bool IsFlagSet(uint value, uint mask)
		{
			return (value & mask) != 0;
		}

		public static bool IsFlagSet(long value, long mask)
		{
			return (value & mask) > 0L;
		}

		public static bool IsFlagSet(ulong value, ulong mask)
		{
			return (value & mask) > 0L;
		}

		public static bool IsInterfaceImplemented(Type objectType, Type interfaceType)
		{
			return objectType.GetInterfaces().Any<Type>((Type type) => type == interfaceType);
		}

		public static bool IsNonNegative(this Thickness thickness)
		{
			if (!thickness.Top.IsFiniteAndNonNegative())
			{
				return false;
			}
			if (!thickness.Left.IsFiniteAndNonNegative())
			{
				return false;
			}
			if (!thickness.Bottom.IsFiniteAndNonNegative())
			{
				return false;
			}
			if (!thickness.Right.IsFiniteAndNonNegative())
			{
				return false;
			}
			return true;
		}

		public static bool IsValid(this CornerRadius cornerRadius)
		{
			if (!cornerRadius.TopLeft.IsFiniteAndNonNegative())
			{
				return false;
			}
			if (!cornerRadius.TopRight.IsFiniteAndNonNegative())
			{
				return false;
			}
			if (!cornerRadius.BottomLeft.IsFiniteAndNonNegative())
			{
				return false;
			}
			if (!cornerRadius.BottomRight.IsFiniteAndNonNegative())
			{
				return false;
			}
			return true;
		}

		public static int LOWORD(int i)
		{
			return (short)(i & 65535);
		}

		public static string MakeValidFileName(string invalidPath)
		{
			return invalidPath.Replace('\\', '\u005F').Replace('/', '\u005F').Replace(':', '\u005F').Replace('*', '\u005F').Replace('?', '\u005F').Replace('\"', '\u005F').Replace('<', '\u005F').Replace('>', '\u005F').Replace('|', '\u005F');
		}

		public static bool MemCmp(byte[] left, byte[] right, int cb)
		{
			GCHandle gCHandle = GCHandle.Alloc(left, GCHandleType.Pinned);
			IntPtr intPtr = gCHandle.AddrOfPinnedObject();
			GCHandle gCHandle1 = GCHandle.Alloc(right, GCHandleType.Pinned);
			IntPtr intPtr1 = gCHandle1.AddrOfPinnedObject();
			bool flag = Utility._MemCmp(intPtr, intPtr1, (long)cb);
			gCHandle.Free();
			gCHandle1.Free();
			return flag;
		}

		public static void RemoveDependencyPropertyChangeListener(object component, DependencyProperty property, EventHandler listener)
		{
			if (component == null)
			{
				return;
			}
			DependencyPropertyDescriptor dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(property, component.GetType());
			dependencyPropertyDescriptor.RemoveValueChanged(component, listener);
		}

		public static int RGB(Color c)
		{
			return c.B | c.G << 8 | c.R << 16;
		}

		public static string SafeCopyFile(string sourceFileName, string destFileName, SafeCopyFileOptions options)
		{
			string str;
			switch (options)
			{
				case SafeCopyFileOptions.PreserveOriginal:
				{
					if (File.Exists(destFileName))
					{
						return null;
					}
					File.Copy(sourceFileName, destFileName);
					return destFileName;
				}
				case SafeCopyFileOptions.Overwrite:
				{
					File.Copy(sourceFileName, destFileName, true);
					return destFileName;
				}
				case SafeCopyFileOptions.FindBetterName:
				{
					string directoryName = Path.GetDirectoryName(destFileName);
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(destFileName);
					string extension = Path.GetExtension(destFileName);
					using (IEnumerator<string> enumerator = Utility.GenerateFileNames(directoryName, fileNameWithoutExtension, extension).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string current = enumerator.Current;
							if (File.Exists(current))
							{
								continue;
							}
							File.Copy(sourceFileName, current);
							str = current;
							return str;
						}
						return null;
					}
					return str;
				}
				default:
				{
					throw new ArgumentException("Invalid enumeration value", "options");
				}
			}
			return null;
		}

		public static void SafeCoTaskMemFree(ref IntPtr ptr)
		{
			IntPtr intPtr = (void*)ptr;
			ptr = IntPtr.Zero;
			if (IntPtr.Zero != intPtr)
			{
				Marshal.FreeCoTaskMem(intPtr);
			}
		}

		public static void SafeDeleteFile(string path)
		{
			if (!string.IsNullOrEmpty(path))
			{
				File.Delete(path);
			}
		}

		public static void SafeDeleteObject(ref IntPtr gdiObject)
		{
			IntPtr intPtr = (void*)gdiObject;
			gdiObject = IntPtr.Zero;
			if (IntPtr.Zero != intPtr)
			{
				Standard.NativeMethods.DeleteObject(intPtr);
			}
		}

		public static void SafeDestroyIcon(ref IntPtr hicon)
		{
			IntPtr intPtr = (void*)hicon;
			hicon = IntPtr.Zero;
			if (IntPtr.Zero != intPtr)
			{
				Standard.NativeMethods.DestroyIcon(intPtr);
			}
		}

		public static void SafeDestroyWindow(ref IntPtr hwnd)
		{
			IntPtr intPtr = (void*)hwnd;
			hwnd = IntPtr.Zero;
			if (Standard.NativeMethods.IsWindow(intPtr))
			{
				Standard.NativeMethods.DestroyWindow(intPtr);
			}
		}

		public static void SafeDispose<T>(ref T disposable)
		where T : IDisposable
		{
			IDisposable disposable1 = disposable;
			disposable = default(T);
			if (disposable1 != null)
			{
				disposable1.Dispose();
			}
		}

		public static void SafeDisposeImage(ref IntPtr gdipImage)
		{
			IntPtr intPtr = (void*)gdipImage;
			gdipImage = IntPtr.Zero;
			if (IntPtr.Zero != intPtr)
			{
				Standard.NativeMethods.GdipDisposeImage(intPtr);
			}
		}

		public static void SafeFreeHGlobal(ref IntPtr hglobal)
		{
			IntPtr intPtr = (void*)hglobal;
			hglobal = IntPtr.Zero;
			if (IntPtr.Zero != intPtr)
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		public static void SafeRelease<T>(ref T comObject)
		where T : class
		{
			T t = comObject;
			comObject = default(T);
			if (t != null)
			{
				Marshal.ReleaseComObject(t);
			}
		}

		public static void SaveToPng(BitmapSource source, string fileName)
		{
			PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
			pngBitmapEncoder.Frames.Add(BitmapFrame.Create(source));
			using (FileStream fileStream = File.Create(fileName))
			{
				pngBitmapEncoder.Save(fileStream);
			}
		}

		public static bool TryFileMove(string sourceFileName, string destFileName)
		{
			if (File.Exists(destFileName))
			{
				return false;
			}
			try
			{
				File.Move(sourceFileName, destFileName);
			}
			catch (IOException oException)
			{
				return false;
			}
			return true;
		}

		public static string UrlDecode(string url)
		{
			if (url == null)
			{
				return null;
			}
			Utility._UrlDecoder __UrlDecoder = new Utility._UrlDecoder(url.Length, Encoding.UTF8);
			int length = url.Length;
			for (int i = 0; i < length; i++)
			{
				char chr = url[i];
				if (chr != '+')
				{
					if (chr == '%' && i < length - 2)
					{
						if (url[i + 1] != 'u' || i >= length - 5)
						{
							int num = Utility._HexToInt(url[i + 1]);
							int num1 = Utility._HexToInt(url[i + 2]);
							if (num < 0 || num1 < 0)
							{
								goto Label1;
							}
							__UrlDecoder.AddByte((byte)(num << 4 | num1));
							i += 2;
							goto Label0;
						}
						else
						{
							int num2 = Utility._HexToInt(url[i + 2]);
							int num3 = Utility._HexToInt(url[i + 3]);
							int num4 = Utility._HexToInt(url[i + 4]);
							int num5 = Utility._HexToInt(url[i + 5]);
							if (num2 < 0 || num3 < 0 || num4 < 0 || num5 < 0)
							{
								goto Label1;
							}
							__UrlDecoder.AddChar((char)(num2 << 12 | num3 << 8 | num4 << 4 | num5));
							i += 5;
							goto Label0;
						}
					}
				Label1:
					if ((chr & '\uFF80') != 0)
					{
						__UrlDecoder.AddChar(chr);
					}
					else
					{
						__UrlDecoder.AddByte((byte)chr);
					}
				}
				else
				{
					__UrlDecoder.AddByte(32);
				}
			Label0:
			}
			return __UrlDecoder.GetString();
		}

		public static string UrlEncode(string url)
		{
			if (url == null)
			{
				return null;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(url);
			bool flag = false;
			int num = 0;
			byte[] numArray = bytes;
			for (int i = 0; i < (int)numArray.Length; i++)
			{
				byte num1 = numArray[i];
				if (num1 == 32)
				{
					flag = true;
				}
				else if (!Utility._UrlEncodeIsSafe(num1))
				{
					num++;
					flag = true;
				}
			}
			if (flag)
			{
				byte[] hex = new byte[(int)bytes.Length + num * 2];
				int num2 = 0;
				byte[] numArray1 = bytes;
				for (int j = 0; j < (int)numArray1.Length; j++)
				{
					byte num3 = numArray1[j];
					if (Utility._UrlEncodeIsSafe(num3))
					{
						int num4 = num2;
						num2 = num4 + 1;
						hex[num4] = num3;
					}
					else if (num3 != 32)
					{
						int num5 = num2;
						num2 = num5 + 1;
						hex[num5] = 37;
						int num6 = num2;
						num2 = num6 + 1;
						hex[num6] = Utility._IntToHex(num3 >> 4 & 15);
						int num7 = num2;
						num2 = num7 + 1;
						hex[num7] = Utility._IntToHex(num3 & 15);
					}
					else
					{
						int num8 = num2;
						num2 = num8 + 1;
						hex[num8] = 43;
					}
				}
				bytes = hex;
			}
			return Encoding.ASCII.GetString(bytes);
		}

		private class _UrlDecoder
		{
			private readonly Encoding _encoding;

			private readonly char[] _charBuffer;

			private readonly byte[] _byteBuffer;

			private int _byteCount;

			private int _charCount;

			public _UrlDecoder(int size, Encoding encoding)
			{
				Class6.yDnXvgqzyB5jw();
				base();
				this._encoding = encoding;
				this._charBuffer = new char[size];
				this._byteBuffer = new byte[size];
			}

			private void _FlushBytes()
			{
				if (this._byteCount > 0)
				{
					this._charCount += this._encoding.GetChars(this._byteBuffer, 0, this._byteCount, this._charBuffer, this._charCount);
					this._byteCount = 0;
				}
			}

			public void AddByte(byte b)
			{
				byte[] numArray = this._byteBuffer;
				int num = this._byteCount;
				this._byteCount = num + 1;
				numArray[num] = b;
			}

			public void AddChar(char ch)
			{
				this._FlushBytes();
				char[] chrArray = this._charBuffer;
				int num = this._charCount;
				this._charCount = num + 1;
				chrArray[num] = ch;
			}

			public string GetString()
			{
				this._FlushBytes();
				if (this._charCount <= 0)
				{
					return "";
				}
				return new string(this._charBuffer, 0, this._charCount);
			}
		}
	}
}