/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 18.01.2024
/// Description: Spaceship class, allowing movements with arrow keys or wasd and fire missile with space
///     - PlayerControl() Reads for input 
///         - GoLeft()
///         - GoRigth()
///         - FireMissile() TODOOOOOOOOOO


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameObjectsNS;
using System.Windows.Input;
using MissileNS;
using Spicy_Invader;



namespace SpaceshipNS
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

        /// <summary>
        /// Horitontal coordinate
        /// </summary>
        private int _left = 20;

        /// <summary>
        /// Vertical coordinates
        /// </summary>
        private int _top = 20;

        /// <summary>
        /// List of players active missile
        /// </summary>
        public List<Missile> missilesList = new List<Missile>();

        /// <summary>
        /// Get sprite width
        /// </summary>
        public int Width
        {
            get { return WIDTH; }
        }

        /// <summary>
        /// Get sprite height
        /// </summary>
        public int Heigth
        {
            get { return HEIGHT; }
        }

        /// <summary>
        /// Get sprite horizontal coordinate
        /// </summary>
        public int Left
        {
            get { return _left; }
            set { _left = value; }
        }

        /// <summary>
        /// Get sprite vertical coordinate
        /// </summary>
        public int Top
        {
            get { return _top; }
            set { _top = value; }
        }

        /// <summary>
        /// Spaceship sprite
        /// </summary>
        private const string SPRITE =  "/-^-\\";



        /// <summary>
        /// Deletes spaceship at old coordinates
        /// </summary>
        public void DelPosition() {
            Console.SetCursorPosition(_left, _top);
            for (int i = 0; i < WIDTH; i++)
            {
                Console.Write(" ");
            }
        }

        /// <summary>
        /// Delete sprite at old position, change coordinates and redraw sprite
        /// </summary>
        public void GoLeft() {
            DelPosition();
            _left--;
            Draw();
        }

        /// <summary>
        /// Delete sprite at old position, change coordinates and redraw sprite
        /// </summary>
        public void GoRight()
        {
            DelPosition();
            _left++;
            Draw();
        }

        /// <summary>
        /// Write spaceship sprite at new coordinates
        /// </summary>
        public void Draw()
        {
            Console.SetCursorPosition(_left, _top);
            Console.Write(SPRITE);
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SpaceShip() {
            Draw();
        }

        /// <summary>
        /// Read for user input (mouvement/fire) 
        /// </summary>
        public void PlayerControl()
        {

            // SPACE pressed,creates a missile at player coordinates
            if (Keyboard.IsKeyDown(Key.Space))
            {
                Missile missile = new Missile(left: _left + (WIDTH / 2), top: _top - (HEIGHT));

                // Add the missile to the list to allow updates
                missilesList.Add(missile);
            }

            // LEFT movement key pressed and right movement key not pressed..
            if ((Keyboard.IsKeyDown(Key.Left) || Keyboard.IsKeyDown(Key.A)) & !Keyboard.IsKeyDown(Key.Right) & !Keyboard.IsKeyDown(Key.D))
            {
                // ..Go left
                GoLeft();
            }

            // RIGTH movement key pressed and left movement key not pressed..
            if ((Keyboard.IsKeyDown(Key.Right) || Keyboard.IsKeyDown(Key.D)) & !Keyboard.IsKeyDown(Key.Left) & !Keyboard.IsKeyDown(Key.A))
            {
                // ..Go right
                GoRight();
            }

        }

        /// <summary>
        /// Update missile position
        /// </summary>
        public void PlayerMissileUpdate()
        {

            // Track if current missile is out of bounds
            bool outOfBounds;

            // Goes through the missile list and update them 
            for (int i = 0; i < missilesList.Count; i++)
            {
                // Missile goes up 
                outOfBounds = missilesList[i].GoUp();
                
                // If missile is out of bounds removes it from the list 
                if (outOfBounds)
                {
                    missilesList.Remove(missilesList[i]);

                    // Decrement as the list is displaced by the removal
                    i--;
                }
            }
           
            
        }


    }
}
