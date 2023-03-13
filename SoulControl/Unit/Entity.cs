using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoulControl.Unit
{
    public class Entity : HandleabledObject
    {
        public Point Position { get; set; }
        public bool Collide { get; set; }
        public long HP { get; set; }
        public long TotalHP { get; set; }
        public string Color { get; set; }
        public float AirReqired { get; set; }
        public float Air { get; set; }
        /// <summary>
        /// <code>"-1"="swimUnderWater"; "0"="swim"; "1"="walk"; "2"="fly"</code>
        /// </summary>
        public int MoveMode { get; set; }

        public Entity() : base() { }
        public Entity(Model model) : base(model) { }

        protected override void LoadValues()
        {
            _eng.SetValue("getPosition", new Func<int[]>(() => { return new int[2] { Position.X, Position.Y }; }));
            _eng.SetValue("setPosition", new Action<int,int>((int X,int Y) => { Position = new Point(X, Y); }));
            _eng.SetValue("getColide", new Func<bool>(() => Collide));
            _eng.SetValue("setColide", new Action<bool>((bool val) => Collide = val));
            base.LoadValues();
        }
    }
}
