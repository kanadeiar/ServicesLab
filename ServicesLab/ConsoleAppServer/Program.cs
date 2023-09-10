using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleAppServer;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Серверное приложение");

        var iHost = IPAddress.Loopback;
        var point = new IPEndPoint(iHost, 8100);

        var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            listener.Bind(point);
            listener.Listen(10);

            while (true)
            {
                var handler = default(Socket);
                try
                {
                    handler = await listener.AcceptAsync();

                    var data = default(string);
                    while (true)
                    {
                        var bytes = new byte[1024];
                        int bytesRec = await handler.ReceiveAsync(bytes);
                        data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                        if (data.Contains("<TheEnd>") || handler.Available == 0)
                        {
                            break;
                        }
                    }

                    Console.WriteLine($"Получен текст: {data}");

                    var reply = "Ответный текст с сервера о приеме информации";
                    var bytes2 = Encoding.UTF8.GetBytes(reply);

                    await handler.SendAsync(bytes2);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    handler?.Shutdown(SocketShutdown.Both);
                    handler?.Close();
                }

            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        Console.WriteLine("Нажать любую кнопку для завершения работы ...");
        Console.ReadKey();
    }
}





