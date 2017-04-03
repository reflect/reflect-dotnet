using System;
using System.Text;

namespace reflect_dotnet
{
    class Parameter
    {
        public enum Op {
            Equals, NotEquals
        }

        public Op op;
        public string value;
        public string[] anyValue;

        private string opString() {
            switch (this.op) {
                case Op.Equals: return "=";
                case Op.NotEquals: return "!=";
                default: return "";
            }
        }

        public string field;

        public Parameter(string f, Op o, string v)
        {
            field = f;
            op = o;
            value = v;
        }

        public Parameter(string f, Op o, string[] v) {
            field = f;
            op = o;
            anyValue = v;
        }

        public override string ToString() {
            string valueString;

            if (anyValue != null)
            {
                Array.Sort(anyValue, StringComparer.Ordinal);
                StringBuilder builder = new StringBuilder();
                foreach (string s in this.anyValue) {
                    builder.Append(s);
                }
                valueString = builder.ToString();
            } else {
                valueString = this.value;
            }

            return $"{field}, {this.opString()}, {valueString}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(new Parameter("field-name", Parameter.Op.Equals, "Value"));
            Console.WriteLine(new Parameter("field-name", Parameter.Op.Equals, new string[]{"zebra", "foo", "bar"}));
        }
    }
}
