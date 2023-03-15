using Jint;
using Jint.Native;
using Jint.Runtime.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoulControl.Utils
{
    public class JSONObject
    {
        private Engine engine = new Engine();
        private Dictionary<string, string> values = new Dictionary<string, string>();
        private Dictionary<string, string> functions = new Dictionary<string, string>();

        public JSONObject() { }

        public JSONObject AddFunction(string Name,Func<JsValue, JsValue[], JsValue> function)
        {
            var obj = (dynamic)function;

            if (obj.Target.GetType() != typeof(ClrFunctionInstance))
                functions.Add(Name, obj.Target.FunctionDeclaration.ToString());
            
            return this;
        }
        public JSONObject AddProperty(string Name,JsValue value)
        {
            Jint.Native.Json.JsonSerializer f = new Jint.Native.Json.JsonSerializer(engine);
            values.Add(Name, f.Serialize(value).AsString());
            return this;
        }
        public JsValue CreateObject()
        {
            string str = this.ToString();
            return engine.Execute("var k = " + str).GetValue("k");
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("var temp = {");
            foreach (var kv in values)
            {
                sb.Append(kv.Key + ": "); sb.Append(kv.Value); sb.AppendLine(", ");
            }
            foreach(var kv in functions)
            {
                sb.Append(kv.Key + ": "); sb.Append(kv.Value); sb.AppendLine(", ");
            }
            sb.Remove(sb.Length - 4, 2);
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
