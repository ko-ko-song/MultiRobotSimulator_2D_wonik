using Defective.JSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class IDEInterface 
{
    private NetworkStream ns;
    private StreamWriter sw;
    private StreamReader sr;
    private TcpListener tcpListener;
    private List<TcpClient> clients;
    private EnvironmentManager environmentManager;
    
    public IDEInterface(EnvironmentManager environmentManager)
    {
        this.environmentManager = environmentManager;
        clients = new List<TcpClient>();
    }

    public void connect(string ip, string port)
    {
        try
        {
            tcpListener = new TcpListener(IPAddress.Parse(ip), Int32.Parse(port));
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(OnAcceptClient, null);
            Debug.Log("IDE Server Open!   " + "   " + ip + ":" + port);
        }
        catch (SocketException e)
        {
            Debug.Log(e.ToString());
            //this.connect(ip, (Int32.Parse(port) + 1).ToString());
        }
    }
    
    private void OnAcceptClient(IAsyncResult ar)
    {
        TcpClient client = tcpListener.EndAcceptTcpClient(ar);
        clients.Add(client);
        Debug.Log("IDE Server : " + clients.Count + "번째 Client 접속 성공");

        tcpListener.BeginAcceptTcpClient(OnAcceptClient, null);
    }

    private void send(string JSONString)
    {
        foreach (TcpClient n in clients.ToArray())
        {
            if (!n.Connected)
                continue;
            ns = n.GetStream();
            sw = new StreamWriter(ns);
            sw.WriteLine(JSONString);
            sw.Flush();
        }
    }

    public void sendMessage(JSONObject JSONObject)
    {
        this.send(JSONObject.ToString(false));
    }
    public void checkMessage()
    {
        foreach (TcpClient c in clients.ToArray())
        {
            if (!c.Connected)
            {
                clients.Remove(c);
                Debug.Log("IDE client 연결 해제");
                continue;
            }
            ns = c.GetStream();
            if (ns.DataAvailable)
            {
                sr = new StreamReader(ns);
                string receivedMessage = sr.ReadLine();
                Debug.Log("IDE : receivedMessage \n" + receivedMessage);
                this.parseMessage(receivedMessage);
            }
        }
    }
    
    private void parseMessage(string receivedMessage)
    {
        JSONObject jSONObject;
        try
        {
            jSONObject = new JSONObject(receivedMessage);
        }
        catch
        {
     
            Debug.Log("IDE: receivedMessage is not JSON format");
            return;
        }
        string messageType = jSONObject.GetField("messageType").getStringValue();
        JSONObject message = jSONObject.GetField("message");


        switch (messageType)
        {
            case "load":
                environmentManager.loadObject(message);
                break;

            case "create":
                environmentManager.loadObject(message);
                break;

            case "update":
                environmentManager.updateObject(message);
                break;

            case "delete":
                environmentManager.deleteObject(message);
                break;

            default:
                Debug.Log("IDE : mesage type error");
                break;
        }



    }


    public void quit()
    {
        if (ns != null)
            ns.Close();
    }
}
