using Esprima.Ast;
using Jint;
using Jint.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoulControl.Environment
{
    public static class CallBackExtension
    {
        public static JsValue CreateCallBack(this JsValue jsValue,in IList<CallBack> collection)
        {
            if (jsValue == null || jsValue == JsValue.Undefined)
                return jsValue;

            if (jsValue.IsArray())
            {
                foreach (JsValue val in jsValue.AsArray())
                    val.CreateCallBack(collection);
                return jsValue;
            }

            CallBack cb = new CallBack(jsValue);
            collection.Add(cb);

            return jsValue;
        }
    }
}
