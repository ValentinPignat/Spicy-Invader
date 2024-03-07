/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 18.01.2024
/// Description: Missile class: 
///     - GoUp() deletes the missile at its old position, change his coordinates, redraw it
///         |-> Returns a boolean true if the missile goes out of bound


using System;

using GameObjectsNS;
using Spicy_Invader;


namespace MissileNS 
{
    /// <summary>
    /// Missile class 
    /// </summary>
    internal class Missile :GameObject
    {
        /// <summary>
        /// Width of the sprite
        /// </summary>
        const int WIDTH = 1;

        /// <summary>
        /// Height of the sprite
        /// </summary>-
        const int HEIGHT = 1;

        /// <summary>
        /// Move vector
        /// </summary>
        private int _vectorY = 0; 

        /// <summary>
        /// Missile sprite
        /// </summary>
        private const string SPRITE =  "|";

        private readonly GameObject OWNER;

        public int VectorY
        {
            get { return _vectorY; }
            set { _vectorY = value; }
        }

        public Missile(int x, int y, int vectorY, Game.collisionStatus collisionStatus, GameObject owner)
        {
            _collisionStatus = collisionStatus;
            _vectorY = vectorY;
            _x = x;
            _y = y;
            _sprite = SPRITE;
            _width = WIDTH;
            _height = HEIGHT;
            _owner = OWNER;
        }

        /// <summary>
        /// Missile constructor
        /// </summary>
        /// <param name="x">Horizontal position</param>
        /// <param name="y">Vertical position</param>
        public Missile(int x, int y) {
            _y = y;
            _x = x;  
        }

        private void OnHit()
        {
            _hp--;
            if (_hp == 0)
            {
                _owner.missilesList.Remove(this);
            }

        }

    }
}
