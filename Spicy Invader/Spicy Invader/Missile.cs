/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 18.01.2024
/// Description: Spaceship class, allowing movements with arrow keys or wasd.

using System;
using SpaceShips;
using GameObjectsN;
using Spicy_Invader;
using System.Threading;
using System.Diagnostics;


namespace MissileN
{
    internal class Missile : GameObjects
    {
        /// <summary>
        /// Width of the sprite
        /// </summary>
        const int WIDTH = 1;

        /// <summary>
        /// Height of the sprite
        /// </summary>-
        const int HEIGHT = 1;

        private int _left = 20;
        private int _top = 20;


        private const string SPRITE =  "|";

        


        public void DelPosition() {
            Console.SetCursorPosition(_left, _top);
            for (int i = 0; i < WIDTH; i++)
            {
                Console.Write(" ");
            }
        }

         void GoUp() {
            
            while (_top > 0) { 
            DelPosition();
            _top--;
            Debug.WriteLine("x:" + _left+" y"+_top);
            Draw();
            Thread.Sleep(50);
            }
            DelPosition();
        }
        
        public void Draw()
        {
            Console.SetCursorPosition(_left, _top);
            Console.Write(SPRITE);
        }
        public Missile(int left, int top) {
            _top = top;
            _left = left;
            Thread t1 = new Thread(GoUp);
            t1.Start();

        }


    }
}
