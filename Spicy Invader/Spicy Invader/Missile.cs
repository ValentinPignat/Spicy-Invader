/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 18.01.2024
/// Description: Missile class inheriting GameObject 
///         - Vector added for repeated moves

namespace Spicy_Invader
{
    /// <summary>
    /// Missile class inheriting GameObject 
    /// </summary>
    public class Missile :GameObject
    {
        /// <summary>
        /// Missile HP
        /// </summary>
        const int HP = 1;

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
        private const string SPRITE =  "|"
;
        /// <summary>
        /// Missile shooter
        /// </summary>
        public GameObject Owner { get; private set; }

        /// <summary>
        /// Missile Y vector getter
        /// </summary>
        public int VectorY
        {
            get { return _vectorY; }
            set { _vectorY = value; }
        }

        /// <summary>
        /// Missile Constructor 
        /// </summary>
        /// <param name="x">Horizontal position</param>
        /// <param name="y">Vertical position</param>
        /// <param name="vectorY">Vertical vector</param>
        /// <param name="collisionStatus">Enemy, Friendly, Neutral</param>
        /// <param name="owner">Object that shooted the missile</param>
        public Missile(int x, int y, int vectorY, Game.collisionStatus collisionStatus, GameObject owner)
        {
            _collisionStatus = collisionStatus;
            _vectorY = vectorY;
            _x = x;
            _y = y;
            _sprite = SPRITE;
            _width = WIDTH;
            _height = HEIGHT;
            Owner = owner;
            _hp = HP;
        }
    }
}
