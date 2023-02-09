using System;
using System.Collections.Generic;
using System.IO;
using WebSocketSharp;

namespace WebSocketClient
{
    public class WebSocketClientTest2
    {
        public static void Main(string[] args)
        {
            using (var ws = new WebSocket("ws://192.168.92.1:30303/WebUI"))
            {
                Console.WriteLine("Connect ----- Client");
                ws.OnMessage += (sender, e) =>
                {

                    if (e.Data.Length < 200)
                    {
                        recvChangeness(e);
                    }
                    else if (e.Data.Length < 3000)
                        recvDump(e);
                    else
                        recvInitEnvironment(e);
                };

                ws.Connect();

                Console.ReadKey(true);
            }
        }

        private static void recvInitEnvironment(MessageEventArgs e)
        {
            MemoryStream memoryStream = new MemoryStream(e.RawData);
            InitializeEnvironment initializeEnvironment = ProtoBuf.Serializer.Deserialize<InitializeEnvironment>(memoryStream);

            Console.WriteLine("InitEnvironment--------------------------------------------");
            Console.WriteLine("EnvironmentID : " + initializeEnvironment.environmentID);

            foreach (VirtualObject v in initializeEnvironment.Objects)
            {
                Console.WriteLine("name         : " + v.Name);
                Console.WriteLine("id           : " + v.objectID);
                Console.WriteLine("pos          : " + v.Position.X + "," + v.Position.Y);
                Console.WriteLine("type         : " + v.Type);

                foreach (KeyValuePair<string, string> entry in v.Properties)
                {
                    Console.WriteLine(entry.Key + "      : " + entry.Value);
                }
                Console.WriteLine(" ");
            }

        }

        private static void recvChangeness(MessageEventArgs e)
        {

            MemoryStream memoryStream = new MemoryStream(e.RawData);
            Changeness deserializeChangeness = ProtoBuf.Serializer.Deserialize<Changeness>(memoryStream);

            Console.WriteLine("CHANGENESS-----------------------------------------------");
            Console.WriteLine("id           : " + deserializeChangeness.objectID);
            Console.WriteLine("content      : " + deserializeChangeness.Content);
            Console.WriteLine("pos          : " + deserializeChangeness.Location.X + "," + deserializeChangeness.Location.Y);
            if(deserializeChangeness.Movement != null)
            {
                Console.WriteLine("movement     : " + deserializeChangeness.Movement.X + "," + deserializeChangeness.Movement.Y);
            }
            

            Console.WriteLine(" ");

        }

        private static void recvDump(MessageEventArgs e)
        {
            MemoryStream memoryStream = new MemoryStream(e.RawData);
            DumpEnvironment deserializeDump = ProtoBuf.Serializer.Deserialize<DumpEnvironment>(memoryStream);

            Console.WriteLine("DUMP----------------------------------------------------");
            Console.WriteLine("environment id   : " + deserializeDump.environmentID);

            foreach (ModifiedVirtualObject obj in deserializeDump.Objects)
            {
                Console.WriteLine("id                : " + obj.objectID);
                Console.WriteLine("orientation angle : " + obj.orientationAngle);
                Console.WriteLine("pos               : " + obj.Position.X + "," + obj.Position.Y);
                Console.WriteLine(" ");

            }
        }
    }
}
