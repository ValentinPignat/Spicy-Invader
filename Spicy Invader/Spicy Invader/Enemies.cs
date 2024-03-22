/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 08.02.2024
/// Description: Ennemies class, object with coodrinates, sprite

using GameObjectsNS;
using Spicy_Invader;
using EnemyBlockNS;
using System.Diagnostics;

namespace EnemiesNS
{
    internal class Enemy : GameObject
    {

        public const int WIDTH = 3;
        public const int HEIGHT = 1;
        private const string SPRITE = "╓╫╖";
        private int _col = 0;
        private int _row = 0;
        private EnemyBlock _owner;
        private const Game.collisionStatus STATUS = Game.collisionStatus.Enemy;
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


        public override int Destroyed()
        {
            _owner.enemiesTab[_row, _col] = null;
            _owner.enemiesByCol[_col]--;
            DelPosition();
            return SCORE; 
        }
    }
}
