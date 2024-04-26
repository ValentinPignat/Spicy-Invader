using BricksNS;
using EnemiesNS;
using EnemyBlockNS;
using GameObjectsNS;
using MissileNS;
using SpaceshipNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace Spicy_Invader
{
    internal class Game
    {
        // Tracks cycles between updates
        private SpaceShip _player;
        private EnemyBlock _enemyBlock;
        private int _missileCycle = 0;
        private int _enemyCycle = 0;
        private int _playerCycle = 0;
        private int _score = 0;
        private List<GameObject> _collisionObjects = new List<GameObject>();
        private ConsoleKey _input;
        private bool _gameRunning = false;

        #region CONSTANTS 
        /// <summary>
        /// Distance between the player and the walls
        /// </summary>
        public const int PLAYER_TO_WALL = 5;

        /// <summary>
        /// Walls height
        /// </summary>
        public const int WALL_HEIGHT = 2;

        /// <summary>
        /// Number of walls to spawn
        /// </summary>
        public const int NB_WALLS = 3;

        /// <summary>
        /// Walls width
        /// </summary>
        public const int WALL_WIDTH = 6;

        /// <summary>
        /// Time between cycles in ms
        /// </summary>
        public const int CYCLE_SPEED = 1;

        /// <summary>
        /// Player speed (number of cycle before update)
        /// </summary>
        public const int PLAYER_SPEED = 2;

        /// <summary>
        /// Enemy speed (number of cycle before update)
        /// </summary>
        public const int ENEMY_SPEED = 5;

        /// <summary>
        /// Missile speed (number of cycle before update)
        /// </summary>
        public const int MISSILES_SPEED = 6;

        /// <summary>
        /// Margin for the display / player and enemies movement zone
        /// </summary>
        public const int MARGIN_SIDE = 5;

        /// <summary>
        /// Margin for the display / player and enemies movement zone
        /// </summary>
        public const int MARGIN_TOP_BOTTOM = 2;

        /// <summary>
        /// Width of the game space 
        /// </summary>
        public const int WIDTH = 40;

        /// <summary>
        /// Console horizontal margin around the map
        /// </summary>
        public const int WIDTH_CONSOLE_MARGIN = 20;

        /// <summary>
        /// Console margin around the map
        /// </summary>
        public const int HEIGHT_CONSOLE_MARGIN = 10;

        /// <summary>
        /// Height of the game space 
        /// </summary>
        public const int HEIGHT = 25;

        private const double RATIO_LINEDOWN_SPEED = 5;

        #endregion

        /// <summary>
        /// Enum used for collision check (Friendly, Enemy, Neutral)
        /// </summary>
        public enum collisionStatus
        {
            Friendly,
            Enemy,
            Neutral,
        }
        /// <summary>
        /// Enum used for collision check (Friendly, Enemy, Neutral)
        /// </summary>

        public Game() { 
            GameSetup();
            GameStart();
        }

        /// <summary>
        /// Add or remove hp/score and update it
        /// </summary>
        /// <param name="score">Player's score</param>
        /// <param name="hp">Player's hp</param>
        static public void DisplayScoreHp(int score, int hp)
        {

            Console.SetCursorPosition(MARGIN_SIDE, MARGIN_TOP_BOTTOM + HEIGHT);
            Console.WriteLine("\n\n HP: " + hp + "\n\n SCORE : " + score);
        }

        /// <summary>
        /// Draws the game map and other interface elements
        /// </summary>
        static void DrawLayout()
        {

            for (int i = MARGIN_SIDE; i <= WIDTH + MARGIN_SIDE; i++)
            {
                for (int j = MARGIN_TOP_BOTTOM; j <= HEIGHT + MARGIN_TOP_BOTTOM; j++)
                {
                    if (i == MARGIN_SIDE && j == MARGIN_TOP_BOTTOM)
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write("╔");
                    }
                    else if (i == MARGIN_SIDE && j == MARGIN_TOP_BOTTOM + HEIGHT)
                    {

                        Console.SetCursorPosition(i, j);
                        Console.Write("╚");
                    }
                    else if (i == MARGIN_SIDE + WIDTH && j == MARGIN_TOP_BOTTOM + HEIGHT)
                    {

                        Console.SetCursorPosition(i, j);
                        Console.Write("╝");
                    }
                    else if (i == MARGIN_SIDE + WIDTH && j == MARGIN_TOP_BOTTOM)
                    {

                        Console.SetCursorPosition(i, j);
                        Console.Write("╗");
                    }

                    else if (i == MARGIN_SIDE || i == WIDTH + MARGIN_SIDE)
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write("║");
                    }
                    else if (j == MARGIN_TOP_BOTTOM || j == HEIGHT + MARGIN_TOP_BOTTOM)
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write("═");
                    }
                }
            }
        }

        private void GameSetup() {
            // Create the spaceship and displays it
            _player = new SpaceShip(x: MARGIN_SIDE + (WIDTH / 2), y: HEIGHT);
            _player.Draw();
            _collisionObjects.Add(_player);

            // Create enemy block 
            _enemyBlock = new EnemyBlock();
            foreach (Enemy enemy in _enemyBlock.enemiesTab)
            {
                _collisionObjects.Add(enemy);
            }

            for (int i = 0; i < NB_WALLS; i++)
            {
                for (int j = 0; j < WALL_WIDTH; j++)
                {
                    for (int k = 0; k < WALL_HEIGHT; k++)
                    {
                        _collisionObjects.Add(new Brick(x: (WIDTH / (NB_WALLS + 1) * (i + 1)) + j + MARGIN_SIDE, y: HEIGHT - PLAYER_TO_WALL + k));
                    }
                }

            }
            _gameRunning = true;
            _score = 0;

            DrawLayout();

        }


        private void GameStart() {


            // Loop : Reads an input, acts accordingly in player / update missiles / sleep
            while (_gameRunning)
            {
                if (Keyboard.IsKeyDown(Key.V))
                {
                    do
                    {
                        _input = Console.ReadKey().Key;
                        Console.SetCursorPosition(HEIGHT / 2, WIDTH / 2);
                        Console.WriteLine("Game is paused, press R to resume");

                    } while (_input != ConsoleKey.R);

                }
                // Player update every PLAYER_SPEED cycles
                if (_playerCycle == PLAYER_SPEED)
                {
                    _player.PlayerControl();
                    _playerCycle = 0;
                }
                else
                {
                    _playerCycle++;
                }

                // Missiles update and MOVE every MISSILE_SPEED cycles
                if (_missileCycle == MISSILES_SPEED)
                {
                    _player.MissileUpdate(moving: true);
                    _enemyBlock.MissileUpdate(moving: true);
                    _missileCycle = 0;
                }
                // Missile update without moving
                else
                {
                    _missileCycle++;
                    _player.MissileUpdate(moving: false);
                    _enemyBlock.MissileUpdate(moving: false);
                }

                // CheckColision() for player's missile(s);
                foreach (Missile missile in _player.missilesList)
                {
                    CheckColision(missile: missile, target: _collisionObjects);

                    //CheckColision(missile: missile, target: enemyBlock.missilesList);
                }


                // CheckColision() for enemy missile(s);
                foreach (Missile missile in _enemyBlock.missilesList)
                {
                    CheckColision(missile: missile, target: _collisionObjects);
                }


                void CheckColision(Missile missile, List<GameObject> target)
                {

                    foreach (GameObject gameObj in target)
                    {
                        if (gameObj != null)
                        {
                            if (missile.X >= gameObj.X && missile.X <= gameObj.X + gameObj.Width && missile.Y >= gameObj.Y && missile.Y <= gameObj.Y + gameObj.Height)
                            {
                                if (missile.CollisionStatus != gameObj.CollisionStatus)
                                {
                                    if (missile.Hp > 0) { missile.Hp--; }
                                    if (gameObj.Hp > 0) { gameObj.Hp--; }
                                }
                                return;
                            }
                        }
                    }

                }
                DeadUpdate(_collisionObjects);

                if (_enemyBlock.IsEmpty())
                {
                    _gameRunning = false;
                }

                DisplayScoreHp(score: _score, hp: _player.Hp);


                void DeadUpdate(List<GameObject> gameObjects)
                {
                    List<GameObject> toRemove = new List<GameObject>();
                    foreach (GameObject gameObj in gameObjects)
                    {

                        if (gameObj.Hp == 0)
                        {
                            if (gameObj == _player)
                            {
                                _gameRunning = false;
                            }
                            _score += gameObj.Destroyed();
                            toRemove.Add(gameObj);
                        }
                        else
                        {
                            gameObj.Draw();
                        }
                    }
                    foreach (GameObject gameObj in toRemove)
                    {
                        _collisionObjects.Remove(gameObj);
                    }
                }

                Console.SetCursorPosition(0, 0);
                //Console.Write(enemyBlock.LinesDown);
                Console.Write((double)ENEMY_SPEED - ((_enemyBlock.LinesDown > 1) ? (double)_enemyBlock.LinesDown / RATIO_LINEDOWN_SPEED : 1));
                // Enemy update every ENEMY_SPEED cycles
                if (_enemyCycle >= (double)ENEMY_SPEED - ((_enemyBlock.LinesDown > 1) ? (double)_enemyBlock.LinesDown / RATIO_LINEDOWN_SPEED : 1))
                {
                    _enemyBlock.Update();
                    _enemyCycle = 0;
                }
                else
                {
                    _enemyCycle += 1;
                }


                // Time between each cycle
                Thread.Sleep(CYCLE_SPEED);
            }
            Console.Clear();
            Console.WriteLine("Terminé");
            Console.ReadLine();

        }

    
    }
}
