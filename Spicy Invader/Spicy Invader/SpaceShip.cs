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
        public const int MAX_HP =50;

        /// <summary>
        /// Maximum active missiles 
        /// </summary>
        public const int MAX_ACTIVE_MISSILES = 20;

        /// <summary>
        /// Width of the sprite
        /// </summary>
        const int WIDTH = 5;

        /// <summary>
        /// Height of the sprite
        /// </summary>
        const int HEIGHT = 1;

        
        /// <summary>
        /// Spaceship sprite
        /// </summary>
        private const string SPRITE =  "/-^-\\";

        private const Game.collisionStatus STATUS = Game.collisionStatus.Friendly;

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
            _collisionStatus = STATUS;
            _hp = MAX_HP;
        }

        /// <summary>
        /// Read for user input (mouvement/fire) 
        /// </summary>
        public void PlayerControl()
        {

            // SPACE pressed,creates a missile at player coordinates
            if (Keyboard.IsKeyDown(Key.Space))
            {
                FireMissile(missilesList: missilesList, vectorY: -1, maxMissiles: MAX_ACTIVE_MISSILES, x: _x + _width/2, y:_y - _height, status:Game.collisionStatus.Friendly, owner: this);
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


    }
}
