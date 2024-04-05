using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class Program
    {
        private static string[] phrases = {
            "Привіт", "Як справи?", "До побачення",
            "Що нового?", "Що робиш?", "Так",
            "Ні", "Не знаю", "Напевно"
        };
        private static Random rnd = new Random();

        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.Write("Виберіть режим (1 - людина, 2 - комп'ютер): ");
            int mode = Convert.ToInt32(Console.ReadLine());

            StartServer(mode);
        }

        private static void StartServer(int mode)
        {
            TcpListener server = new TcpListener(
                IPAddress.Parse("127.0.0.1"), 8080);
            server.Start();
            Console.WriteLine("Сервер запущено...");

            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Клієнт приєднався...\n");

            NetworkStream stream = client.GetStream();

            HandleClient(stream, client, mode);

            client.Close();
            server.Stop();
        }

        private static void HandleClient(NetworkStream stream, TcpClient client, int mode)
        {
            while (true)
            {
                if (!client.Connected)
                {
                    Console.WriteLine("Клієнт відключився...");
                    break;
                }
                try
                {
                    byte[] data = new byte[256];
                    int bytes = stream.Read(data, 0, data.Length);
                    string msg = Encoding.UTF8.GetString(data, 0, bytes);
                    Console.WriteLine($"Отримано: {msg}");

                    if (msg.ToLower() == "до побачення")
                        break;

                    if (mode == 2)
                    {
                        msg = phrases[rnd.Next(phrases.Length)];
                    }
                    else
                    {
                        Console.Write("\nВведіть відповідь: ");
                        msg = Console.ReadLine();
                    }

                    data = Encoding.UTF8.GetBytes(msg);
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine($"Відправлено: {msg}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Помилка: {e.Message}");
                }
            }
        }
    }
}
