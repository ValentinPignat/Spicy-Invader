/// ETML
/// Author: Valentin Pignat
/// Date (creation) : 26.04.2024
/// Description : Game class for Spicy Invader
///         -   Create a game with many GameObjects and loops until end of game
///         -   Constants can be changed to tweak different aspects of the game

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;

namespace Spicy_Invader
{
    /// <summary>
    /// Game class for Spicy Invader
    /// </summary>
    public class Game
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
        private const int CYCLE_SPEED = 1;

        /// <summary>
        /// Player speed (number of cycle before update)
        /// </summary>
        private const int PLAYER_SPEED = 2;

        /// <summary>
        /// Enemy speed (number of cycle before update)
        /// </summary>
        private const int ENEMY_SPEED = 5;

        /// <summary>
        /// Missile speed (number of cycle before update)
        /// </summary>
        private const int MISSILES_SPEED = 3;

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
        public const int WIDTH = 50;

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

        /// <summary>
        /// Every x line went down speed goes up by 1 
        /// </summary>
        private const double RATIO_LINEDOWN_ACCELETATION = 5;

        /// <summary>
        /// Control scheme for player
        /// </summary>
        private const string CONTROL_SCHEME = "SPACE : Shoot    V : Pause   R : Resume  A/D or <-/-> : Move";
        #endregion

        #region ATTRIBUTES

        /// <summary>
        /// Easy mode
        /// </summary>
        public bool _easymode = true;

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

        /// <summary>
        /// SoundManager to call for audio
        /// </summary>
        private SoundManager _soundManager;

        /// <summary>
        /// Number of wave spawned
        /// </summary>
        private int _waveCount = 0;
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
        /// <param name="easyMode">True for easy / False for hard (default true)</param>
        public Game(Menu menu, SoundManager soundManager, bool easyMode = true) { 

            _pauseMenu = menu;
            _easymode = easyMode;
            _soundManager = soundManager;
            GameSetup();
            GameStartLoop();
        }

        /// <summary>
        /// Add or remove hp/score and update it
        /// </summary>
        /// <param name="score">Player's score to add/remove</param>
        /// <param name="hp">Player's hp to add/remove</param>
        public static void DisplayScoreHp(int score = 0, int hp = 0)
        {
            Console.SetCursorPosition(0, MARGIN_TOP_BOTTOM + HEIGHT);
            Console.WriteLine("\n\n HP: " + hp + "\n\n SCORE : " + score);
        }

        /// <summary>
        /// Display command scheme for space invaders
        /// </summary>
        public static void DisplayControlScheme()
        {
            Console.SetCursorPosition(MARGIN_SIDE, Console.WindowHeight - 2 );
            Console.WriteLine(CONTROL_SCHEME);
  
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

            // Start game with score at 0
            _gameRunning = true;
            _score = 0;

            // Draw Layout
            DrawAll();
        }

        /// <summary>
        /// Draw every game and UI element
        /// </summary>
        private void DrawAll() { 
            DrawLayout();
            DisplayControlScheme();  
            DisplayScoreHp(score: 0, hp:0) ;
            foreach (GameObject go in _collisionObjects) {
                go.Draw();
            }
            foreach (Missile missile in _player._missilesList)
            {
               missile.Draw();
            }
            foreach (Missile missile in _enemyBlock._missilesList)
            {
                missile.Draw();
            }
        }

        /// <summary>
        /// Populate _enemyBlock with and EnemyBlock
        /// </summary>
        private void SpawnEnemies() {

            // Create enemy block with scaling bonus missile on each wave 
            _enemyBlock = new EnemyBlock(easymode: _easymode, soundManager:_soundManager, bonusMissile: _waveCount);

            // Create enemies
            foreach (Enemy enemy in _enemyBlock._enemiesTab)
            {
                _collisionObjects.Add(enemy);
            }

            // Increment wave count 
            _waveCount++;
        }

        /// <summary>
        /// Enter game loop until end of game
        /// </summary>
        private void GameStartLoop() {

            // Loop : Check for pause > Player action > Missile upate > Enemy update > Colisions check > Dispose of dead objects
            while (_gameRunning)
            {
                // If V is pressed enter pause
                if (Keyboard.IsKeyDown(Key.V))
                {
                    _soundManager.MenuSound();
                    _pauseMenu.Pause();
                    DrawAll();
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
                    _player.UpdateMissile(moving: true);
                    _enemyBlock.UpdateMissile(moving: true);
                    _missileCycle = 0;
                }
                // Missile update without moving
                else
                {
                    _missileCycle++;
                    _player.UpdateMissile(moving: false);
                    _enemyBlock.UpdateMissile(moving: false);
                }

                // CheckColision() for player's missile(s);
                foreach (Missile missile in _player._missilesList)
                {
                    CheckColision(missile: missile, target: _collisionObjects, targetMissile: _enemyBlock._missilesList);
                }

                // CheckColision() for enemy missile(s);
                foreach (Missile missile in _enemyBlock._missilesList)
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
                if (_enemyCycle >= (double)ENEMY_SPEED - ((_enemyBlock.LinesDown > 1) ? (double)_enemyBlock.LinesDown / RATIO_LINEDOWN_ACCELETATION : 1))
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

                // If game is not running bc enemyBlock is empty and easy mode is off
                if (_gameRunning == false && !_easymode && _player.Hp > 0)
                {

                    // Respawn ennemies and continue
                    SpawnEnemies();
                    _gameRunning = true;
                }                               
            } // End of Game loop

            // After game end add score to highscore
            _pauseMenu.GameOver(score: _score);
           
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
                if (_player._missilesList.Count < SpaceShip.MAX_ACTIVE_MISSILES)
                {
                    _soundManager.FiringSound();
                }
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
        /// <param name="target">List of GameObjects to target</param>
        /// <param name="targetMissile">List of Missile to target</param>
        private void CheckColision(Missile missile, List<GameObject> target, List<Missile> targetMissile = null)
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
                            _soundManager.CollisionSound();
                            // ... deal one damage to both objects
                            if (missile.Hp > 0) { missile.Hp--; }
                            if (gameObj.Hp > 0) { gameObj.Hp--; }
                        }
                        return;
                    }
                }
            }

            if (targetMissile != null) {
                // Foreach Missile ...
                foreach (Missile tarMissile in targetMissile)
                {
                    if (tarMissile != null)
                    {

                        // ... if missile is on any coordinate in missile target square hitbox ...
                        if (missile.X >= tarMissile.X && missile.X <= tarMissile.X + tarMissile.Width && missile.Y >= tarMissile.Y && missile.Y <= tarMissile.Y + tarMissile.Height)
                        {
                            if (missile.CollisionStatus != tarMissile.CollisionStatus)
                            {
                                _soundManager.CollisionSound();
                                // ... deal one damage to both objects
                                if (missile.Hp > 0) { missile.Hp--; }
                                if (tarMissile.Hp > 0) { tarMissile.Hp--; }
                            }
                            return;
                        }
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
