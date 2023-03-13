using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoulControl.NPC
{
    public class NPC : Unit.Entity
    {
        public int RelationShip { get; set; } = 1;
        public int ReqiredRelationShip { get; set; } = 100;

        public NPC(Model model) : base(model) { }
        public NPC() : base() { }

        protected override void LoadValues()
        {
            _eng.SetValue("getRelationShip", new Func<int>(() => RelationShip));
            _eng.SetValue("setRelationShip", new Action<int>((int arg) => RelationShip = arg));
            _eng.SetValue("getReqiredRelationShip", new Func<int>(() => ReqiredRelationShip));
            _eng.SetValue("setReqiredRelationShip", new Action<int>((int arg) => ReqiredRelationShip = arg));
            base.LoadValues();
        }
    }
}
