using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SpaceShips
{
    internal class SpaceShip
    {
        const int WIDTH = 4;
        const int HEIGHT = 1;

        private int _left = 20;
        private int _up = 20;

        public string sprite = "/--\\";

        public void DelPosition() {
            Console.SetCursorPosition(_left+WIDTH, _up);
            for (int i = 0; i < WIDTH; i++)
            {
                Console.Write("\b");
            }
        }

        public void Left() {
            _left--;
        }
        public void Draw() {
            Console.SetCursorPosition(_left, _up);
            Console.Write(sprite);
        }

        public SpaceShip() { }


    }
}
