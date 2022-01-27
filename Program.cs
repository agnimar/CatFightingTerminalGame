namespace ADS
{
    class Program
    {
        static void Main(string[] args)
        {
            int pasirinkimas;
            Zaidimas zaidimas = new Zaidimas();
            zaidimas.Nuskaitymas(); 
            while (true)
            {
                zaidimas.Pradzia();
                pasirinkimas = zaidimas.PradinisEkranas() - '0';
                if (pasirinkimas == 1)
                {
                    zaidimas.SudetingumoPasirinkimas();
                    zaidimas.Taisykles();
                    zaidimas.Paleidimas();
                    zaidimas.Istrinimas();
                }
                else
                {
                    zaidimas.Atsisveikinimas();
                    break;
                }
            }
            return;
        }
    }
}
