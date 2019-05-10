using Aris.Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Aries.Lib
{
	public class PortForwardingService
	{
		public Aries.Lib.WarpMessage WarpMessage;

		private Dictionary<int, PortForwardingWorker> workers;

		public PortForwardingService()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.workers = new Dictionary<int, PortForwardingWorker>();
		}

		public void AddForwarding(int localPort, string host, int port)
		{
			PortForwardingWorker portForwardingWorker = new PortForwardingWorker(localPort, host, port);
			portForwardingWorker.show += new Action<string>(this.SendMessage);
			if (!this.workers.ContainsKey(localPort))
			{
				this.workers.Add(localPort, portForwardingWorker);
			}
			else
			{
				try
				{
					this.workers[localPort].stop();
					this.workers[localPort] = portForwardingWorker;
				}
				catch (Exception exception)
				{
				}
			}
		}

		public async void Launch(Action<bool> callback)
		{
			await Task.Run(() => {
				this.SendMessage("正在开启端口映射...");
				int num = 0;
				foreach (PortForwardingWorker value in this.workers.Values)
				{
					value.start();
					num += (value.IsRunning ? 0 : 1);
				}
				if (num <= 0)
				{
					callback(true);
					return;
				}
				this.Stop();
				callback(false);
			});
		}

		private void SendErrorMessage(string Msg)
		{
			Aries.Lib.WarpMessage warpMessage = this.WarpMessage;
			if (warpMessage == null)
			{
				return;
			}
			warpMessage(1, Msg);
		}

		private void SendMessage(string Msg)
		{
			Aries.Lib.WarpMessage warpMessage = this.WarpMessage;
			if (warpMessage == null)
			{
				return;
			}
			warpMessage(0, Msg);
		}

		public void Stop()
		{
			foreach (PortForwardingWorker value in this.workers.Values)
			{
				try
				{
					value.stop();
				}
				catch
				{
				}
			}
			this.workers.Clear();
		}
	}
}