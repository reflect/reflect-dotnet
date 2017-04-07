﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using static Newtonsoft.Json.JsonConvert;
using static System.Text.Encoding;

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
            Equals, NotEquals, GreaterThan, LessThan, GreaterThanOrEqualTo, LessThanOrEqualTo, Contains, DoesNotContain
        }

        private string opString()
        {
            switch (op)
            {
                case Op.Equals: return "=";
                case Op.NotEquals: return "!=";
                case Op.GreaterThan: return ">";
                case Op.LessThan: return "<";
                case Op.GreaterThanOrEqualTo: return ">=";
                case Op.LessThanOrEqualTo: return "<=";
                case Op.Contains: return "=~";
                case Op.DoesNotContain: return "!~";
                default: return "";
            }
        }

        public override string ToString()
        {
            object[] s = new object[4]{field, opString(), value, anyValue};
            return SerializeObject(s);
        }

        public Parameter(string f, Op o, string v)
        {
            field = f;
            op = o;
            value = v;
            anyValue = new string[0];
        }

        public Parameter(string f, Op o, string[] anyV)
        {
            field = f;
            op = o;
            value = "";
            anyValue = anyV;
        }
    }

    public class TokenGenerator
    {
        public static string Generate(string secretKey, Parameter[] parameters)
        {
            string[] ps = new string[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                ps[i] = parameters[i].ToString();
            }

            Array.Sort(ps);
            string joined = String.Join("\n", ps);
            HMACSHA256 mac = new HMACSHA256(Encoding.ASCII.GetBytes(secretKey));
            byte[] hash = mac.ComputeHash(Encoding.ASCII.GetBytes($"V2\n{joined}"));
            return $"=2={System.Convert.ToBase64String(hash)}";
        }
    }
}
