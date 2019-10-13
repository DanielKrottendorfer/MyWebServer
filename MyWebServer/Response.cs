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

        public int ContentLength => throw new NotImplementedException();

        public string ContentType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
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
                        s= " default";
                        break;
                }
                return this.StatusCode.ToString() + s;
            }
        }

        public string ServerHeader { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
            throw new NotImplementedException();
        }

        public void SetContent(string content)
        {
            throw new NotImplementedException();
        }

        public void SetContent(byte[] content)
        {
            throw new NotImplementedException();
        }

        public void SetContent(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
