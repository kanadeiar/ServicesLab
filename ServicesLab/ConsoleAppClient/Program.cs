using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleAppClient;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Клиентское приложение");

        var bytes = new byte[1024];

        var sender = default(Socket);
        try
        {
            var iHost = IPAddress.Loopback;
            var point = new IPEndPoint(iHost, 8100);

            sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await sender.ConnectAsync(point);
            Console.WriteLine($"Соединение с сервером {point} установлено");
            var message = "Это тестовое сообщение с клиента";

            var bytesSend = Encoding.UTF8.GetBytes(message + "<TheEnd>");
            await sender.SendAsync(bytesSend);
            await sender.ReceiveAsync(bytes);

            Console.WriteLine($"Сервер ответил: {Encoding.UTF8.GetString(bytes)}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            sender?.Shutdown(SocketShutdown.Both);
            sender?.Close();
        }

        Console.WriteLine("Нажать любую кнопку для завершения работы ...");
        Console.ReadKey();
    }
}