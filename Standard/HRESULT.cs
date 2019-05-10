using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Standard
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct HRESULT
	{
		[FieldOffset(0)]
		private readonly uint _value;

		[FieldOffset(-1)]
		public readonly static HRESULT S_OK;

		[FieldOffset(-1)]
		public readonly static HRESULT S_FALSE;

		[FieldOffset(-1)]
		public readonly static HRESULT E_PENDING;

		[FieldOffset(-1)]
		public readonly static HRESULT E_NOTIMPL;

		[FieldOffset(-1)]
		public readonly static HRESULT E_NOINTERFACE;

		[FieldOffset(-1)]
		public readonly static HRESULT E_POINTER;

		[FieldOffset(-1)]
		public readonly static HRESULT E_ABORT;

		[FieldOffset(-1)]
		public readonly static HRESULT E_FAIL;

		[FieldOffset(-1)]
		public readonly static HRESULT E_UNEXPECTED;

		[FieldOffset(-1)]
		public readonly static HRESULT STG_E_INVALIDFUNCTION;

		[FieldOffset(-1)]
		public readonly static HRESULT OLE_E_ADVISENOTSUPPORTED;

		[FieldOffset(-1)]
		public readonly static HRESULT DV_E_FORMATETC;

		[FieldOffset(-1)]
		public readonly static HRESULT DV_E_TYMED;

		[FieldOffset(-1)]
		public readonly static HRESULT DV_E_CLIPFORMAT;

		[FieldOffset(-1)]
		public readonly static HRESULT DV_E_DVASPECT;

		[FieldOffset(-1)]
		public readonly static HRESULT REGDB_E_CLASSNOTREG;

		[FieldOffset(-1)]
		public readonly static HRESULT DESTS_E_NO_MATCHING_ASSOC_HANDLER;

		[FieldOffset(-1)]
		public readonly static HRESULT DESTS_E_NORECDOCS;

		[FieldOffset(-1)]
		public readonly static HRESULT DESTS_E_NOTALLCLEARED;

		[FieldOffset(-1)]
		public readonly static HRESULT E_ACCESSDENIED;

		[FieldOffset(-1)]
		public readonly static HRESULT E_OUTOFMEMORY;

		[FieldOffset(-1)]
		public readonly static HRESULT E_INVALIDARG;

		[FieldOffset(-1)]
		public readonly static HRESULT INTSAFE_E_ARITHMETIC_OVERFLOW;

		[FieldOffset(-1)]
		public readonly static HRESULT COR_E_OBJECTDISPOSED;

		[FieldOffset(-1)]
		public readonly static HRESULT WC_E_GREATERTHAN;

		[FieldOffset(-1)]
		public readonly static HRESULT WC_E_SYNTAX;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_GENERIC_ERROR;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_INVALIDPARAMETER;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_OUTOFMEMORY;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_NOTIMPLEMENTED;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_ABORTED;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_ACCESSDENIED;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_VALUEOVERFLOW;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_WRONGSTATE;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_VALUEOUTOFRANGE;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_UNKNOWNIMAGEFORMAT;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_UNSUPPORTEDVERSION;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_NOTINITIALIZED;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_ALREADYLOCKED;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_PROPERTYNOTFOUND;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_PROPERTYNOTSUPPORTED;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_PROPERTYSIZE;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_CODECPRESENT;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_CODECNOTHUMBNAIL;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_PALETTEUNAVAILABLE;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_CODECTOOMANYSCANLINES;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_INTERNALERROR;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_SOURCERECTDOESNOTMATCHDIMENSIONS;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_COMPONENTNOTFOUND;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_IMAGESIZEOUTOFRANGE;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_TOOMUCHMETADATA;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_BADIMAGE;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_BADHEADER;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_FRAMEMISSING;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_BADMETADATAHEADER;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_BADSTREAMDATA;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_STREAMWRITE;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_STREAMREAD;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_STREAMNOTAVAILABLE;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_UNSUPPORTEDPIXELFORMAT;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_UNSUPPORTEDOPERATION;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_INVALIDREGISTRATION;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_COMPONENTINITIALIZEFAILURE;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_INSUFFICIENTBUFFER;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_DUPLICATEMETADATAPRESENT;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_PROPERTYUNEXPECTEDTYPE;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_UNEXPECTEDSIZE;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_INVALIDQUERYREQUEST;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_UNEXPECTEDMETADATATYPE;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_REQUESTONLYVALIDATMETADATAROOT;

		[FieldOffset(-1)]
		public readonly static HRESULT WINCODEC_ERR_INVALIDQUERYCHARACTER;

		public int Code
		{
			get
			{
				return HRESULT.GetCode((int)this._value);
			}
		}

		public Standard.Facility Facility
		{
			get
			{
				return HRESULT.GetFacility((int)this._value);
			}
		}

		public bool Failed
		{
			get
			{
				return this._value < 0;
			}
		}

		public bool Succeeded
		{
			get
			{
				return this._value >= 0;
			}
		}

		static HRESULT()
		{
			Class6.yDnXvgqzyB5jw();
			HRESULT.S_OK = new HRESULT(0);
			HRESULT.S_FALSE = new HRESULT(1);
			HRESULT.E_PENDING = new HRESULT((uint)-2147483638);
			HRESULT.E_NOTIMPL = new HRESULT((uint)-2147467263);
			HRESULT.E_NOINTERFACE = new HRESULT((uint)-2147467262);
			HRESULT.E_POINTER = new HRESULT((uint)-2147467261);
			HRESULT.E_ABORT = new HRESULT((uint)-2147467260);
			HRESULT.E_FAIL = new HRESULT((uint)-2147467259);
			HRESULT.E_UNEXPECTED = new HRESULT((uint)-2147418113);
			HRESULT.STG_E_INVALIDFUNCTION = new HRESULT((uint)-2147287039);
			HRESULT.OLE_E_ADVISENOTSUPPORTED = new HRESULT((uint)-2147221501);
			HRESULT.DV_E_FORMATETC = new HRESULT((uint)-2147221404);
			HRESULT.DV_E_TYMED = new HRESULT((uint)-2147221399);
			HRESULT.DV_E_CLIPFORMAT = new HRESULT((uint)-2147221398);
			HRESULT.DV_E_DVASPECT = new HRESULT((uint)-2147221397);
			HRESULT.REGDB_E_CLASSNOTREG = new HRESULT((uint)-2147221164);
			HRESULT.DESTS_E_NO_MATCHING_ASSOC_HANDLER = new HRESULT((uint)-2147217661);
			HRESULT.DESTS_E_NORECDOCS = new HRESULT((uint)-2147217660);
			HRESULT.DESTS_E_NOTALLCLEARED = new HRESULT((uint)-2147217659);
			HRESULT.E_ACCESSDENIED = new HRESULT((uint)-2147024891);
			HRESULT.E_OUTOFMEMORY = new HRESULT((uint)-2147024882);
			HRESULT.E_INVALIDARG = new HRESULT((uint)-2147024809);
			HRESULT.INTSAFE_E_ARITHMETIC_OVERFLOW = new HRESULT((uint)-2147024362);
			HRESULT.COR_E_OBJECTDISPOSED = new HRESULT((uint)-2146232798);
			HRESULT.WC_E_GREATERTHAN = new HRESULT((uint)-1072894429);
			HRESULT.WC_E_SYNTAX = new HRESULT((uint)-1072894419);
			HRESULT.WINCODEC_ERR_GENERIC_ERROR = HRESULT.E_FAIL;
			HRESULT.WINCODEC_ERR_INVALIDPARAMETER = HRESULT.E_INVALIDARG;
			HRESULT.WINCODEC_ERR_OUTOFMEMORY = HRESULT.E_OUTOFMEMORY;
			HRESULT.WINCODEC_ERR_NOTIMPLEMENTED = HRESULT.E_NOTIMPL;
			HRESULT.WINCODEC_ERR_ABORTED = HRESULT.E_ABORT;
			HRESULT.WINCODEC_ERR_ACCESSDENIED = HRESULT.E_ACCESSDENIED;
			HRESULT.WINCODEC_ERR_VALUEOVERFLOW = HRESULT.INTSAFE_E_ARITHMETIC_OVERFLOW;
			HRESULT.WINCODEC_ERR_WRONGSTATE = HRESULT.Make(true, Standard.Facility.WinCodec, 12036);
			HRESULT.WINCODEC_ERR_VALUEOUTOFRANGE = HRESULT.Make(true, Standard.Facility.WinCodec, 12037);
			HRESULT.WINCODEC_ERR_UNKNOWNIMAGEFORMAT = HRESULT.Make(true, Standard.Facility.WinCodec, 12039);
			HRESULT.WINCODEC_ERR_UNSUPPORTEDVERSION = HRESULT.Make(true, Standard.Facility.WinCodec, 12043);
			HRESULT.WINCODEC_ERR_NOTINITIALIZED = HRESULT.Make(true, Standard.Facility.WinCodec, 12044);
			HRESULT.WINCODEC_ERR_ALREADYLOCKED = HRESULT.Make(true, Standard.Facility.WinCodec, 12045);
			HRESULT.WINCODEC_ERR_PROPERTYNOTFOUND = HRESULT.Make(true, Standard.Facility.WinCodec, 12096);
			HRESULT.WINCODEC_ERR_PROPERTYNOTSUPPORTED = HRESULT.Make(true, Standard.Facility.WinCodec, 12097);
			HRESULT.WINCODEC_ERR_PROPERTYSIZE = HRESULT.Make(true, Standard.Facility.WinCodec, 12098);
			HRESULT.WINCODEC_ERR_CODECPRESENT = HRESULT.Make(true, Standard.Facility.WinCodec, 12099);
			HRESULT.WINCODEC_ERR_CODECNOTHUMBNAIL = HRESULT.Make(true, Standard.Facility.WinCodec, 12100);
			HRESULT.WINCODEC_ERR_PALETTEUNAVAILABLE = HRESULT.Make(true, Standard.Facility.WinCodec, 12101);
			HRESULT.WINCODEC_ERR_CODECTOOMANYSCANLINES = HRESULT.Make(true, Standard.Facility.WinCodec, 12102);
			HRESULT.WINCODEC_ERR_INTERNALERROR = HRESULT.Make(true, Standard.Facility.WinCodec, 12104);
			HRESULT.WINCODEC_ERR_SOURCERECTDOESNOTMATCHDIMENSIONS = HRESULT.Make(true, Standard.Facility.WinCodec, 12105);
			HRESULT.WINCODEC_ERR_COMPONENTNOTFOUND = HRESULT.Make(true, Standard.Facility.WinCodec, 12112);
			HRESULT.WINCODEC_ERR_IMAGESIZEOUTOFRANGE = HRESULT.Make(true, Standard.Facility.WinCodec, 12113);
			HRESULT.WINCODEC_ERR_TOOMUCHMETADATA = HRESULT.Make(true, Standard.Facility.WinCodec, 12114);
			HRESULT.WINCODEC_ERR_BADIMAGE = HRESULT.Make(true, Standard.Facility.WinCodec, 12128);
			HRESULT.WINCODEC_ERR_BADHEADER = HRESULT.Make(true, Standard.Facility.WinCodec, 12129);
			HRESULT.WINCODEC_ERR_FRAMEMISSING = HRESULT.Make(true, Standard.Facility.WinCodec, 12130);
			HRESULT.WINCODEC_ERR_BADMETADATAHEADER = HRESULT.Make(true, Standard.Facility.WinCodec, 12131);
			HRESULT.WINCODEC_ERR_BADSTREAMDATA = HRESULT.Make(true, Standard.Facility.WinCodec, 12144);
			HRESULT.WINCODEC_ERR_STREAMWRITE = HRESULT.Make(true, Standard.Facility.WinCodec, 12145);
			HRESULT.WINCODEC_ERR_STREAMREAD = HRESULT.Make(true, Standard.Facility.WinCodec, 12146);
			HRESULT.WINCODEC_ERR_STREAMNOTAVAILABLE = HRESULT.Make(true, Standard.Facility.WinCodec, 12147);
			HRESULT.WINCODEC_ERR_UNSUPPORTEDPIXELFORMAT = HRESULT.Make(true, Standard.Facility.WinCodec, 12160);
			HRESULT.WINCODEC_ERR_UNSUPPORTEDOPERATION = HRESULT.Make(true, Standard.Facility.WinCodec, 12161);
			HRESULT.WINCODEC_ERR_INVALIDREGISTRATION = HRESULT.Make(true, Standard.Facility.WinCodec, 12170);
			HRESULT.WINCODEC_ERR_COMPONENTINITIALIZEFAILURE = HRESULT.Make(true, Standard.Facility.WinCodec, 12171);
			HRESULT.WINCODEC_ERR_INSUFFICIENTBUFFER = HRESULT.Make(true, Standard.Facility.WinCodec, 12172);
			HRESULT.WINCODEC_ERR_DUPLICATEMETADATAPRESENT = HRESULT.Make(true, Standard.Facility.WinCodec, 12173);
			HRESULT.WINCODEC_ERR_PROPERTYUNEXPECTEDTYPE = HRESULT.Make(true, Standard.Facility.WinCodec, 12174);
			HRESULT.WINCODEC_ERR_UNEXPECTEDSIZE = HRESULT.Make(true, Standard.Facility.WinCodec, 12175);
			HRESULT.WINCODEC_ERR_INVALIDQUERYREQUEST = HRESULT.Make(true, Standard.Facility.WinCodec, 12176);
			HRESULT.WINCODEC_ERR_UNEXPECTEDMETADATATYPE = HRESULT.Make(true, Standard.Facility.WinCodec, 12177);
			HRESULT.WINCODEC_ERR_REQUESTONLYVALIDATMETADATAROOT = HRESULT.Make(true, Standard.Facility.WinCodec, 12178);
			HRESULT.WINCODEC_ERR_INVALIDQUERYCHARACTER = HRESULT.Make(true, Standard.Facility.WinCodec, 12179);
		}

		public HRESULT(uint i)
		{
			Class6.yDnXvgqzyB5jw();
			this._value = i;
		}

		public HRESULT(int i)
		{
			Class6.yDnXvgqzyB5jw();
			this._value = (uint)i;
		}

		public override bool Equals(object obj)
		{
			bool flag;
			try
			{
				flag = ((HRESULT)obj)._value == this._value;
			}
			catch (InvalidCastException invalidCastException)
			{
				flag = false;
			}
			return flag;
		}

		public static int GetCode(int error)
		{
			return error & 65535;
		}

		public static Standard.Facility GetFacility(int errorCode)
		{
			return (Standard.Facility)(errorCode >> 16 & 8191);
		}

		public override int GetHashCode()
		{
			return this._value.GetHashCode();
		}

		public static HRESULT Make(bool severe, Standard.Facility facility, int code)
		{
			Standard.Facility facility1;
			if (severe)
			{
				facility1 = (Standard.Facility)-2147483648;
			}
			else
			{
				facility1 = Standard.Facility.Null;
			}
			return new HRESULT((uint)((int)facility1 | (int)facility << 16 | code));
		}

		public static bool operator ==(HRESULT hrLeft, HRESULT hrRight)
		{
			return hrLeft._value == hrRight._value;
		}

		public static explicit operator Int32(HRESULT hr)
		{
			return (int)hr._value;
		}

		public static bool operator !=(HRESULT hrLeft, HRESULT hrRight)
		{
			return !(hrLeft == hrRight);
		}

		public void ThrowIfFailed()
		{
			this.ThrowIfFailed(null);
		}

		public void ThrowIfFailed(string message)
		{
			if (this.Failed)
			{
				if (string.IsNullOrEmpty(message))
				{
					message = this.ToString();
				}
				Exception exceptionForHR = Marshal.GetExceptionForHR((int)this._value, new IntPtr(-1));
				if (exceptionForHR.GetType() != typeof(COMException))
				{
					ConstructorInfo constructor = exceptionForHR.GetType().GetConstructor(new Type[] { typeof(string) });
					if (null != constructor)
					{
						exceptionForHR = constructor.Invoke(new object[] { message }) as Exception;
					}
				}
				else if (this.Facility != Standard.Facility.Win32)
				{
					exceptionForHR = new COMException(message, (int)this._value);
				}
				else
				{
					exceptionForHR = new Win32Exception(this.Code, message);
				}
				throw exceptionForHR;
			}
		}

		public static void ThrowLastError()
		{
			((HRESULT)Win32Error.GetLastError()).ThrowIfFailed();
		}

		public override string ToString()
		{
			FieldInfo[] fields = typeof(HRESULT).GetFields(BindingFlags.Static | BindingFlags.Public);
			for (int i = 0; i < (int)fields.Length; i++)
			{
				FieldInfo fieldInfo = fields[i];
				if (fieldInfo.FieldType == typeof(HRESULT) && (HRESULT)fieldInfo.GetValue(null) == this)
				{
					return fieldInfo.Name;
				}
			}
			if (this.Facility == Standard.Facility.Win32)
			{
				FieldInfo[] fieldInfoArray = typeof(Win32Error).GetFields(BindingFlags.Static | BindingFlags.Public);
				for (int j = 0; j < (int)fieldInfoArray.Length; j++)
				{
					FieldInfo fieldInfo1 = fieldInfoArray[j];
					if (fieldInfo1.FieldType == typeof(Win32Error) && (HRESULT)((Win32Error)fieldInfo1.GetValue(null)) == this)
					{
						return string.Concat("HRESULT_FROM_WIN32(", fieldInfo1.Name, ")");
					}
				}
			}
			return string.Format(CultureInfo.InvariantCulture, "0x{0:X8}", new object[] { this._value });
		}
	}
}