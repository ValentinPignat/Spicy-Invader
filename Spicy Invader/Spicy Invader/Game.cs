/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 18.01.2024
/// Description: Spicy Invader Game

using SpaceShips;
using System;
using System.Threading;
using MissileN;


namespace Spicy_Invader
{
    internal class Game
    {
        static void Main(string[] args)
        {
            // Main thread = current
            Thread mainThread = Thread.CurrentThread;
            mainThread.Name = "Main Thread";

            Thread playerControl = new Thread(PlayerControl);
            playerControl.Name = "Read Input";


            Console.WriteLine("This is a menu");
            Console.ReadKey();
            Console.Clear();
            
            
            playerControl.Start();
            

            void PlayerControl()
            {
                // Readkey variable
                ConsoleKey keyPressed;

                // Hide the cursor 
                Console.CursorVisible = false;

                // Créate the spaceship and displays it
                SpaceShip player = new SpaceShip();

                player.Draw();

                while (true)
                {
                    keyPressed = Console.ReadKey(true).Key;
                    switch (keyPressed)
                    {
                        case ConsoleKey.Spacebar:
                            Missile missile = new Missile(left : player.Left+(player.Width/2), top :player.Top-(player.Heigth)) ;
                            break;
                        case ConsoleKey.A:
                        case ConsoleKey.LeftArrow:
                            player.GoLeft();
                            break;

                        case ConsoleKey.D:
                        case ConsoleKey.RightArrow:
                            player.GoRight();
                            break;

                        default:
                            break;
                    }

                }
            }
  
        }
    }
}
