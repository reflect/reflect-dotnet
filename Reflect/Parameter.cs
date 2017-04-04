using System;

namespace Reflect
{
    public class Parameter
    {
        public string field;
        public Op op;
        public string value;

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
            return "string";
        }
    }

    public class Generator
    {
        public static string GenerateToken(string secretKey, Parameter[] parameters)
        {
            return "string";
        }
    }
}
