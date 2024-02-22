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
                for (int i = ROW; i >= 0; i--) {
                    if (enemiesTab[i,colFiring] != null) {
                        enemiesTab[i, colFiring].DelPosition();
                        enemiesTab[i, colFiring] = null;
                        break;
                    }
                }

            }

        
        }
        
                                                                     
                                                                     
    }
}
