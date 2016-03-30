using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace CaveStarter.net
{
    public partial class CaveStarterForm : Form
    {
        public CaveStarterForm()
        {
            InitializeComponent();

			Task.Factory.StartNew(() => { Thread.Sleep(3500); Invoke(new Action(() => { WindowState = FormWindowState.Minimized; }));  });
			Task.Factory.StartNew(() =>
			{
				Thread.Sleep(100);
				JObject o = JObject.Parse(File.ReadAllText("data/CaveStarter.net/config.json"));
				bool isMaster = false;
				if (((string)o["server"]).ToLower() == System.Environment.MachineName.ToLower())
				{
					Server server = new Server(this, (int)o["port"]);
					isMaster = true;
				}

				Client client = new Client(this, (string)o["server"], (int)o["port"], isMaster);
			});
		
		}

        private void CaveStarterForm_Resize(object sender, EventArgs e)
        {
	        if (this.WindowState == FormWindowState.Minimized)
	        {
		        this.ShowInTaskbar = false;
		        this.Visible = false;
	        }
	        else
	        {
		        this.ShowInTaskbar = true;
		        this.Visible = true;
	        }
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
	        if (this.WindowState == FormWindowState.Minimized)
	        {
		        this.WindowState = FormWindowState.Normal;
		        this.ShowInTaskbar = true;
		        this.Visible = true;
	        }
	        else
	        {
				this.WindowState = FormWindowState.Minimized;
				this.ShowInTaskbar = false;
		        this.Visible = false;
	        }
        }


	    public void addLine(string line)
	    {
			this.Invoke(new Action(() => { realAddLine(line); }));
	    }

		public void showMessage(string line)
		{
			this.Invoke(new Action(() =>
				{
					notifyIcon1.ShowBalloonTip(5000, "CaveStarter", line, ToolTipIcon.Error);
				}));
		}


		private void realAddLine(string line)
		{
			try
			{
				bool scrollDown = logBox.SelectionStart == logBox.Text.Length;
				logBox.Text += "[" + DateTime.Now.ToString() + "] " + line + "\r\n";
				if (scrollDown)
				{
					logBox.SelectionStart = logBox.Text.Length;
					logBox.ScrollToCaret();
					logBox.Refresh();
				}
			}
			catch (Exception)
			{
			}
        }

		private void CaveStarterForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			notifyIcon1.Visible = false;
			System.Environment.Exit(0);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			notifyIcon1.Visible = false;
			notifyIcon1.Visible = true;
		}

		private void CaveStarterForm_Load(object sender, EventArgs e)
		{
		}
    }
}
