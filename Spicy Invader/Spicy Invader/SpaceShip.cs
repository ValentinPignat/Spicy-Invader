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
        private int _x = 20;

        /// <summary>
        /// Vertical coordinates
        /// </summary>
        private int _y = 20;

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
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Get sprite vertical coordinate
        /// </summary>
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// Spaceship sprite
        /// </summary>
        private const string SPRITE =  "/-^-\\";

        /// <summary>
        /// Constructor SpaceShip, position depending on map size
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public SpaceShip(int x, int y) {
            _x = x;
            _y = y;
        }

        /// <summary>
        /// Deletes spaceship at old coordinates
        /// </summary>
        public void DelPosition() {
            Console.SetCursorPosition(_x, _y);
            for (int i = 0; i < WIDTH; i++)
            {
                Console.Write(" ");
            }
        }

        /// <summary>
        /// Delete sprite at old position, change coordinates and redraw sprite
        /// </summary>
        public void GoLeft() {

            // if player isn't on the left edge of the map
            if (_x > Game.MARGIN_SIDE+1) { 
            DelPosition();
            _x--;
            Draw();
            }
        }

        /// <summary>
        /// Delete sprite at old position, change coordinates and redraw sprite
        /// </summary>
        public void GoRight()
        {
            if (_x < Game.WIDTH + Game.MARGIN_SIDE - WIDTH)
            {
                DelPosition();
                _x++;
                Draw();
            }
        }

        /// <summary>
        /// Write spaceship sprite at new coordinates
        /// </summary>
        public void Draw()
        {
            Console.SetCursorPosition(_x, _y);
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
                FireMissile();
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
        /// Create a missile object at the player coordinate
        /// </summary>
        public void FireMissile() {

            // if the player has no active missile
            if (missilesList.Count == 0) { 

            // Create a new missile 
            Missile missile = new Missile(x: _x + (WIDTH / 2), y: _y - (HEIGHT));

            // Add the missile to the list to allow updates
            missilesList.Add(missile);
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
                
                // If missile is out of bounds.. 
                if (outOfBounds)
                {

                    // ..removes it from the list 
                    missilesList.Remove(missilesList[i]);

                    // Decrement as the list is displaced by the removal
                    i--;
                }
            }
           
            
        }


    }
}
