using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Klient
{
    
    class Klient
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Czat over TCP");
            var decyzja = new ConsoleKeyInfo();
            while (decyzja.Key!= ConsoleKey.Escape)
            {
                //Console.Write("Podaj adres:");
                //string ip = Console.ReadLine();
                //Console.WriteLine("Port:");
                //int port = int.Parse( Console.ReadLine());

                Console.Write("Wpisz i wyślij komunikat:");
                string komunikat = Console.ReadLine();
                Connect("192.168.0.73", 6666, komunikat);
               
               // Connect("172.20.9.65", 6666, komunikat);
               // Connect(ip, port, komunikat);

                Console.WriteLine("ESC - koniec, inny klawisz kontynuacja:\n");
                decyzja = Console.ReadKey();
            }
        }
        static void Connect(String server, int port, string komunikat)
        {
            try
            {                       
                TcpClient client = new TcpClient(server, port);
              
                // Przerabiamy komunikat w utf8 na tablicę bajtów
                Byte[] dane = System.Text.Encoding.UTF8.GetBytes(komunikat);
               //utworzenie strumienia do czytania i pisania
                NetworkStream stream = client.GetStream();

                //wysłanie komunikatu do serwera 
                stream.Write(dane, 0, dane.Length);
              
                // wyczyszczenie bufora danych do przyjęcia odpowiedzi serwera
                dane = new Byte[256];               
                string odpowiedzSerwera = String.Empty;
                // wczytanie odpowiedzi serwera
                int bytes;
                while ( (bytes = stream.Read(dane, 0, dane.Length))!=0)
                {

                    Console.WriteLine($"Odebrałem {bytes} bajtów");
                    //zdekodowanie odpowiedzi na odpowiedni format utf8
                    odpowiedzSerwera = System.Text.Encoding.UTF8.GetString(dane, 0, bytes);
                   
                    if (bytes==1)
                    {
                        Console.WriteLine("Serwer potwierdza koniec transmisji danych");
                        stream.Close();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Serwer potwierdza paczkę danych");
                    }
                }
                Console.WriteLine("Otrzymałem: {0}", odpowiedzSerwera);
                // zamknięcie strumienia i klienta
                
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

        
        }
    }
}
