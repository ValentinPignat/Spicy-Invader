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
    internal class EnemyBlock 
    {
        
        private const int BETWEEN_X = 3;
        private const int BETWEEN_Y = 1;
        private const int ROW = 3;
        private const int COL = 4;
        private bool _right = true;
        public bool _edge = false;
        public bool _change = false;
        private double randomShootProbability = 0.35;
        Random rnd = new Random();
        public Enemy[,] enemiesTab = new Enemy[COL,ROW];
        public int[] enemiesByCol = new int[COL];
        /// <summary>
        /// List of players active missile
        /// </summary>
        public List<Missile> missilesList = new List<Missile>();


        public EnemyBlock() {
            for (int i = 0; i <COL; i++)
            {
                for (int j = 0; j < ROW; j++) {
                    Enemy enemy = new Enemy(x: Game.MARGIN_SIDE + 1 + i * Enemy.WIDTH + i * BETWEEN_X, y: Game.MARGIN_TOP_BOTTOM + 1 + j * Enemy.HEIGHT + j * BETWEEN_Y, row: i, col: j);
                    enemiesTab[i, j] = enemy;
                }
            }

            for (int i = 0; i < enemiesByCol.Length; i++) { 
                enemiesByCol[i] = ROW;
            }
        }
        /// <summary>
        /// Create a missile object at the player coordinate
        /// </summary>
        public void FireMissile(int x, int y, int vectorY)
        {

            // if the player has no active missile
            if (missilesList.Count < 4)
            {

                // Create a new missile 
                Missile missile = new Missile(x: x , y: y, vectorY: vectorY, collisionStatus: Game.collisionStatus.Enemy);

                // Add the missile to the list to allow updates
                missilesList.Add(missile);
            }
        }
        public void Update() {
            int colFiring = 0;

            foreach (Enemy enemy in enemiesTab)
            {
                if (enemy != null) {
                    if (enemy.Move(_right, _edge))
                    {
                        _change = true;
                    }
                }
                

            }
            if (_change)
            {
                _edge = true;
                _right = !_right;
                _change = false;
            }
            else if (_edge) {
                _edge = false;
            }

            if (rnd.NextDouble() < randomShootProbability) {
                do{
                    colFiring = rnd.Next(0, COL);
                } while (enemiesByCol[colFiring] == 0);
                for (int i = ROW -1; i >= 0; i--) {
                    if (enemiesTab[colFiring,i] != null) {
                        Enemy firingEnnemy = enemiesTab[colFiring, i];
                        FireMissile(x: firingEnnemy.X + (firingEnnemy.Width/2), y: firingEnnemy.Y + 1, vectorY: 1);
                        break;
                    }
                }

            }

        
        }
        /// <summary>
        /// Update missile position
        /// </summary>
        public void MissileUpdate()
        {

            // Track if current missile is out of bounds
            bool outOfBounds;

            // Goes through the missile list and update them 
            for (int i = 0; i < missilesList.Count; i++)
            {
                // Missile goes up 
                outOfBounds = missilesList[i].Move();

                // If missile is out of bounds.. 
                if (outOfBounds)
                {

                    // ..removes it from the list 
                    missilesList.Remove(missilesList[i]);

                    // Decrement as the list is displaced by the removal
                    i--;
                }
            }


        }



    }
}
