using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

namespace VerteilteSysteme_Server
{
    /// <summary>
    /// creation and update a logfile
    /// </summary>
    class Log
    {
        public static string logFolderPath = "";    ///< folder where all logs are stored
        private static string logFolderAndFileName = "";    ///< folderpath where all logs are stored combined with the name of the current logfile
        private static List<string> logBuffer = new List<string>();    ///< buffer where logdata will be temporary saved
        private static Timer updateLogFileTimer;    ///< timer to write buffer in the file

        /// <summary>
        /// init a new logfile and create folder structure
        /// </summary>
        public static bool InitLog()
        {
            try
            {
                Console.WriteLine(DateTime.Now + " - INFO: Init Log");
                AddValueToLog(DateTime.Now + " - INFO: Init Log");
                string[] pathParts = logFolderPath.Split('\\'); //split folderpath to a array which contains each subfolder
                pathParts[0] += "\\";   //add backslash after Driver: .. old windows versions have problems with Drive:Folder and needs Driver:\Folder
                for (int i = 0; i < pathParts.Length; i++)  //create Directory tree if folders does not exists
                {
                    if (i > 0) pathParts[i] = Path.Combine(pathParts[i - 1], pathParts[i]); //add a part of the path in every loop
                    if (!Directory.Exists(pathParts[i]))    //check if Directory dies exists
                    {
                        Console.WriteLine(DateTime.Now + " - INFO: Creating Folder: " + pathParts[i]);
                        AddValueToLog(DateTime.Now + " - INFO: Creating Folder: " + pathParts[i]);
                        Directory.CreateDirectory(pathParts[i]);    //create subdirectory
                    }
                }
                string logFileName = "ServerLog_" + (DateTime.Now).ToString().Replace(" ", "_").Replace(".", "").Replace(":", "") + ".log"; //create a new filename for the logfile
                logFolderAndFileName = logFolderPath + "\\" + logFileName;  //combine logfolderpath and logfilename
                if (!File.Exists(logFolderAndFileName)) //create new file
                {
                    FileStream logFile = File.Create(logFolderAndFileName);
                    logFile.Close();
                }
                updateLogFileTimer = new Timer();   // init timer to update logfile
                updateLogFileTimer.Elapsed += new ElapsedEventHandler(UpdateLogFileTimerEvent);
                updateLogFileTimer.Interval = 1000; //Set interval to 1 second
                updateLogFileTimer.Enabled = true;
                Console.WriteLine(DateTime.Now + " - INFO: Init Log complete");
                AddValueToLog(DateTime.Now + " - INFO: Init Log complete");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(DateTime.Now + " - ERROR: Failed to init Log\r\nC#-Message: " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// add a message to the logbuffer
        /// </summary>
        /// <param name="logMessage">message which will be written to the logbuffer</param>
        public static void AddValueToLog(string logMessage)
        {
            logBuffer.Add(logMessage);  //add message
        }

        /// <summary>
        /// timerfunction for the update logfile timer
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">event</param>
        private static void UpdateLogFileTimerEvent(object sender, EventArgs e)
        {
            UpdateLogFile();
        }

        /// <summary>
        /// write content of the logbuffer to the logfile
        /// </summary>
        private static void UpdateLogFile()
        {
            try
            {
                if (!File.Exists(logFolderAndFileName)) //if file was deleted restart log init and create a new logfile
                {
                    Console.WriteLine(DateTime.Now + " - WARNING: Logfile does not exists, creating a new one...");
                    updateLogFileTimer.Enabled = false;
                    if (!InitLog())
                    {
                        Server.StopServer();
                        return;
                    }
                }
                using (StreamWriter sw = File.AppendText(logFolderAndFileName)) //add content of the logbuffer to the logfile
                {
                    for (int i = 0; i < logBuffer.Count; i++)
                    {
                        sw.WriteLine(logBuffer[i]);
                        logBuffer.RemoveAt(i);
                        i--;
                    }
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now + " - ERROR: Failed to update Log\r\nC#-Message: " + ex.Message);
            }
        }

        /// <summary>
        /// close logfile
        /// </summary>
        public static void CloseLog()
        {
            try
            {
                updateLogFileTimer.Enabled = false; //stop timer
                UpdateLogFile();  //write content to the logfile
                logBuffer.Clear();  //clear buffer
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now + " - ERROR: Failed to close Log\r\nC#-Message: " + ex.Message);
            }
        }
    }
}