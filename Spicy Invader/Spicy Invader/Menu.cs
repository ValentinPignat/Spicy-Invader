using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Threading;

namespace Spicy_Invader
{
    internal static class Menu
    {
        /// <summary>
        /// Menu Title
        /// Adapted from https://patorjk.com/software/taag/#p=display&f=Graffiti&t=Type%20Something%20
        /// </summary>
        private const string TITLE = "\n\n\n" +
                                    "                  _____         _                 \n" +
                                    "                 /  ___|       (_)                \n" +
                                    "                 \\ `--.  _ __   _   ___  _   _   \n" +
                                    "                  `--. \\| '_ \\ | | / __|| | | | \n" +
                                    "                 /\\__/ /| |_) || || (__ | |_| |  \n" +
                                    "                 \\____/ | .__/ |_| \\___| \\__,  \n" +
                                    "                        | |               __/ |   \n" +
                                    "                        |_|              |___/    \n" +
                                    "      _____                           _                        \n" +
                                    "     |_   _|                         | |                       \n" +
                                    "       | |   _ __  __   __  __ _   __| |  ___  _ __  ___       \n" +
                                    "       | |  | '_ \\ \\ \\ / / / _` | / _` | / _ \\| '__|/ __|  \n" +
                                    "      _| |_ | | | | \\ V / | (_| || (_| ||  __/| |   \\__ \\   \n" +
                                    "      \\___/ |_| |_|  \\_/   \\__,_| \\__,_| \\___||_|   |___/ \n";
        private const string MAIN_MENU = "\n\n\n" +
                                        "                       1) RESUME / NEW GAME\n" +
                                        "                       3) OPTIONS\n" +
                                        "                       4) HELP\n" +
                                        "                       5) EXIT\n";

        /*private readonly string[] ONE_CD ={"\n" ,
                                  " __  \n" ,
                                  "/_ | \n" ,
                                  " | | \n" ,
                                  " | | \n" ,
                                  " | | \n" ,
                                  " |_| \n"};
        private readonly string[] TWO_CD = {"\n",
                                  " ___   \n" ,
                                  "|__ \\ \n" ,
                                  "   ) | \n" ,
                                  "  / /  \n" ,
                                  " / /_  \n" ,
                                  "|____| \n"};
        private readonly string[] THREE_CD = { "\n" ,
                                       " ____   \n" ,
                                       "|___ \\ \n" ,
                                       "  __) | \n" ,
                                       " |__ <  \n" ,
                                       " ___) | \n" ,
                                       "|____/  \n" };
        */
        //private readonly string[string[]]COUNTDOWN_NB = { ONE_CD, TWO_CD, THREE_CD};

        /*public void Enter() {

            char input = ' ';
            Console.Clear();
            Console.WriteLine(TITLE);
            Console.WriteLine(MAIN_MENU);
            while (true) {
                input = Console.ReadKey().KeyChar;
                switch (input)
                {
                    case '1':
                        Console.Clear();
                        Countdown();
                        return;
                    default:
                        break;
                }
            }        
        }*/

        /*public void Countdown() {
            for (int i = COUNTDOWN_NB.Length -1; i >= 0; i--)
            {
                for (int j = 0; j < COUNTDOWN_NB[i].Length - 1; j++) {
                    Console.SetCursorPosition(Game.WIDTH, Game.HEIGHT + j);
                    Console.Clear();
                    Console.WriteLine(COUNTDOWN_NB[i][j]);
                    Thread.Sleep(1000);
                }
            }
            
        }*/
    }
}
