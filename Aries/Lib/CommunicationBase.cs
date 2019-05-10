using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Aries.Lib
{
	internal class CommunicationBase
	{
		public CommunicationBase()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public KeyValuePair<short, string> ReceiveMsg(TcpClient tmpTcpClient)
		{
			byte[] numArray;
			KeyValuePair<short, string> keyValuePair;
			string empty = string.Empty;
			try
			{
				numArray = new byte[tmpTcpClient.ReceiveBufferSize];
			}
			catch (ObjectDisposedException objectDisposedException)
			{
				keyValuePair = new KeyValuePair<short, string>(0, "");
				return keyValuePair;
			}
			int num = 0;
			short num1 = 0;
			if (!tmpTcpClient.Connected)
			{
				return new KeyValuePair<short, string>(0, "");
			}
			NetworkStream stream = tmpTcpClient.GetStream();
			try
			{
				if (stream.CanRead)
				{
					do
					{
						num = stream.Read(numArray, 0, tmpTcpClient.ReceiveBufferSize);
						num1 = BitConverter.ToInt16(numArray, 0);
						empty = Encoding.UTF8.GetString(numArray, 2, num);
						if (empty.Length > 0)
						{
							empty = empty.Substring(0, empty.Length - 2);
						}
						else
						{
							keyValuePair = new KeyValuePair<short, string>(0, "");
							return keyValuePair;
						}
					}
					while (stream.DataAvailable);
				}
			}
			catch (IOException oException)
			{
				Console.WriteLine(oException.ToString());
			}
			return new KeyValuePair<short, string>(num1, empty);
		}

		public void SendMsg(string msg, TcpClient tmpTcpClient, short head)
		{
			NetworkStream stream = tmpTcpClient.GetStream();
			if (stream.CanWrite)
			{
				byte[] bytes = BitConverter.GetBytes(head);
				byte[] numArray = Encoding.Default.GetBytes(msg);
				byte[] numArray1 = new byte[(int)bytes.Length + (int)numArray.Length];
				Buffer.BlockCopy(bytes, 0, numArray1, 0, (int)bytes.Length);
				Buffer.BlockCopy(numArray, 0, numArray1, (int)bytes.Length, (int)numArray.Length);
				stream.Write(numArray1, 0, (int)numArray1.Length);
			}
		}
	}
}