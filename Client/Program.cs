using System;
using System.Text;
using System.Net.Sockets;

namespace Client
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

            StartClient(mode);
        }

        private static void StartClient(int mode)
        {
            TcpClient client = new TcpClient("127.0.0.1", 8080);
            NetworkStream stream = client.GetStream();

            HandleServer(stream, mode);

            stream.Close();
            client.Close();

            Console.ReadLine();
        }

        private static void HandleServer(NetworkStream stream, int mode)
        {
            while (true)
            {
                string msg;

                if (mode == 2)
                {
                    msg = phrases[rnd.Next(phrases.Length)];
                    Console.WriteLine($"Відправлено: {msg}");
                }
                else
                {
                    Console.Write("\nВведіть повідомлення: ");
                    msg = Console.ReadLine();
                }

                if (msg.ToLower() == "до побачення")
                    break;

                byte[] data = Encoding.UTF8.GetBytes(msg);
                stream.Write(data, 0, data.Length);

                data = new byte[256];
                int bytes = stream.Read(data, 0, data.Length);
                msg = Encoding.UTF8.GetString(data, 0, bytes);
                Console.WriteLine($"Отримано: {msg}");

                if (msg.ToLower() == "До побачення")
                    break;
            }
        }
    }
}
