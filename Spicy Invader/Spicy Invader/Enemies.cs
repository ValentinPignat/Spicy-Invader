/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 08.02.2024
/// Description: Ennemies class, object with coodrinates, sprite

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spicy_Invader
{
    internal class Enemies
    {
        private int _x = 0;
        private int _y = 0;
        public const int WIDTH = 3;
        public const int HEIGHT = 1;
        private const string SPRITE = "╓╫╖";

        public Enemies(int x, int y) {
            _x = x;
            _y = y;

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
