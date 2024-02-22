/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 08.02.2024
/// Description: Ennemies class, object with coodrinates, sprite

using System;
using Spicy_Invader;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnemiesNS
{
    internal class Enemy
    {
        private int _x = 0;
        private int _y = 0;
        public const int WIDTH = 3;
        public const int HEIGHT = 1;
        private const string SPRITE = "╓╫╖";
        private int _col = 0;
        private int _row = 0;

        public Enemy(int x, int y, int col, int row) {
            _x = x;
            _y = y;
            _col = col;
            _row = row;
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
        /// Get enemy horizontal coordinate
        /// </summary>
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Get enemy vertical coordinate
        /// </summary>
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// Get enemy sprite width
        /// </summary>
        public int Width {
            get { return WIDTH; }
        }

        public bool Move(bool right, bool edge)
        {
            
            

            // ..Move it up
            DelPosition();
            if (edge)
            {
                _y++;
            }
            else if (right)
            {
                _x++;
            }
            else
            {
                _x--;
            }
                
            Draw();

            if ((_x == Game.MARGIN_SIDE + 1 || _x == Game.MARGIN_SIDE + Game.WIDTH - WIDTH)&& !edge) {
                return true;
            }
            return false;

        }
        public void Draw()
        {
            Console.SetCursorPosition(_x, _y);
            Console.Write(SPRITE);
        }
        public void DelPosition()
        {
            Console.SetCursorPosition(_x, _y);

            // Draw as much empty character as the sprite's width
            for (int i = 0; i < WIDTH; i++)
            {
                Console.Write(" ");
            }
        }

    }
}
