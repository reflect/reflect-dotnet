using System;
using static Newtonsoft.Json.JsonConvert;
using static System.Text.Encoding;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Reflect
{
    public class Parameter
    {
        public string field;
        public Op op;
        public string value;
        public string[] anyValue;

        public enum Op
        {
            Equals, NotEquals
        }

        private string opString()
        {
            switch (op)
            {
                case Op.Equals: return "=";
                case Op.NotEquals: return "!=";
                default: return "=";
            }
        }

        public override string ToString()
        {
            string[] s = new string[3]{field, opString(), value};
            return SerializeObject(s);
        }

        public Parameter(string f, Op o, string v)
        {
            field = f;
            op = o;
            value = v;
        }

        public Parameter(string f, Op o, string[] anyV)
        {
            field = f;
            op = o;
            anyValue = anyV;
        }
    }

    public class TokenGenerator
    {
        public static string Generate(string secretKey, List<Parameter> parameters)
        {
            List<string> s = new List<string>();

            foreach (Parameter param in parameters)
            {
                if (param.anyValue != null)
                {
                    string[] vals = param.anyValue;
                    Array.Sort(vals);
                }

                s.Add("some string");
            }
            string joined = String.Join("\n", s);

            string msg = $"V2\n{joined}";
            HMACSHA256 mac = new HMACSHA256(Encoding.ASCII.GetBytes(secretKey));
            byte[] hash = mac.ComputeHash(Encoding.ASCII.GetBytes(msg));

            return $"=2={msg}";
        }
    }
}
