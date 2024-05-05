using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Schema;

namespace Spicy_Invader
{
    internal class Menu
    {
        private const int MAX_SCORES = 10;
        private const char SEPARATOR = '\n';
        private bool _easyMode = true;
        private bool _soundOn = true;
        private ConsoleKey _input;
        private const int MARGIN_TOP = 5;

        private const string HIGHSCORE_PATH = @"D:\Temp\SpicyInvaderHighScores.txt";

        private List <int> _highscores = new List<int>();

        private const int RATIO_CONSOLE_HEIGHT= 4;

        /// <summary>
        /// Pause Title
        /// Adapted from https://patorjk.com/software/taag/#p=display&f=Graffiti&t=Type%20Something%20  font: DOOM
        /// </summary>
        private const string PAUSED = "╔═══════════════════════════════════╗\n" +
                                "║ ______                        _   ║\n" +
                                "║ | ___ \\                      | |  ║\n" +
                                "║ | |_/ /_ _ _   _ ___  ___  __| |  ║\n" +
                                "║ |  __/ _` | | | / __|/ _ \\/ _` |  ║\n" +
                                "║ | | | (_| | |_| \\__ \\  __/ (_| |  ║\n" +
                                "║ \\_|  \\__,_|\\__,_|___/\\___|\\__,_|  ║\n" +
                                "╚═══════════════════════════════════╝\n";

        private const string HIGHSCORE_TITLE = " _     _       _                            \n" +
            "| |   (_)     | |                           \n" +
            "| |__  _  __ _| |__  ___  ___ ___  _ __ ___ \n" +
            "| '_ \\| |/ _` | '_ \\/ __|/ __/ _ \\| '__/ _ \\\n" +
            "| | | | | (_| | | | \\__ \\ (_| (_) | | |  __/\n" +
            "|_| |_|_|\\__, |_| |_|___/\\___\\___/|_|  \\___|\n" + 
            "          __/ |                             \n" + 
            "         |___/                              \n";

        /// <summary>
        /// Menu Title
        /// Adapted from https://patorjk.com/software/taag/#p=display&f=Graffiti&t=Type%20Something%20  font: DOOM
        /// </summary>
        private const string MAIN_TITLE =
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
            _highscores =  GetHighscore();
            MainMenu();
        }

        /// <summary>
        /// Check if the file path exists and return its content, else try to create it
        /// </summary>
        /// <returns>List of scores</returns>
        private List<int> GetHighscore()
        {
            List <int> highscores = new List<int>();
            string _content = "";
            int score = 0;

            // If file doesn't exist ...
            while (!File.Exists(HIGHSCORE_PATH))
            {
                // ... try to create it
                try
                {
                    // https://stackoverflow.com/questions/5156254/closing-a-file-after-file-create
                    // https://stackoverflow.com/questions/66537978/c-sharp-system-io-ioexception-file-cannot-be-accessed-because-it-is-accessed-by
                    // Create and CLOSE file to avoid used by other process error when reading content
                    FileStream file = File.Create(HIGHSCORE_PATH);
                    file.Close();
                }
                // If the path is incorect, warning and exit eith false
                catch
                {
                    return new List<int>();
                }
            }

            // Get txt file content as a string and then split it into lines 
            _content = File.ReadAllText(HIGHSCORE_PATH);
            List<string> lines = _content.Split('\n').ToList();

            // For each of the lines add the score to the _highscores
            foreach (string line in lines)
            {
                if (Int32.TryParse(line, out score))
                {
                    _highscores.Add(score);
                }
            }
            // If file found or created return highscores
            return _highscores;
        }
        private void WriteCenterHorizontal(string toDisplay, int top) {
            int maxLineSize = 0;
            string[] lines = toDisplay.Split('\n');
            foreach (string line in lines) {
                if (line.Length > maxLineSize) { 
                    maxLineSize = line.Length;
                }
            }

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
                WriteCenterHorizontal(toDisplay: MAIN_TITLE, top: MARGIN_TOP);
                WriteCenterHorizontal(toDisplay: MAIN_MENU, top: MARGIN_TOP * 2 + MAIN_TITLE.Split('\n').Length);
                _input = Console.ReadKey(intercept:true).Key;

                // Switch input and start the designated method or exit
                switch (_input)
                {
                    case ConsoleKey.D1:
                        Game game = new Game(menu:this, easymode:_easyMode);                    
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

                WriteCenterHorizontal(toDisplay: PAUSED, top: Console.WindowHeight/ RATIO_CONSOLE_HEIGHT);
                _input = Console.ReadKey(intercept: true).Key;

            } while (_input != ConsoleKey.R);
            Console.ForegroundColor = ConsoleColor.Black;
            WriteCenterHorizontal(toDisplay: PAUSED, top: Console.WindowHeight / RATIO_CONSOLE_HEIGHT);
            Console.ForegroundColor = ConsoleColor.White;
            Countdown();
            Console.ForegroundColor = ConsoleColor.Black;
            WriteCenterHorizontal(toDisplay: PAUSED, top: Console.WindowHeight / RATIO_CONSOLE_HEIGHT);
            Console.ForegroundColor = ConsoleColor.White;
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

        private void SortScores() {
            _highscores.Sort();
            _highscores.Reverse();
            if (_highscores.Count > MAX_SCORES) { 
                _highscores.RemoveRange(MAX_SCORES, _highscores.Count - MAX_SCORES);
            }
        }
        private void HighscoreMenu() {
            Console.Clear();
            string toDisplay = "";

            WriteCenterHorizontal(toDisplay: HIGHSCORE_TITLE, top: MARGIN_TOP);

            SortScores();
            for(int i = 0; i< _highscores.Count; i++) {
                toDisplay += (i + 1+ ". " + _highscores[i] + "\n");
            }
            WriteCenterHorizontal(toDisplay: toDisplay, top: MARGIN_TOP *2 + HIGHSCORE_TITLE.Split('\n').Length);
            Console.ReadLine();
        }

        public void AddToHighscore(int score) {
            SortScores();
            if (score > _highscores.Min()) { 
                _highscores.Add(score);
                SortScores();
            }
            UpdadeHighscoreFile();
        }

        private void UpdadeHighscoreFile()
        {
            string content = "";
            foreach (int score in _highscores) {
                content += score;
                content += SEPARATOR;
            }
            File.WriteAllText(HIGHSCORE_PATH, content);
        }

        private void DisplayOptions() {
            WriteCenterHorizontal(toDisplay: MAIN_TITLE, top: MARGIN_TOP);
            if (_easyMode && _soundOn)
            {
                WriteCenterHorizontal(top: MARGIN_TOP * 2 + MAIN_TITLE.Split('\n').Length, toDisplay: EASY_MODE + SOUND_ON + BACK);
            }
            else if (_easyMode && !_soundOn)
            {
                WriteCenterHorizontal(top: MARGIN_TOP * 2 + MAIN_TITLE.Split('\n').Length, toDisplay: EASY_MODE + SOUND_OFF + BACK);
            }
            else if (!_easyMode && !_soundOn)
            {
                WriteCenterHorizontal(top: MARGIN_TOP * 2 + MAIN_TITLE.Split('\n').Length, toDisplay: HARD_MODE + SOUND_OFF + BACK);
            }
            else if (!_easyMode && _soundOn)
            {
                WriteCenterHorizontal(top: MARGIN_TOP * 2 + MAIN_TITLE.Split('\n').Length, toDisplay: HARD_MODE + SOUND_ON + BACK);
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
                WriteCenterHorizontal(top: Console.WindowHeight/4, toDisplay: number);
                Thread.Sleep(1000);
            
            }
            
        }
    }
}
