using System;
//Przestrzeń innych klas związanych z komunikacją sieciową
using System.Net;
//Przestrzeń klasy TcpListener
using System.Net.Sockets;

namespace Gniazdka
{
    class Serwer
    {
        static void Main(string[] args)
        {
            //IPAddress adres = IPAddress.Parse("172.20.9.65");
            IPAddress adres = IPAddress.Parse("192.168.0.73");
            int port = 6666;
            TcpListener serwer = null;
            try
            {
                 serwer = new TcpListener(adres, port);
                serwer.Start(); // uruchomienie nasłuchiwania na podanym adresie i porcie
                Byte[] dane = new Byte[8];
                string komunikat = null;

                while (true)
                {
                    Console.Write("Słucham... ");
                   
                    // Oczekiwanie na żądanie klienta.
                    TcpClient klient = serwer.AcceptTcpClient();
                    Console.WriteLine("Klient połączony!");
                    komunikat = "";

                    // Get a stream object for reading and writing
                    NetworkStream stream = klient.GetStream();
                    int i;
                    // Pętla pobierająca wszystkie dane klienta - idą paczkami
                    while ((i = stream.Read(dane, 0, dane.Length)) != 0)
                    {
                        // konwersja danych binarnych na string w UTF-8
                        string kawalek= System.Text.Encoding.UTF8.GetString(dane, 0, i);
                        komunikat += kawalek;
                        //Wysłanie potwierdzenia odebrania fragmentu - bez tego klient nie wyśle kolejnej porcji danych
                        stream.Write(new byte[2] , 0, i<dane.Length?1:2);
                        Console.WriteLine($"Odbieram kawałek..{kawalek}");
                    }
                    Console.WriteLine($"Klient wysłał: {komunikat}");

                    string odp = null;
                    switch (komunikat.ToLower())
                    {
                        case "siemka": odp = "No cześć"; break;

                        case "jak cię zwą?":
                            odp = System.Environment.MachineName; break;
                        case "lubisz mnie?":
                            odp = "pewnie :)"; break;
                        case "która godzina?":
                            odp = DateTime.Now.ToLongTimeString(); break;

                        default: odp = "hmmm....."; break;

                    }

                    byte[] odpowiedz = System.Text.Encoding.UTF8.GetBytes(odp);

                    // wysłanie odpowiedzi
                    stream.Write(odpowiedz, 0, odpowiedz.Length);
                    Console.WriteLine($"Odpowiadam: {odp}");
                    // zamknięcie połączenia
                    klient.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Zamknięcie nasłuchiwania
                serwer.Stop();
            }




            Console.ReadKey();
         

        }
    }
}
