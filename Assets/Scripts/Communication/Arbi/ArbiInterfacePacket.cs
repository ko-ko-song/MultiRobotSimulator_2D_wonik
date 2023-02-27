using System.Collections;
using System.Collections.Generic;
using System;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System.Threading;
using Defective.JSON;
using System.Text;

public class ArbiInterfacePacket
{
    private NetworkStream ns;
    private SensorActuatorModule sensorActuatorModule;
    private int HEADER_SIZE;
    private TcpListener tcpListener;
    public List<TcpClient> clients;
    
    public ArbiInterfacePacket(SensorActuatorModule sensorActuatorModule)
    {
        clients = new List<TcpClient>();
        this.sensorActuatorModule = sensorActuatorModule;
        this.setHeaderSize(sensorActuatorModule);
    }
    
    private void setHeaderSize(SensorActuatorModule sensorActuatorModule)
    {
        foreach (ActionProtocol a in sensorActuatorModule.actionProtocols.Values)
        {
            this.HEADER_SIZE = a.HEADER_SIZE;
            break;
        }
    }
    
    
    public void connect(string ip, string port)
    {
        try
        {
            tcpListener = new TcpListener(IPAddress.Parse(ip), Int32.Parse(port));
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(OnAcceptClient, null);
            Debug.Log("Arbi Server Open!   " + "   " + ip + ":" + port);
        }
        catch(SocketException e)
        {
            Debug.Log(e.ToString());
            //this.connect(ip, (Int32.Parse(port) + 1).ToString());
        }
    }
    
    private void OnAcceptClient(IAsyncResult ar)
    {
        TcpClient client = tcpListener.EndAcceptTcpClient(ar);
        if (client == null)
            return;

        clients.Add(client);
        Debug.Log("Arbi Server : " + clients.Count + "번째 Client 접속 성공");

        tcpListener.BeginAcceptTcpClient(OnAcceptClient, null);
    }

    private void send(ByteBuffer byteBuffer)
    {
        foreach(TcpClient n in clients.ToArray())
        {
            if (!n.Connected)
                continue;
            try
            {
                ns = n.GetStream();
                byte[] bf = byteBuffer.getArray();
                ns.Write(bf, 0, bf.Length);
                ns.Flush();
            }
            catch(Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
    }
    
    public void sendMessage(JSONObject JSONMessage)
    {
        bool isRTSR = false;
        foreach (JSONObject obj in JSONMessage.GetField("packetHeader").list)
        {
            string name = obj.GetField("name").getStringValue();
            int value = obj.GetField("value").intValue;

            if (name.Equals("packetID") && value == 1397136893)
            {
                isRTSR = true;
            }
        }

        
        if (!isRTSR)
        {
            Debug.Log("send message : \n\n" + JSONMessage.ToString(true));
        }
        
        ByteBuffer byteBuffer = new ByteBuffer();
        foreach(JSONObject obj in JSONMessage.GetField("packetHeader").list){
            string name = obj.GetField("name").getStringValue();
            string type = obj.GetField("type").getStringValue();
            int size = obj.GetField("size").intValue;
            switch (type)
            {
                case "int":
                    int intValue = obj.GetField("value").intValue;
                    byteBuffer.putInt(intValue);
                    break;
                case "float":
                    float floatValue = obj.GetField("value").floatValue;
                    byteBuffer.putFloat(floatValue);
                    
                    break;
                case "bool":
                    bool boolValue = obj.GetField("value").boolValue;
                    byteBuffer.put(boolValue);
                    break;
            }
        }
        foreach (JSONObject obj in JSONMessage.GetField("packetData").list)
        {
            string name = obj.GetField("name").getStringValue();
            string type = obj.GetField("type").getStringValue();
            int size = obj.GetField("size").intValue;
            switch (type)
            {
                case "int":
                    int intValue = obj.GetField("value").intValue;
                    byteBuffer.putInt(intValue);
                    break;
                case "float":
                    float floatValue = obj.GetField("value").floatValue;
                    byteBuffer.putFloat(floatValue);
                    break;
                case "bool":
                    bool boolValue = obj.GetField("value").boolValue;
                    byteBuffer.put(boolValue);
                    break;
            }
        }
        this.send(byteBuffer);
    }


    public void checkMessage()
    {
        foreach (TcpClient c in clients.ToArray())
        {
            if (c == null)
                return;
            if (!c.Connected)
            {
                clients.Remove(c);
                Debug.Log("client 연결 해제");
                continue;
            }
            ns = c.GetStream();
            if (ns.DataAvailable)
            {

                Debug.Log("some message received");
                ByteBuffer byteBuffer = new ByteBuffer();
                byteBuffer.wrap(readByte(HEADER_SIZE));

                List<int> header = new List<int>();

                for (int i = 0; i < (HEADER_SIZE / 4); i++)
                {
                    int value = byteBuffer.getInt();
                    header.Add(value);
                }
                int packetSize = getPacketSize(header);

                if (packetSize == 0)
                    return;

                byte[] packetData = readByte(packetSize - HEADER_SIZE);

                this.parseMessgae(header, packetData);
            }
        }
    }
    
    private void parseMessgae(List<int> header, byte[] packetData)
    {
        ActionProtocol actionProtocol = this.getMatchedProtocol(header);
        if (actionProtocol == null)
            return;
        ActionProtocolInstance actionProtocolInstance = actionProtocol.getInstance(packetData);
        sensorActuatorModule.receiveMessage(actionProtocolInstance);
    }

    private ActionProtocol getMatchedProtocol(List<int> header)
    {
        foreach (ActionProtocol actionProtocol in this.sensorActuatorModule.actionProtocols.Values)
        {
            List<JSONObject> headerTemplate = actionProtocol.requestMessageTemplate.GetField("packetHeader").list;

            for (int i = 0; i < header.Count; i++)
            {
                if (headerTemplate[i].GetField("classification").getStringValue().Equals("const"))
                {
                    if (header[i] != headerTemplate[i].GetField("value").intValue)
                        break;
                }
                
                if (i == (header.Count - 1))
                {
                    return actionProtocol;
                }
            }

        }
        return null;
    }

    private int getPacketSize(List<int> header)
    {
        if (header == null) {
            Debug.Log("header is null");
            return 0;
        }
        
        int packetSize = 0;
        foreach (ActionProtocol actionProtocol in this.sensorActuatorModule.actionProtocols.Values)
        {
            if (actionProtocol.requestMessageTemplate == null)
            {
                Debug.Log(actionProtocol.protocolId + " : requestMessageTemplate is null");
                return 0;
            }
                

            if (actionProtocol.requestMessageTemplate.GetField("packetHeader") == null)
            {
                Debug.Log(actionProtocol.protocolId + " : requestMessageTemplate pakcetHeader in null");
                return 0;
            }
            
            List<JSONObject> headerTemplate = actionProtocol.requestMessageTemplate.GetField("packetHeader").list;

            for (int i = 0; i<header.Count; i++)
            {
                if (headerTemplate[i].GetField("name").getStringValue().Equals("packetSize"))
                {
                    packetSize = header[i];
                }
                else if (header[i] != headerTemplate[i].GetField("value").intValue)
                {
                    break;
                }
                
                if(i == (header.Count - 1))
                {
                    return packetSize;
                }
            }
            
        }
        return 0;
    }

    public byte[] readByte(int length)
    {
        byte[] b = new byte[length];

        ns.Read(b, 0, length);

        return b;
    }

    public void quit()
    {
        if(ns != null)    
            ns.Close();
    }
}
