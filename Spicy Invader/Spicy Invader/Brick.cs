/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 14.03.2024
/// Description: Brick class derived from GameObject, used to create bunkers

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleToAttribute("SpicyTest")]
namespace Spicy_Invader
{
    /// <summary>
    /// Brick class derived from GameObject, used to create bunkers
    /// </summary>
    public class Brick : GameObject
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
        }
    }
}
