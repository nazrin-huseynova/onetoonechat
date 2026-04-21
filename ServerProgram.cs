using System.Net;
using System.Net.Sockets;

namespace server2;

public class Program
{
    static TcpListener listener = null;
    static BinaryReader br = null;
    static BinaryWriter bw = null;

    static void Main(string[] args)
    {
        var ip = IPAddress.Parse("192.168.1.72");
        var port = 27001;
        var ep = new IPEndPoint(ip, port);
        listener = new TcpListener(ep);
        listener.Start();

        Console.WriteLine($"Listener on {listener.LocalEndpoint}");


        while (true)
        {
            var client = listener.AcceptTcpClient();
            Console.WriteLine($"{client.Client.RemoteEndPoint} connected ...");

            #region one client

            Task.Run(() =>
            {
                var reader = Task.Run(() =>
                {
                    var stream = client.GetStream();
                    br = new BinaryReader(stream);

                    while (true)
                    {
                        var msg = br.ReadString();
                        Console.WriteLine(msg);
                    }
                });

                var writer = Task.Run(()=> 
                    {
                    var stream = client.GetStream();
                    bw = new BinaryWriter(stream);
                    while (true)
                    {
                        var msg = Console.ReadLine();
                        bw.Write(msg);
                    }
                });

            });
            #endregion
        }
    }
}
