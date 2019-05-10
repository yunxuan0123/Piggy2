using System;
using System.IO;

namespace Aries.Lib
{
	public static class FileUtil
	{
		public static bool FileMD5Validation(string md5Str, string filePath)
		{
			bool flag;
			if (!File.Exists(filePath))
			{
				return false;
			}
			using (FileStream fileStream = File.OpenRead(filePath))
			{
				byte[] numArray = new byte[checked((IntPtr)fileStream.Length)];
				fileStream.Read(numArray, 0, (int)numArray.Length);
				string mD5 = EncryptUtil.ToMD5(numArray);
				Console.WriteLine(mD5);
				flag = md5Str == mD5;
			}
			return flag;
		}
	}
}