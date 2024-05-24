/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 08.02.2024
/// Description: EnemyBlock class
///     - Update all ennemies positions and decide next movement
///     - Manage ennemy fire 

using System;
using System.Linq;

namespace Spicy_Invader
{
    /// <summary>
    /// EnemyBlock class, manages enemies
    /// </summary>
    public class EnemyBlock : GameObject
    {
        /// <summary>
        /// Max active missiles in easy mode
        /// </summary>
        public const int MAX_ACTIVE_MISSILES = 1;

        /// <summary>
        /// Max active missile in hard mode
        /// </summary>
        public const int MAX_ACTIVE_MISSILES_HARD = 2;

        /// <summary>
        /// Max active missile depensding on game difficulty
        /// </summary>
        public int MaxActiveMissile { get; private set; }

        /// <summary>
        /// Horizontal space between each enemy
        /// </summary>
        private const int BETWEEN_X = 3;

        /// <summary>
        /// Vertical space betwenn each enemy
        /// </summary>
        private const int BETWEEN_Y = 1;

        /// <summary>
        /// Nb of enemy rows
        /// </summary>
        private const int ROW = 3;

        /// <summary>
        /// Nb of enemy collums
        /// </summary>
        private const int COL = 4;

        /// <summary>
        /// EnemyBlock X vector
        /// </summary>
        private int _vectorX = 1;

        /// <summary>
        /// EnemyBlock Y vector
        /// </summary>
        private int _vectorY = 0;

        /// <summary>
        /// EnemyBlock previous X vector
        /// </summary>
        private int _lastVectorX;

        /// <summary>
        /// EnemyBlock next X vector
        /// </summary>
        private int _nextVectorX = 1;

        /// <summary>
        /// Enemy next Y vector
        /// </summary>
        private int _nextVectorY = 0;

        /// <summary>
        /// EnemyBlock fire probability at each update
        /// </summary>
        private double randomShootProbability = 0.15;

        /// <summary>
        /// EnemyBlcock nb of lines went down
        /// </summary>
        private int _linesDown = 0;

        /// <summary>
        /// EnemyBlcock nb of lines went down getter
        /// </summary>
        public int LinesDown
        {
            get { return _linesDown; }
        }

        /// <summary>
        /// Random
        /// </summary>
        private Random rnd = new Random();

        /// <summary>
        /// Enemy tab, storing each enemy by position
        /// </summary>
        public Enemy[,] enemiesTab = new Enemy[COL,ROW];

        /// <summary>
        /// Nb of alive enemies by column
        /// </summary>
        public int[] enemiesByCol = new int[COL];

        /// <summary>
        /// SoundManager to call for audio
        /// </summary>
        public SoundManager SoundManager { get; private set; }

        /// <summary>
        /// EnemyBlock constructor
        /// </summary>
        /// <param name="easymode">True if easy mode on</param>
        /// <param name="bonusMissile">Additional max missile (default 0)</param>
        public EnemyBlock(bool easymode, SoundManager soundManager, int bonusMissile = 0) {

            SoundManager = soundManager;

            // Maximum nb of active missile for the ennemyBlock depending on difficulty
            if (easymode)
            {
                MaxActiveMissile = MAX_ACTIVE_MISSILES + bonusMissile;
            }
            else {
                MaxActiveMissile = MAX_ACTIVE_MISSILES_HARD + bonusMissile;
            }

            // Create enemies and put them in the 2D array
            for (int i = 0; i <COL; i++)
            {
                for (int j = 0; j < ROW; j++) {
                    Enemy enemy = new Enemy(x: Game.MARGIN_SIDE + 1 + i * Enemy.WIDTH + i * BETWEEN_X, y: Game.MARGIN_TOP_BOTTOM + 1 + j * Enemy.HEIGHT + j * BETWEEN_Y, row: i, col: j, owner: this);
                    enemiesTab[i, j] = enemy;
                }
            }
            
            // Foreach column, nb of enemies = nb of rows
            for (int i = 0; i < enemiesByCol.Length; i++) { 
                enemiesByCol[i] = ROW;
            }
        }

        
        /// <summary>
        /// Update every enemy in EnemyBlock, moving them and tryning to Fire. Calculate next move.
        /// </summary>
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
            
            // Random chance of making a random enemy fire
            if (rnd.NextDouble() < randomShootProbability) {
                RandomFire();
                SoundManager.FiringSound();
            }  
        }

        /// <summary>
        /// Method used to make a random enemy at the bottom of a column to shoot
        /// </summary>
        private void RandomFire() {
            int colFiring = 0;

            // Keep getting random column if empty
            do{
                colFiring = rnd.Next(0, COL);
            } while (enemiesByCol[colFiring] == 0);

            // Iterate through line of enemy and make lowest fire
            for (int i = ROW - 1; i >= 0; i--)
            {
                if (enemiesTab[colFiring, i] != null)
                {
                    Enemy firingEnnemy = enemiesTab[colFiring, i];
                    FireMissile(missilesList: missilesList, vectorY: +1, maxMissiles: MaxActiveMissile, x: firingEnnemy.X + firingEnnemy.Height, y: firingEnnemy.Y + firingEnnemy.Width / 2, status: Game.collisionStatus.Enemy, owner: this); ;
                    break;
                }
            }
        }

        /// <summary>
        /// Check if enemiesTab îs empty
        /// </summary>
        /// <returns>True if empty</returns>
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
