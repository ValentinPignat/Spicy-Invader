using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceShips;

namespace Spicy_Invader
{
    internal class Game
    {
        static void Main(string[] args)
        {
            SpaceShip player = new SpaceShip();
            while (true) {
                player.Draw();
                player.DelPosition();
                player.Left();
            }
            


        }
    }
}
