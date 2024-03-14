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
        private int MAX_ACTIVE_MISSILES = 4;
        private const int BETWEEN_X = 3;
        private const int BETWEEN_Y = 1;
        private const int ROW = 3;
        private const int COL = 4;
        private int _vectorX = 1;
        private int _vectorY = 0;
        public bool _change = false;
        private double randomShootProbability = 0.35;
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
            int colFiring = 0;

            foreach (Enemy enemy in enemiesTab)
            {
                if (enemy != null) {
                    if (_vectorY == 1)
                    {
                        if (!enemy.Move(vectorX: 0, vectorY: _vectorY))
                        {
                            Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                        };
                    }
                    else {
                        enemy.Move(vectorX: _vectorX, vectorY: _vectorY);
                        if (!enemy.CastOutOfBounds(vectorXCast: _vectorX, vectorYCast: _vectorY))
                        {
                            _change = true;
                        }
                    }
                    
                }
                

            }
            if (_change && _vectorY == 0)
            {
                _vectorY = 1;
            }
            else if (_change && _vectorY == 1) {
                _vectorY = 0;
                _vectorX *= -1;
                _change = false;
            }

            if (rnd.NextDouble() < randomShootProbability) {
                do{
                    colFiring = rnd.Next(0, COL);
                } while (enemiesByCol[colFiring] == 0);
                for (int i = ROW -1; i >= 0; i--) {
                    if (enemiesTab[colFiring,i] != null) {
                        Enemy firingEnnemy = enemiesTab[colFiring, i];
                        FireMissile(missilesList: missilesList, vectorY: +1, maxMissiles: MAX_ACTIVE_MISSILES, x: firingEnnemy.X + firingEnnemy.Height, y: firingEnnemy.Y + firingEnnemy.Width/2, status: Game.collisionStatus.Enemy, owner: this);
                        break;
                    }
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
