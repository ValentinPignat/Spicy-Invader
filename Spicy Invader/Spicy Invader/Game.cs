/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 18.01.2024
/// Description: Spicy Invader Game, initiate the player's ship and enter a loop:
///     - Reads for input 
///     - Update
///     - Thread.Sleep()

/// TODO :
///     - Change to onhit method in each object to manage collision and avoid updatig all ? THTHTHTHTH
///     ADD MENU
///     see cdc
///     DISABLE SCROLL AND CLICK
///     CALCULATE WIDTH AND HEIGHT FROM SPRTIE (No need for manual change)
///     ENNEMIES FASTER WITH LINES DOWN
///     ENEMY MOVE AFTER COLISION AND DEAD UPDATE ? 
///     Stop console change / remove nativemethods ?


using SpaceshipNS;
using System;
using System.Threading;
using EnemiesNS;
using EnemyBlockNS;
using MissileNS;
using System.Collections.Generic;
using GameObjectsNS;
using BricksNS;
using System.Windows.Input;
using System.Diagnostics;




// Handle multiple input at each cycle 
// Assembly added: WindowsBase and PresentationCore
// https://learn.microsoft.com/en-us/dotnet/api/system.windows.input.keyboard.iskeydown?view=windowsdesktop-8.0
// https://learn.microsoft.com/en-us/dotnet/api/system.windows.input.key?view=windowsdesktop-8.0
// https://stackoverflow.com/questions/6373645/c-sharp-winforms-how-to-set-main-function-stathreadattribute

namespace Spicy_Invader
{

    internal class Game
    {


        public partial class NativeMethods
        {

            /// Return Type: BOOL->int
            ///fBlockIt: BOOL->int
            [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "BlockInput")]
            [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
            public static extern bool BlockInput([System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)] bool fBlockIt);

        }

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
        public const int WIDTH =40;

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
        
        // Allows Keyboard.IsKeyDown
        [STAThread]
        static void Main(string[] args)
        {
            // Game running 
            bool gameRunning = false;
            NativeMethods.BlockInput(true);

            // Tracks cycles between updates
            int missileCycle = 0;
            int enemyCycle = 0;
            int playerCycle = 0;
            int score = 0;
            List<GameObject> collisionObjects = new List<GameObject>();
            ConsoleKey input;

            // Placeholder menu
            ConsoleSetup();
            Console.WriteLine("Space Invaders");
            Console.ReadKey();

            // Game starts
            gameRunning = true;
            score = 0;

            DrawLayout();

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
                if (Keyboard.IsKeyDown(Key.V))
                {
                    do
                    {
                        input = Console.ReadKey().Key;
                        Console.SetCursorPosition(HEIGHT / 2, WIDTH / 2);
                        Console.WriteLine("Game is paused, press R to resume");

                    } while (input != ConsoleKey.R);
                    
                }
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

                // CheckColision() for player's missile(s);
                foreach (Missile missile in player.missilesList)
                {
                    CheckColision(missile: missile, target: collisionObjects) ;

                    //CheckColision(missile: missile, target: enemyBlock.missilesList);
                }


                // CheckColision() for enemy missile(s);
                foreach ( Missile missile in enemyBlock.missilesList)
                {
                    CheckColision(missile: missile, target: collisionObjects);
                }


                void CheckColision(Missile missile, List <GameObject> target)
                {

                    foreach (GameObject gameObj in target)
                    {
                        if (gameObj != null)
                        {
                            if (missile.X >= gameObj.X && missile.X <= gameObj.X + gameObj.Width && missile.Y >= gameObj.Y && missile.Y <= gameObj.Y + gameObj.Height)
                            {
                                if (missile.CollisionStatus != gameObj.CollisionStatus) { 
                                    if (missile.Hp > 0) { missile.Hp--; }
                                    if (gameObj.Hp > 0) { gameObj.Hp--; }
                                }
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
                        else {
                            gameObj.Draw();
                        }
                    }
                    foreach (GameObject gameObj in toRemove)
                    {
                        collisionObjects.Remove(gameObj);
                    }
                }
                
                Console.SetCursorPosition(0, 0);
                //Console.Write(enemyBlock.LinesDown);
                Console.Write((double)ENEMY_SPEED - ((enemyBlock.LinesDown > 1) ? (double)enemyBlock.LinesDown / RATIO_LINEDOWN_SPEED : 1));
                // Enemy update every ENEMY_SPEED cycles
                if (enemyCycle >= (double)ENEMY_SPEED - ((enemyBlock.LinesDown > 1) ? (double)enemyBlock.LinesDown/RATIO_LINEDOWN_SPEED: 1))
                {   
                    enemyBlock.Update();
                    enemyCycle = 0;
                }
                else
                {
                    enemyCycle += 1;
                }
                

                // Time between each cycle
                Thread.Sleep(CYCLE_SPEED);
            }
            Console.Clear();
            Console.WriteLine("Terminé");
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
