using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Threading;

namespace Server
{
    // Server TCP minimale: accetta un client alla volta e risponde a ogni messaggio.
    class Program
    {
        const int PORTA = 5000;

        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, PORTA);
            listener.Start();
            Console.WriteLine("Server in ascolto sulla porta " + PORTA + "...");

            while (true)
            {
                // Bloccante: attende che un client si connetta
                using (TcpClient client = listener.AcceptTcpClient())
                {
                    Thread th1  = new Thread (EseguiLavoro);
                    th1.Start();
                    System.Console.WriteLine("Thread principale in esecuzione...");
                    Console.WriteLine("Client connesso: " + client.Client.RemoteEndPoint);

                    using (NetworkStream stream = client.GetStream())
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                            {
                                writer.AutoFlush = true;

                                string messaggio;
                                // ReadLine restituisce null quando il client chiude la connessione
                                while ((messaggio = reader.ReadLine()) != null)
                                {
                                    System.Console.WriteLine("il messaggio è lungo " + messaggio.Length + " caratteri");
                                    Console.WriteLine("Ricevuto: " + messaggio);

                                    writer.WriteLine("ECO: " + messaggio);

                                    if (messaggio.ToUpper() == "FINE")
                                    {
                                        break;
                                    }

                                }
                            }
                        }
                    }

                    Console.WriteLine("Client disconnesso.");
                }
            }
        }
        static void EseguiLavoro()
        {
            System.Console.WriteLine("Sono il thread che eseguirà il lavoro");
        }
    }
}
