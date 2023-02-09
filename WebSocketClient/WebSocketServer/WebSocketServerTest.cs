using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace WebSocketServerTest
{
    class WebSocketServerTest : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine("from  client " + e.Data);

            Sessions.Broadcast("hi");
        }

    }

    class main
    {
        public static void Main(string[] args)
        {
            WebSocketServer wssv = new WebSocketServer("ws://172.16.165.157:30303");
            wssv.AddWebSocketService<WebSocketServerTest>("/WebUI");
            wssv.Start();
            Console.WriteLine("Start Server");

            Thread.Sleep(5000);

            ByteBuffer b = new ByteBuffer();

            b.putInt(1);
            b.putInt(2);
            b.putInt(3);
            b.putInt(4);
            int k = 0;
            if (wssv.IsListening)
            {
                wssv.WebSocketServices["/WebUI"].Sessions.Broadcast(k.ToString());
                Console.WriteLine("to client : " + b.getArray());
            }

            Thread.Sleep(500);
            k = k + 1;
            wssv.WebSocketServices["/WebUI"].Sessions.Broadcast(k.ToString());

            Thread.Sleep(500);
            k = k + 1;
            wssv.WebSocketServices["/WebUI"].Sessions.Broadcast(k.ToString());
            Thread.Sleep(500);
            k = k + 1;
            wssv.WebSocketServices["/WebUI"].Sessions.Broadcast(k.ToString());
            Thread.Sleep(500);
            k = k + 1;
            wssv.WebSocketServices["/WebUI"].Sessions.Broadcast(k.ToString());
            Thread.Sleep(500);
            k = k + 1;
            wssv.WebSocketServices["/WebUI"].Sessions.Broadcast(k.ToString());
            Thread.Sleep(500);
            k = k + 1;
            wssv.WebSocketServices["/WebUI"].Sessions.Broadcast(k.ToString());

            Console.ReadKey(true);
            wssv.Stop();

            
        }
    }
}
