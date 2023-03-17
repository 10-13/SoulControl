using Esprima.Ast;
using Jint.Native;
using SoulControl.Unit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoulControl.Map
{
    public class Map : Unit.HandleabledObject
    {
        private Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

        public List<KeyValuePair<Point,Tile>> TilesCollection
        {
            get
            {
                return tiles.ToList();
            }
            set
            {
                foreach (var ppt in value)
                    tiles.Add(ppt.Key, ppt.Value);
            }
        }
        public Tile this[int x,int y]
        {
            get
            {
                if (!tiles.ContainsKey(new Point(x, y)))
                    tiles.Add(new Point(x, y), new Tile(GetParentModel()));
                return tiles[new Point(x, y)];
            }
            set
            {
                if (!tiles.ContainsKey(new Point(x, y)))
                    tiles.Add(new Point(x, y), value);
                else
                    tiles[new Point(x, y)] = value;
                tiles[new Point(x, y)].LinkModel(this.GetParentModel());
            }
        }

        public Map(Model model) : base(model) { }
        public Map() : base() { }
        public Tile GetTile(int X,int Y)
        {
            return this[X, Y];
        }
        public void SetTile(int X, int Y,Tile tile)
        {
            this[X, Y] = tile;
        }
        public Point GetTilePosition(Tile tile)
        {
            return tiles.First((KeyValuePair<Point, Tile> p) => { return p.Value == tile; }).Key;
        }
        public void Validate()
        {
            foreach(Tile t in tiles.Values)
                t.LinkModel(GetParentModel());
        }
    }
}
