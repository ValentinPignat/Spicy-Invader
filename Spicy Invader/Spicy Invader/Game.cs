/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 18.01.2024
/// Description: Spicy Invader Game, initiate the player's ship and enter a loop:
///     - Reads for input 
///     - Update
///     - Thread.Sleep()

/// TODO :
///     - Change to onhit method in each object to manage collision and avoid updatig all
///     - Colision between ennemies and bricks/player (Physical)



using SpaceshipNS;
using System;
using System.Threading;
using EnemiesNS;
using EnemyBlockNS;
using MissileNS;
using System.Collections.Generic;
using GameObjectsNS;
using System.Diagnostics;
using BricksNS;




// Handle multiple input at each cycle 
// Assembly added: WindowsBase and PresentationCore
// https://learn.microsoft.com/en-us/dotnet/api/system.windows.input.keyboard.iskeydown?view=windowsdesktop-8.0
// https://learn.microsoft.com/en-us/dotnet/api/system.windows.input.key?view=windowsdesktop-8.0
// https://stackoverflow.com/questions/6373645/c-sharp-winforms-how-to-set-main-function-stathreadattribute

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
        public const int ENEMY_SPEED = 8;

        /// <summary>
        /// Missile speed (number of cycle before update)
        /// </summary>
        public const int MISSILES_SPEED = 10;

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
        public const int WIDTH =40;

        /// <summary>
        /// Console horizontal margin around the map
        /// </summary>
        public const int WIDTH_CONSOLE_MARGIN = 10;

        /// <summary>
        /// Console margin around the map
        /// </summary>
        public const int HEIGHT_CONSOLE_MARGIN = 10;

        /// <summary>
        /// Height of the game space 
        /// </summary>
        public const int HEIGHT = 25;

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
        
        // Allows Keyboard.IsKeyDown
        [STAThread]
        static void Main(string[] args)
        {
            // Game running 
            bool gameRunning = false;
            // Tracks cycles between updates
            int missileCycle = 0;
            int enemyCycle = 0;
            int playerCycle = 0;
            int score = 0;

            // Placeholder menu
            Console.WriteLine("This is a menu");
            Console.ReadKey();
            Console.Clear();

            // Game starts
            gameRunning = true;
            score = 0;
            ConsoleSetup();
            DrawLayout();
            List<GameObject> collisionObjects = new List<GameObject>();

            // Create the spaceship and displays it
            SpaceShip player = new SpaceShip(x: MARGIN_SIDE + (WIDTH/2), y : HEIGHT);
            player.Draw();
            collisionObjects.Add(player);

            // Create enemy block 
            EnemyBlock enemyBlock = new EnemyBlock();
            foreach (Enemy enemy in enemyBlock.enemiesTab) {
                collisionObjects.Add(enemy);
            }



            for (int i = 0; i< NB_WALLS; i++) {
                for (int j = 0; j < WALL_WIDTH; j++) {
                    for (int k = 0; k<WALL_HEIGHT; k++) { 
                    collisionObjects.Add(new Brick(x: (WIDTH/(NB_WALLS+1) * (i+1))+j + MARGIN_SIDE, y: HEIGHT - PLAYER_TO_WALL + k));
                    }
                }
            
            }

            // Loop : Reads an input, acts accordingly in player / update missiles / sleep
            while (gameRunning) {

                // Player update every PLAYER_SPEED cycles
                if (playerCycle == PLAYER_SPEED)
                {
                    player.PlayerControl();
                    playerCycle = 0;
                }
                else
                {
                    playerCycle++;
                }
                
                // Missiles update and MOVE every MISSILE_SPEED cycles
                if (missileCycle == MISSILES_SPEED)
                {
                    player.MissileUpdate(moving: true) ;
                    enemyBlock.MissileUpdate(moving: true);
                    missileCycle = 0;
                }
                // Missile update without moving
                else { 
                    missileCycle++;
                    player.MissileUpdate(moving: false);
                    enemyBlock.MissileUpdate(moving: false);
                }

                //CheckColision();
                foreach (Missile missile in player.missilesList)
                {
                    CheckColision(missile: missile);
                }
                foreach (Missile missile in enemyBlock.missilesList)
                {
                    CheckColision(missile: missile);
                }

                void CheckColision(Missile missile)
                {

                    foreach (GameObject gameObj in collisionObjects)
                    {
                        if (gameObj != null)
                        {
                            if (missile.X >= gameObj.X && missile.X <= gameObj.X + gameObj.Width && missile.Y == gameObj.Y)
                            {
                                if (missile.ColisionStatus != gameObj.ColisionStatus)
                                    if (missile.Hp > 0) { missile.Hp--; }
                                if (gameObj.Hp > 0) { gameObj.Hp--; }
                                return;
                            }
                        }
                    }

                }
                DeadUpdate(collisionObjects);
                
                if (enemyBlock.IsEmpty())
                {
                    gameRunning = false;
                }

                DisplayScoreHp(score: score, hp: player.Hp);


                void DeadUpdate(List<GameObject> gameObjects)
                {
                    List<GameObject> toRemove = new List<GameObject>();
                    foreach (GameObject gameObj in gameObjects)
                    {

                        if (gameObj.Hp == 0)
                        {
                            if (gameObj == player)
                            {
                                gameRunning = false;
                            }
                            score += gameObj.Destroyed();
                            toRemove.Add(gameObj);
                        }
                    }
                    foreach (GameObject gameObj in toRemove)
                    {
                        collisionObjects.Remove(gameObj);
                    }
                }


                // Enemy update every ENEMY_SPEED cycles
                if (enemyCycle == ENEMY_SPEED)
                {
                    enemyBlock.Update();
                    enemyCycle = 0;
                }
                else
                {
                    enemyCycle++;
                }
                

                // Time between each cycle
                Thread.Sleep(CYCLE_SPEED);
            }
            Console.Clear();
            Console.WriteLine("Boooo");
            Console.ReadLine();

        }

        /// <summary>
        /// Set up console parameters (Dimension, Cursor)
        /// </summary>
        static void ConsoleSetup() {
            // Set console size depending on map size 
            Console.WindowWidth = WIDTH + WIDTH_CONSOLE_MARGIN;
            Console.WindowHeight = HEIGHT + HEIGHT_CONSOLE_MARGIN;

            // Hides cursor
            Console.CursorVisible = false;
        }
        /// <summary>
        /// Draws the game map and other interface elements
        /// </summary>
        static void DrawLayout() {

            for (int i = MARGIN_SIDE; i <= WIDTH + MARGIN_SIDE; i++) {
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
                    else if (i == MARGIN_SIDE + WIDTH && j == MARGIN_TOP_BOTTOM )
                    {

                        Console.SetCursorPosition(i, j);
                        Console.Write("╗");
                    }

                    else if (i == MARGIN_SIDE || i == WIDTH + MARGIN_SIDE) {
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
        /// Add or remove hp/score and update it
        /// </summary>
        /// <param name="score">Player's score</param>
        /// <param name="hp">Player's hp</param>
        static public void DisplayScoreHp(int score, int hp) {

            Console.SetCursorPosition(MARGIN_SIDE, MARGIN_TOP_BOTTOM + HEIGHT);
            Console.WriteLine("\n\n HP: " + hp + "\n\n SCORE : " + score);
        }
    }
}
