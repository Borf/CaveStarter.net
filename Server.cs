using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CaveStarter.net
{
	public class Server
	{
		private int port;
		Thread thread;
		ManualResetEvent allDone = new ManualResetEvent(false);
		public CaveStarterForm form;


		public List<ServerClient> clients = new List<ServerClient>();
		


		public Server(CaveStarterForm form, int port)
		{
			this.form = form;
			this.port = port;
			thread = new Thread(new ThreadStart(this.loop));
			thread.Start();
		}


		public void waitForFinish()
		{
			thread.Join();
		}
		public void loop()
		{
			IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
			IPEndPoint localEP = new IPEndPoint(new IPAddress(0), port);

			Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

			serverSocket.Bind(localEP);
			serverSocket.Listen(10);
			form.addLine("[server] Starting to listen on port " + port);
			while (true)
			{
				allDone.Reset();
				serverSocket.BeginAccept(new AsyncCallback(this.acceptCallback), serverSocket);
				allDone.WaitOne();
			}

		}
		public void acceptCallback(IAsyncResult ar)
		{
			// Get the socket that handles the client request.
			Socket listener = (Socket)ar.AsyncState;
			Socket handler = listener.EndAccept(ar);

			// Signal the main thread to continue.
			allDone.Set();
            
			// Create the state object.
			ServerClient client = new ServerClient(this);
			clients.Add(client);
			client.workSocket = handler;

			form.addLine("[server] Client from " + handler.RemoteEndPoint.ToString() + " logged in");

			handler.Send(Encoding.UTF8.GetBytes("hello\r\n"));

			handler.BeginReceive(client.tmpBuffer, 0, ServerClient.BufferSize, 0, new AsyncCallback(this.readCallback), client);
		}
		public void readCallback(IAsyncResult ar)
		{
			ServerClient client = (ServerClient)ar.AsyncState;
			Socket handler = client.workSocket;
			try
			{
				// Read data from the client socket.
				int read = handler.EndReceive(ar);

				// Data was read from the client socket.
				if (read > 0)
				{
					client.totalBuffer.Append(Encoding.ASCII.GetString(client.tmpBuffer, 0, read));
					lock (this)
					{
						client.handleData();
					}
					handler.BeginReceive(client.tmpBuffer, 0, ServerClient.BufferSize, 0, new AsyncCallback(readCallback), client);
				}
				else
				{
					clients.Remove(client);
					form.addLine("[server] Connection for " + client.name + "closed");
					handler.Close();
				}
			}
			catch (SocketException)
			{
				clients.Remove(client);
				form.addLine("[server] Connection closed");
				handler.Close();
			}
		}


		public void quit()
		{
			form.addLine("[server] Quitting server");
			Process.GetCurrentProcess().Kill();
		}

	}
}
