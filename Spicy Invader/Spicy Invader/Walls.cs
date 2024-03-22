/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 14.03.2024
/// Description: Wall class 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameObjectsNS;
using BricksNS;
using Spicy_Invader;

namespace WallsNS
{
    internal class Walls : GameObject
    {
        private const string SPRITE = "▓";
        private const Game.collisionStatus STATUS = Game.collisionStatus.Neutral;

        private List<Brick> bricksList = new List<Brick>();

        /*public Walls(int x, int y, int width, int height) {
            _x = x;
            _y = y;
            for (int i = 0; i < width; i++) {
                Brick brick = new Brick(x: _x + i, y: _y, sprite: SPRITE);
                bricksList.Add(brick);
            }
            _width = width;
        }*/
    }
}
