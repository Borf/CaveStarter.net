namespace CaveStarter.net
{
    partial class CaveStarterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaveStarterForm));
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.logBox = new System.Windows.Forms.TextBox();
			this.btnReconnect = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
			this.notifyIcon1.Text = "Cave Starter";
			this.notifyIcon1.Visible = true;
			this.notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);
			// 
			// logBox
			// 
			this.logBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.logBox.BackColor = System.Drawing.Color.White;
			this.logBox.Font = new System.Drawing.Font("Consolas", 9.75F);
			this.logBox.Location = new System.Drawing.Point(12, 12);
			this.logBox.Multiline = true;
			this.logBox.Name = "logBox";
			this.logBox.ReadOnly = true;
			this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.logBox.Size = new System.Drawing.Size(684, 378);
			this.logBox.TabIndex = 0;
			// 
			// btnReconnect
			// 
			this.btnReconnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnReconnect.Location = new System.Drawing.Point(621, 399);
			this.btnReconnect.Name = "btnReconnect";
			this.btnReconnect.Size = new System.Drawing.Size(75, 23);
			this.btnReconnect.TabIndex = 1;
			this.btnReconnect.Text = "Reconnect";
			this.btnReconnect.UseVisualStyleBackColor = true;
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 60000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// CaveStarterForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(708, 434);
			this.Controls.Add(this.btnReconnect);
			this.Controls.Add(this.logBox);
			this.Name = "CaveStarterForm";
			this.Text = "CaveStarterStatus";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CaveStarterForm_FormClosing);
			this.Load += new System.EventHandler(this.CaveStarterForm_Load);
			this.Resize += new System.EventHandler(this.CaveStarterForm_Resize);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.Button btnReconnect;
		private System.Windows.Forms.Timer timer1;
    }
}

