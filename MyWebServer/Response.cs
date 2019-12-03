using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebServer
{
    public class Response : IResponse
    {
        public Response()
        {
            this.Headers = new Dictionary<string, string>();
            this._StatusCode = -1;
        }
        public IDictionary<string, string> Headers { get; }

        public int ContentLength {
            get {
                return _content.Length;
            }
        }

        public string ContentType { get; set; }
        public int _StatusCode;
        public int StatusCode {
            set { this._StatusCode = value; }
            get {
                if (this._StatusCode < 0)
                {
                    throw new Exception();
                }
                return this._StatusCode;
            }
        }

        public string Status {
            get {
                string s;
                switch (this.StatusCode) {
                    case 200:
                        s = " OK";
                        break;
                    case 404:
                        s = " Not Found";
                        break;
                    case 500:
                        s = " Internal Server Error";
                        break;
                    default:
                        s = " default";
                        break;
                }
                return this.StatusCode.ToString() + s;
            }
        }

        private string _serverHeader = "BIF-SWE1-Server";
        public string ServerHeader {
            get {
                return _serverHeader;
            }
            set{
                if (!string.IsNullOrEmpty(value))
                {
                    string[] temp = value.Split('_');
                    
                    if(temp.Length == 2)
                    { 
                        string s1 = temp[0];
                        s1 = char.ToUpper(s1[0]) + s1.Substring(1);
                        string s2 = temp[1];

                        this._serverHeader = s1 + ": " + value;
                    }
                    else
                    {
                        this._serverHeader = value;
                    }
                }
            } 
        }

        public void AddHeader(string header, string value)
        {
            if(!string.IsNullOrEmpty(header) && !string.IsNullOrEmpty(value))
            {
                if(this.Headers.ContainsKey(header))
                {
                    this.Headers[header] = value;
                }
                else
                {
                    this.Headers.Add(header, value);
                }
            }
        }

        public void Send(Stream network)
        {
            StreamWriter sw = new StreamWriter(network);
            string temp = "HTTP/1.1 " + this.Status + "\n";
            sw.Write(temp);
            if (Headers.Count > 0)
            {
                foreach (var x in this.Headers)
                {
                    temp = x.Key + ": " + x.Value + "\n";
                    sw.Write(temp);
                }
            }
            else if (!string.IsNullOrEmpty(ServerHeader))
            {
                temp = ServerHeader + "\n";
                var ba = UTF8Encoding.UTF8.GetBytes(temp);
                sw.Write(ba);
            }
            sw.Write("\n");
            if (_content != null)
            {
                sw.Write(this.GetContentString());
            }
            else if (ContentType != null)
            {
                throw new Exception();
            }
            sw.Flush();
        }
        public string GetContentString()
        {
            return UTF8Encoding.UTF8.GetString(_content);
        }

        private byte[] _content;
        public void SetContent(string content)
        {
            _content = UTF8Encoding.UTF8.GetBytes(content);
            AddHeader("content-length", "" + _content.Length);
        }

        public void SetContent(byte[] content)
        {
            _content = content;
            AddHeader("content-length", "" + _content.Length);
        }

        public void SetContent(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                _content = ms.ToArray();
                AddHeader("content-length", "" + _content.Length);
            }
        }
    }
}
