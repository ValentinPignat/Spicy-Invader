/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 08.02.2024
/// Description: EnnemyBlock class
///     - Update all ennemies positions
///     - Manage ennemy fire 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnemiesNS;
using Spicy_Invader;
using MissileNS;
using GameObjectsNS;




namespace EnemyBlockNS
{
    internal class EnemyBlock : GameObject
    {
        private int MAX_ACTIVE_MISSILES = 2;
        private const int BETWEEN_X = 3;
        private const int BETWEEN_Y = 1;
        private const int ROW = 3;
        private const int COL = 4;
        private const int Y_LIMIT = 18;
        private int _vectorX = 1;
        private int _vectorY = 0;
        private int _lastVectorX;
        private int _nextVectorX = 1;
        private int _nextVectorY = 0;
        public bool _change = false;
        private double randomShootProbability = 0.35;
        private int _linesDown = 0;

        public int LinesDown
        {
            get { return _linesDown; }
            set { _linesDown = value; }
        }

        Random rnd = new Random();
        public Enemy[,] enemiesTab = new Enemy[COL,ROW];
        public int[] enemiesByCol = new int[COL];


        public EnemyBlock() {
            for (int i = 0; i <COL; i++)
            {
                for (int j = 0; j < ROW; j++) {
                    Enemy enemy = new Enemy(x: Game.MARGIN_SIDE + 1 + i * Enemy.WIDTH + i * BETWEEN_X, y: Game.MARGIN_TOP_BOTTOM + 1 + j * Enemy.HEIGHT + j * BETWEEN_Y, row: i, col: j, owner: this);
                    enemiesTab[i, j] = enemy;
                }
            }

            for (int i = 0; i < enemiesByCol.Length; i++) { 
                enemiesByCol[i] = ROW;
            }
        }
        
            
        public void Update() {

            // Update direction before moving the enemies individually
            _vectorX = _nextVectorX;
            _vectorY = _nextVectorY;

            // Keep track of nb of lines gone down
            _linesDown += _vectorY;

            // For each enemy ... 
            foreach (Enemy enemy in enemiesTab)
            {
                if (enemy != null)
                {
                    // ... they move in their vectors direction
                    enemy.Move(vectorX: _vectorX, vectorY: _vectorY);

                    // If enemy moved horizontally and unable to move in that same direction next update 
                    if (!enemy.CastOutOfBounds(vectorXCast: _vectorX, vectorYCast: _vectorY)&& _vectorY == 0)
                    {
                        // Change the vectors to go down
                        _nextVectorY = 1;
                        _nextVectorX = 0;

                        // Keep track of last horizonzal direction
                        _lastVectorX = _vectorX;
                    }

                    // If enemy moved vertically, change vectors to move to the oposite side of _lastVectorX
                    if (_vectorY > 0) { 
                        _nextVectorX = -1*_lastVectorX;
                        _nextVectorY = 0;
                    }
                }
            }

            // If next movement is down, foreach enemy ...
            if (_nextVectorY > 0) {
                foreach (Enemy enemy in enemiesTab)
                {
                    if (enemy != null)
                    {
                        //  ... in the lowest row
                        if (enemy.Row == enemiesByCol.ToList().Max())
                        {
                            // If can't do next move
                            if (!enemy.CastOutOfBounds(vectorXCast: 0, vectorYCast: _nextVectorY))
                            {
                                // Change vectors to move to the oposite side of _lastVectorX
                                _nextVectorY = 0;
                                _nextVectorX = -1 * _lastVectorX;
                            }
                        }
                    }
                }
            }
            
            // Random chance of making a random enemy fire
            if (rnd.NextDouble() < randomShootProbability) {
                RandomFire();              
            }  
        }

        /// <summary>
        /// Method used to make a random enemy at the bottom of a column to shoot
        /// </summary>
        private void RandomFire() {
            int colFiring = 0;
            do
            {
                colFiring = rnd.Next(0, COL);
            } while (enemiesByCol[colFiring] == 0);
            for (int i = ROW - 1; i >= 0; i--)
            {
                if (enemiesTab[colFiring, i] != null)
                {
                    Enemy firingEnnemy = enemiesTab[colFiring, i];
                    FireMissile(missilesList: missilesList, vectorY: +1, maxMissiles: MAX_ACTIVE_MISSILES, x: firingEnnemy.X + firingEnnemy.Height, y: firingEnnemy.Y + firingEnnemy.Width / 2, status: Game.collisionStatus.Enemy, owner: this);
                    break;
                }
            }
        }


        public bool IsEmpty() {
            bool isEmpty = true;
            foreach (Enemy enemy in enemiesTab) {
                if (enemy != null)
                {
                    isEmpty = false;
                }  
            }
            return isEmpty ;
        }


    }
}
