using Esprima.Ast;
using Jint;
using Jint.Native;
using Jint.Runtime.Interop;
using MoonSharp.Interpreter;
using SoulControl.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static IronPython.Modules._ast;

namespace SoulControl.Unit
{
    public class HandleabledObject : ScriptedObject, ICloneable
    {
        private Model _model;

        public string OnAction = "";
        public HandleabledObject(Model model) : base() { _model = model; }
        public HandleabledObject() : base() { }

        public Model GetParentModel() 
        { 
            return _model; 
        }

        protected override void LoadValues()
        {
            _eng.SetValue("BufebleUnit()", (JsValue val) => { return new BufebleUnit<JsValue>(val); });
            _eng.SetValue("CreateStringObject", () => { return new JSONObject(); });
            _eng.SetValue("StringifyFunction", (Func<JsValue, JsValue[], JsValue> function) =>
            {
                var obj = (dynamic)function;

                if (obj.Target.GetType() != typeof(ClrFunctionInstance))
                    return obj.Target.FunctionDeclaration.ToString();

                return "function() { [native code] }";
            });
            _eng.SetValue("AnalyzeType", Analysis.TypeAnalyser.GetTypeAnalysis);
            _eng.SetValue("Log", (string str) =>GetParentModel().Log(str));
            _eng.SetValue("GetModel", 
                (Func<Model>)(()=> { return GetParentModel(); }));
            _eng.SetValue("InvokeAction", (Func<string, object[], object>)this.InvokeAction);
            base.LoadValues();
        }

        public void LinkModel(in Model model)
        {
            _model = model;
        }
        private bool compiled = false;
        private string script = "";
        public string HandlerScript { get => script; set { script = value; compiled = false; } }
        public JsValue InvokeAction(string Name, params object[] args)
        {
            if (!compiled)
            {
                _eng.Execute(script);
                compiled = true;
            }

            try
            {
                return _eng.Invoke(Name, args);
            }
            catch (Exception ex)
            {
                GetParentModel().Log($"<ERROR>\n\t<Occured>{Name}</Occured>\n<ERROR>");
                GetParentModel().Log(ex.ToString());
            }
            return null;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
    public static class HandleabledObjectExtension
    {
        public static Engine CreateEngine(this HandleabledObject obj,bool SourceMode = true)
        {
            Options options = new Options();

            
            if(SourceMode)
            {
                options.AllowClr();
                options.Interop.AllowGetType = true;
                options.Interop.AllowSystemReflection = true;
                options.Interop.AllowWrite = true;
                options.Interop.AllowOperatorOverloading = true;
            }
            else
            {
                options.AllowClr(typeof(HandleabledObject).Assembly);
            }

            Engine res = new Engine(options);

            return res;
        }
        public static bool Invoke(this HandleabledObject obj,params object[] args)
        {
            try
            {
                Engine eng = obj.CreateEngine();

                string source = "function target(...args) {" + obj.OnAction + "}";

                eng.Execute(source);

                eng.Invoke("target", args);
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }
        public static bool Invoke(this HandleabledObject obj,string ActionType, params object[] args)
        {
            List<object> list = new List<object>();
            list.Add(ActionType);
            list.AddRange(args);
            return obj.Invoke(list);
        }
    }
}
