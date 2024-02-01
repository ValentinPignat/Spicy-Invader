/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 18.01.2024
/// Description: Missile class: 
///     - GoUp() deletes the missile at its old position, change his coordinates, redraw it
///         |-> Returns a boolean true if the missile goes out of bound


using System;
using SpaceshipNS;
using GameObjectsNS;
using Spicy_Invader;
using System.Threading;
using System.Diagnostics;

namespace MissileNS
{
    /// <summary>
    /// Missile class 
    /// </summary>
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

        /// <summary>
        /// Horizontal coordinates
        /// </summary>
        private int _left = 20;

        /// <summary>
        /// Vertical coordinates
        /// </summary>
        private int _top = 20;

        /// <summary>
        /// Missile sprite
        /// </summary>
        private const string SPRITE =  "|";

        /// <summary>
        /// Deletes missiles old coordinates
        /// </summary>
        public void DelPosition() {
            Console.SetCursorPosition(_left, _top);

            // Draw as much empty character as the sprite's width
            for (int i = 0; i < WIDTH; i++)
            {
                Console.Write(" ");
            }
        }

        /// <summary>
        /// Delete sprite at old position, change coordinates and redraw sprite
        /// </summary>
        /// <returns>True boolean if missile goes out of bounds</returns>
        public bool GoUp() {

            // Missile isn't out of bounds
            bool outOfBounds = false; 

            // If missile isn't at the screen top border..
            if (_top > 0)
            {

                // ..Move it up
                DelPosition();
                _top--;
                Draw();    
            }

            // else change the boolen to true
            else { 
                DelPosition(); 
                outOfBounds = true;
            }

            // Return (true) out of bounds / (false) in bounds
            return outOfBounds;
        }
        
        /// <summary>
        /// Write missile sprite at new coordinates
        /// </summary>
        public void Draw()
        {
            Console.SetCursorPosition(_left, _top);
            Console.Write(SPRITE);
        }

        /// <summary>
        /// Missile constructor
        /// </summary>
        /// <param name="left">Horizontal position</param>
        /// <param name="top">Vertical position</param>
        public Missile(int left, int top) {
            _top = top;
            _left = left;  
        }
        


    }
}
