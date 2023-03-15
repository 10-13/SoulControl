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
        public BufebleUnit<int> Moves { get; set; } = new BufebleUnit<int>(15);

        public Player(Model model) : base(model) { }
        public Player() : base() { }
    }
}
