/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 29.02.2024
/// Description: Game objects class, used for commomn atributes between missiles, player, ennemies.
///     - Movement, and out of bounds test
///     - Shooting Object utiliy FireMissile() UpdateMissile()
///     
/// Note: The "Y" attribute is an inverted axis used for the console position "top" atribute.

using MissileNS;
using Spicy_Invader;
using System;
using System.Collections.Generic;

namespace GameObjectsNS
{
    internal abstract class GameObject 
    {
        /// <summary>
        /// GameObject maximum X position
        /// </summary>
        protected int _maxX = Game.MARGIN_SIDE + Game.WIDTH;

        /// <summary>
        /// GameObject minimum X position
        /// </summary>
        protected int _minX = Game.MARGIN_SIDE;

        /// <summary>
        /// GameObject maximum Y position
        /// </summary>
        protected int _minY = Game.MARGIN_TOP_BOTTOM;

        /// <summary>
        /// GameObject minimum Y position
        /// </summary>
        protected int _maxY = Game.MARGIN_TOP_BOTTOM + Game.HEIGHT;

        /// <summary>
        /// GameObject hp
        /// </summary>
        protected int _hp = 1;

        /// <summary>
        /// GameObject x axis position
        /// </summary>
        protected int _x = 0;

        /// <summary>
        /// GameObject y axis position
        /// </summary>
        protected int _y = 0;

        /// <summary>
        /// GameObject status used for collision : Friendly, Ennemy, Neutral
        /// </summary>
        protected Game.collisionStatus _collisionStatus;

        /// <summary>
        /// List of GameObject active missile
        /// </summary>
        public List<Missile> missilesList = new List<Missile>();

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
        /// Get GameObject _collisionStatus
        /// </summary>
        public Game.collisionStatus CollisionStatus
        {
            get { return _collisionStatus; }
        }

        /// <summary>
        /// Get GameObject horizontal coordinate
        /// </summary>
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Get GameObject vertical coordinate
        /// </summary>
        public int Y
        {
            get { return _y; }
        }

        /// <summary>
        /// Get GameObject width
        /// </summary>
        public int Width
        {
            get { return _width; }
        }

        /// <summary>
        /// Get Get GameObject height
        /// </summary>
        public int Height
        {
            get { return _height; }
        }

        /// <summary>
        /// Get/Set GameObject _hp
        /// </summary>
        public int Hp
        {
            get { return _hp; }
            set { _hp = value; }
        }

        /// <summary>
        /// Delete sprite at old position, change coordinates and redraw sprite
        /// </summary>
        /// <returns>True if moved successfully, false if obstacle</returns>
        public bool Move(int vectorX, int vectorY)
        {

            // If destination isn't out of bounds...
            if (CastOutOfBounds(vectorXCast: vectorX, vectorYCast: vectorY))
            {

                //  .. move and return true
                DelPosition();
                _y += vectorY;
                _x += vectorX;
                Draw();
                return true;
            }

            // else return false
            else {
                return false;
            }
        }

        /// <summary>
        /// Write GameObject sprite (one or multiple lines) at coordinates
        /// </summary>
        public void Draw()
        {
            // Separate sprite in lines
            string[] lines = _sprite.Split('\n');

            // Iterate through changing the line
            for (int i = 0; i < lines.Length; i++) {
                Console.SetCursorPosition(_x, _y+i);
                Console.Write(_sprite);

            }
        }

        /// <summary>
        /// Deletes GameObject sprite from console
        /// </summary>
        public void DelPosition()
        {
            // print spaces in a square width*height
            Console.SetCursorPosition(_x, _y);
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    Console.SetCursorPosition(_x+j, _y+i);
                    Console.Write(" ");
                }               
            }
        }

        /// <summary>
        /// Check validity of position after a vector
        /// </summary>
        /// <param name="vectorXCast">X axis vector</param>
        /// <param name="vectorYCast">Y axis vector</param>
        /// <returns>True if position is valid</returns>
        public bool CastOutOfBounds(int vectorXCast, int vectorYCast)
        {

            // Check X limit
            if (_x + vectorXCast > _maxX - _width || _x + vectorXCast <= _minX) {
                return false;
            }

            // Check X limit
            if (_y + vectorYCast > _maxY - _height || _y + vectorYCast<= _minY)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Create a missile object at the owner position
        /// </summary>
        public void FireMissile(List<Missile> missilesList, int vectorY, int maxMissiles, int x, int y, Game.collisionStatus status, GameObject owner)
        {

            // if the owner doesnt exceed his maximum missile cap
            if (missilesList.Count < maxMissiles)
            {
                // Create a new missile 
                Missile missile = new Missile(x: x, y: y, vectorY: vectorY,collisionStatus: status, owner: owner);

                // Add the missile to the missile list
                missilesList.Add(missile);
                
                missile.Draw();
            }
        }

        /// <summary>
        /// Update GameObject's missiles, cleaning dead missile and optionally moving them
        /// </summary>
        /// <param name="moving">true to make missile move</param>
        public void UpdateMissile(bool moving)
        {

            // Track if current missile is out of bounds
            bool moved = true;

            // Goes through the missile list and update them 
            for (int i = 0; i < missilesList.Count; i++)
            {

                // Missile goes up  if not dead
                if (moving && missilesList[i].Hp != 0) {

                    // False if destination was out of bounds
                    moved = missilesList[i].Move(vectorX: 0, vectorY: missilesList[i].VectorY);
                }

                // If missile is out of bounds or dead ..
                if (!moved || missilesList[i].Hp == 0)
                {

                    // delete his sprite
                    missilesList[i].DelPosition();

                    // .. removes it from the list 
                    missilesList.Remove(missilesList[i]);

                    // Decrement as the list is displaced by the removal
                    i--;
                }
            }
        }

        /// <summary>
        /// Allows the return of a value on death, used for score
        /// </summary>
        /// <returns>int value</returns>
        public virtual int Destroyed() {
            return 0;
        }
    }
}
