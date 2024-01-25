﻿/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 18.01.2024
/// Description: Spaceship class, allowing movements with arrow keys or wasd.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameObjectsN;



namespace SpaceShips
{
    internal class SpaceShip : GameObjects
    {
        /// <summary>
        /// Width of the sprite
        /// </summary>
        const int WIDTH = 5;

        /// <summary>
        /// Height of the sprite
        /// </summary>
        const int HEIGHT = 1;

        private int _left = 20;
        private int _top = 20;


        

        public int Width
        {
            get { return WIDTH; }
        }
        public int Heigth
        {
            get { return HEIGHT; }
        }

        public int Left
        {
            get { return _left; }
            set { _left = value; }
        }

        public int Top
        {
            get { return _top; }
            set { _top = value; }
        }


        private const string SPRITE =  "/-^-\\";

        


        public void DelPosition() {
            Console.SetCursorPosition(_left, _top);
            for (int i = 0; i < WIDTH; i++)
            {
                Console.Write(" ");
            }
        }

        public void GoLeft() {
            DelPosition();
            _left--;
            Draw();
        }
        public void GoRight()
        {
            DelPosition();
            _left++;
            Draw();
        }
        public void Draw()
        {
            Console.SetCursorPosition(_left, _top);
            Console.Write(SPRITE);
        }
        public SpaceShip() { }


    }
}
