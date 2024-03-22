/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 14.03.2024
/// Description: Brick class 

using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GameObjectsNS;
using Spicy_Invader;

namespace BricksNS
{
    internal class Brick : GameObject
    {
        private const string SPRITE = "░";
        private const int HP = 1;
        private const int WIDTH = 1;
        private const Game.collisionStatus STATUS = Game.collisionStatus.Neutral;
        public Brick(int x, int y) {
            _x = x;
            _y = y;
            _sprite = SPRITE;
            _collisionStatus = STATUS;
            _hp = HP;
            Draw();
        }
        public void Update() { 
            
            
        }
    }
}
