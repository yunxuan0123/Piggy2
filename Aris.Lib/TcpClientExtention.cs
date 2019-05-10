using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace Aris.Lib
{
	internal static class TcpClientExtention
	{
		public static bool Connected(this TcpClient c)
		{
			Socket client = c.Client;
			if (client.Poll(1000, SelectMode.SelectRead) & client.Available == 0)
			{
				return false;
			}
			return true;
		}
	}
}