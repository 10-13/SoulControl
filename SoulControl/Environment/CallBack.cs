using Jint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SoulControl.Environment
{
    public class CallBack
    {
        public bool Initialized { get; set; } = false;

        public string Type 
        { 
            get
            {
                if (FullType == null)
                    return "";
                var minmax = (int a, int b) => { return a < 0 ? b : a; };
                return FullType.Substring(0, minmax(FullType.IndexOf(' '), FullType.Length));
            }
            set
            {
                if (FullType == null)
                    FullType = value;
                if (FullType.IndexOf(' ') == -1)
                {
                    FullType = value;
                    return;
                }
                FullType = value + " " + FullType.Substring(FullType.IndexOf(' '));
            }
        }
        public string Value { get; set; } = null;
        public string Source { get; set; } = null;

        public string FullType { get; set; } = null;
        public string[] Flags 
        { 
            get
            {
                if(FullType == null)
                    return new string[] { };
                if (Type.Length == FullType.Length)
                    return new string[] { };
                return FullType.Substring(Type.Length).Split(' ');
            }
            set
            {
                if (FullType == null)
                    return;
                FullType = Value;
                foreach (string s in value)
                    FullType += ' ' + s;
            }
        }

        public CallBack() { }
        public CallBack(Jint.Native.JsValue obj)
        {
            if (!obj.IsObject())
            {
                if (obj.IsString())
                {
                    Type = "string";
                    Value = WebUtility.HtmlEncode(obj.AsString());
                    Initialized = true;
                }
                return;
            }

            if (obj.AsObject().TryGetValue("type", out Jint.Native.JsValue val1))
                FullType = val1.ToString();
            if (obj.AsObject().TryGetValue("value", out Jint.Native.JsValue val2))
                Value = val2.ToString();
            if (obj.AsObject().TryGetValue("source", out Jint.Native.JsValue val3))
                Source = val3.ToString();
            Initialized = true;
        }

        public override string ToString()
        {
            if(Type != null && Value != null && Source != null)
                return "{\n" + "\ttype: " + Type + ",\n\tvalue: " + Value + ",\n\tsource: " + Source + "\n}";
            else if(Type != null && Value != null)
                return "{\n" + "\ttype: " + Type + ",\n\tvalue: " + Value + "\n}";
            return "{" + " type: \"empty\" }";
        }
    }
}
