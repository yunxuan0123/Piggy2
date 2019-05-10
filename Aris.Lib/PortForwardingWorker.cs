using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Aris.Lib
{
	internal class PortForwardingWorker
	{
		private string host;

		private int port;

		private int localPort;

		private Dictionary<NetworkStream, bool> first;

		private TcpListener listener;

		public Action<string> show;

		public bool IsRunning
		{
			get;
			set;
		}

		public PortForwardingWorker(int localPort, string host, int port)
		{
			Class6.yDnXvgqzyB5jw();
			this.first = new Dictionary<NetworkStream, bool>();
			base();
			this.host = host;
			this.port = port;
			this.localPort = localPort;
			this.listener = new TcpListener(IPAddress.Parse("0.0.0.0"), localPort);
		}

		private void Pipe(NetworkStream a, NetworkStream b)
		{
			a.CopyToAsync(b);
			b.CopyToAsync(a);
		}

		public async void start()
		{
			PortForwardingWorker.<start>d__13 variable = new PortForwardingWorker.<start>d__13();
			variable.<>4__this = this;
			variable.<>t__builder = AsyncVoidMethodBuilder.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<PortForwardingWorker.<start>d__13>(ref variable);
		}

		public void stop()
		{
			this.IsRunning = false;
			this.listener.Stop();
		}
	}
}