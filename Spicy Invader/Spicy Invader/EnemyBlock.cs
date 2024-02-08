using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Spicy_Invader
{
    internal class EnemyBlock
    {
        public List<Enemies> enemiesList = new List<Enemies>();
        private const int BETWEEN_X = 3;
        private const int BETWEEB_Y = 1;
        private const int ROW = 4;
        private const int COL = 5;
        private bool _border = false;
        private bool _right = true;
        public bool _edge = false;
        public bool _change = false;

        public bool Change { 
            get { return _change; }

        }

        public EnemyBlock() {
            for (int i = 0; i <COL; i++)
            {
                for (int j = 0; j < ROW; j++) { 
                    Enemies enemy = new Enemies(x : Game.MARGIN_SIDE +1 + j*Enemies.WIDTH +j*BETWEEN_X, y : Game.MARGIN_TOP_BOTTOM + 1 + i*Enemies.HEIGHT + i*BETWEEB_Y);
                    enemiesList.Add(enemy);

                }
            }
        }

        public void Update() {
            foreach (Enemies enemy in enemiesList)
            {
                if (enemy.Move(_right, _edge)) {
                    _change = true;
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

        
        }
        
                                                                     
                                                                     
    }
}
