using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CaveStarter.net
{
	public class ServerClient
	{
		private Server server;
		public Socket workSocket;
		public const int BufferSize = 1024;
		public byte[] tmpBuffer = new byte[BufferSize];
		public StringBuilder totalBuffer = new StringBuilder();

		public string name;

		public ServerClient(Server server)
		{
			this.server = server;
		}

		public void handleData()
		{
			while (true)
			{
				string data = totalBuffer.ToString();
				if (data.IndexOf("\n") == -1)
					break;
				totalBuffer.Remove(0, data.IndexOf("\n")+1);
				data = data.Replace("\r", "");
				string line = data.Substring(0, data.IndexOf("\n"));

				server.form.addLine("[server] Got message " + data);

				string command = line;
				if (command.IndexOf(" ") != -1)
					command = command.Substring(0, command.IndexOf(" "));


				switch (command)
				{
					case "restart":
					case "simplestart":
					case "simplestartslaves":
					case "stopvr":
						foreach (var c in server.clients)
							c.workSocket.Send(Encoding.UTF8.GetBytes("do" + line + "\r\n"));

						break;
					default:
						server.form.addLine("[server] Unknown command: " + command);
						break;
				}




			}
		}
	}
}
