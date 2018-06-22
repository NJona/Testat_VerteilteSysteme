using System;
using System.Collections.Generic;
using System.Threading;

namespace VerteilteSysteme_Server
{
    /// <summary>
    /// handles the reading and writing of the databuffer
    /// </summary>
    class DataBuffer
    {
        private static Dictionary<string, MessageObject> buffer;    ///< contains the data
        private static bool writeLock = false;    ///< true if a writing action will performed
            
        private struct MessageObject    ///< contais the message content and timestamps
        {
            public string message;
            public DateTime createTime;
            public DateTime? expireTime;
            public int expireTimeInS;
        }

        public struct MessageStatusReturn   ///< contais a status of the message
        {
            public bool messageIsValid;
            public byte returnvalue;
        }

        public struct MessageReturn ///< contais the messagecontent and the status of a message
        {
            public string message;
            public bool messageIsValid;
            public byte returnvalue;
        }

        /// <summary>
        /// init a new databuffer
        /// </summary>
        public static void InitBuffer()
        {
            buffer = new Dictionary<string, MessageObject>();
        }

        /// <summary>
        /// creates new empty sampling message and set the expire time
        /// </summary>
        /// <param name="messageName">name of the message</param>
        /// <param name="expireTime">time to expire in seconds, 0: does not expire</param>
        /// <returns>0: samplemessage created, 1: samplemessage allready exists, 2: internal error</returns>
        public static byte AddMessage(string messageName, int expireTime)
        {
            try
            {
                if (buffer.ContainsKey(messageName))    //check if message already exists
                {
                    return 1;
                }
                MessageObject messageObject = new MessageObject();  //create a new messageobject
                messageObject.message = ""; //message is empty
                messageObject.createTime = DateTime.Now;    //set creationtime
                messageObject.expireTimeInS = expireTime;   //set time in s when the message will expire
                messageObject = CalculateExpireTimestamp(messageObject, messageObject.expireTimeInS);   //calculate the exipre time
                while (writeLock) Thread.Sleep(1);  //check if buffer is locked
                writeLock = true;   //lock buffer
                buffer.Add(messageName, messageObject); //add message
                writeLock = false;  //unlock buffer
                return 0;
            }
            catch { return 2; }
        }

        /// <summary>
        /// updated the content of a sample message. updates also the expire time
        /// </summary>
        /// <param name="messageName">name of the message</param>
        /// <param name="message">content of the message</param>
        /// <returns>0: samplemessage updated, 1: samplemessage does not exists, 2: internal error</returns>
        public static byte WriteMessage(string messageName, string message)
        {
            try
            {
                if (!buffer.ContainsKey(messageName))    //check if message already exists
                {
                    return 1;
                }
                MessageObject messageObject = buffer[messageName];  //read message object
                messageObject.message = message;    //set message´content
                messageObject = CalculateExpireTimestamp(messageObject, messageObject.expireTimeInS);   //calculate new expire time
                while (writeLock) Thread.Sleep(1);  //check if buffer is locked
                writeLock = true;   //lock buffer
                buffer.Remove(messageName); //remove old message
                buffer.Add(messageName, messageObject); //write new message
                writeLock = false;  //unlock buffer
                return 0;
            }
            catch { return 2; }
        }

        /// <summary>
        /// clears the samplemessage and marks them as invalid
        /// </summary>
        /// <param name="messageName">name of the message</param>
        /// <returns>0: samplemessage cleared, 1: samplemessage does not exists, 2: internal error</returns>
        public static byte ClearMessage(string messageName)
        {
            try
            {
                if (!buffer.ContainsKey(messageName))    //check if message already exists
                {
                    return 1;
                }
                MessageObject messageObject = buffer[messageName];  //get message
                messageObject.message = ""; //clear message
                messageObject.expireTime = DateTime.MinValue;   //mark message as invalid
                while (writeLock) Thread.Sleep(1);  //check if buffer is locked
                writeLock = true;   //lock buffer
                buffer.Remove(messageName); //remove old message
                buffer.Add(messageName, messageObject); //write new message
                writeLock = false;  //unlock buffer
            }
            catch { return 2; }
            return 0;
        }

        /// <summary>
        /// deletes a sample message
        /// </summary>
        /// <param name="messageName">name of the message</param>
        /// <returns>0: samplemessage deleted, 1: samplemessage does not exists, 2: internal error</returns>
        public static byte DeleteMessage(string messageName)
        {
            try
            {
                if (!buffer.ContainsKey(messageName))    //check if message already exists
                {
                    return 1;
                }
                while (writeLock) Thread.Sleep(1);  //check if buffer is locked
                writeLock = true;   //lock buffer
                buffer.Remove(messageName); //delete message
                writeLock = false;  //unlock buffer
            }
            catch { return 2; }
            return 0;
        }

        /// <summary>
        /// returns the status of a samplemessage
        /// </summary>
        /// <param name="messageName">name of the message</param>
        /// <returns>[messageIsValid(0: not valid, 1: valid), returnValue(0:samplemessage readed, 1:samplemessage does not exists, 2:internal error, 3:samplemessage is empty)]</returns>
        public static MessageStatusReturn GetMessageStatus(string messageName)
        {
            MessageStatusReturn messageStatusReturn = new MessageStatusReturn();    //init return values
            messageStatusReturn.messageIsValid = false;
            messageStatusReturn.returnvalue = 0;
            try
            {
                if (!buffer.ContainsKey(messageName))    //check if message already exists
                {
                    messageStatusReturn.returnvalue = 1;
                    return messageStatusReturn;
                }
                if (buffer[messageName].expireTime == null || buffer[messageName].expireTime > DateTime.Now) messageStatusReturn.messageIsValid = true; //if no expire time or expiretime is higher than actual time --> valid message
                if (buffer[messageName].message == "") messageStatusReturn.returnvalue = 3; //check if message is empty
            }
            catch
            {
                messageStatusReturn.messageIsValid = false;
                messageStatusReturn.returnvalue = 2;
                return messageStatusReturn;
            }
            return messageStatusReturn;
        }

        /// <summary>
        /// returns the content and status of a samplemessage
        /// </summary>
        /// <param name="messageName">name of the message</param>
        /// <returns>[message(content of the message), messageIsValid(0: not valid, 1: valid), returnValue(0:samplemessage readed, 1:samplemessage does not exists, 2:internal error, 3:samplemessage is empty)]</returns>
        public static MessageReturn GetMessage(string messageName)
        {
            MessageReturn messageReturn = new MessageReturn();    //init return values
            messageReturn.message = "";
            messageReturn.messageIsValid = false;
            messageReturn.returnvalue = 0;
            try
            {
                if (!buffer.ContainsKey(messageName))    //check if message already exists
                {
                    messageReturn.returnvalue = 1;
                    return messageReturn;
                }
                if (buffer[messageName].expireTime == null || buffer[messageName].expireTime > DateTime.Now) messageReturn.messageIsValid = true; //if no expire time or expiretime is higher than actual time --> valid message
                if (buffer[messageName].message == "") messageReturn.returnvalue = 3;   //check if message is empty
                messageReturn.message = buffer[messageName].message;    //get message content
            }
            catch
            {
                messageReturn.returnvalue = 2;
                return messageReturn;
            }
            return messageReturn;
        }

        /// <summary>
        /// calculate the expired timestamp
        /// </summary>
        /// <param name="messageObject"></param>
        /// <param name="expireTime"></param>
        /// <returns></returns>
        private static MessageObject CalculateExpireTimestamp(MessageObject messageObject, int expireTime)
        {
            if (expireTime == 0)    //set expire time to "null" because the message does not expire
            {
                messageObject.expireTime = null;
            }
            else
            {
                messageObject.expireTime = DateTime.Now.AddSeconds(expireTime); //add expire time to the actual time
            }
            return messageObject;
        }
    }
}