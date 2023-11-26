// path/filename: ADS/UI.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ADS
{
    public class UI
    {
        protected string[] actionDisplay = new string[23];
        protected string initialText, rules, farewell;
        protected int myHealth, enemyHealth;
        protected bool iHealed, enemyHealed;
        protected bool isFighting, victory, paused;
        protected Queue<int> myActions = new Queue<int>();
        protected Queue<int> enemyActions = new Queue<int>();
        protected Random random = new Random();
        protected int difficulty;

        public void Start()
        {
            myHealth = enemyHealth = 100;
            iHealed = enemyHealed = false;
            isFighting = true;
            paused = false;
        }

        public void ReadFiles()
        {
            var currentDirectory = Environment.CurrentDirectory;
            actionDisplay = ReadActionDisplay(Path.Combine(currentDirectory, "imageFiles.txt"));
            initialText = File.ReadAllText(Path.Combine(currentDirectory, "pradinisTekstas.txt"));
            rules = File.ReadAllText(Path.Combine(currentDirectory, "taisykles.txt"));
            farewell = File.ReadAllText(Path.Combine(currentDirectory, "atsisveikinimas.txt"));
        }

        private string[] ReadActionDisplay(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            string[] display = new string[22];
            for (int i = 0; i < display.Length; i++)
            {
                display[i] = String.Join("\n\t\t", lines, i * 9, Math.Min(9, lines.Length - i * 9));
            }
            return display;
        }

        public char DisplayInitialScreen()
        {
            SetupConsole(ConsoleColor.Blue);
            Console.WriteLine(initialText);
            return CheckInput("start");
        }

        public void ChooseDifficulty()
        {
            SetupConsole(ConsoleColor.White);
            DisplayDifficultyOptions();
            difficulty = CheckInput("difficulty") - '0';
        }
        
        private void DisplayDifficultyOptions()
        {
            string[] difficultyOptions = { "Lengvas", "Normalus", "Sunkus", "Neįveikiamas" };
            ConsoleColor[] colors = { ConsoleColor.Green, ConsoleColor.Yellow, ConsoleColor.Red, ConsoleColor.Magenta };

            Console.WriteLine("\t\t_________________________________________");
            Console.WriteLine("\t\t|\t\t\t\t\t|");
            Console.WriteLine("\t\t|\t Pasirinkite sudėtingumą \t|");
            Console.WriteLine("\t\t|---------------------------------------|");
            Console.WriteLine("\t\t|\t\t\t\t\t|");

            for (int i = 0; i < difficultyOptions.Length; i++)
            {
                Console.ForegroundColor = colors[i];
                Console.WriteLine($"\t\t|\t[ {i + 1} ] - {difficultyOptions[i]}\t\t|");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\t\t|_______________________________________|");
        }

        public void DisplayRules()
        {
            SetupConsole(ConsoleColor.Yellow);
            Console.WriteLine(rules);
            Console.ReadKey();
        }

        public void DisplayGameScreen(int index, ConsoleColor color)
        {
            SetupConsole(ConsoleColor.Magenta);
            Console.WriteLine("\t[Q] Exit to game menu");
            Console.WriteLine("\t_________________________________________________________________________");
            Console.WriteLine($"\t|   ENEMY HEALTH: {enemyHealth}\t\t\t\t\t\t\t|");
            Console.WriteLine("\t|_______________________________________________________________________|");

            Console.ForegroundColor = color;
            Console.WriteLine("\n\n{0}\n", actionDisplay[index]);

            Console.ForegroundColor = ConsoleColor.Cyan;
            DisplayPlayerOptions();
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void DisplayPlayerOptions()
        {
            Console.WriteLine("\t_________________________________________________________________________");
            Console.WriteLine($"\t|   MY HEALTH: {myHealth}\t\t\t\t\t\t\t|");
            Console.WriteLine("\t|   [1] - STANCE    [2] - HISS    [3] - PAW ATTACK                  |");
            Console.WriteLine($"\t|   [H] - HEAL({(iHealed ? 0 : 1)})                                             |");
            Console.WriteLine("\t|-----------------------------------------------------------------------|");
            Console.WriteLine("\t|  POSSIBLE COMBINATIONS:                                              |");
            Console.WriteLine("\t|  [1][1][1]  ->  LION'S POSE                                          |");
            Console.WriteLine("\t|  [2][2][2]  ->  DRAGON'S FIRE                                        |");
            Console.WriteLine("\t|  [3][3][3]  ->  IRON SHIELD                                          |");
            Console.WriteLine("\t|  [1][2][3]  ->  ULTIMATE FORM                                        |");
            Console.WriteLine("\t|_______________________________________________________________________|");
        }

        private char CheckInput(string context)
        {
            char input = Console.ReadKey().KeyChar;
            while (!IsValidInput(context, input))
            {
                Console.WriteLine("\nNetinkamas pasirinkimas. Iveskite is naujo: ");
                input = Console.ReadKey().KeyChar;
            }
            return input;
        }

        private bool IsValidInput(string context, char input)
        {
            switch (context)
            {
                case "start":
                    return input == '1' || input == '0';
                case "difficulty":
                    return input >= '1' && input <= '4';
                default:
                    return false;
            }
        }


        public void PerformActions(string myAction, string enemyAction, int winner)
        {
            ConsoleColor myColor = winner == 1 ? ConsoleColor.Green : ConsoleColor.Red;
            ConsoleColor enemyColor = winner == 1 ? ConsoleColor.Red : ConsoleColor.Green;

            Console.ForegroundColor = myColor;
            Console.WriteLine($"\n\t\t\tMY ACTION - {myAction}");
            Console.ForegroundColor = enemyColor;
            Console.WriteLine($"\t\t\tENEMY ACTION - {enemyAction}");
            Console.ResetColor();
        }

        public void DisplayEndScreen(string text, ConsoleColor color)
        {
            SetupConsole(color);
            Console.WriteLine("\n\t\t_________________________________________________________");
            Console.WriteLine("\t\t|\t\t\t\t\t\t\t|");
            Console.WriteLine($"\t\t| {text} |");
            Console.WriteLine("\t\t|_______________________________________________________|\n");
            PauseGame();
        }

        public void SayGoodbye()
        {
            SetupConsole(ConsoleColor.Green);
            Console.WriteLine(farewell);
            Console.ReadKey();
        }

        public void PauseGame()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("\n\nPress any key to continue: ");
            Console.ReadKey();
        }

        public void ClearActionQueues()
        {
            myActions.Clear();
            enemyActions.Clear();
        }

        private void SetupConsole(ConsoleColor color)
        {
            Console.Clear();
            Console.ForegroundColor = color;
        }
    }
}
