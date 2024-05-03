/// ETML
/// Author: Valentin Pignat
/// Date (creation) : 26.04.2024
/// Description : Game class for Spicy Invader
///         -   Create a game and loops until end of game
///         -   Constants can be changed to tweak different aspects of the game

using BricksNS;
using EnemiesNS;
using EnemyBlockNS;
using GameObjectsNS;
using MissileNS;
using SpaceshipNS;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;

namespace Spicy_Invader
{
    internal class Game
    {

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

        private bool _easymode = true;

        #endregion

        #region ATTRIBUTES
        /// <summary>
        /// Menu called for pause
        /// </summary>
        private Menu _pauseMenu;

        /// <summary>
        /// Player spaceship
        /// </summary>
        private SpaceShip _player;

        /// <summary>
        /// EnemyBlock
        /// </summary>
        private EnemyBlock _enemyBlock;

        /// <summary>
        /// Nb of cycles since last missile action
        /// </summary>
        private int _missileCycle = 0;

        /// <summary>
        /// Nb of cycles since last enemy action
        /// </summary>
        private int _enemyCycle = 0;

        /// <summary>
        /// Nb of cycles since last player action
        /// </summary>
        private int _playerCycle = 0;

        /// <summary>
        /// Player score
        /// </summary>
        private int _score = 0;

        /// <summary>
        /// List of object to check collision on
        /// </summary>
        private List<GameObject> _collisionObjects = new List<GameObject>();

        /// <summary>
        /// Bool for  main game loop
        /// </summary>
        private bool _gameRunning = false;

        #endregion

        /// <summary>
        /// Public Enum used for collision check (Friendly, Enemy, Neutral)
        /// </summary>
        public enum collisionStatus
        {
            Friendly,
            Enemy,
            Neutral,
        }

        /// <summary>
        /// Game constructor. Setup and start game
        /// </summary>
        /// <param name="menu">Menu used for pause during play and highscore update</param>
        /// <param name="easymode">True for easy / False for hard (default true)</param>
        public Game(Menu menu, bool easymode = true) { 
            _pauseMenu = menu;
            _easymode = easymode;
            GameSetup();
            GameStart();
        }

        /// <summary>
        /// Add or remove hp/score and update it
        /// </summary>
        /// <param name="score">Player's score</param>
        /// <param name="hp">Player's hp</param>
        public static void DisplayScoreHp(int score, int hp)
        {

            Console.SetCursorPosition(MARGIN_SIDE, MARGIN_TOP_BOTTOM + HEIGHT);
            Console.WriteLine("\n\n HP: " + hp + "\n\n SCORE : " + score);
        }

        /// <summary>
        /// Draws the game map and other interface elements
        /// </summary>
        private static void DrawLayout()
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

        /// <summary>
        /// Start a game, create player, ennemies, bunkers and draw layout
        /// </summary>
        private void GameSetup() {

            Console.Clear();

            // Create the spaceship and displays it
            _player = new SpaceShip(x: MARGIN_SIDE + (WIDTH / 2), y: HEIGHT, easymode: _easymode);
            _player.Draw();
            _collisionObjects.Add(_player);

            SpawnEnemies();

            // Create bricks to form bunkers
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

        public void RedrawAll() { 
            DrawLayout();
            foreach (GameObject go in _collisionObjects) {
                go.Draw();
            }
            foreach (Missile missile in _player.missilesList)
            {
               missile.Draw();
            }
            foreach (Missile missile in _enemyBlock.missilesList)
            {
                missile.Draw();
            }
        }
        private void SpawnEnemies() {
            // Create enemy block 
            _enemyBlock = new EnemyBlock(easymode: _easymode);

            // Create enemies
            foreach (Enemy enemy in _enemyBlock.enemiesTab)
            {
                _collisionObjects.Add(enemy);
            }

        }

        /// <summary>
        /// Enter game loop until end of game
        /// </summary>
        private void GameStart() {

            // Loop : Check for pause > Player action > Missile upate > Enemy update > Colisions check > Dispose of dead objects
            while (_gameRunning)
            {
                // If V is pressed enter pause
                if (Keyboard.IsKeyDown(Key.V))
                {
                    _pauseMenu.Pause();
                    RedrawAll();
                }

                // Player update every PLAYER_SPEED cycles
                if (_playerCycle == PLAYER_SPEED)
                {
                    PlayerControl();
                    _playerCycle = 0;
                }
                else
                {
                    _playerCycle++;
                }

                // Missiles update and MOVE every MISSILE_SPEED cycles
                if (_missileCycle == MISSILES_SPEED)
                {
                    _player.Update(moving: true);
                    _enemyBlock.Update(moving: true);
                    _missileCycle = 0;
                }
                // Missile update without moving
                else
                {
                    _missileCycle++;
                    _player.Update(moving: false);
                    _enemyBlock.Update(moving: false);
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

                // Check for "dead" objects (hp = 0)
                DeadUpdate(_collisionObjects);

                // If _enemyBlock is empty end game
                if (_enemyBlock.IsEmpty())
                {
                    _gameRunning = false;
                }

                // Display score
                DisplayScoreHp(score: _score, hp: _player.Hp);

                // Enemy speed goes up when they go down, ratio can be changed
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

                // 
                if (_gameRunning == false && !_easymode && _player.Hp > 0)
                {
                    SpawnEnemies();
                    _gameRunning = true;

                }
                
                
                
            }

            // After game end add score to highscore
            _pauseMenu.AddToHighscore(_score);
        }

        /// <summary>
        /// Read for user input (mouvement/fire) 
        /// </summary>
        public void PlayerControl()
        {

            // SPACE pressed,creates a missile at player coordinates
            if (Keyboard.IsKeyDown(Key.Space))
            {
                _player.Shoot();
            }

            // LEFT movement key pressed and right movement key not pressed..
            if ((Keyboard.IsKeyDown(Key.Left) || Keyboard.IsKeyDown(Key.A)) & !Keyboard.IsKeyDown(Key.Right) & !Keyboard.IsKeyDown(Key.D))
            {
                // ..Go left
                _player.Move(vectorX: -1, vectorY: 0);
            }

            // RIGTH movement key pressed and left movement key not pressed..
            if ((Keyboard.IsKeyDown(Key.Right) || Keyboard.IsKeyDown(Key.D)) & !Keyboard.IsKeyDown(Key.Left) & !Keyboard.IsKeyDown(Key.A))
            {
                // ..Go right
                _player.Move(vectorX: 1, vectorY: 0);
            }
        }

        /// <summary>
        /// Check colision bewtween a Missile and a list of GameObject()
        /// Supports sprite on multiple lines
        /// </summary>
        /// <param name="missile">Missile</param>
        /// <param name="target">List of GameObjects</param>
        private void CheckColision(Missile missile, List<GameObject> target)
        {

            // Foreach GameObject ...
            foreach (GameObject gameObj in target)
            {
                if (gameObj != null)
                {

                    // ... if missile is on any coordinate in his square hitbox ...
                    if (missile.X >= gameObj.X && missile.X <= gameObj.X + gameObj.Width && missile.Y >= gameObj.Y && missile.Y <= gameObj.Y + gameObj.Height)
                    {
                        if (missile.CollisionStatus != gameObj.CollisionStatus)
                        {

                            // ... deal one damage to both objects
                            if (missile.Hp > 0) { missile.Hp--; }
                            if (gameObj.Hp > 0) { gameObj.Hp--; }
                        }
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Dispose of DeadObjects
        /// </summary>
        /// <param name="gameObjects">List of game update to purge of dead update</param>
        private void DeadUpdate(List<GameObject> gameObjects)
        {

            // Create new list with only dead objects
            List<GameObject> toRemove = new List<GameObject>();

            // Foreach object 
            foreach (GameObject gameObj in gameObjects)
            {
                // If object is dead
                if (gameObj.Hp == 0)
                {

                    // If the object is the player - End Game
                    if (gameObj == _player)
                    {
                        _gameRunning = false;
                    }

                    // Add score on death (nb of point defined in enemy)
                    _score += gameObj.Destroyed();
                    toRemove.Add(gameObj);
                }

                // Redraw the sprite
                else
                {
                    gameObj.Draw();
                }
            }

            // Remove every dead object from _colisionObjets
            foreach (GameObject gameObj in toRemove)
            {
                _collisionObjects.Remove(gameObj);
            }
        }
    }
}
