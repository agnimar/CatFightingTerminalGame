// path/filename: ADS/Game.cs
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using static System.Collections.Specialized.BitVector32;

namespace ADS
{
    public class Game : UI
    {
        public void Launch()
        {
            while (isFighting && !paused)
            {
                DisplayGameScreen(0, ConsoleColor.White);
                HandlePlayerAction();
            }
            HandleEndGame();
        }

        

        private char HandleActionInput()
        {
            char choice = Console.ReadKey().KeyChar;
            while (!"123HQhq".Contains(choice))
            {
                Console.WriteLine("\n\t\tInvalid choice. Enter again: ");
                choice = Console.ReadKey().KeyChar;
            }
            return choice;
        }

        private void HandlePlayerAction()
        {
            char action = HandleActionInput();
            if (action == 'H' || action == 'h')
                Heal();
            else if (action == 'Q' || action == 'q')
                paused = true;
            else
                Attack(action);
        }

        private void Attack(char action)
        {
            int myAction = action - '0';
            int enemyAction = ComputerAction(difficulty, myAction);
            if (myAction == enemyAction)
                Tie(myAction);
            else if ((myAction == 1 && enemyAction == 3) || (myAction == 2 && enemyAction == 1) || (myAction == 3 && enemyAction == 2))
                Win(myAction);
            else
                Lose(enemyAction);
        }

        private void Tie(int action)
        {
            DisplayGameResult(action, ConsoleColor.Yellow,"\n\n\t\t\tBOTH CHOSE THE SAME ACTION.\t\t\t\t");
        }

        private void Win(int action)
        {
            int damage = CalculateDamage(1, 1);
            var actionMap = new Dictionary<int, (int, string, string)>
            {
                { 1, (7, "STANCE", "PAW ATTACK") },
                { 2, (4, "HISS", "STANCE") },
                { 3, (8, "PAW ATTACK", "HISS") }
            };
            HandleGameOutcome(action, damage, 1, actionMap, ConsoleColor.Green, "\t\t\tWOOHOO! DAMAGE DEALT TO ENEMY - ");
        }

        private void Lose(int action)
        {
            int damage = CalculateDamage(0, 1);
            var actionMap = new Dictionary<int, (int, string, string)>
            {
                { 2, (6, "STANCE", "HISS") },
                { 3, (9, "HISS", "PAW ATTACK") },
                { 1, (5, "PAW ATTACK", "STANCE") }
            };
            HandleGameOutcome(action, damage, 0, actionMap, ConsoleColor.Red, "\t\t\tOH NO :( ! DAMAGE RECEIVED - ");
        }

        private void DisplayGameResult(int action, ConsoleColor displayColor, string message)
        {
            DisplayGameScreen(action, displayColor);
            Console.BackgroundColor = displayColor;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(message);
            PauseGame();
        }

        private void HandleGameOutcome(int action, int damage, int winner, Dictionary<int, (int, string, string)> actionMap, ConsoleColor color, string message)
        {
            if (actionMap.TryGetValue(action, out var outcome))
            {
                DisplayGameScreen(outcome.Item1, color);
                Console.WriteLine($"{message}{damage}\t\t\t\t\t");
                PerformActions(outcome.Item2, outcome.Item3, winner);
            }
            HandleQueueChanges(action, winner);
            PauseGame();
            CheckCombo(winner == 1 ? myActions : enemyActions, winner, color);
        }

        private int CalculateDamage(int winner, int multiplier)
        {
            int damage = random.Next(5, 15) * multiplier;
            AdjustHealth(winner, damage);
            return damage;
        }

        private void AdjustHealth(int winner, int damage)
        {
            ref int health = ref (winner == 1 ? ref enemyHealth : ref myHealth);
            bool isEnemy = winner == 1;
            
            if(damage < health)
            {
                health -= damage; 
            }
            else
            {
                health = 0;
                victory = isEnemy;
                isFighting = false;
            }
        }

        private void Heal()
        {
            if (!iHealed)
            {
                PerformHealing();
            }
            else
            {
                DisplayHealLimitReached();
            }
        }

        private void PerformHealing()
        {
            int healing = random.Next(15, 30);
            myHealth += healing;
            DisplayHealingOutcome(10, ConsoleColor.Green, ConsoleColor.Cyan, $"\n\n\t\t\tYOU HEALED! +{healing} ");
            iHealed = true;
        }

        private void DisplayHealLimitReached()
        {
            DisplayHealingOutcome(0, ConsoleColor.White, ConsoleColor.Red, "\n\n\t\t\tCANNOT HEAL AGAIN :(");
        }

        private void DisplayHealingOutcome(int action, ConsoleColor displayColor, ConsoleColor foregroundColor, string message)
        {
            DisplayGameScreen(action, displayColor);
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(message);
            PauseGame();
        }

        private void HandleQueueChanges(int action, int winner)
        {
            Queue<int> relevantQueue = winner == 1 ? myActions : enemyActions;
            relevantQueue.Enqueue(action);
            if (relevantQueue.Count > 3)
                relevantQueue.Dequeue();
        }

        private void CheckCombo(Queue<int> actionQueue, int winner, ConsoleColor color)
        {
            if (actionQueue.Count < 3)
                return;

            string lastActions = string.Join("", actionQueue.ToArray());
            if (lastActions.EndsWith("111") || lastActions.EndsWith("222") || lastActions.EndsWith("333") || lastActions.EndsWith("123"))
            {
                actionQueue.Clear();
                PerformCombo(lastActions.Substring(lastActions.Length - 3), winner, color);
            }
        }

        private void PerformCombo(string combo, int winner, ConsoleColor color)
        {
            int damage = CalculateDamage(winner, 2);
            int displayIndex = 0;
            switch (combo)
            {
                case "111":
                    displayIndex = 13 - winner;
                    break;
                case "222":
                    displayIndex = 15 - winner;
                    break;
                case "333":
                    displayIndex = 17 - winner;
                    break;
                default:
                    displayIndex = 19 - winner;
                    break;
            }
            DisplayGameScreen(displayIndex, color);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\t\t_________________________________________________________");
            Console.WriteLine("\t\t|\t\t\t\t\t\t\t|");
            Console.WriteLine($"\t\t|   !!!COMBO!!! DAMAGE DEALT - {damage}\t\t\t\t\t|\t\t\t");
            Console.WriteLine("\t\t|_______________________________________________________|");
            PauseGame();
        }

        private int ComputerAction(int difficulty, int myAction)
        {
            if (enemyHealth <= 40 && !enemyHealed)
                ComputerHeal();
            return SmartAction(myAction, difficulty + 2);
        }

        private void ComputerHeal()
        {
            int healing = random.Next(15, 30);
            enemyHealth += healing;
            DisplayGameScreen(11, ConsoleColor.Magenta);
            Console.WriteLine($"\n\n\t\tENEMY HEALED! +{healing} ");
            enemyHealed = true;
            PauseGame();
        }

        private int SmartAction(int myAction, int difficulty)
        {
            int action = random.Next(0, difficulty);
            if (action == difficulty - 1)
                return myAction;
            else if (action == difficulty - 2)
                return (myAction % 3) + 1;
            else
                return ((myAction + 1) % 3) + 1;
        }
        private void HandleEndGame()
        {
            if (!isFighting)
            {
                if (victory)
                    DisplayEndScreen("\tENEMY DEFEATED! YOUR FISH IS SAFE :3\t", ConsoleColor.Green);
                else
                    DisplayEndScreen(" ENEMY DEFEATED YOU :( SADLY, YOUR FISH WAS STOLEN ", ConsoleColor.Red);
            }
        }
    }
}