using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Spicy_Invader
{
    internal class Menu
    {
        private bool _easyMode =  true;
        private bool _soundOn = true;
        private ConsoleKey _input;
        private const int MARGIN_TOP = 5;

        private string PAUSED = "╔═══════════════════════════════════╗\n" +
            "║ ______                        _   ║\n" +
            "║ | ___ \\                      | |  ║\n" +
            "║ | |_/ /_ _ _   _ ___  ___  __| |  ║\n" +
            "║ |  __/ _` | | | / __|/ _ \\/ _` |  ║\n" +
            "║ | | | (_| | |_| \\__ \\  __/ (_| |  ║\n" +
            "║ \\_|  \\__,_|\\__,_|___/\\___|\\__,_|  ║\n" +
            "╚═══════════════════════════════════╝\n";

        /// <summary>
        /// Menu Title
        /// Adapted from https://patorjk.com/software/taag/#p=display&f=Graffiti&t=Type%20Something%20  font: DOOM
        /// </summary>
        private const string TITLE =
                                    "             _____         _                \n" +
                                    "            /  ___|       (_)               \n" +
                                    "            \\ `--.  _ __   _   ___  _   _  \n" +
                                    "             `--. \\| '_ \\ | | / __|| | | |\n" +
                                    "            /\\__/ /| |_) || || (__ | |_| | \n" +
                                    "            \\____/ | .__/ |_| \\___| \\__, \n" +
                                    "                   | |               __/ |  \n" +
                                    "                   |_|              |___/   \n" +
                                    " _____                           _                       \n" +
                                    "|_   _|                         | |                      \n" +
                                    "  | |   _ __  __   __  __ _   __| |  ___  _ __  ___      \n" +
                                    "  | |  | '_ \\ \\ \\ / / / _` | / _` | / _ \\| '__|/ __| \n" +
                                    " _| |_ | | | | \\ V / | (_| || (_| ||  __/| |   \\__ \\  \n" +
                                    " \\___/ |_| |_|  \\_/   \\__,_| \\__,_| \\___||_|   |___/\n";
        private const string MAIN_MENU = 
                                        "1) NEW GAME\n" +
                                        "2) OPTIONS\n" +
                                        "3) HIGHSCORE\n" +
                                        "4) ABOUT\n" +
                                        "5) EXIT\n";

        private const string SOUND_ON = "2) SOUND ON / sound off\n";
        private const string SOUND_OFF = "2) sound on / SOUND OFF\n";
        private const string EASY_MODE = "1) EASY / hard\n";
        private const string HARD_MODE = "1) easy / HARD\n";
        private const string BACK = "3) MAIN MEMU\n";
        public Menu()
        {
            MainMenu();
        }

        private void WriteCenterHorizontal(string toDisplay, int top) {
            int maxLineSize = 0;
            string[] lines = toDisplay.Split('\n');
            foreach (string line in lines) {
                if (line.Length > maxLineSize) { 
                    maxLineSize = line.Length;
                }
            }
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(MAIN_MENU.Split('\n').Length);

            WriteAt(toDisplay: toDisplay, top: top, left: (Console.WindowWidth / 2) - (maxLineSize / 2));

        }

        private void WriteAt(string toDisplay, int top, int left) {
            string[] lines = toDisplay.Split('\n');
            for (int i = 0; i < lines.Length; i++) { 
                Console.SetCursorPosition(top: top + i, left: left);
                Console.Write(lines[i]);
            }
        }

        private void MainMenu() {


            do
            {
                Console.Clear();
                WriteCenterHorizontal(toDisplay: TITLE, top: MARGIN_TOP);
                WriteCenterHorizontal(toDisplay: MAIN_MENU, top: MARGIN_TOP * 2 + TITLE.Split('\n').Length);
                _input = Console.ReadKey(intercept:true).Key;

                // Switch input and start the designated method or exit
                switch (_input)
                {
                    case ConsoleKey.D1:
                        Game game = new Game(pauseMenu:this);                    
                        break;
                    case ConsoleKey.D2:
                        OptionsMenu();
                        break;
                    case ConsoleKey.D3:
                        HighscoreMenu();
                        break;
                    case ConsoleKey.D4:
                        AboutMenu();
                        break;
                    case ConsoleKey.D5:
                        Environment.Exit(0);
                        break;
                    default:
                        break;

                }

            }  while (true);

        }
        public void Pause() {
            
            do
            {

                WriteCenterHorizontal(toDisplay: PAUSED, top: Console.WindowHeight);
                _input = Console.ReadKey().Key;


            } while (_input != ConsoleKey.R);
            Console.ForegroundColor = ConsoleColor.Black;
            WriteCenterHorizontal(toDisplay: PAUSED, top: Console.WindowHeight);
            Console.ForegroundColor= ConsoleColor.White;
            Countdown();
        }
        private void OptionsMenu() {
            Console.Clear();
            DisplayOptions();
            do
            {
                _input = Console.ReadKey(intercept: true).Key;

                // Switch input and start the designated method or exit
                switch (_input)
                {
                    case ConsoleKey.D1:
                        _easyMode = !_easyMode;
                        Console.Clear();
                        DisplayOptions();
                        break;
                    case ConsoleKey.D2:
                        _soundOn = !_soundOn;
                        Console.Clear();
                        DisplayOptions();
                        break;
                    case ConsoleKey.D3:
                        return;
                    default:
                        break;

                }

            } while (true);
        }

        private void HighscoreMenu() {
            Console.Clear();
            Console.WriteLine("Under construction\nPress any key to return");
            Console.ReadLine();
        }
        private void DisplayOptions() {
            WriteCenterHorizontal(toDisplay: TITLE, top: MARGIN_TOP);
            if (_easyMode && _soundOn)
            {
                WriteCenterHorizontal(top: MARGIN_TOP * 2 + TITLE.Split('\n').Length, toDisplay: EASY_MODE + SOUND_ON + BACK);
            }
            else if (_easyMode && !_soundOn)
            {
                WriteCenterHorizontal(top: MARGIN_TOP * 2 + TITLE.Split('\n').Length, toDisplay: EASY_MODE + SOUND_OFF + BACK);
            }
            else if (!_easyMode && !_soundOn)
            {
                WriteCenterHorizontal(top: MARGIN_TOP * 2 + TITLE.Split('\n').Length, toDisplay: HARD_MODE + SOUND_OFF + BACK);
            }
            else if (!_easyMode && _soundOn)
            {
                WriteCenterHorizontal(top: MARGIN_TOP * 2 + TITLE.Split('\n').Length, toDisplay: HARD_MODE + SOUND_ON + BACK);
            }
        }

        private void AboutMenu() {
            Console.Clear();
            Console.WriteLine("Under construction\nPress any key to return");
            Console.ReadLine();
        }

        private const string ONE_CD = " __  \n" +
                                  "/_ | \n" +
                                  " | | \n" +
                                  " | | \n" +
                                  " | | \n" +
                                  " |_| \n";
        private const string TWO_CD =  " ___   \n" +
                                  "|__ \\ \n" +
                                  "   ) | \n" +
                                  "  / /  \n" +
                                  " / /_  \n" +
                                  "|____| \n";
        private const string THREE_CD = " ____   \n" +
                                       "|___ \\ \n" +
                                       "  __) | \n" +
                                       " |__ <  \n" +
                                       " ___) | \n" +
                                       "|____/  \n";
        private readonly string[] NB_COUNTDOWN = { THREE_CD, TWO_CD, ONE_CD };


        public void Countdown() {
            foreach (string number in NB_COUNTDOWN) {
                WriteCenterHorizontal(top: Console.WindowHeight, toDisplay: number);
                Thread.Sleep(1000);
            
            }
            
        }
    }
}
