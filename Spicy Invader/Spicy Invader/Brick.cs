/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 14.03.2024
/// Description: Brick class 

using GameObjectsNS;
using Spicy_Invader;

namespace BricksNS
{
    internal class Brick : GameObject
    {
        /// <summary>
        /// Brick sprite
        /// </summary>
        private const string SPRITE = "░";

        /// <summary>
        /// Brick hp
        /// </summary>
        private const int HP = 1;

        /// <summary>
        /// Brick widht 
        /// </summary>
        private const int WIDTH = 1;

        /// <summary>
        /// Brick colision status
        /// </summary>
        private const Game.collisionStatus STATUS = Game.collisionStatus.Neutral;

        /// <summary>
        /// Brick constructor
        /// </summary>
        /// <param name="x">X axis position</param>
        /// <param name="y">Y axis position</param>
        public Brick(int x, int y) {
            _x = x;
            _y = y;
            _sprite = SPRITE;
            _collisionStatus = STATUS;
            _hp = HP;
            Draw();
        }
    }
}
