using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleAppServer;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Серверное приложение");

        var cts = new CancellationTokenSource();
        var server = new Server(IPAddress.Loopback, 8100);
        server.Start(cts.Token);

        Console.WriteLine("Нажать любую кнопку для завершения работы ...");
        cts.Cancel();
        Console.ReadKey();
    }
}


public class Server
{
    private readonly TcpListener _listener;
    public Server(IPAddress address, int port)
    {
        _listener = new TcpListener(address, port);
    }

    public async Task Start(CancellationToken token)
    {
        _listener.Start();

        while (true)
        {
            var data = string.Empty;
            token.ThrowIfCancellationRequested();
            var accp = _listener.AcceptTcpClient();
            token.ThrowIfCancellationRequested();
            var stream = accp.GetStream();
            var bytes = new byte[1024];
            while (true)
            {
                var readAsync = await stream.ReadAsync(bytes, token);
                data += Encoding.UTF8.GetString(bytes);
                if (data.Contains("<END>"))
                {
                    break;
                }
            }
            Console.WriteLine("Принятые данные: " + data);
            var message = "Ответ от сервера";
            var bytesOut = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(bytesOut, 0, bytesOut.Length, token);
            await stream.FlushAsync(token);

        }


    }

    public void Stop()
    {
        _listener.Stop();
    }
}





