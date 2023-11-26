// path/filename: ADS/Program.cs
using System;
using static ADS.Game;

namespace ADS
{
    class Program
    {
        static void Main(string[] args)
        {
            int choice;
            Game game = new Game();
            game.ReadFiles();

            while (true)
            {
                game.Start();
                choice = game.DisplayInitialScreen() - '0';
                if (choice == 1)
                {
                    game.ChooseDifficulty();
                    game.DisplayRules();
                    game.Launch();
                    game.ClearActionQueues();
                }
                else
                {
                    game.SayGoodbye();
                    break;
                }
            }
            return;
        }
    }
}
