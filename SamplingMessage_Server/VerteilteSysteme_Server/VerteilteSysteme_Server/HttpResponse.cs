using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace VerteilteSysteme_Server
{
    /// <summary>
    /// build a response message depends on the request
    /// </summary>
    class HttpResponse
    {
        private byte[] data = { 0 };    ///< contains the data which will be send as response to the client
        private string status = "";    ///< http status code
        private string reason = "";    ///< reason for the http status code
        private string mime = "";    ///< http mime
        /// <summary>
        /// checks the data from the http request and build a response message
        /// </summary>
        /// <param name="httpRequest">httprequest object</param>
        public void BuildResponse(HttpRequest httpRequest)
        {
            if (httpRequest == null) return;    //checks if http request is null
            mime = "text/json"; //mime is every time the same (JSON)
            if(!httpRequest.url.StartsWith("/sampling_message"))    //check if first url is correct
            {
                status = "404";
                reason = "Invalid Page Request";
            }
            else
            {
                if (httpRequest.requestType == "POST")   //post identify to add a new message
                {
                    if (httpRequest.jsonData == null)   //check if json data is empty
                    {
                        Console.WriteLine(DateTime.Now + " - WARNING: No JSON data found @ " + httpRequest.clientIpAddress);
                        reason = "ERROR: No JSON data found";
                        status = "400";
                        return;
                    }
                    try
                    {
                        byte responseCode = DataBuffer.AddMessage(httpRequest.jsonData["name"], Convert.ToInt32(httpRequest.jsonData["validity_time"]));    //call function to add a new message
                        switch(responseCode)    //build return message depends on return value of the called function
                        {
                            case 0:
                                reason = "INFO: Samplemessage created successfull";
                                status = "200";
                                break;
                            case 1:
                                reason = "WARNING: Samplemessage already exists";
                                status = "403";
                                break;
                            case 2:
                                reason = "ERROR: Internal unknown Error";
                                status = "500";
                                break;
                            default:
                                reason = "ERROR: Internal unknown Error";
                                status = "500";
                                break;
                        }
                    }
                    catch
                    {
                        reason = "ERROR: Invalid Parameter send";
                        status = "400";
                    }
                }
                else if (httpRequest.requestType == "PUT")   //put identify to write the content of a message
                {
                    if (httpRequest.jsonData == null)   //check if json data is empty
                    {
                        Console.WriteLine(DateTime.Now + " - WARNING: No JSON data found @ " + httpRequest.clientIpAddress);
                        reason = "ERROR: No JSON data found";
                        status = "400";
                        return;
                    }
                    byte responseCode = DataBuffer.WriteMessage(httpRequest.url.Replace("/sampling_message/", ""), httpRequest.jsonData["message_content"]);    //call function to write content to a message
                    switch (responseCode)    //build return message depends on return value of the called function
                    {
                        case 0:
                            reason = "INFO: Samplemessage updated successfull";
                            status = "200";
                            break;
                        case 1:
                            reason = "WARNING: Samplemessage does not exists";
                            status = "403";
                            break;
                        case 2:
                            reason = "ERROR: Internal unknown Error";
                            status = "500";
                            break;
                        default:
                            reason = "ERROR: Internal unknown Error";
                            status = "500";
                            break;
                    }
                }
                else if (httpRequest.requestType == "PATCH")   //patch identify to clear the content of a message
                {
                    byte responseCode = DataBuffer.ClearMessage(httpRequest.url.Replace("/sampling_message/", ""));    //call function to clear a message
                    switch (responseCode)    //build return message depends on return value of the called function
                    {
                        case 0:
                            reason = "INFO: Samplemessage cleard and marked as invalid";
                            status = "200";
                            break;
                        case 1:
                            reason = "WARNING: Samplemessage does not exists";
                            status = "403";
                            break;
                        case 2:
                            reason = "ERROR: Internal unknown Error";
                            status = "500";
                            break;
                        default:
                            reason = "ERROR: Internal unknown Error";
                            status = "500";
                            break;
                    }
                }
                else if (httpRequest.requestType == "DELETE")   //delete identify to delete message
                {
                    byte responseCode = DataBuffer.DeleteMessage(httpRequest.url.Replace("/sampling_message/", ""));    //call function to delete a message
                    switch (responseCode)    //build return message depends on return value of the called function
                    {
                        case 0:
                            reason = "INFO: Samplemessage deleted successfull";
                            status = "200";
                            break;
                        case 1:
                            reason = "WARNING: Samplemessage does not exists";
                            status = "403";
                            break;
                        case 2:
                            reason = "ERROR: Internal unknown Error";
                            status = "500";
                            break;
                        default:
                            reason = "ERROR: Internal unknown Error";
                            status = "500";
                            break;
                    }
                }
                else if (httpRequest.requestType == "GET")   //get identify to get the status or the status and content of a message
                {
                    Dictionary<string, string> responseJsonData = new Dictionary<string, string>();
                    status = "200";
                    if (httpRequest.url.StartsWith("/sampling_message/content"))    //get status and content
                    {
                        DataBuffer.MessageReturn responseCode = DataBuffer.GetMessage(httpRequest.url.Replace("/sampling_message/content/", ""));   //call function to get messageinformations
                        switch (responseCode.returnvalue)    //build return message depends on return value of the called function
                        {
                            case 0:
                                reason = "INFO: Samplemessage readed successfull";
                                status = "200";
                                responseJsonData.Add("validity", responseCode.messageIsValid.ToString());   //add the valid parameter
                                responseJsonData.Add("message_status", "INFO: Samplemessage is not empty"); //add emtpy parameter
                                responseJsonData.Add("message_content", responseCode.message);  //add the message content
                                data = BuildJsonData(responseJsonData); //build JSON and put them into a byte array for transfer
                                break;
                            case 1:
                                reason = "WARNING: Samplemessage does not exists";
                                status = "403";
                                break;
                            case 2:
                                reason = "ERROR: Internal unknown Error";
                                status = "500";
                                break;
                            case 3:
                                reason = "INFO: Samplemessage readed successfull";
                                status = "200";
                                responseJsonData.Add("validity", responseCode.messageIsValid.ToString());   //add the valid parameter
                                responseJsonData.Add("message_status", "INFO: Samplemessage is empty"); //add emtpy parameter
                                responseJsonData.Add("message_content", responseCode.message);  //add the message content
                                data = BuildJsonData(responseJsonData); //build JSON and put them into a byte array for transfer
                                break;
                            default:
                                reason = "ERROR: Internal unknown Error";
                                status = "500";
                                break;
                        }
                    }
                    else if (httpRequest.url.StartsWith("/sampling_message/status/"))   //get content only
                    {
                        DataBuffer.MessageStatusReturn responseCode = DataBuffer.GetMessageStatus(httpRequest.url.Replace("/sampling_message/status/", ""));   //call function to get the messagestatus
                        switch (responseCode.returnvalue)    //build return message depends on return value of the called function
                        {
                            case 0:
                                reason = "INFO: Samplemessage readed successfull";
                                status = "200";
                                responseJsonData.Add("validity", responseCode.messageIsValid.ToString());   //add the valid parameter
                                responseJsonData.Add("message_status", "INFO: Samplemessage is not empty"); //add emtpy parameter
                                data = BuildJsonData(responseJsonData); //build JSON and put them into a byte array for transfer
                                break;
                            case 1:
                                reason = "WARNING: Samplemessage does not exists";
                                status = "403";
                                break;
                            case 2:
                                reason = "ERROR: Internal unknown Error";
                                status = "500";
                                break;
                            case 3:
                                reason = "INFO: Samplemessage readed successfull";
                                status = "200";
                                responseJsonData.Add("validity", responseCode.messageIsValid.ToString());   //add the valid parameter
                                responseJsonData.Add("message_status", "INFO: Samplemessage is empty"); //add emtpy parameter
                                data = BuildJsonData(responseJsonData); //build JSON and put them into a byte array for transfer
                                break;
                            default:
                                reason = "ERROR: Internal unknown Error";
                                status = "500";
                                break;
                        }
                    }
                    else //request (page) not found
                    {
                        status = "404";
                        reason = "Invalid Page Request";
                    }
                }
            }
        }

        /// <summary>
        /// build a json from the Dictionary and encode them to a byte array for transfer
        /// </summary>
        /// <param name="responseJsonData">Dictionary which contains the data</param>
        /// <returns>byte array for transfer</returns>
        private static byte[] BuildJsonData(Dictionary<string, string> responseJsonData)
        {
            return Encoding.ASCII.GetBytes(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(responseJsonData));
        }

        /// <summary>
        /// send a responsemessage to the client
        /// </summary>
        /// <param name="networkstream">informations of the tcp client</param>
        /// <param name="httpRequest">httprequest object</param>
        public void SendResponse(NetworkStream networkstream, HttpRequest httpRequest)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(networkstream);    //init new streamwriter
                streamWriter.WriteLine(String.Format("{0} {1} {2}\r\nServer: {3}\r\nContent-Type: {4}\r\nAccept-Ranges: bytes\r\nContent-Length: {5}\r\n",
                    Server.HTTPVERSION, status, reason, Server.SERVER_NAME, mime, data.Length));    //Send http informations
                streamWriter.Flush();   //close streamwriter
                networkstream.Write(data, 0, data.Length);  //send data array
            }
            catch
            {
                Console.WriteLine(DateTime.Now + " - WARNING: Client lost connection @ " + httpRequest.clientIpAddress);
            }
        }
    }
}