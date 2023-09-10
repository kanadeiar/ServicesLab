using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Handler
    {
        private readonly TcpClient _client;

        public Handler(TcpClient client)
        {
            _client = client;
        }

        public async Task RunClient()
        {
            var reader = new StreamReader(_client.GetStream());
            var writer = new StreamWriter(_client.GetStream());

            var returnData = await reader.ReadLineAsync();
            var userName = returnData;
            Console.WriteLine($"User {userName} connect to server");
            
            while (true)
            {
                returnData = await reader.ReadLineAsync();
                if (returnData.IndexOf("QUIT", StringComparison.Ordinal) > -1)
                {
                    Console.WriteLine($"User {userName} is disconnected from server");
                    break;
                }

                Console.WriteLine($"{userName} : {returnData}");

                var bytes = Encoding.UTF8.GetBytes(returnData);
                await writer.WriteAsync(Encoding.UTF8.GetChars(bytes));

            }

            _client.Close();
        }
    }

    internal class Program
    {
        const int Port = 8080;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Лаборатория по разработке сервисных приложений");
            var listener = new TcpListener(IPAddress.Any, Port);
            try
            {
                listener.Start();
                Console.WriteLine("Ожидание подключений ...");

                while (true)
                {
                    var client = await listener.AcceptTcpClientAsync();

                    var handler = new Handler(client);

                    Task.Run(() => handler.RunClient());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                listener.Stop();
            }


            Console.WriteLine("Нажать любую кнопку для завершения работы ...");
            Console.ReadKey();
        }
    }
}
