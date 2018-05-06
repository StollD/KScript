using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace KScript
{
    public class ConsoleRedirect : TextWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }

        private String _cache = "";

        public override void Write(String value)
        {
            _cache += value;
        }
        
        public override void WriteLine()
        {
            Debug.Log(_cache);
            _cache = "";
        }
    }
}