using SocketClient2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace socketClient2
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

                //string[] subs = input.Split(' ');

                sendMessage(input);
                Thread.Sleep(10);
                if (!client.Connected)
                {
                    ns.Close();
                    thread.Abort();
                    break;
                }
            }
        }
    //    "requestMessageTemplate" : {
	   //             "robotID" : "$robotID",
	   //             "mType" : "requestMove",
	   //             "path" : ["$path"]
    //},
        public void sendMessage(string line)
        {
            //string req = subs[0];
            //int packetSize = (subs.Length + 2) * 4;
            //int robotID;
            //int nodeID;
            //ByteBuffer b = new ByteBuffer();
            
            switch (line)
            {
                case "ReqMove":
                    string message ="{\"robotID\" : \"1\",\"mType\" : \"requestMove\",\"path\" : [120, 119, 118, 117, 23]}";
                    
                    sendLine(message);
                    break;


                case "case1":
                    string message2 = "{\"robotID\" : \"1\",\"mType\" : \"requestMove\",\"path\" : [120, 119, 118,117, 23]}";
                    sendLine(message2);

                    Thread.Sleep(1);
                    message2 = "{\"robotID\" : \"3\",\"mType\" : \"requestMove\",\"path\" : [121, 122, 124, 125]}";
                    sendLine(message2);
                    break;

                case "case2":
                    string message3 = "{\"robotID\" : \"1\",\"mType\" : \"requestLoad\",\"nodeID\" : 23}";

                    sendLine(message3);
                    break;
                case "case3":
                    string message4 = "{\"robotID\" : \"1\",\"mType\" : \"requestUnload\",\"nodeID\" : \"23\"}";

                    sendLine(message4);
                    break;
                case "reqCharge":
                    message4 = "{\"robotID\" : \"1\",\"mType\" : \"requestCharge\",\"nodeID\" : \"500\"}";

                    sendLine(message4);
                    break;
                case "reqChargeStop":
                    message4 = "{\"robotID\" : \"1\",\"mType\" : \"requestChargeStop\",\"nodeID\" : \"157\"}";

                    sendLine(message4);
                    break;
                case "reqPause":
                    message4 = "{\"robotID\" : \"1\",\"mType\" : \"requestPause\"}";

                    sendLine(message4);
                    break;
                case "reqResume":
                    message4 = "{\"robotID\" : \"1\",\"mType\" : \"requestResume\"}";

                    sendLine(message4);
                    break;
                //case "ReqMove":
  
                default:
                    sendLine(line);
                    //Console.WriteLine("다시 입력해주세요");
                    break;
            }
        }

        private void sendLine(string m)
        {
            StreamWriter sw = new StreamWriter(ns);
            sw.WriteLine(m);
            sw.Flush();
            Console.WriteLine(m);
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
            StreamReader sr = new StreamReader(ns);
            while (true)
            {
                if (ns.DataAvailable)
                {
                    Console.WriteLine("received : \n" + sr.ReadLine());
                    //ByteBuffer byteBuffer = new ByteBuffer();
                    //byteBuffer.wrap(readByte(HEADER_SIZE));

                    //int protocolID = byteBuffer.getInt();
                    //int messageTypeID = byteBuffer.getInt();
                    //int packetSize = byteBuffer.getInt();

                    //byte[] packetData = readByte(packetSize - HEADER_SIZE);

                    ////parseMessage(protocolID, messageTypeID, packetData);
                }
                Thread.Sleep(1);
            }
        }

        private void parseMessage(int protocolID, int messageTypeID, byte[] packetData)
        {
            ByteBuffer byteBuffer = new ByteBuffer();
            byteBuffer.wrap(packetData);

            int robotID = byteBuffer.getInt();
            //if (!(robotID == 1))
                //return;
            int result;
            switch (messageTypeID)
            {
                case (int)MessageTypeEnum.MessageType.RTSR:

                    int status = byteBuffer.getInt();
                    float x = byteBuffer.getFloat();
                    float y = byteBuffer.getFloat();
                    float theta = byteBuffer.getFloat();
                    float speed = byteBuffer.getFloat();
                    int battery = byteBuffer.getInt();
                    bool loading = byteBuffer.get();


                    string sx = string.Format("{0:0.0}", x);
                    string sy = string.Format("{0:0.0}", y);
                    string stheta = string.Format("{0:0.0}", theta);
                    string sspeed = string.Format("{0:0.##0}", speed);

                    Console.WriteLine("            RTSR :  " + "id = " + robotID + ", status = " + (RobotStatusEnum.RobotStatus)status + ", x = " + sx + ", y = " + sy +
                        ", theta = " + stheta + ", speed = " + sspeed + ", battery = " + battery + ", loading = " + loading);
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
            Program p = new Program("172.16.165.208", 30404);
            //{ "robotID" : "$robotID", "mType" : "requestCharge", "nodeID" : "$nodeID"}

            //p.sendMessage(new string[]{"ReqMove"});
            //{"robotID" : "0","mType" : "requestPause"}
        }
        
    }
}
