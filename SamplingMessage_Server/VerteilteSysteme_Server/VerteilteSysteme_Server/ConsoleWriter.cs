using System;                      
using System.IO;     
using System.Text;            

namespace VerteilteSysteme_Server
{
    /// <summary>
    /// redirect output from the console
    /// </summary>
    public class ConsoleWriterEventArgs : EventArgs
    {
        public string Value { get; private set; }  ///< stores current string
        
        /// <summary>
        /// set value
        /// </summary>
        /// <param name="value">current string</param>
        public ConsoleWriterEventArgs(string value)
        {
            Value = value;
        }
    }

    /// <summary>
    /// redirect output from the console
    /// </summary>
    public class ConsoleWriter : TextWriter
    {
        /// <summary>
        /// get uft8 encoding
        /// </summary>
        public override Encoding Encoding { get { return Encoding.UTF8; } }

        /// <summary>
        /// capture writes
        /// </summary>
        /// <param name="value">current string</param>
        public override void Write(string value)
        {
            if (WriteEvent != null) WriteEvent(this, new ConsoleWriterEventArgs(value));
            base.Write(value);
        }

        /// <summary>
        /// capture writelines
        /// </summary>
        /// <param name="value">current string</param>
        public override void WriteLine(string value)
        {
            if (WriteLineEvent != null) WriteLineEvent(this, new ConsoleWriterEventArgs(value));
            base.WriteLine(value);
        }

        public event EventHandler<ConsoleWriterEventArgs> WriteEvent; ///< event for write capture
        public event EventHandler<ConsoleWriterEventArgs> WriteLineEvent;  ///< event for writeline capture
    }
}
