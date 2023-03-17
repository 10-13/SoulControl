using Esprima.Ast;
using Jint;
using Jint.Native;
using MoonSharp.Interpreter.Debugging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IronPython.Modules._ast;

namespace SoulControl.Unit
{
    public abstract class ScriptedObject
    {
        
        protected Engine _eng = null;
        

        public JsValue GetValue(string name) 
        {
            var type = this.GetType().GetProperty(name);
            if(type != null)
                return JsValue.FromObject(_eng, type.GetValue(this));
            
            if (_eng == null)
                return JsValue.Undefined;
            try
            {
                return _eng.GetValue(name);
            }
            catch (Exception ex) {}
            return JsValue.Undefined;
        }
        public void SetValue(string name, JsValue val)
        {
            var type = this.GetType().GetProperty(name);
            if (type != null)
            {
                type.SetValue(this, val);
                return;
            }

            if (_eng == null)
                return;
            try
            {
                _eng.SetValue(name, val);
            }
            catch (Exception ex) { }
        }

        public ScriptedObject() 
        {
            Options options = new Options();
            options.AllowClr();
            options.Interop.AllowGetType = true;
            options.Interop.AllowSystemReflection = true;
            options.Interop.AllowWrite = true;
            options.Interop.AllowOperatorOverloading = true;
            _eng = new Engine(options);
            LoadValues();
        }

        

        protected virtual void LoadValues()
        {
            _eng.SetValue("CreatePoint", (int X, int Y) => { return new Point(X, Y); });
           
            _eng.SetValue("GetAPIBase", 
                () => { 
                    return this; 
                });
        }
    }
}
