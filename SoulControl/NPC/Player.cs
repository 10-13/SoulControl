using SoulControl.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoulControl.NPC
{
    public class Player : Entity
    {
        public int Moves { get; set; }

        public Player(Model model) : base(model) { }
        public Player() : base() { }
    }
}
