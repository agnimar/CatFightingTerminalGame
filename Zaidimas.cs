using System;
using System.Collections.Generic;
using System.Linq;

namespace ADS
{
    public class Zaidimas : UI
    {
        public void Paleidimas()
        {
            while(vykstaMustynes && !sustabdyta)
            {
                ZaidimoEkranas(0, 'W');
                ManoVeiksmas();
            }
            if (!vykstaMustynes && pergale)
                PabaigosEkranas("\tPRIEŠAS BUVO NUGALĖTAS! JŪSŲ ŽUVIS SAUGI :3\t", 'G');
            else if(!vykstaMustynes && !pergale)
                PabaigosEkranas(" PRIEŠAS JUS NUGALĖJO :( DEJA, JŪSŲ ŽUVIS BUVO PAVOGTA ", 'R');
            else Console.WriteLine("");
        }
        public char VeiksmoIvestis()
        {
            char pasirinkimas = Console.ReadKey().KeyChar;
            while (pasirinkimas != '1' && pasirinkimas != '2' && pasirinkimas != '3' && pasirinkimas != 'H' && pasirinkimas != 'Q' && pasirinkimas != 'q' && pasirinkimas != 'h')
            {
                Console.WriteLine("\n\t\tNetinkamas pasirinkimas. Iveskite is naujo: ");
                pasirinkimas = Console.ReadKey().KeyChar;
            }
            return pasirinkimas;
        }
        public void ManoVeiksmas()
        {
            char veiksmas = VeiksmoIvestis();
            if (veiksmas == 'H' || veiksmas == 'h')
                Pasigydimas();
            else if (veiksmas == 'Q' || veiksmas == 'q')
                sustabdyta = true;
            else
                Ataka(veiksmas);
        }
        public void Ataka(char veiksmas)
        {
            int manoV = veiksmas - '0';
            int kompoV = KompiuterioVeiksmas(sudetingumas, manoV);
            if(manoV == kompoV)
                Lygiosios(manoV);
            else if((manoV == 1 && kompoV == 3)|| (manoV == 2 && kompoV == 1)|| (manoV == 3 && kompoV == 2))
                Laimejimas(manoV);
            else
                Pralaimejimas(kompoV);
        }
        public void Lygiosios(int x)
        {
            ZaidimoEkranas(x, 'Y');
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("\n\n\t\t\tABU PASIRINKOTE TĄ PATĮ VEIKSMĄ.\t\t\t\t");
            Sustabdymas();
        }
        public void Laimejimas(int x)
        {
            int zala = Zala(1, 1);
            switch (x)
            {
                case 1:
                    ZaidimoEkranas(7, 'G');
                    VeiksmuIsvedimas("STOVĖSENA", "LETENĖLĖS ATAKA", 1);
                    break;
                case 2:
                    ZaidimoEkranas(4, 'G');
                    VeiksmuIsvedimas("ŠNYPŠTIMAS", "STOVĖSENA", 1);
                    break;
                default:
                    ZaidimoEkranas(8, 'G');
                    VeiksmuIsvedimas("LETENĖLĖS ATAKA", "ŠNYPŠTIMAS", 1);
                    break;
            }
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("\t\t\tWOOHOO! PADARYTA ŽALA PRIEŠUI - {0}\t\t\t\t\t", zala);
            Sustabdymas();
            VeiksmaiSuEile(x, 1);
            ComboTikrinimas(manoVeiksmai, 1, 'G');
        }
        public void Pralaimejimas(int x)
        {
            int zala = Zala(0, 1);
            switch (x)
            {
                case 1:
                    ZaidimoEkranas(5, 'R');
                    VeiksmuIsvedimas("STOVĖSENA", "ŠNYPŠTIMAS", 0);
                    break;
                case 2:
                    ZaidimoEkranas(6, 'R');
                    VeiksmuIsvedimas("ŠNYPŠTIMAS", "LETENĖLĖS ATAKA", 0);
                    break;
                default:
                    ZaidimoEkranas(9, 'R');
                    VeiksmuIsvedimas("LETENĖLĖS ATAKA", "STOVĖSENA", 0);
                    break;
            }
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("\t\t\tO NE :( ! JUMS PADARYTA ŽALA - {0}\t\t\t\t", zala);
            Sustabdymas();
            VeiksmaiSuEile(x, 0);
            ComboTikrinimas(priesoVeiksmai, 0, 'R');
        }
        public int Zala(int kasLaimejo, int x)
        {
            int padarytaZala = random.Next(5,15) * x;
            switch (kasLaimejo)
            {
                case 1:
                    if(padarytaZala < priesoGyvybes)
                        priesoGyvybes -= padarytaZala;
                    else
                    {
                        priesoGyvybes = 0;
                        pergale = true;
                        vykstaMustynes = false;
                    }
                    break;
                default:
                    if (padarytaZala < manoGyvybes)
                        manoGyvybes -= padarytaZala;
                    else
                    {
                        manoGyvybes = 0;
                        pergale = false;
                        vykstaMustynes = false;
                    }
                    break;
            }
            return padarytaZala;
        }
        public void Pasigydimas()
        {
            if (!asPasigydziau)
            {
                int pasigydymas = random.Next(15, 30);
                manoGyvybes += pasigydymas;
                ZaidimoEkranas(10, 'G');
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n\n\t\t\tJŪS PASIGYDĖTE! +{0} ", pasigydymas);
                asPasigydziau = true;
                Sustabdymas();
            }
            else
            {
                ZaidimoEkranas(0, 'W');
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\n\t\t\tNEBEGALITE PASIGYDYTI :(");
                Sustabdymas();
            }
        }
        //Veiksmai su eilės duomenų struktūra
        public void VeiksmaiSuEile(int x, int kasLaimejo)
        {
            switch (kasLaimejo)
            {
                case 1:
                    manoVeiksmai.Enqueue(x);
                    if (manoVeiksmai.Count > 3)
                        manoVeiksmai = new Queue<int>(manoVeiksmai.Where((value, index) => index > 0));
                    break;
                default:
                    priesoVeiksmai.Enqueue(x);
                    if (priesoVeiksmai.Count > 3)
                        priesoVeiksmai = new Queue<int>(priesoVeiksmai.Where((value, index) => index > 0));
                    break;
            }
        }
        public void ComboTikrinimas(Queue<int> eile, int kasLaimejo, char spalva)
        {
            string paskutiniaiVeiksmai = "";
            foreach(int n in eile)
            {
                paskutiniaiVeiksmai += n;
            }
            if(paskutiniaiVeiksmai == "111" || paskutiniaiVeiksmai == "222" || paskutiniaiVeiksmai == "333" || paskutiniaiVeiksmai == "123")
            {
                eile.Clear();
                ComboAtlikimas(paskutiniaiVeiksmai, kasLaimejo, spalva);
            }
        }
        public void ComboAtlikimas(string combo, int i, char spalva)
        {
            int zala = Zala(i, 2);
            switch (combo)
            {
                case "111":
                    ZaidimoEkranas(13 - i, spalva);
                    break;
                case "222":
                    ZaidimoEkranas(15 - i, spalva);
                    break;
                case "333":
                    ZaidimoEkranas(17 - i, spalva);
                    break;
                default:
                    ZaidimoEkranas(19 - i, spalva);
                    break;
            }
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\t\t_________________________________________________________");
            Console.WriteLine("\t\t|\t\t\t\t\t\t\t|");
            Console.WriteLine("\t\t|   !!!CCCCCCCCCOOOOOOOOOOOOMMMMMMMMBOOOOOOOOOOOOO!!!   |\n\t\t|   PADARYTA ZALA - {0}\t\t\t\t\t|\t\t\t", zala);
            Console.WriteLine("\t\t|_______________________________________________________|");
            Sustabdymas();
        }
        //Kompiuterio AI
        public int KompiuterioVeiksmas(int sunkumas, int manoVeiksmas)
        {
            if (priesoGyvybes <= 40 && !priesasPasigyde)
                KompiuterisPasigydo();
            return ProtingasVeiksmas(manoVeiksmas, sunkumas + 2);
        }
        public void KompiuterisPasigydo()
        {
            int pasigydymas = random.Next(15, 30);
            priesoGyvybes += pasigydymas;
            ZaidimoEkranas(11, 'M');
            Console.WriteLine("\n\n\t\tPRIEŠAS PASIGYDĖ! +{0} ", pasigydymas);
            priesasPasigyde = true;
            Sustabdymas();
        }
        public int ProtingasVeiksmas(int manoV, int sunkumas)
        {
            int x = random.Next(0, sunkumas);
            switch (manoV)
            {
                case 1:
                    if (x == sunkumas-1)
                        return 1;
                    if (x == sunkumas - 2)
                        return 3;
                    else return 2;
                case 2:
                    if (x == sunkumas - 1)
                        return 2;
                    if (x == sunkumas - 2)
                        return 1;
                    else return 3; 
                default:
                    if (x == sunkumas - 1)
                        return 3;
                    if (x == sunkumas - 2)
                        return 2;
                    else return 1;
            }
        }
    }
}
