using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SoulControl.Map
{
    public class Tile : Unit.HandleabledObject
    {
        public string Color { get; set; } = "#000";
        public string TileType { get; set; } = "UNDEFINED";

        public Tile(Model model) : base(model) { }
        public Tile() { }

        protected override void LoadValues()
        {
            _eng.SetValue("getColor", new Func<string>(() => Color));
            _eng.SetValue("setColor", new Action<string>((string arg) => Color = arg));
            _eng.SetValue("getTileType", new Func<string>(() => TileType));
            _eng.SetValue("setTileType", new Action<string>((string arg) => TileType = arg));
            base.LoadValues();
        }
    }
}
