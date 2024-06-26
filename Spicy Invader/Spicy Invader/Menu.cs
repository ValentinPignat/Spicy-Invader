﻿/// ETML 
/// Author : Valentin Pignat
/// Date (creation): 21.03.2024
/// Description : Menu class used to display various menus (Main menu and pause menu) and manage options / highscores
///     - Console utilitary WriteCenterHorizontal() / WriteAt()
///     - Various menus (options, highscore, about, main menu, pause menu)
///     - Manage highscores, read them from file and update them

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

[assembly: InternalsVisibleToAttribute("SpicyTest")]
namespace Spicy_Invader
{
    /// <summary>
    ///  Menu class used to display various menus and manage options / highscores
    /// </summary>
    public class Menu
    {
        #region CONSTANTS
        /// <summary>
        /// Separator between username and highscore in text file
        /// </summary>
        private const char SEPARATOR = ';';

        /// <summary>
        /// Max number of high scores - keep x highest scores and discard rest
        /// </summary>
        private const int MAX_SCORES = 10;

        /// <summary>
        /// Top margin size
        /// </summary>
        private const int MARGIN_TOP = 2;

        /// <summary>
        /// Maximum lenght of usernames
        /// </summary>
        private const int MAX_NAME_SIZE = 15;

        /// <summary>
        /// Highscore file path
        /// </summary>
        private const string HIGHSCORE_PATH = @"..\..\highscore.txt";

        /// <summary>
        /// Position relative to console height 1/x of Console.WindowHeight
        /// </summary>
        private const int RATIO_CONSOLE_HEIGHT= 4;

        /// <summary>
        /// Pause title
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

        /// <summary>
        /// Highscore title
        /// Adapted from https://patorjk.com/software/taag/#p=display&f=Graffiti&t=Type%20Something%20  font: DOOM
        /// </summary>
        private const string HIGHSCORE_TITLE = " _   _ _       _                            \n" +
            "| | | (_)     | |                           \n" +
            "| |_| |_  __ _| |__  ___  ___ ___  _ __ ___ \n" +
            "|  _  | |/ _` | '_ \\/ __|/ __/ _ \\| '__/ _ \\\n" +
            "| | | | | (_| | | | \\__ \\ (_| (_) | | |  __/\n" +
            "\\_| |_/_|\\__, |_| |_|___/\\___\\___/|_|  \\___|\n" +
            "          __/ |                             \n" +
            "         |___/                              \n";

        /// <summary>
        /// Menu title
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

        /// <summary>
        /// Game over title
        /// </summary>
        private const string GAME_OVER_TITLE = " _____                      \n" +
                                    "|  __ \\                     \n" +
                                    "| |  \\/ __ _ _ __ ___   ___ \n" +
                                    "| | __ / _` | '_ ` _ \\ / _ \\\n" +
                                    "| |_\\ \\ (_| | | | | | |  __/\n" +
                                    " \\____/\\__,_|_| |_| |_|\\___|\n" +
                                    "                            \n" +
                                    "                            \n" +
                                    " _____                      \n" +
                                    "|  _  |                     \n" +
                                    "| | | |_   _____ _ __          \n" +
                                    "| | | \\ \\ / / _ \\ '__|      \n" +
                                    "\\ \\_/ /\\ V /  __/ |         \n" +
                                    " \\___/  \\_/ \\___|_|         \n";
        
        /// <summary>
        /// Main menu options
        /// </summary>
        private const string MAIN_MENU = 
                                        "1) NEW GAME\n" +
                                        "2) OPTIONS\n" +
                                        "3) HIGHSCORE\n" +
                                        "4) ABOUT\n" +
                                        "5) EXIT\n";

        /// <summary>
        /// Game Over prompt
        /// </summary>
        private const string GAME_OVER_PROMPT = "Enter username for highscores:  ";

        /// <summary>
        /// Sound on highlighted
        /// </summary>
        private const string SOUND_ON = "2) SOUND ON / sound off\n";

        /// <summary>
        /// Sound off highlighted
        /// </summary>
        private const string SOUND_OFF = "2) sound on / SOUND OFF\n";

        /// <summary>
        /// Easy mode highlighted
        /// </summary>
        private const string EASY_MODE = "1) EASY / hard\n";

        /// <summary>
        /// Hard mode highlighted
        /// </summary>
        private const string HARD_MODE = "1) easy / HARD\n";

        /// <summary>
        /// Return to main menu
        /// </summary>
        private const string BACK = "3) MAIN MEMU\n";

        /// <summary>
        /// Any key to return to main menu
        /// </summary>
        private const string RETURN = "Press any key to go back to main menu";

        /// <summary>
        /// About title
        /// </summary>
        private const string ABOUT_TITLE = "  ___  ______  _____ _   _ _____ \n" +
            " / _ \\ | ___ \\|  _  | | | |_   _|\n" +
            "/ /_\\ \\| |_/ /| | | | | | | | |  \n" +
            "|  _  || ___ \\| | | | | | | | |  \n" +
            "| | | || |_/ /\\ \\_/ / |_| | | |  \n" +
            "\\_| |_/\\____/  \\___/ \\___/  \\_/  \n" ;

        /// <summary>
        /// About section
        /// </summary>
        private const string ABOUT = "COMMANDS:\n\n" +
            "Shoot\tSPACEBAR\n" +
            "Pause\tV\n" + 
            "Resume\tR\n"+
            "Go left\tA or left arrow\n"+
            "Go right\tD or right arrow\n\n"+
            "RULES:\n\n"+
            "Kill a maximum of enemies and dodge their shots\n\n"+
            "HARD MODE:\n\tLess health \n\tMore enemy missiles \n\tEnemy respawn \n\tHigher scores!";
        
        /// <summary>
        /// Number one display
        /// Adapted from https://patorjk.com/software/taag/#p=display&f=Graffiti&t=Type%20Something%20 font:DOOM
        /// </summary>
        private const string ONE_CD = " __  \n" +
                                  "/  | \n" +
                                  "`| | \n" +
                                  " | | \n" +
                                  "_| |_\n" +
                                  "\\___/\n";

        /// <summary>
        /// Number two display
        /// Adapted from https://patorjk.com/software/taag/#p=display&f=Graffiti&t=Type%20Something%20 font:DOOM
        /// </summary>
        private const string TWO_CD = " _____ \n" +
                                  "/ __  \\\n" +
                                  "`' / /'\n" +
                                  "  / /  \n" +
                                  "./ /___\n" +
                                  "\\_____/\n";

        /// <summary>
        /// Number three display
        /// Adapted from https://patorjk.com/software/taag/#p=display&f=Graffiti&t=Type%20Something%20 font:DOOM
        /// </summary>
        private const string THREE_CD = " _____ \n" +
                                       "|____ |\n" +
                                       "    / /\n" +
                                       "    \\ \\\n" +
                                       ".___/ /\n" +
                                       "\\____/ \n";

        /// <summary>
        /// Array of every countdown number
        /// </summary>
        private readonly string[] NB_COUNTDOWN = { THREE_CD, TWO_CD, ONE_CD };

        /// <summary>
        /// Index of usernames in _highscores
        /// </summary>
        private const int INDEX_USERNAME = 0;

        /// <summary>
        /// Index of scores in _highscores
        /// </summary>
        private const int INDEX_SCORE = 1;
        #endregion

        /// <summary>
        /// SoundManager to call for audio
        /// </summary>
        private SoundManager _soundManager;

        /// <summary>
        /// Easy mode toggle
        /// </summary>
        private bool _easyMode = true;

        /// <summary>
        /// User input
        /// </summary>
        private ConsoleKey _input;


        /// <summary>
        /// Highscores list 
        /// </summary>
        private List<Highscore> _highscores = new List<Highscore>();

        /// <summary>
        /// Menu default constructor
        /// </summary>
        public Menu()
        {
            // Setup
            _soundManager = new SoundManager();
            _soundManager.Load();

            _highscores =  GetHighscore();

            // Enter main menu
            MainMenu();
        }

        /// <summary>
        /// Check if the file path exists and return its content, else try to create it
        /// </summary>
        /// <returns>List of scores</returns>
        private List<Highscore> GetHighscore()
        {
            List <Highscore> highscores = new List<Highscore>();
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
                // If the path is incorect, warning and exit with empty highscore
                catch
                {
                    return new List<Highscore>();
                }
            }

            // Get txt file content as a string and then split it into lines 
            _content = File.ReadAllText(HIGHSCORE_PATH);
            List<string> lines = _content.Split('\n').ToList();

            // For each of the lines add the score to the _highscores
            foreach (string line in lines)
            {
                // Split username and score 
                string[] splited = line.Split(SEPARATOR);

                // Test that two elements are on the line
                if (splited.Length == 2) {

                    // Test if highscore has a valid value
                    if (Int32.TryParse(splited[INDEX_SCORE], out score))
                    {
                        Highscore highscore = new Highscore(score: score, username: splited[INDEX_USERNAME]);
                        highscores.Add(highscore);
                    }
                }
                
            }
            // If file found or created return highscores
            return highscores;
        }

        /// <summary>
        /// Write at given top coordinate and center horizontally
        /// </summary>
        /// <param name="toDisplay">String to display</param>
        /// <param name="top">Top position</param>
        public void WriteCenterHorizontal(string toDisplay, int top) {

            // Calculate maximum line size
            int maxLineSize = 0;
            string[] lines = toDisplay.Split('\n');
            foreach (string line in lines) {
                if (line.Length > maxLineSize) { 
                    maxLineSize = line.Length;
                }
            }

            // If console width is too small for string - display nothing
            if (maxLineSize > Console.WindowWidth) { return; }

            WriteAt(toDisplay: toDisplay, top: top, left: (Console.WindowWidth / 2) - (maxLineSize / 2));

        }

        /// <summary>
        /// Display multiple line string, newline align with precedent line
        /// </summary>
        /// <param name="toDisplay">Sting to display</param>
        /// <param name="top">Console vertical position</param>
        /// <param name="left">Console horizontal position</param>
        private void WriteAt(string toDisplay, int top, int left) {
            string[] lines = toDisplay.Split('\n');
            for (int i = 0; i < lines.Length; i++) { 
                Console.SetCursorPosition(top: top + i, left: left);
                Console.Write(lines[i]);
            }
        }

        /// <summary>
        /// Display main menu and take input
        /// </summary>
        private void MainMenu() {
            do
            {

                // Display title and content, align content on title size
                Console.Clear();
                WriteCenterHorizontal(toDisplay: MAIN_TITLE, top: MARGIN_TOP);
                WriteCenterHorizontal(toDisplay: MAIN_MENU, top: MARGIN_TOP * 2 + MAIN_TITLE.Split('\n').Length);

                // Take input without displaying it
                _input = Console.ReadKey(intercept:true).Key;

                // Switch input and start the designated method or exit
                switch (_input)
                {
                    case ConsoleKey.D1:
                        Game game = new Game(menu:this, easyMode:_easyMode, soundManager: _soundManager);                    
                        break;
                    case ConsoleKey.D2:
                        _soundManager.MenuSound();                      
                        OptionsMenu();
                        break;
                    case ConsoleKey.D3:
                        _soundManager.MenuSound();
                        HighscoreMenu();
                        break;
                    case ConsoleKey.D4:
                        _soundManager.MenuSound();
                        AboutMenu();
                        break;
                    case ConsoleKey.D5:
                        _soundManager.MenuSound();          
                        Environment.Exit(0);
                        break;
                    default:
                        break;

                }

            }  while (true); // keep looping if input didn't lead to other menu

        }

        /// <summary>
        /// Pause menu 
        /// </summary>
        public void Pause() {

            do
            {
                WriteCenterHorizontal(toDisplay: PAUSED, top: Console.WindowHeight/ RATIO_CONSOLE_HEIGHT);
                _input = Console.ReadKey(intercept: true).Key;

            } while (_input != ConsoleKey.R);
            _soundManager.MenuSound();

            // Remove pause box by displaying it in black
            Console.ForegroundColor = ConsoleColor.Black;
            WriteCenterHorizontal(toDisplay: PAUSED, top: Console.WindowHeight / RATIO_CONSOLE_HEIGHT);
            Console.ForegroundColor = ConsoleColor.White;

            // Countdown before restart
            Countdown();
        }

        /// <summary>
        /// Display options and take input
        /// </summary>
        private void OptionsMenu() {

            // Display menu
            Console.Clear();
            DisplayOptions();

            do
            {
                // Read input without displaying it
                _input = Console.ReadKey(intercept: true).Key;

                // Switch input, change options in accordance and display options
                switch (_input)
                {
                    case ConsoleKey.D1:
                        _soundManager.MenuSound();
                        _easyMode = !_easyMode;
                        Console.Clear();
                        DisplayOptions();
                        break;
                    case ConsoleKey.D2:
                        _soundManager.MenuSound();
                        _soundManager.SoundOn = !_soundManager.SoundOn;
                        Console.Clear();
                        DisplayOptions();
                        break;
                    case ConsoleKey.D3:
                        _soundManager.MenuSound();
                        return;
                    default:
                        break;
                }
            } while (true);
        }

        /// <summary>
        /// Sort score from highest to lowest and entries beyond MAX_SCORES
        /// </summary>
        private void SortScores() {
            _highscores = _highscores.OrderBy(x => x.Score).ToList();
            _highscores.Reverse();
            if (_highscores.Count > MAX_SCORES) { 
                _highscores.RemoveRange(MAX_SCORES, _highscores.Count - MAX_SCORES);
            }
        }

        /// <summary>
        /// Highscore menu
        /// </summary>
        private void HighscoreMenu() {

            Console.Clear();
            string toDisplay = "";

            // Display title
            WriteCenterHorizontal(toDisplay: HIGHSCORE_TITLE, top: MARGIN_TOP);

            // Sort scores and display them 
            SortScores();
            for(int i = 0; i< _highscores.Count; i++) {
                toDisplay += (i + 1+ ". " + _highscores[i].Score + "\t" + _highscores[i].Username + "\n");
            }
            WriteCenterHorizontal(toDisplay: toDisplay, top: MARGIN_TOP *2 + HIGHSCORE_TITLE.Split('\n').Length);

            // Display how to go back to main menu
            WriteCenterHorizontal(toDisplay: RETURN, top: MARGIN_TOP * 3 + HIGHSCORE_TITLE.Split('\n').Length + toDisplay.Split('\n').Length);

            Console.ReadKey(intercept:true);
            _soundManager.MenuSound();
        }

        /// <summary>
        /// Add a Highscore object in highscores
        /// </summary>
        /// <param name="score">Score</param>
        /// <param name="username">Username</param>
        public void AddToHighscore(int score, string username) {
            
            // Sort existant highscore and create a new highscore
            SortScores();
            Highscore highscore = new Highscore (score: score, username: username);

            _highscores.Add(highscore);
            
            // Update highscore to file
            UpdadeHighscoreFile();
        }

        /// <summary>
        /// Write highscores in text file
        /// </summary>
        private void UpdadeHighscoreFile()
        {
            string content = "";
            foreach (Highscore highscore in _highscores) {
                content += highscore.Username.ToString();
                content += SEPARATOR;
                content += highscore.Score.ToString();
                content += "\n";
            }
            File.WriteAllText(HIGHSCORE_PATH, content);
        }

        /// <summary>
        /// Display options menu
        /// </summary>
        private void DisplayOptions() {
            WriteCenterHorizontal(toDisplay: MAIN_TITLE, top: MARGIN_TOP);

            // Display line in options depending on corrent active value 
            if (_easyMode && _soundManager.SoundOn)
            {
                WriteCenterHorizontal(top: MARGIN_TOP * 2 + MAIN_TITLE.Split('\n').Length, toDisplay: EASY_MODE + SOUND_ON + BACK);
            }
            else if (_easyMode && !_soundManager.SoundOn)
            {
                WriteCenterHorizontal(top: MARGIN_TOP * 2 + MAIN_TITLE.Split('\n').Length, toDisplay: EASY_MODE + SOUND_OFF + BACK);
            }
            else if (!_easyMode && !_soundManager.SoundOn)
            {
                WriteCenterHorizontal(top: MARGIN_TOP * 2 + MAIN_TITLE.Split('\n').Length, toDisplay: HARD_MODE + SOUND_OFF + BACK);
            }
            else if (!_easyMode && _soundManager.SoundOn)
            {
                WriteCenterHorizontal(top: MARGIN_TOP * 2 + MAIN_TITLE.Split('\n').Length, toDisplay: HARD_MODE + SOUND_ON + BACK);
            }
        }

        /// <summary>
        /// About menu
        /// </summary>
        private void AboutMenu() {

            Console.Clear();

            // Display title
            WriteCenterHorizontal(toDisplay: ABOUT_TITLE, top: MARGIN_TOP * 1);

            // Display about section content
            WriteCenterHorizontal(toDisplay: ABOUT, top: MARGIN_TOP * 2 + ABOUT_TITLE.Split('\n').Length);

            // Display how to go back to main menu
            WriteCenterHorizontal(toDisplay: RETURN, top: MARGIN_TOP * 3 + ABOUT_TITLE.Split('\n').Length + ABOUT.Split('\n').Length);
            Console.ReadKey(intercept: true);
            _soundManager.MenuSound();
        }

        /// <summary>
        /// Game Over screen
        /// </summary>
        public void GameOver(int score) {

            Console.Clear();

            // Player name
            string username = "";

            // Display title
            WriteCenterHorizontal(toDisplay: GAME_OVER_TITLE, top: MARGIN_TOP * 1);

            // Delay before taking input to not take involuntary input
            Thread.Sleep(1000);

            // Flush keyboard buffer
            //https://stackoverflow.com/questions/978091/c-sharp-or-net-flushing-keyboard-buffer
            while (Console.KeyAvailable) { Console.ReadKey(intercept: true); }

            // Display prompt for input
            WriteCenterHorizontal(toDisplay: GAME_OVER_PROMPT, top: MARGIN_TOP * 2 + GAME_OVER_TITLE.Split('\n').Length);

            // Read input and add to highscores
            // Username is cut if longer that MAX_NAME_SIZE 
            username = Console.ReadLine();
            username = username.Trim();
            if (username.Length > MAX_NAME_SIZE)
            {
                username = username.Substring(0, MAX_NAME_SIZE);
            }
            AddToHighscore(score: score, username: username);
        }

        /// <summary>
        /// Display a countdown
        /// </summary>
        public void Countdown() {
            foreach (string number in NB_COUNTDOWN) {
                _soundManager.MenuSound();
                WriteCenterHorizontal(top: Console.WindowHeight/4, toDisplay: number);
                Thread.Sleep(1000);

                // Remove countdown by displaying it in black
                Console.ForegroundColor = ConsoleColor.Black;
                WriteCenterHorizontal(toDisplay: PAUSED, top: Console.WindowHeight / RATIO_CONSOLE_HEIGHT);
                Console.ForegroundColor = ConsoleColor.White;
            }
            _soundManager.MenuSound();
            
        }
    }
}
