using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaveStarter.net
{
	class Client
	{
		private CaveStarterForm form;
		private Socket socket;
		private byte[] tmpBuffer = new byte[1024];
		public StringBuilder totalBuffer = new StringBuilder();
		private bool isMaster;
		private Process runningProcess = null;
		private string hostname;
		private int port;
	
		public Client(CaveStarterForm form, string hostname, int port, bool isMaster)
		{
			this.hostname = hostname;
			this.port = port;
			this.form = form;
			this.isMaster = isMaster;
			connect();

		}

		private void connect()
		{
			form.addLine("[client] Connecting to " + hostname + ":" + port);
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
			socket.BeginConnect(hostname, port, connectCallback, this);
		}

		private void connectCallback(IAsyncResult ar)
		{
			try
			{
				socket.EndConnect(ar);
				if (!socket.Connected)
					Task.Factory.StartNew(() => { Thread.Sleep(10000); connect(); });
			}
			catch (SocketException)
			{
				form.addLine("[client] Unable to connect");
				Task.Factory.StartNew(() => { Thread.Sleep(10000); connect(); });
				return;
			}
			socket.BeginReceive(tmpBuffer, 0, tmpBuffer.Length, 0, readCallback, this);
		}

		private void readCallback(IAsyncResult ar)
		{
			try
			{
				int read = socket.EndReceive(ar);

				// Data was read from the client socket.
				if (read > 0)
				{
					totalBuffer.Append(Encoding.ASCII.GetString(tmpBuffer, 0, read));
					handleData();
					socket.BeginReceive(tmpBuffer, 0, tmpBuffer.Length, 0, readCallback, this);
				}
				else
				{
					form.addLine("[client] Connection closed!");
					form.showMessage("[client] Connection closed!");
					socket.Close();//TODO: give a notification!
					Task.Factory.StartNew(() => { Thread.Sleep(10000); connect(); });
				}
			}
			catch (SocketException)
			{
				form.addLine("[client] Connection closed!");
				form.showMessage("[client] Connection closed!");
				Task.Factory.StartNew(() => { Thread.Sleep(10000); connect(); });
			}
		}

		public void handleData()
		{
			while (true)
			{
				string data = totalBuffer.ToString();
				if (data.IndexOf("\n") == -1)
					break;
				totalBuffer.Remove(0, data.IndexOf("\n") + 1);
				data = data.Replace("\r", "");
				string line = data.Substring(0, data.IndexOf("\n"));

				form.addLine("[client] Got message " + data);

				string command = line;
				if (command.IndexOf(" ") != -1)
					command = command.Substring(0, command.IndexOf(" "));


				switch (command)
				{
					case "hello":
						break;
					case "dostopvr":
						Process[] prs = Process.GetProcesses();
						foreach (Process pr in prs)
							if (pr.ProcessName == "WerFault")
								pr.Kill();

						if (runningProcess != null)
						{
							try
							{
								runningProcess.Kill();
								runningProcess.Close();
							}
							catch (InvalidOperationException)
							{
							}
							runningProcess = null;
				
						}
						break;
					case "dorestart":
						File.WriteAllText("c:\\cave\\runtime\\restartcavestarter.bat", "ping -n 10 127.0.0.1\r\nstart c:\\cave\\runtime\\cavestarter.net.exe\r\ndel c:\\cave\\runtime\\restartcavestarter.bat");
						runningProcess = Process.Start(new ProcessStartInfo("c:\\cave\\runtime\\restartcavestarter.bat"));
						System.Environment.Exit(0);
						break;
					case "dosimplestart":
					case "dosimplestartslaves":
						List<string> p = new List<string>();
						string parameters = line.Substring(command.Length + 1).Trim();

						while (parameters.Length != 0)
						{
							if (parameters[0] == '"')
							{
								int i = 0;
								do
								{
									i++;
									if (parameters[i] == '\\')
										i += 2;
								} while (i < parameters.Length && parameters[i] != '"');
								p.Add(parameters.Substring(1, i-1).Replace("\\\\", "\\"));
								parameters = parameters.Substring(i+1).Trim();
							}
							else if(parameters.IndexOf(" ") != -1)
							{
								p.Add(parameters.Substring(0, parameters.IndexOf(" ")));
								parameters = parameters.Substring(parameters.IndexOf(" ")).Trim();
							}
							else
							{
								p.Add(parameters);
								break;
							}
						}

						if (p.Count != 3)
						{
							form.addLine("[client] Invalid number of parameters for dosimplestart!");
							break;
						}

						if (runningProcess != null)
						{
							try
							{
								runningProcess.Kill();
								runningProcess.Close();
							}
							catch (InvalidOperationException)
							{
								
							}
							runningProcess = null;
						}
						if (command == "dosimplestartslaves" && isMaster)
							break;

//						var cursor = new Cursor(Cursor.Current.Handle);
						if(!isMaster)
							Cursor.Position = new Point(0,0);
						
	
						ProcessStartInfo startinfo = new ProcessStartInfo(p[1], p[2]);
						startinfo.WorkingDirectory = p[0];
						try
						{
							runningProcess = Process.Start(startinfo);
						}
						catch (Exception e)
						{
							if (isMaster)
								MessageBox.Show(e.StackTrace);
						}
						break;

					default:
						form.addLine("[client] Unknown command: " + command);
						break;
				}


			}
		}
	}
}
