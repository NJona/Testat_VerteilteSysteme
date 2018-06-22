namespace VerteilteSysteme_Server
{
    partial class ServerWindow
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox_ServerAddress = new System.Windows.Forms.TextBox();
            this.label_ServerAddress = new System.Windows.Forms.Label();
            this.button_ServerStart = new System.Windows.Forms.Button();
            this.label_ServerPort = new System.Windows.Forms.Label();
            this.textBox_ServerPort = new System.Windows.Forms.TextBox();
            this.button_ServerStop = new System.Windows.Forms.Button();
            this.textBox_Output = new System.Windows.Forms.TextBox();
            this.label_Output = new System.Windows.Forms.Label();
            this.label_LogFolderPath = new System.Windows.Forms.Label();
            this.textBox_LogFolderPath = new System.Windows.Forms.TextBox();
            this.button_LogFolderPath_Browse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_ServerAddress
            // 
            this.textBox_ServerAddress.Location = new System.Drawing.Point(94, 13);
            this.textBox_ServerAddress.Name = "textBox_ServerAddress";
            this.textBox_ServerAddress.Size = new System.Drawing.Size(100, 20);
            this.textBox_ServerAddress.TabIndex = 0;
            this.textBox_ServerAddress.Text = "127.0.0.1";
            // 
            // label_ServerAddress
            // 
            this.label_ServerAddress.AutoSize = true;
            this.label_ServerAddress.Location = new System.Drawing.Point(12, 16);
            this.label_ServerAddress.Name = "label_ServerAddress";
            this.label_ServerAddress.Size = new System.Drawing.Size(48, 13);
            this.label_ServerAddress.TabIndex = 1;
            this.label_ServerAddress.Text = "Address:";
            // 
            // button_ServerStart
            // 
            this.button_ServerStart.Location = new System.Drawing.Point(713, 16);
            this.button_ServerStart.Name = "button_ServerStart";
            this.button_ServerStart.Size = new System.Drawing.Size(75, 23);
            this.button_ServerStart.TabIndex = 2;
            this.button_ServerStart.Text = "Start";
            this.button_ServerStart.UseVisualStyleBackColor = true;
            this.button_ServerStart.Click += new System.EventHandler(this.button_ServerStart_Click);
            // 
            // label_ServerPort
            // 
            this.label_ServerPort.AutoSize = true;
            this.label_ServerPort.Location = new System.Drawing.Point(12, 42);
            this.label_ServerPort.Name = "label_ServerPort";
            this.label_ServerPort.Size = new System.Drawing.Size(29, 13);
            this.label_ServerPort.TabIndex = 4;
            this.label_ServerPort.Text = "Port:";
            // 
            // textBox_ServerPort
            // 
            this.textBox_ServerPort.Location = new System.Drawing.Point(94, 39);
            this.textBox_ServerPort.Name = "textBox_ServerPort";
            this.textBox_ServerPort.Size = new System.Drawing.Size(100, 20);
            this.textBox_ServerPort.TabIndex = 3;
            this.textBox_ServerPort.Text = "81";
            // 
            // button_ServerStop
            // 
            this.button_ServerStop.Location = new System.Drawing.Point(713, 45);
            this.button_ServerStop.Name = "button_ServerStop";
            this.button_ServerStop.Size = new System.Drawing.Size(75, 23);
            this.button_ServerStop.TabIndex = 5;
            this.button_ServerStop.Text = "Stop";
            this.button_ServerStop.UseVisualStyleBackColor = true;
            this.button_ServerStop.Click += new System.EventHandler(this.button_ServerStop_Click);
            // 
            // textBox_Output
            // 
            this.textBox_Output.Location = new System.Drawing.Point(15, 111);
            this.textBox_Output.Multiline = true;
            this.textBox_Output.Name = "textBox_Output";
            this.textBox_Output.ReadOnly = true;
            this.textBox_Output.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Output.Size = new System.Drawing.Size(776, 327);
            this.textBox_Output.TabIndex = 6;
            // 
            // label_Output
            // 
            this.label_Output.AutoSize = true;
            this.label_Output.Location = new System.Drawing.Point(9, 95);
            this.label_Output.Name = "label_Output";
            this.label_Output.Size = new System.Drawing.Size(42, 13);
            this.label_Output.TabIndex = 7;
            this.label_Output.Text = "Output:";
            // 
            // label_LogFolderPath
            // 
            this.label_LogFolderPath.AutoSize = true;
            this.label_LogFolderPath.Location = new System.Drawing.Point(9, 67);
            this.label_LogFolderPath.Name = "label_LogFolderPath";
            this.label_LogFolderPath.Size = new System.Drawing.Size(79, 13);
            this.label_LogFolderPath.TabIndex = 8;
            this.label_LogFolderPath.Text = "Log Foderpath:";
            // 
            // textBox_LogFolderPath
            // 
            this.textBox_LogFolderPath.Location = new System.Drawing.Point(94, 64);
            this.textBox_LogFolderPath.Name = "textBox_LogFolderPath";
            this.textBox_LogFolderPath.Size = new System.Drawing.Size(372, 20);
            this.textBox_LogFolderPath.TabIndex = 9;
            this.textBox_LogFolderPath.Text = "C:\\VerteilteSystemeLogs";
            // 
            // button_LogFolderPath_Browse
            // 
            this.button_LogFolderPath_Browse.Location = new System.Drawing.Point(472, 62);
            this.button_LogFolderPath_Browse.Name = "button_LogFolderPath_Browse";
            this.button_LogFolderPath_Browse.Size = new System.Drawing.Size(75, 23);
            this.button_LogFolderPath_Browse.TabIndex = 10;
            this.button_LogFolderPath_Browse.Text = "Browse";
            this.button_LogFolderPath_Browse.UseVisualStyleBackColor = true;
            this.button_LogFolderPath_Browse.Click += new System.EventHandler(this.button_LogFolderPath_Browse_Click);
            // 
            // ServerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_LogFolderPath_Browse);
            this.Controls.Add(this.textBox_LogFolderPath);
            this.Controls.Add(this.label_LogFolderPath);
            this.Controls.Add(this.label_Output);
            this.Controls.Add(this.textBox_Output);
            this.Controls.Add(this.button_ServerStop);
            this.Controls.Add(this.label_ServerPort);
            this.Controls.Add(this.textBox_ServerPort);
            this.Controls.Add(this.button_ServerStart);
            this.Controls.Add(this.label_ServerAddress);
            this.Controls.Add(this.textBox_ServerAddress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ServerWindow";
            this.Text = "Verteilte Systeme Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerWindow_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_ServerAddress;
        private System.Windows.Forms.Label label_ServerAddress;
        private System.Windows.Forms.Button button_ServerStart;
        private System.Windows.Forms.Label label_ServerPort;
        private System.Windows.Forms.TextBox textBox_ServerPort;
        private System.Windows.Forms.Button button_ServerStop;
        private System.Windows.Forms.TextBox textBox_Output;
        private System.Windows.Forms.Label label_Output;
        private System.Windows.Forms.Label label_LogFolderPath;
        private System.Windows.Forms.TextBox textBox_LogFolderPath;
        private System.Windows.Forms.Button button_LogFolderPath_Browse;
    }
}

