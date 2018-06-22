using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace VerteilteSysteme_Server
{
    /// <summary>
    /// split the values which where received via tcp/http
    /// </summary>
    class HttpRequest
    {

        public string requestType = ""; ///< http request code
        public string url = ""; ///< url which was accessed by the client
        public Dictionary<string, string> jsonData; ///< data which where received from the client
        public string clientIpAddress = ""; ///< ip of the client which send a request

        /// <summary>
        /// split the received values from the client in practical parts
        /// </summary>
        /// <param name="requestString">unfiltered data which was sent by the client</param>
        /// <param name="clientIp">ip of the client which send a request</param>
        /// <returns>splited data as httprequest object</returns>
        public HttpRequest GetRequest(string requestString, string clientIp)
        {
            if (String.IsNullOrEmpty(requestString)) return null;  //error if no data are received
            if (String.IsNullOrEmpty(clientIp)) return null;  //error if client ip was not received
            clientIpAddress = clientIp; //set client ip
            string[] requestSplit = requestString.Split(' ');   //splits received data on every space
            requestType = requestSplit[0];  //set request type
            url = requestSplit[1];  //set url
            try
            {
                string jsonDataString = "{" + requestString.Split('{')[1].Split('}')[0] + "}";  //get json data
                jsonData = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(jsonDataString);  //write jsondata into a object
            }
            catch
            {
                jsonData = null;
            }
            return this;
        }
    }
}