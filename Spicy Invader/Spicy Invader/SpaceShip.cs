/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 18.01.2024
/// Description: Spaceship class derived from GameObject
///     - Shoot() method fires a missile upwards from player position

namespace Spicy_Invader
{
    /// <summary>
    /// Spaceship class derived from GameObject
    /// </summary>
    public class SpaceShip : GameObject
    {
        /// <summary>
        /// Maximum hp
        /// </summary>
        public const int MAX_HP = 5;

        /// <summary>
        /// (HARD MODE) Maximum hp
        /// </summary>
        public const int MAX_HP_HARD = 3;

        /// <summary>
        /// Maximum active missiles 
        /// </summary>
        public const int MAX_ACTIVE_MISSILES = 1;

        /// <summary>
        /// Width of the sprite
        /// </summary>
        const int WIDTH = 5;

        /// <summary>
        /// Height of the sprite
        /// </summary>
        const int HEIGHT = 1;

        /// <summary>
        /// Spaceship sprite
        /// </summary>
        private const string SPRITE =  "/-^-\\";

        /// <summary>
        /// Spaceship collision status
        /// </summary>
        private const Game.collisionStatus STATUS = Game.collisionStatus.Friendly;

        /// <summary>
        /// Constructor SpaceShip, position depending on map size
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public SpaceShip(int x, int y, bool easymode) {

            // Hp depending on dificulty
            if (easymode)
            {
                _hp= MAX_HP;
            }
            else {
                _hp = MAX_HP_HARD;
            }

            _x = x;
            _y = y;
            _sprite = SPRITE;
            _height = HEIGHT;
            _width = WIDTH;
            _collisionStatus = STATUS;
        }

        /// <summary>
        /// Shoot upwards from player position
        /// </summary>
        public void Shoot() {
            FireMissile(missilesList: _missilesList, vectorY: -1, maxMissiles: MAX_ACTIVE_MISSILES, x: _x + _width / 2, y: _y - _height, status: Game.collisionStatus.Friendly, owner: this);
        }
    }
}
