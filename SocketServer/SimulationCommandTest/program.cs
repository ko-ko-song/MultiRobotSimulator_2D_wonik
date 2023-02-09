using socketClient;
using SocketClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketClient
{
    class Program
    {
        public NetworkStream ns;
        public Thread thread;
        public int HEADER_SIZE = 12;
        TcpClient client;

        public Program(string ip, int port)
        {
            
            connect(ip, port);
            start();
        }

        private void connect(string ip, int port)
        {
            client = new TcpClient(ip, port);
            ns = client.GetStream();
          
            thread = new Thread(new ThreadStart(listenFormServer));
            thread.Start();
            Console.WriteLine("ip : " + ip + "  port : " + port + "  연결 성공");

        }

        public void start()
        {
            while (true)
            {
                string input = Console.ReadLine();

                string[] subs = input.Split(' ');

                sendMessage(subs);
                Thread.Sleep(10);
                if (!client.Connected)
                {
                    ns.Close();
                    thread.Abort();
                    break;
                }
            }
        }

        private void sendMessage(string[] subs)
        {
            string req = subs[0];
            int packetSize = (subs.Length + 2) * 4;
            int robotID;
            int nodeID;
            ByteBuffer b = new ByteBuffer();

            switch (req)
            {
                case "ReqLogin":
                    robotID = Int32.Parse(subs[1]);
                    b.putInt(30000);
                    b.putInt(1956887904);
                    b.putInt(packetSize);
                    b.putInt(robotID);
                    send(b);
                    break;

                case "ReqMove":
                    robotID = Int32.Parse(subs[1]);
                    int pathSize = Int32.Parse(subs[2]);

                    b.putInt(30000);
                    b.putInt((int)MessageTypeEnum.MessageType.ReqMove);
                    b.putInt(packetSize);
                    b.putInt(robotID);
                    b.putInt(pathSize);

                    for (int i = 3; i < pathSize + 3; i++)
                    {
                        try
                        {
                            b.putInt(Int32.Parse(subs[i]));
                        }
                        catch
                        {
                            Console.WriteLine("다시 입력해 주세요");
                        }

                    }
                    send(b);
                    break;
                case "ReqCancleMove":
                    robotID = Int32.Parse(subs[1]);
                    b.putInt(30000);
                    b.putInt((int)MessageTypeEnum.MessageType.ReqCancelMove);
                    b.putInt(packetSize);
                    b.putInt(robotID);

                    send(b);
                    break;
                case "ReqLoad":
                    robotID = Int32.Parse(subs[1]);
                    nodeID = Int32.Parse(subs[2]);
                    b.putInt(30000);
                    b.putInt((int)MessageTypeEnum.MessageType.ReqLoad);
                    b.putInt(packetSize);
                    b.putInt(robotID);
                    b.putInt(nodeID);

                    send(b);
                    break;
                case "ReqUnload":
                    robotID = Int32.Parse(subs[1]);
                    nodeID = Int32.Parse(subs[2]);
                    b.putInt(30000);
                    b.putInt((int)MessageTypeEnum.MessageType.ReqUnload);
                    b.putInt(packetSize);
                    b.putInt(robotID);
                    b.putInt(nodeID);

                    send(b);
                    break;
                case "ReqCharge":
                    robotID = Int32.Parse(subs[1]);
                    nodeID = Int32.Parse(subs[2]);
                    b.putInt(30000);
                    b.putInt((int)MessageTypeEnum.MessageType.ReqCharge);
                    b.putInt(packetSize);
                    b.putInt(robotID);
                    b.putInt(nodeID);

                    send(b);
                    break;
                case "ReqChargeStop":
                    robotID = Int32.Parse(subs[1]);
                    nodeID = Int32.Parse(subs[2]);
                    b.putInt(30000);
                    b.putInt((int)MessageTypeEnum.MessageType.ReqChargeStop);
                    b.putInt(packetSize);
                    b.putInt(robotID);
                    b.putInt(nodeID);

                    send(b);
                    break;
                case "ReqPause":
                    robotID = Int32.Parse(subs[1]);
                    b.putInt(30000);
                    b.putInt((int)MessageTypeEnum.MessageType.ReqPause);
                    b.putInt(packetSize);
                    b.putInt(robotID);

                    send(b);
                    break;
                case "ReqResume":
                    robotID = Int32.Parse(subs[1]);
                    b.putInt(30000);
                    b.putInt((int)MessageTypeEnum.MessageType.ReqResume);
                    b.putInt(packetSize);
                    b.putInt(robotID);

                    send(b);
                    break;
                case "ReqDoorOpen":
                    robotID = Int32.Parse(subs[1]);
                    b.putInt(30000);
                    b.putInt((int)MessageTypeEnum.MessageType.ReqDoorOpen);
                    b.putInt(packetSize);
                    b.putInt(robotID);

                    send(b);
                    break;
                case "ReqDoorClose":
                    robotID = Int32.Parse(subs[1]);
                    b.putInt(30000);
                    b.putInt((int)MessageTypeEnum.MessageType.ReqDoorClose);
                    b.putInt(packetSize);
                    b.putInt(robotID);

                    send(b);
                    break;
                default:
                    Console.WriteLine("다시 입력해주세요");
                    break;
            }
        }

        public void send(ByteBuffer b)
        {
            byte[] buffer = b.getArray();
            ns.Write(buffer, 0, buffer.Length);
            ns.Flush();
        }

        public byte[] readByte(int length)
        {
            byte[] b = new byte[length];
            ns.Read(b, 0, length);
            return b;
        }


        public void listenFormServer()
        {
            while (true)
            {
                if (ns.DataAvailable)
                {
                    ByteBuffer byteBuffer = new ByteBuffer();
                    byteBuffer.wrap(readByte(HEADER_SIZE));

                    int protocolID = byteBuffer.getInt();
                    int messageTypeID = byteBuffer.getInt();
                    int packetSize = byteBuffer.getInt();

                    byte[] packetData = readByte(packetSize - HEADER_SIZE);

                    parseMessage(protocolID, messageTypeID, packetData);
                }
                Thread.Sleep(10);
            }
        }

        private void parseMessage(int protocolID, int messageTypeID, byte[] packetData)
        {
            ByteBuffer byteBuffer = new ByteBuffer();
            byteBuffer.wrap(packetData);

            int robotID = byteBuffer.getInt();
            int result;

            switch (messageTypeID)
            {

                case 851424104:
                    Console.WriteLine("AckMove : " + "id = " + robotID);
                    break;

                case 420142072:
                    result = byteBuffer.getInt();
                    Console.WriteLine("AckEndMove : " + "id = " + robotID + ", result = " + result);
                    break;

                case (int)MessageTypeEnum.MessageType.RTSR:

                    int status = byteBuffer.getInt();
                    float x = byteBuffer.getInt();
                    float y = byteBuffer.getInt();
                    float theta = byteBuffer.getFloat();
                    float speed = byteBuffer.getFloat();
                    int battery = byteBuffer.getInt();
                    bool loading = byteBuffer.get();

                    string sx = string.Format("{0:0.0}", x);
                    string sy = string.Format("{0:0.0}", y);
                    string stheta = string.Format("{0:0.0}", theta);
                    string sspeed = string.Format("{0:0.##0}", speed);

                    //Console.WriteLine("            RTSR :  " + "id = " + robotID + ", status = " + (RobotStatusEnum.RobotStatus)status + ", x = " + sx + ", y = " + sy +
                    //    ", theta = " + stheta + ", speed = " + sspeed + ", battery = " + battery + ", loading = " + loading);
                    break;

                case (int)MessageTypeEnum.MessageType.AckMove:

                    Console.WriteLine("AckMove : " + "id = " + robotID);
                    break;

                case (int)MessageTypeEnum.MessageType.AckEndMove:
                    result = byteBuffer.getInt();
                    Console.WriteLine("AckEndMove : " + "id = " + robotID + ", result = " + result);
                    break;

                case (int)MessageTypeEnum.MessageType.AckCancelMove:
                    Console.WriteLine("AckCancleMove : " + "id = " + robotID);
                    break;

                case (int)MessageTypeEnum.MessageType.AckEndCancelMove:
                    result = byteBuffer.getInt();
                    Console.WriteLine("AckEndCancleMove : " + "id = " + robotID + ", result = " + result);
                    break;

                case (int)MessageTypeEnum.MessageType.AckLoad:
                    Console.WriteLine("AckLoad : " + "id = " + robotID);
                    break;
                    
                case (int)MessageTypeEnum.MessageType.AckEndLoad:
                    result = byteBuffer.getInt();
                    Console.WriteLine("AckEndLoad : " + "id = " + robotID + ", result = " + result);
                    break;

                case (int)MessageTypeEnum.MessageType.AckUnload:
                    Console.WriteLine("AckUnload : " + "id = " + robotID);
                    break;

                case (int)MessageTypeEnum.MessageType.AckEndUnload:
                    result = byteBuffer.getInt();
                    Console.WriteLine("AckEndUnload : " + "id = " + robotID + ", result = " + result);
                    break;

                case (int)MessageTypeEnum.MessageType.AckCharge:
                    Console.WriteLine("AckCharge : " + "id = " + robotID);
                    break;

                case (int)MessageTypeEnum.MessageType.AckEndCharge:
                    result = byteBuffer.getInt();
                    Console.WriteLine("AckEndCharge : " + "id = " + robotID + ", result = " + result);
                    break;

                case (int)MessageTypeEnum.MessageType.AckChargeStop:
                    Console.WriteLine("AckChargeStop : " + "id = " + robotID);
                    break;

                case (int)MessageTypeEnum.MessageType.AckEndChargeStop:
                    result = byteBuffer.getInt();
                    Console.WriteLine("AckEndChargeStop : " + "id = " + robotID + ", result = " + result);
                    break;

                case (int)MessageTypeEnum.MessageType.AckEndPause:
                    Console.WriteLine("AckEndPause : " + "id = " + robotID);
                    break;

                case (int)MessageTypeEnum.MessageType.AckEndResume:
                    Console.WriteLine("AckEndResume : " + "id = " + robotID);
                    break;

                case (int)MessageTypeEnum.MessageType.AckDoorOpen:
                    Console.WriteLine("AckDoorOpen : " + "id = " + robotID);
                    break;

                case (int)MessageTypeEnum.MessageType.AckEndDoorOpen:
                    result = byteBuffer.getInt();
                    Console.WriteLine("AckEndDoorOpen : " + "id = " + robotID + ", result = " + result);
                    break;

                case (int)MessageTypeEnum.MessageType.AckDoorClose:
                    Console.WriteLine("AckDoorClose : " + "id = " + robotID);
                    break;

                case (int)MessageTypeEnum.MessageType.AckEndDoorClose:
                    result = byteBuffer.getInt();
                    Console.WriteLine("AckEndDoorClose : " + "id = " + robotID + ", result = " + result);
                    break;

                case (int)MessageTypeEnum.MessageType.AckPersonCall:
                    Console.WriteLine("Person Call : " + "id = " + robotID);
                    break;
                default:
                    Console.WriteLine("undefinedMessage");
                    break;

            }

        }

        static void Main(string[] args)
        {
            new Program("172.16.165.208", 36666);
        }
    }
}
