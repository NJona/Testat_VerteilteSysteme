using System;
using System.Windows.Forms;

namespace VerteilteSysteme_Server
{
    /// <summary>
    /// starts the application and redirect console output
    /// </summary>
    static class ApplicationRun
    {
        public static ServerWindow serverWindow;
        /// <summary>
        /// starting point of the application
        /// </summary>
        [STAThread]
        static void Main()
        {

            using (var consoleWriter = new ConsoleWriter()) //open consolewriter to redirect everything from "Console.WriteLine()"
            {
                consoleWriter.WriteLineEvent += ConsoleWriter_WriteLineEvent;   //add new event
                Console.SetOut(consoleWriter);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                serverWindow = new ServerWindow();  //init GUI class
                Application.Run(serverWindow);  //show GUI
            }
        }

        /// <summary>
        /// write all console output into the stauslabel in the mainform
        /// </summary>               
        /// <param name="sender">object</param>
        /// <param name="e">event</param>
        static void ConsoleWriter_WriteLineEvent(object sender, ConsoleWriterEventArgs e)
        {
            try
            {
                serverWindow.SetOutput(e.Value);    //writes all console traffic to output textbox
            }
            catch { }
        }
    }
}
