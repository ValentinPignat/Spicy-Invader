/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 14.03.2024
/// Description: Brick class 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameObjectsNS;

namespace BricksNS
{
    internal class Bricks : GameObject
    {
        private const int HP = 2;
        public Bricks(int x, int y, string sprite) {
            _x = x;
            _y = y;
            _sprite = sprite;
            Draw();
        }
        public void Update() { 
            
            
        }
    }
}
