/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 18.01.2024
/// Description: Spicy Invader Game, initiate the player's ship and enter a loop:
///     - Reads for input 
///     - Update
///     - Thread.Sleep()

/// TODO :
///     - create two groups of objects: friendly, ennemy neutral
///     - use checkcolision by giving it two lists with var (), to then use with both ennemy missile and friendly collisions.
///     - change Goup, godown, go up to a single method using directions
///     - change postions to vectors
///     - use vectors for directions
///     - gamerunning bool useful ?



using SpaceshipNS;
using System;
using System.Threading;
using EnemiesNS;
using EnemyBlockNS;
using MissileNS;



// Handle multiple input at each cycle 
// Assembly added: WindowsBase and PresentationCore
// https://learn.microsoft.com/en-us/dotnet/api/system.windows.input.keyboard.iskeydown?view=windowsdesktop-8.0
// https://learn.microsoft.com/en-us/dotnet/api/system.windows.input.key?view=windowsdesktop-8.0
// https://stackoverflow.com/questions/6373645/c-sharp-winforms-how-to-set-main-function-stathreadattribute

namespace Spicy_Invader
{
    internal class Game
    {
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
        public const int MISSILES_SPEED = 1;

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

        // Allows Keyboard.IsKeyDown
        [STAThread]
        static void Main(string[] args)
        {
            // Set console size depending on map size 
            Console.WindowWidth = WIDTH + WIDTH_CONSOLE_MARGIN;
            Console.WindowHeight = HEIGHT + HEIGHT_CONSOLE_MARGIN;
            
            // Game running 
            bool gameRunning = false;

            // Hides cursor
            Console.CursorVisible = false;

            // Placeholder menu
            Console.WriteLine("This is a menu");
            Console.ReadKey();
            Console.Clear();

            // Game starts
            gameRunning = true;
            DrawLayout();
            
            // Create the spaceship and displays it
            SpaceShip player = new SpaceShip(x: MARGIN_SIDE + (WIDTH/2), y : HEIGHT);
            player.Draw();

            // Create enemy block 
            EnemyBlock enemyBlock = new EnemyBlock();

            // Tracks cycles between updates
            int missileCycle = 0;
            int enemyCycle = 0;
            int playerCycle = 0;

            // Loop : Reads an input, acts accordingly in player / update missiles / sleep
            while (gameRunning) {

                // Missiles update every PLAYER_SPEED cycles
                if (playerCycle == PLAYER_SPEED)
                {
                    player.PlayerControl();
                    playerCycle = 0;
                }
                else
                {
                    playerCycle++;
                }
                
                // Missiles update every MISSILE_SPEED cycles
                if (missileCycle == MISSILES_SPEED)
                {

                    player.PlayerMissileUpdate();
                    CheckColision();
                    
                    void CheckColision() {
                        foreach (Missile playerm in player.missilesList)
                        {
                            foreach (Enemy enemy in enemyBlock.enemiesTab)
                            {
                                if (enemy != null) {
                                      if (playerm.X >= enemy.X && playerm.X <= enemy.X + enemy.Width && playerm.Y == enemy.Y)
                                    {
                                        player.missilesList.Remove(playerm);
                                        enemy.DelPosition();
                                        playerm.DelPosition();
                                        enemyBlock.enemiesTab[enemy.Row, enemy.Col] = null;
                                        enemyBlock.enemiesByCol[enemy.Col]--;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    
                    missileCycle = 0;
                }
                else { 
                    missileCycle++;
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
    }
}
