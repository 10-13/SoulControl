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
        public BufebleUnit<long> HP { get; set; } = new BufebleUnit<long>(100);
        public string Color { get; set; }
        public BufebleUnit<float> Air { get; set; } = new BufebleUnit<float>(3);
        /// <summary>
        /// <code>"-1"="swimUnderWater"; "0"="swim"; "1"="walk"; "2"="fly"</code>
        /// </summary>
        public BufebleUnit<int> MoveMode { get; set; } = new BufebleUnit<int>(0x0110);

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
