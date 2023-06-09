﻿using Esprima.Ast;
using Jint;
using Jint.Native;
using SoulControl.Environment;
using SoulControl.NPC;
using SoulControl.ModelOptions;
using SoulControl.Unit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Scripting.Actions;

namespace SoulControl
{
    public class Model : Unit.HandleabledObject
    {
        private Engine eng = new Engine();
        private Queue<KeyValuePair<string, object[]>> requests = new Queue<KeyValuePair<string, object[]>>();
        private List<CallBack> modelLogs = new List<CallBack>();

        public Map.Map ActualField { get; set; }
        public List<Entity> entities { get; set; } = new List<Entity>();
        public Player Player { get; set; }
        public ModelOptions.Options Options { get; set; } = new ModelOptions.Options();

        public Model() : base(null) 
        {
            Player = new Player(this);
            eng.SetValue("api_execute", new Action<string, object[]>((string name, object[] args) => requests.Enqueue(new KeyValuePair<string, object[]>(name, args))));
            eng.Execute("var execute = (name,...params) => { api_execute(name,params); }\n");
            eng.SetValue("Options", Options);
        }

        protected override void LoadValues()
        {
            _eng.SetValue("FindEntity", new Func<int, int, Entity>((int X, int Y) => { foreach (Entity en in entities) if (en.Position.X == X && en.Position.Y == Y) return en; return null; }));
            _eng.SetValue("GetTile", new Func<int, int, Map.Tile>((int X, int Y) => ActualField[X, Y]));
            _eng.SetValue("LoadMap", new Action<string>((string fileName) => {
                XmlSerializer s = new XmlSerializer(typeof(Map.Map),new Type[] { typeof(Map.Tile) });
                XmlReader xr = XmlReader.Create(new FileStream(fileName, FileMode.Open));
                if (!s.CanDeserialize(xr))
                    throw new Exception($"[ERROR:\\MODEL] : Unable to deserialize map from \"{fileName}\"");
                this.ActualField = s.Deserialize(xr) as Map.Map;
                this.ActualField.LinkModel(this);
                }));
            base.LoadValues();
        }

        public void Request(string requestString)
        {
            if (Options.Request.ClearQueueOnRequest)
                requests.Clear();
            eng.Execute(requestString);
        }

        public IEnumerable<CallBack> Tick()
        {
            List<CallBack> callBacks = new List<CallBack>();
            Player.InvokeAction("onApply");
            for (int i = 0;i < Player.Moves.Value && requests.Count > 0;i++)
            {
                var call = requests.Dequeue();
                if (!Options.Interop.AllowEval && call.Key == "eval")
                    continue;
                Player.InvokeAction("onReset").CreateCallBack(callBacks);
                Player.InvokeAction("onApply").CreateCallBack(callBacks);
                Player.InvokeAction(call.Key, call.Value).CreateCallBack(callBacks);
                Player.InvokeAction("onTick").CreateCallBack(callBacks);
                ActualField[Player.Position.X, Player.Position.Y].InvokeAction("onStay", Player).CreateCallBack(callBacks);
            }
            Player.InvokeAction("onReset");
            if (Options.Request.ClearQueueOnTick)
                requests.Clear();

            return from h in callBacks where (Options.CallBack.FilterPrivate ? !h.Flags.Contains(Options.CallBack.PrivateFlag) : true) select h;
        }
        public IEnumerable<CallBack> EntityTick()
        {
            List<CallBack> callBacks = new List<CallBack>();
            foreach (Entity ent in entities)
            {
                ent.InvokeAction("onReset").CreateCallBack(callBacks);
                ent.InvokeAction("onApply").CreateCallBack(callBacks);
                ent.InvokeAction("onTick").CreateCallBack(callBacks);
                ActualField[ent.Position.X, ent.Position.Y].InvokeAction("onStay", ent).CreateCallBack(callBacks);
            }

            return from h in callBacks where (Options.CallBack.FilterPrivate ? !h.Flags.Contains(Options.CallBack.PrivateFlag) : true) select h; ;
        }
        public void Log(JsValue value)
        {
            value.CreateCallBack(modelLogs);
        }
        public IEnumerable<CallBack> GetCallBacks(bool clear = true)
        {
            var res = (from h in modelLogs where (Options.CallBack.FilterPrivate ? !h.Flags.Contains(Options.CallBack.PrivateFlag) : true) select h).ToList();
            if (clear)
                modelLogs.Clear();
            return res;
        }
        public void Validate()
        {
            Player.LinkModel(this);
            ActualField.LinkModel(this);
            ActualField.Validate();
            foreach (Entity ent in entities)
                ent.LinkModel(this);
        }
    }
}
