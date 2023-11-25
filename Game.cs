// path/filename: ADS/Game.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace ADS
{
    public class Game : UI
    {
        public void Launch()
        {
            while (isFighting && !paused)
            {
                DisplayGameScreen(0, ConsoleColor.White);
                MyAction();
            }
            if (!isFighting && victory)
                DisplayEndScreen("\tENEMY DEFEATED! YOUR FISH IS SAFE :3\t", ConsoleColor.Green);
            else if (!isFighting && !victory)
                DisplayEndScreen(" ENEMY DEFEATED YOU :( SADLY, YOUR FISH WAS STOLEN ", ConsoleColor.Red);
            else
                Console.WriteLine("");
        }

        public char ActionInput()
        {
            char choice = Console.ReadKey().KeyChar;
            while (!"123HQhq".Contains(choice))
            {
                Console.WriteLine("\n\t\tInvalid choice. Enter again: ");
                choice = Console.ReadKey().KeyChar;
            }
            return choice;
        }

        public void MyAction()
        {
            char action = ActionInput();
            if (action == 'H' || action == 'h')
                Heal();
            else if (action == 'Q' || action == 'q')
                paused = true;
            else
                Attack(action);
        }

        public void Attack(char action)
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

        public void Tie(int action)
        {
            DisplayGameScreen(action, ConsoleColor.Yellow);
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("\n\n\t\t\tBOTH CHOSE THE SAME ACTION.\t\t\t\t");
            PauseGame();
        }

        public void Win(int action)
        {
            int damage = CalculateDamage(1, 1);
            switch (action)
            {
                case 1:
                    DisplayGameScreen(7, ConsoleColor.Green);
                    PerformActions("STANCE", "SHIELD ATTACK", 1);
                    break;
                case 2:
                    DisplayGameScreen(4, ConsoleColor.Green);
                    PerformActions("SNEAK", "STANCE", 1);
                    break;
                default:
                    DisplayGameScreen(8, ConsoleColor.Green);
                    PerformActions("SHIELD ATTACK", "SNEAK", 1);
                    break;
            }
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"\t\t\tWOOHOO! DAMAGE DEALT TO ENEMY - {damage}\t\t\t\t\t");
            PauseGame();
            QueueActions(action, 1);
            CheckCombo(myActions, 1, ConsoleColor.Green);
        }

        public void Lose(int action)
        {
            int damage = CalculateDamage(0, 1);
            switch (action)
            {
                case 1:
                    DisplayGameScreen(5, ConsoleColor.Red);
                    PerformActions("STANCE", "SNEAK", 0);
                    break;
                case 2:
                    DisplayGameScreen(6, ConsoleColor.Red);
                    PerformActions("SNEAK", "SHIELD ATTACK", 0);
                    break;
                default:
                    DisplayGameScreen(9, ConsoleColor.Red);
                    PerformActions("SHIELD ATTACK", "STANCE", 0);
                    break;
            }
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"\t\t\tOH NO :( ! DAMAGE RECEIVED - {damage}\t\t\t\t");
            PauseGame();
            QueueActions(action, 0);
            CheckCombo(enemyActions, 0, ConsoleColor.Red);
        }

        public int CalculateDamage(int winner, int multiplier)
        {
            int damage = random.Next(5, 15) * multiplier;
            switch (winner)
            {
                case 1:
                    if (damage < enemyHealth)
                        enemyHealth -= damage;
                    else
                    {
                        enemyHealth = 0;
                        victory = true;
                        isFighting = false;
                    }
                    break;
                default:
                    if (damage < myHealth)
                        myHealth -= damage;
                    else
                    {
                        myHealth = 0;
                        victory = false;
                        isFighting = false;
                    }
                    break;
            }
            return damage;
        }

        public void Heal()
        {
            if (!iHealed)
            {
                int healing = random.Next(15, 30);
                myHealth += healing;
                DisplayGameScreen(10, ConsoleColor.Green);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n\n\t\t\tYOU HEALED! +{healing} ");
                iHealed = true;
                PauseGame();
            }
            else
            {
                DisplayGameScreen(0, ConsoleColor.White);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\n\t\t\tCANNOT HEAL AGAIN :(");
                PauseGame();
            }
        }

        public void QueueActions(int action, int winner)
        {
            Queue<int> relevantQueue = winner == 1 ? myActions : enemyActions;
            relevantQueue.Enqueue(action);
            if (relevantQueue.Count > 3)
                relevantQueue = new Queue<int>(relevantQueue.Skip(1));
        }

        public void CheckCombo(Queue<int> actionQueue, int winner, ConsoleColor color)
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

        public void PerformCombo(string combo, int winner, ConsoleColor color)
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


        public int ComputerAction(int difficulty, int myAction)
        {
            if (enemyHealth <= 40 && !enemyHealed)
                ComputerHeal();
            return SmartAction(myAction, difficulty + 2);
        }

        public void ComputerHeal()
        {
            int healing = random.Next(15, 30);
            enemyHealth += healing;
            DisplayGameScreen(11, ConsoleColor.Magenta);
            Console.WriteLine($"\n\n\t\tENEMY HEALED! +{healing} ");
            enemyHealed = true;
            PauseGame();
        }

        public int SmartAction(int myAction, int difficulty)
        {
            int action = random.Next(0, difficulty);
            if (action == difficulty - 1)
                return myAction;
            else if (action == difficulty - 2)
                return (myAction % 3) + 1;
            else
                return ((myAction + 1) % 3) + 1;
        }
    }
}
