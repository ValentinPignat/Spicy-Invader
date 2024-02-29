/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 29.02.2024
/// Description: Game objects class, used for commomn atributes between missiles, player, ennemies.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MissileNS;
using Spicy_Invader;


namespace GameObjectsNS
{
    internal class GameObject 
    {  
        /// <summary>
        /// GameObject x axis position
        /// </summary>
        protected int _x = 0;

        /// <summary>
        /// GameObject y axis position
        /// </summary>
        protected int _y = 0;

        /// <summary>
        /// GameObject sprite
        /// </summary>
        protected string _sprite = string.Empty;

        /// <summary>
        /// GameObject width
        /// </summary>
        protected int _width = 0;

        /// <summary>
        /// GameObject height
        /// </summary>
        protected int _height = 0;

        /// <summary>
        /// Delete sprite at old position, change coordinates and redraw sprite
        /// </summary>
        /// <returns>True boolean if missile goes out of bounds</returns>
        public bool Move(int vectorX, int vectorY)
        {
            if (CastCollision(vectorXCast : vectorX, vectorYCast: vectorY)) {
                // ..Move it up
                DelPosition();
                _y += vectorY;
                _x += vectorX;
                Draw();
                return true;
            }    
            return false;
            
        }

        /// <summary>
        /// Write spaceship sprite at new coordinates
        /// </summary>
        public void Draw()
        {
            Console.SetCursorPosition(_x, _y);
            Console.Write(_sprite);
        }


        /// <summary>
        /// Deletes spaceship at old coordinates
        /// </summary>
        public void DelPosition()
        {
            Console.SetCursorPosition(_x, _y);
            for (int i = 0; i < _width; i++)
            {
                Console.Write(" ");
            }
        }

        public bool CastCollision(int vectorXCast, int vectorYCast)
        {
            if (_x + vectorXCast >= Game.MARGIN_SIDE + Game.WIDTH || _x + vectorXCast <= Game.MARGIN_SIDE) {
                return false;
            }
            if (_y + vectorYCast >= Game.MARGIN_TOP_BOTTOM + Game.HEIGHT || _y + vectorXCast <= Game.MARGIN_TOP_BOTTOM)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Create a missile object at the player coordinate
        /// </summary>
        public void FireMissile(List<Missile> missilesList, int vectorY, int maxMissiles)
        {

            // if the player has no active missile
            if (missilesList.Count < maxMissiles)
            {

                // Create a new missile 
                Missile missile = new Missile(x: _x + (_width / 2), y: _y - (_height), vectorY: vectorY, Game.collisionStatus.Friendly);

                // Add the missile to the list to allow updates
                missilesList.Add(missile);
            }
        }

    }
}
