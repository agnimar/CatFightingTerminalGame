using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ADS
{
    public class UI
    {
        protected string[] veiksmoVaizdas = new string[23];
        protected string pradinisTekstas, taisykles, atsisveikinimas;
        protected int manoGyvybes, priesoGyvybes;
        protected bool asPasigydziau, priesasPasigyde;
        protected bool vykstaMustynes, pergale, sustabdyta;
        protected Queue<int> manoVeiksmai = new Queue<int>();
        protected Queue<int> priesoVeiksmai = new Queue<int>();
        protected Random random = new Random();
        protected int sudetingumas;
        public void Pradzia()
        {
            manoGyvybes = 100;
            priesoGyvybes = 100;
            asPasigydziau = false;
            priesasPasigyde = false;
            vykstaMustynes = true;
            sustabdyta = false;
        }
        public void Nuskaitymas()
        {
            var CurrentDirectory = Environment.CurrentDirectory;
            string filePath0 = Path.Combine(CurrentDirectory, @"imageFiles.txt");
            string filePath1 = Path.Combine(CurrentDirectory, @"pradinisTekstas.txt");
            string filePath2 = Path.Combine(CurrentDirectory, @"taisykles.txt");
            string filePath3 = Path.Combine(CurrentDirectory, @"atsisveikinimas.txt");
            List<string> lines = new List<string>();
            lines = File.ReadAllLines(filePath0).ToList();
            for (int i = 0; i < 22; i++)
            {
                int situation = i * 9;
                for (int j = situation; j < situation + 9; j++)
                    veiksmoVaizdas[i] += "\t\t" + lines[j] + '\n';
            }
            lines = File.ReadAllLines(filePath1).ToList();
            foreach(string line in lines)
            {
                pradinisTekstas += line + '\n';
            }
            lines = File.ReadAllLines(filePath2).ToList();
            foreach (string line in lines)
            {
                taisykles += line + '\n';
            }
            lines = File.ReadAllLines(filePath3).ToList();
            foreach (string line in lines)
            {
                atsisveikinimas += line + '\n';
            }
        }
        public char PradinisEkranas()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(pradinisTekstas);
            return Tikrinimas("pradzia");
        }
        public void SudetingumoPasirinkimas()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\t\t_________________________________________");
            Console.WriteLine("\t\t|\t\t\t\t\t|");
            Console.WriteLine("\t\t|\t Pasirinkite sudėtingumą \t|");
            Console.WriteLine("\t\t|---------------------------------------|");
            Console.WriteLine("\t\t|\t\t\t\t\t|");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\t\t|\t[ 1 ] - Lengvas\t\t\t|");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t\t|\t[ 2 ] - Normalus\t\t|");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\t\t|\t[ 3 ] - Sunkus\t\t\t|");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\t\t|\t[ 4 ] - Neiveikiamas\t\t|");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\t\t|_______________________________________|");
            sudetingumas = Tikrinimas("sudetingumas") - '0';
        }
        public void Taisykles()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(taisykles);
            Console.ReadKey();
        }
        public void ZaidimoEkranas(int x, char spalva)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\t[Q] Išeiti į žaidimo meniu");
            Console.WriteLine("\t_________________________________________________________________________");
            Console.WriteLine("\t|                                                                       |");
            Console.WriteLine("\t|   PRIEŠO GYVYBĖS: {0}\t\t\t\t\t\t\t|", priesoGyvybes);
            Console.WriteLine("\t|_______________________________________________________________________|");
            switch (spalva)
            {
                case 'W':
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case 'Y':
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 'R':
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case 'G':
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
            }
            Console.WriteLine("\n\n{0}\n", veiksmoVaizdas[x]);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\t_________________________________________________________________________");
            Console.WriteLine("\t|                                                                       |");
            Console.WriteLine("\t|   MANO GYVYBĖS: {0}\t\t\t\t\t\t\t|", manoGyvybes);
            Console.WriteLine("\t|   [1] - STOVĖSENA    [2] - ŠNYPŠTIMAS    [3] - LETENĖLĖS ATAKA        |");
            int temp;
            if (asPasigydziau)
                temp = 0;
            else temp = 1;
            Console.WriteLine("\t|   [H] - PASIGYDYTI({0})                                                 |", temp);
            Console.WriteLine("\t|-----------------------------------------------------------------------|");
            Console.WriteLine("\t|  GALIMOS KOMBINACIJOS:                                                |");
            Console.WriteLine("\t|  [1][1][1]  ->  LIŪTO POZA                                            |");
            Console.WriteLine("\t|  [2][2][2]  ->  DRAKONO UGNIS                                         |");
            Console.WriteLine("\t|  [3][3][3]  ->  GELEŽINĖ LETENĖLĖ                                     |");
            Console.WriteLine("\t|  [1][2][3]  ->  ULTIMATE FORMA                                        |");
            Console.WriteLine("\t|_______________________________________________________________________|");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public char Tikrinimas(string x)
        {
            char pasirinkimas = Console.ReadKey().KeyChar;
            switch (x)
            {
                case "pradzia":
                    while (pasirinkimas != '1' && pasirinkimas != '0')
                    {
                        Console.WriteLine("\nNetinkamas pasirinkimas. Iveskite is naujo: ");
                        pasirinkimas = Console.ReadKey().KeyChar;
                    }
                    break;
                default:
                    while (pasirinkimas != '1' && pasirinkimas != '2' && pasirinkimas != '3' && pasirinkimas != '4')
                    {
                        Console.WriteLine("\nNetinkamas pasirinkimas. Iveskite is naujo: ");
                        pasirinkimas = Console.ReadKey().KeyChar;
                    }
                    break;
            }
            return pasirinkimas;
        }
        public void VeiksmuIsvedimas(string manoVeiksmas, string priesoVeiksmas, int kasLaimejo)
        {
            if (kasLaimejo == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\t\t\tMANO VEIKSMAS - {0}", manoVeiksmas);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t\t\tPRIEŠO VEIKSMAS - {0}", priesoVeiksmas);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\t\t\tMANO VEIKSMAS - {0}", manoVeiksmas);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\t\t\tPRIEŠO VEIKSMAS - {0}", priesoVeiksmas);
            }
        }
        public void PabaigosEkranas(string tekstas, char spalva)
        {
            switch (spalva)
            {
                case 'G':
                    ZaidimoEkranas(20, 'G');
                    Console.BackgroundColor = ConsoleColor.Green;
                    break;
                default:
                    ZaidimoEkranas(21, 'R');
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;
            }
            
            Console.WriteLine("\n\t\t_________________________________________________________");
            Console.WriteLine("\t\t|\t\t\t\t\t\t\t|");
            Console.WriteLine("\t\t|{0}|", tekstas);
            Console.WriteLine("\t\t|_______________________________________________________|\n");
            Console.BackgroundColor = ConsoleColor.Black;
            Sustabdymas();
        }
        public void Atsisveikinimas()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(atsisveikinimas);
            Console.ReadKey();
        }
        public void Sustabdymas()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("\n\nPaspauskite bet ką, kad tęsti: ");
            Console.ReadKey();
        }
        public void Istrinimas()
        {
            manoVeiksmai.Clear();
            priesoVeiksmai.Clear();
        }
    }
}

