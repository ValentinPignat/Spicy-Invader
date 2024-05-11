/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 08.02.2024
/// Description: Ennemies class derived from GameObject

using EnemyBlockNS;
using GameObjectsNS;
using Spicy_Invader;

namespace EnemiesNS
{
    internal class Enemy : GameObject
    {
        /// <summary>
        /// Enemy max hp
        /// </summary>
        private const int MAX_HP = 1;

        /// <summary>
        /// Max y position changed from GameObject so that ennemies can't go below walls
        /// </summary>
        private const int MAX_Y = Game.MARGIN_TOP_BOTTOM + Game.HEIGHT - Game.PLAYER_TO_WALL - Game.WALL_HEIGHT - 1;

        /// <summary>
        /// Enemy width
        /// </summary>
        public const int WIDTH = 3;

        /// <summary>
        /// Enemy height
        /// </summary>
        public const int HEIGHT = 1;

        /// <summary>
        /// Enemy sprite
        /// </summary>
        private const string SPRITE = "╓╫╖";

        /// <summary>
        /// Enemy column in EnemyBlock
        /// </summary>
        private int _col = 0;

        /// <summary>
        /// Enemy row in EnemyBlock
        /// </summary>
        private int _row = 0;

        /// <summary>
        /// Enemy block controlling this enemy
        /// </summary>
        private EnemyBlock _owner;

        /// <summary>
        /// Collision status = Enemy
        /// </summary>
        private const Game.collisionStatus STATUS = Game.collisionStatus.Enemy;

        /// <summary>
        /// Score returned on death
        /// </summary>
        private const int SCORE = 40;

        public Enemy(int x, int y, int col, int row, EnemyBlock owner) {
            _x = x;
            _y = y;
            _col = col;
            _row = row;
            _sprite = SPRITE;
            _width = WIDTH;
            _height = HEIGHT;
            _collisionStatus = STATUS;
            _owner = owner;
            _hp = MAX_HP;
            _maxY = MAX_Y;
        }

        /// <summary>
        /// Get enemy horizontal coordinate
        /// </summary>
        public int Col
        {
            get { return _col; }
            set { _col = value; }
        }

        /// <summary>
        /// Get enemy vertical coordinate
        /// </summary>
        public int Row
        {
            get { return _row; }
            set { _row = value; }
        }

        /// <summary>
        /// Return a score on death
        /// </summary>
        /// <returns>Score to award on destruction</returns>
        public override int Destroyed()
        {
            _owner.enemiesTab[_row, _col] = null;
            _owner.enemiesByCol[_col]--;
            DelPosition();
            return SCORE; 
        }
    }
}
