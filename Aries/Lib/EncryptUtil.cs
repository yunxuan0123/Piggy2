using System;
using System.Security.Cryptography;
using System.Text;

namespace Aries.Lib
{
	public static class EncryptUtil
	{
		public static string ToMD5(byte[] input)
		{
			return BitConverter.ToString((new MD5CryptoServiceProvider()).ComputeHash(input)).Replace("-", "");
		}

		public static string ToMD5(string input)
		{
			return EncryptUtil.ToMD5(Encoding.Default.GetBytes(input));
		}
	}
}