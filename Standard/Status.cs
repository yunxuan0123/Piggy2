using System;

namespace Standard
{
	internal enum Status
	{
		Ok,
		GenericError,
		InvalidParameter,
		OutOfMemory,
		ObjectBusy,
		InsufficientBuffer,
		NotImplemented,
		const_7,
		WrongState,
		Aborted,
		FileNotFound,
		ValueOverflow,
		AccessDenied,
		UnknownImageFormat,
		FontFamilyNotFound,
		FontStyleNotFound,
		NotTrueTypeFont,
		UnsupportedGdiplusVersion,
		GdiplusNotInitialized,
		PropertyNotFound,
		PropertyNotSupported,
		ProfileNotFound
	}
}