using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketServer
{
    class SocketServer
    {
        public static NetworkStream ns;
        public static void connect(string ip, int port)
        {
            try
            {
                TcpListener tcpListener = new TcpListener(IPAddress.Parse(ip), port);
                tcpListener.Start();

                TcpClient client = tcpListener.AcceptTcpClient();

                ns = client.GetStream();

                //Thread t = new Thread(new ThreadStart(ThreadRun));
                //t.Start();
            }
            catch (SocketException e)
            {
            }
        }

        
        public byte[] readByte(int length)
        {
            byte[] b = new byte[length];

            ns.Read(b, 0, length);

            return b;
        }

        private static void send(NetworkStream ns, byte[] buffer)
        {
            ns.Write(buffer, 0, buffer.Length);
            ns.Flush();
        }
        
        private static void send(int[] value)
        {
            ByteBuffer byteBuffer = new ByteBuffer();

            foreach(int i in value)
            {
                byteBuffer.putInt(i);
            }

            send(ns, byteBuffer.getArray());
        }

        
    static void Main(string[] args)
            {
            try
            {
                connect("127.0.0.1", 9090);

             
                //while (i<10)
                //{
                //    send(value1);
                //    send(value2);
                //    send(value3);
                //    Thread.Sleep(1000);
                //    i++;
                //}

                
                
                
                

                ns.Close();

                Console.WriteLine("Client 연결 종료!");
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        //StartListening();
    }
        

        }
    


