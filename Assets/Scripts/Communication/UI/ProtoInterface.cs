using kr.ac.uos.ai.mcmonitor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using WebSocketSharp.Server;
using WebSocketSharp;
using System.Collections;

public class ProtoInterface
{
    private NetworkManager networkManager;
    private WebSocketServer webSocketServer;
    public List<UIClient> clients;

    public ProtoInterface(NetworkManager networkManager)
    {
        this.networkManager = networkManager;
        this.clients = new List<UIClient>();
    }
    
    public void connect(string ip, string port)
    {
        webSocketServer = new WebSocketServer("ws://" + ip + ":" + Int32.Parse(port));
        webSocketServer.AddWebSocketService<WebUIServer>("/WebUI", ()=> new WebUIServer(this.clients));
        
        Debug.Log("Web UI Server 시작!    " + "ws://" + ip + ":" + port+"/WebUI");
        webSocketServer.Start();
    }
    
    private void send(byte[] buffer)
    {
        if (webSocketServer.IsListening)
        {
            webSocketServer.WebSocketServices["/WebUI"].Sessions.Broadcast(buffer);
        }
    }

    private void send(byte[] buffer, string sessionID)
    {
        if (webSocketServer.IsListening)
        {
            webSocketServer.WebSocketServices["/WebUI"].Sessions.SendTo(buffer, sessionID);
        }
    }

    public void quit()
    {
        if(webSocketServer!=null)
            webSocketServer.Stop();
    }
    
    public void checkClient()
    {
        foreach (UIClient client in clients)
        {
            if (!client.isSendInitMessage)
            {
                sendInitialEnvironment(client.sessionID);
                client.isSendInitMessage = true;
            }
        }
    }

    public void sendDumpEnvironment()
    {
        GameObject[] robots = GameObject.FindGameObjectsWithTag("Robot");
        DumpEnvironment environment = new DumpEnvironment
        {
            environmentID = "Environment" + EnvironmentManager.instance.id
        };

        foreach (GameObject robot in robots)
        {
            float x = robot.transform.position.x;
            float y = robot.transform.position.y;
            ModifiedVirtualObject vo = new ModifiedVirtualObject {
                objectID = robot.name,
                Position = new Point
                {
                    X = x,
                    Y = y
                },
                orientationAngle = robot.transform.eulerAngles.z
            };
            environment.Objects.Add(vo);
            //MemoryStream ms2 = new MemoryStream();
            //ProtoBuf.Serializer.Serialize(ms2, vo);
            //Debug.Log(Encoding.Default.GetString(ms2.ToArray()));
        }
        
        MemoryStream ms = new MemoryStream();
        StreamReader sr = new StreamReader(ms);
        ProtoBuf.Serializer.Serialize(ms, environment);

        send(ms.ToArray());
        ms.Flush();

        //Debug.Log(Encoding.Default.GetString(ms.ToArray()));
    }
    public void sendInitialEnvironment(string sessionID)
    {
        InitializeEnvironment initEnvironment = new InitializeEnvironment
        {
            environmentID = "Environment" + EnvironmentManager.instance.id
        };

        GameObject[] robots = GameObject.FindGameObjectsWithTag("Robot");
        GameObject[] vertices = GameObject.FindGameObjectsWithTag("Vertex");
        GameObject[] stations = GameObject.FindGameObjectsWithTag("Station");
        GameObject[] charger = GameObject.FindGameObjectsWithTag("Charger");
        GameObject[] products = GameObject.FindGameObjectsWithTag("Product");
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        GameObject[] racks = GameObject.FindGameObjectsWithTag("Rack");


        foreach (VirtualObject v in getVirtualObject(vertices))
            initEnvironment.Objects.Add(v);
        foreach (VirtualObject v in getVirtualObject(stations))
            initEnvironment.Objects.Add(v);
        foreach (VirtualObject v in getVirtualObject(charger))
            initEnvironment.Objects.Add(v);
        foreach (VirtualObject v in getVirtualObject(products))
            initEnvironment.Objects.Add(v);
        foreach (VirtualObject v in getVirtualObject(doors))
            initEnvironment.Objects.Add(v);
        foreach (VirtualObject v in getVirtualObject(robots))
            initEnvironment.Objects.Add(v);
        foreach (VirtualObject v in getVirtualObject(racks))
            initEnvironment.Objects.Add(v);

        MemoryStream ms = new MemoryStream();
        StreamReader sr = new StreamReader(ms);
        ProtoBuf.Serializer.Serialize(ms, initEnvironment);

        send(ms.ToArray(), sessionID);
        ms.Flush();

    }

    private List<VirtualObject> getVirtualObject(GameObject[] gobjs)
    {
        List<VirtualObject> virtualObjs = new List<VirtualObject>();
        foreach (GameObject gobj in gobjs)
        {
            string objID = gobj.name;
            string objName = gobj.name;

            float x = gobj.transform.position.x;
            float y = gobj.transform.position.y;

            string type = gobj.tag;

            VirtualObject v = new VirtualObject
            {
                objectID = objID,
                Name = objName,
                Position = new Point
                {
                    X = x,
                    Y = y
                },
                Type = type
            };
            if (type.Equals("Robot"))
            {
                Robot robot = gobj.GetComponent<Robot>();
                v.Properties.Add("speed", robot.speed.ToString());
                v.Properties.Add("turningSpeed", robot.turningSpeed.ToString());
            }
            //삭제 예정
            //환경 오브젝트 스키마를 수정하면서 vertext에 edge를 포함하도록 변경하면 삭제할 예정
            //웹 모니터에서 edge를 받기 위해 임시로 사용
            if (type.Equals("Vertex"))
            {
                List<string> vertexEdges = new List<string>();
                foreach(Edge edge in EnvironmentManager.instance.edges)
                {
                    if(edge.vertex1Id == gobj.GetComponent<EnvironmentObject>().id)
                    {
                        vertexEdges.Add(edge.vertex2Id);
                    }

                    if (edge.vertex2Id == gobj.GetComponent<EnvironmentObject>().id)
                    {
                        vertexEdges.Add(edge.vertex1Id);
                    }
                }
                
                if(vertexEdges.Count != 0)
                {
                    string[] vertexEdgesString = vertexEdges.ToArray();
                    string edges = string.Join(",", vertexEdgesString);
                    v.Properties.Add("edges", edges);
                    Debug.Log(gobj.GetComponent<EnvironmentObject>().id + ": " + edges);
                }
                
                //if (gobj.GetComponent<VertexComponent>().edges != null)
                //{
                //    string edges = string.Join(",", gobj.GetComponent<VertexComponent>().edges);
                //    v.Properties.Add("edges", edges);
                //}
            }
            
            virtualObjs.Add(v);
        }
        return virtualObjs;
    }
    
    public void sendLogMessage(LogMessage logMessage)
    {
        Changeness changeness = new Changeness
        {
            objectID = logMessage.getObjectID(),
            Content = logMessage.getContent(),
            Location = new Point
            {
                X = logMessage.getLocationX(),
                Y = logMessage.getLocationY()
            }
        };
        float targetLocationX = logMessage.getTargetLocationX();
        float targetLocationY = logMessage.getTargetLocationY();
        if (targetLocationX != 9999)
        {
            changeness.Movement = new Point{
                X = targetLocationX, 
                Y = targetLocationY 
            };
        }
        
        MemoryStream ms = new MemoryStream();
        StreamReader sr = new StreamReader(ms);
        ProtoBuf.Serializer.Serialize(ms, changeness);
        
        send(ms.ToArray());
        ms.Flush();
    }
}