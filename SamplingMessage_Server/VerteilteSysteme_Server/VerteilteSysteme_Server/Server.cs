using System;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace VerteilteSysteme_Server
{
    /// <summary>
    /// handles the comunication between server and client
    /// </summary>
    class Server
    {
        public static string HTTPVERSION = "HTTP/1.1";  ///< http version
        public static string SERVER_NAME = "Verteilte_Systeme_Server";  ///< name of the server

        private static TcpListener tcpListener;  ///< listerner for tcp connections
        public static bool serverIsRunning = false;  ///< identify if the server is running or not

        /// <summary>
        /// init the server
        /// </summary>
        /// <param name="serverAddressNew">ip of the server</param>
        /// <param name="serverPortNew">port of the server</param>
        /// <param name="logFolderPathNew">path where the logfiles will be saved</param>
        public static void InitServer(string serverAddress, string serverPortString, string logFolderPathNew)
        {
            if (serverIsRunning) //exit function if server is already running
            {
                Console.WriteLine(DateTime.Now + " - INFO: Server is already started");
                return;
            }
            serverIsRunning = true;
            Console.WriteLine(DateTime.Now + " - INFO: Starting Server @ IP: " + serverAddress + " Port: " + serverPortString);
            Log.AddValueToLog(DateTime.Now + " - INFO: Starting Server @ IP: " + serverAddress + " Port: " + serverPortString);
            int serverPort = 0; //port of the server
            try
            {
                serverPort = Convert.ToInt32(serverPortString);   //write serverport to variable
            }
            catch (Exception ex)    //exit function if error happend
            {
                Console.WriteLine(DateTime.Now + " - ERROR: Invalid Port\r\nC#-Message: " + ex.Message);
                Log.AddValueToLog(DateTime.Now + " - ERROR: Invalid Port\r\nC#-Message: " + ex.Message);
                StopServer();   //Stop server
                return;
            }
            Log.logFolderPath = logFolderPathNew; //set logfolderpath to variable
            DataBuffer.InitBuffer();    //init databuffer
            if (!Log.InitLog()) //init log
            {
                StopServer();    //exit function if log init failed
                return;
            }
            try
            {
                tcpListener = new TcpListener(IPAddress.Parse(serverAddress), serverPort); //init tcp listener
            }
            catch (Exception e)
            {
                Console.WriteLine(DateTime.Now + " - ERROR: Invalid IP Address\r\nC#-Message: " + e.Message);
                Log.AddValueToLog(DateTime.Now + " - ERROR: Invalid IP Address\r\nC#-Message: " + e.Message);
                StopServer();   //Stop server
                return;
            }
            Task.Run(() =>
            {
                RunServer(); //start server in a new task
            });
        }

        /// <summary>
        /// wait for clients and manage their requests
        /// </summary>
        public static void RunServer()
        {
            try
            {
                tcpListener.Start();    //start tcp listener
            }
            catch (Exception e)
            {
                Console.WriteLine(DateTime.Now + " - ERROR: Failed to start tcp listener\r\nC#-Message: " + e.Message);
                Log.AddValueToLog(DateTime.Now + " - ERROR: Failed to start tcp listener\r\nC#-Message: " + e.Message);
                StopServer();   //Stop server
                return;
            }
            Console.WriteLine(DateTime.Now + " - INFO: Server ready");
            while (serverIsRunning) //while server is running
            {
                try
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();    //wait for client
                    Task.Run(() =>
                    {
                        string clientIpAddress = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();
                        Console.WriteLine(DateTime.Now + " - CONNECTED: Client connected @ " + clientIpAddress);
                        Log.AddValueToLog(DateTime.Now + " - CONNECTED: Client connected @ " + clientIpAddress);
                        HandleTcpClient(tcpClient.GetStream(), clientIpAddress); //handle client request
                        tcpClient.Close();  //close connection
                        Console.WriteLine(DateTime.Now + " - DISCONNECTED: Client disconnected @ " + clientIpAddress);
                        Log.AddValueToLog(DateTime.Now + " - DISCONNECTED: Client disconnected @ " + clientIpAddress);
                    });
                }
                catch (Exception e)
                {
                    Thread.Sleep(2);
                    if (serverIsRunning)    //ignore exception if server is not running
                    {
                        Console.WriteLine(DateTime.Now + " - WARNING: TCP Client Error\r\nC#-Message: " + e.Message);
                        Log.AddValueToLog(DateTime.Now + " - WARNING: TCP Client Error\r\nC#-Message: " + e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// handles the request and sends a response to the client
        /// </summary>
        /// <param name="networkStream">tcpclient networkstream</param>
        /// <param name="clientIpAddress">client ip address</param>
        public static void HandleTcpClient(NetworkStream networkStream, string clientIpAddress)
        {
            HttpRequest httpRequest = new HttpRequest();    //init new http request
            HttpResponse httpResponse = new HttpResponse(); //init new http response
            byte[] receiveBuffer = new byte[1000000];   //init buffer for receiving message
            networkStream.Read(receiveBuffer, 0, receiveBuffer.Length); //receive data from the client
            string tcpClientMessage = Encoding.Default.GetString(receiveBuffer);    //encode received data
            httpRequest = httpRequest.GetRequest(tcpClientMessage, clientIpAddress);    //handles request
            if (httpRequest == null) return;    //ignore connection if request was empty
            httpResponse.BuildResponse(httpRequest);    //build response message
            httpResponse.SendResponse(networkStream, httpRequest);  //send response message
        }

        /// <summary>
        /// stop the server
        /// </summary>
        public static void StopServer()
        {
            if (!serverIsRunning)    //check if server is running
            {
                Console.WriteLine(DateTime.Now + " - INFO: Server is already stopped");
                return;
            }
            Console.WriteLine(DateTime.Now + " - INFO: Stopping Server");
            Log.AddValueToLog(DateTime.Now + " - INFO: Stopping Server");
            Thread.Sleep(10);
            try
            {
                tcpListener.Stop(); //stop tcp listener
            }
            catch (Exception e)
            {
                Console.WriteLine(DateTime.Now + " - ERROR: Failed to stop server\r\nC#-Message: " + e.Message);
                Log.AddValueToLog(DateTime.Now + " - ERROR: Failed to stop server\r\nC#-Message: " + e.Message);
            }
            Console.WriteLine(DateTime.Now + " - INFO: Server Stopped");
            Log.AddValueToLog(DateTime.Now + " - INFO: Server Stopped");
            Log.CloseLog(); //close current logfile
            serverIsRunning = false;    //set server status
        }
    }
}