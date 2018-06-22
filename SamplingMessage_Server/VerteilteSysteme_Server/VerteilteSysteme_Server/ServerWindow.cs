using System;
using System.Windows.Forms;

namespace VerteilteSysteme_Server
{
    /// <summary>
    /// handles the server GUI
    /// </summary>
    public partial class ServerWindow : Form
    {
        /// <summary>
        /// init the form components
        /// </summary>
        public ServerWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// start the server
        /// </summary>
        /// <param name="sender">button objects</param>
        /// <param name="e">button event</param>
        private void button_ServerStart_Click(object sender, EventArgs e)
        {
            textBox_Output.Text = "";   //clear output
            Server.InitServer(textBox_ServerAddress.Text, textBox_ServerPort.Text, textBox_LogFolderPath.Text); //init and start server
        }

        /// <summary>
        /// stop the server
        /// </summary>
        /// <param name="sender">button objects</param>
        /// <param name="e">button event</param>
        private void button_ServerStop_Click(object sender, EventArgs e)
        {
            Server.StopServer();
        }

        /// <summary>
        /// browse for the destination of the logfolder
        /// </summary>
        /// <param name="sender">button objects</param>
        /// <param name="e">button event</param>
        private void button_LogFolderPath_Browse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "Select folder for Logfiles";
            folderDialog.SelectedPath = @"C:\";       // default path
            DialogResult dialogResult = folderDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                textBox_LogFolderPath.Text = folderDialog.SelectedPath; //write selected path to textbox if "OK" was pressed
            }
        }

        /// <summary>
        /// event if the server GUI will close
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">event</param>
        private void ServerWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.CloseLog(); //close logfile
        }

        /// <summary>
        /// write the message into the output textbox
        /// </summary>
        /// <param name="message">message which will be added to the textbox</param>
        public void SetOutput(string message)
        {
            Invoke((MethodInvoker)delegate  //invoke to GUI Thread
            {
                textBox_Output.Text += message + "\r\n";    //write new text
                textBox_Output.Select(textBox_Output.TextLength + 1, 0);    //set cursor to last textposition
                textBox_Output.ScrollToCaret(); //jump to the bottom of the text
            });
        }
    }
}