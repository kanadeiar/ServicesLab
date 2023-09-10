using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleAppClient;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Клиентское приложение");

        var point = new IPEndPoint(IPAddress.Loopback, 8101);
        var client = new TcpClient(point);
        await client.ConnectAsync(IPAddress.Loopback, 8100);

        var stream = client.GetStream();
        var byt1 = Encoding.UTF8.GetBytes("Сообщение на сервер<END>");
        await stream.WriteAsync(byt1);

        await Task.Delay(500);
        
        var bytesRead = new byte[client.ReceiveBufferSize];
        await stream.ReadAsync(bytesRead, 0, client.ReceiveBufferSize);
        await stream.FlushAsync();
        Console.WriteLine($"Получено с сервера: {Encoding.UTF8.GetString(bytesRead)}");

        client.Close();

        Console.WriteLine("Нажать любую кнопку для завершения работы ...");
        Console.ReadKey();
    }
}