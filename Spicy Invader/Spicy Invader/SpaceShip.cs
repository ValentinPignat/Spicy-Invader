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
    internal class SpaceShip : GameObject
    {
        /// <summary>
        /// Maximum active missiles 
        /// </summary>
        public const int MAX_ACTIVE_MISSILES = 1;

        /// <summary>
        /// Width of the sprite
        /// </summary>
        const int WIDTH = 5;

        /// <summary>
        /// Height of the sprite
        /// </summary>
        const int HEIGHT = 1;

        /// <summary>
        /// List of players active missile
        /// </summary>
        public List<Missile> missilesList = new List<Missile>();

        /// <summary>
        /// Get sprite width
        /// </summary>
        public int Width
        {
            get { return _width; }
        }

        /// <summary>
        /// Get sprite height
        /// </summary>
        public int Heigth
        {
            get { return _height; }
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
            _sprite = SPRITE;
            _height = HEIGHT;
            _width = WIDTH;
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
                FireMissile(missilesList: missilesList, vectorY: -1, maxMissiles: MAX_ACTIVE_MISSILES);
            }

            // LEFT movement key pressed and right movement key not pressed..
            if ((Keyboard.IsKeyDown(Key.Left) || Keyboard.IsKeyDown(Key.A)) & !Keyboard.IsKeyDown(Key.Right) & !Keyboard.IsKeyDown(Key.D))
            {
                // ..Go left
                Move(vectorX: -1, vectorY: 0);
            }

            // RIGTH movement key pressed and left movement key not pressed..
            if ((Keyboard.IsKeyDown(Key.Right) || Keyboard.IsKeyDown(Key.D)) & !Keyboard.IsKeyDown(Key.Left) & !Keyboard.IsKeyDown(Key.A))
            {
                // ..Go right
                Move(vectorX: 1, vectorY: 0);
            }

        }

        /// <summary>
        /// Update missile position
        /// </summary>
        public void MissileUpdate()
        {

            // Track if current missile is out of bounds
            bool outOfBounds;

            // Goes through the missile list and update them 
            for (int i = 0; i < missilesList.Count; i++)
            {
                // Missile goes up 
                outOfBounds = missilesList[i].Move();
                
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
