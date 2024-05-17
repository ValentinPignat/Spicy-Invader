/// ETML
/// Author: Valentin Pignat 
/// Date (creation): 18.01.2024
/// Description: Main Program for the Spicy Invader Game
///     - Setup console and enter menus

/// TODO :
///     DISABLE SCROLL AND CLICK
///     replace playceholser
///     
/// Missing/Incomplete feature:
///     - A propos
///     -Test unitaires 


using System;
using System.Runtime.InteropServices;

// IsKeyDown() used to handle multiple input at each cycle 
// References: WindowsBase and PresentationCore
// https://learn.microsoft.com/en-us/dotnet/api/system.windows.input.keyboard.iskeydown?view=windowsdesktop-8.0
// https://learn.microsoft.com/en-us/dotnet/api/system.windows.input.key?view=windowsdesktop-8.0
// https://stackoverflow.com/questions/6373645/c-sharp-winforms-how-to-set-main-function-stathreadattribute

namespace Spicy_Invader
{
    internal class Program
    {

        // https://stackoverflow.com/questions/38426338/c-sharp-console-disable-resize
        // Used for the DisableResize() Method
        private const int MF_BYCOMMAND = 0x00000000; 
        public const int SC_CLOSE = 0xF060; 
        public const int SC_MINIMIZE = 0xF020; 
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;//resize
        [DllImport("user32.dll")] public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32.dll")] private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("kernel32.dll", ExactSpelling = true)] private static extern IntPtr GetConsoleWindow();


        // Allows Keyboard.IsKeyDown
        [STAThread]
        static void Main(string[] args)
        {
            ConsoleSetup();

            // Enter menus
            Menu menu = new Menu();
        }


        /// <summary>
        /// Set up console parameters (Dimension, Cursor, Scrolling, Resize)
        /// </summary>
        static void ConsoleSetup()
        {
            // Set console size depending on map size 
            Console.WindowWidth = Game.WIDTH + Game.WIDTH_CONSOLE_MARGIN;
            Console.WindowHeight = Game.HEIGHT + Game.HEIGHT_CONSOLE_MARGIN;

            // Hides cursor
            Console.CursorVisible = false;

            // Disable scrolling 
            // https://stackoverflow.com/questions/19568127/hide-scrollbars-in-console-without-flickering
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);

            // Disable Resize
            DisableResize();
        }

        /// <summary>
        /// Disable resizing, minimizing and maximizing
        /// https://stackoverflow.com/questions/38426338/c-sharp-console-disable-resize
        /// </summary>
        static void DisableResize() {
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                // DeleteMenu(sysMenu, SC_CLOSE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);//resize
            }
        }
    }
}
