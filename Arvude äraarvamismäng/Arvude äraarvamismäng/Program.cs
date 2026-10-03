using System.Linq.Expressions;

namespace Arvude_äraarvamismäng
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Loob suvalise numbri ühe ja saja vahel ning küsib kasutajalt numbrit
            Random number = new Random();
            int suvalinenr = number.Next(1, 101);
            int pakkumine = 0;
            Console.WriteLine("Paku täisarv ühest sajani: ");

            while (pakkumine != suvalinenr)
            {
                

                //Kasutaja sisendi kontroll
                while (true)
                {
                    //Kasutaja pakkumine
                    string p = Console.ReadLine();

                    //Kontrollib kasutaja sisendit ja proovib seda integeriks konverteerida
                    bool onnumber = Int32.TryParse(p, out pakkumine); //Konverteerimine

                    if (!onnumber)
                    {
                        //Juhul kui pakkumine on kirjutatud sõnadega või komaga
                        Console.WriteLine("Antud valik ei sobi!");
                        Console.WriteLine("Paku uuesti: ");

                    }
                    else if (pakkumine < 1 || pakkumine > 100)
                    {
                        //Juhul kui pakkumine ei ole sobivas vahemikus
                        Console.WriteLine("Antud valik ei sobi!");
                        Console.WriteLine("Paku uuesti: ");

                    }
                    else
                    {
                        break;
                    }
                }

                //Äraarvamise osa
                while (pakkumine != suvalinenr)
                {
                    if (pakkumine < suvalinenr)
                    {
                        Console.WriteLine("Pakutud arv on liiga väike!");
                        Console.WriteLine("Paku uuesti: ");
                        break;
                    }
                    else if (pakkumine > suvalinenr)
                    {
                        Console.WriteLine("Pakutud arv on liiga suur!");
                        Console.WriteLine("Paku uuesti: ");
                        break;
                    }



                }
                
            }
            Console.WriteLine("ÄRA ARVASID!");


        }
    }
}
