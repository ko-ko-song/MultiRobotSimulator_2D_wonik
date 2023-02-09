using WebSocketSharp.Server;
using WebSocketSharp;
using UnityEngine;
using System.Collections.Generic;

public class WebUIServer : WebSocketBehavior
{
    public List<UIClient> Clients;

    public WebUIServer(List<UIClient> clients)
    {
        this.Clients = clients;
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        Debug.Log("WebUI Interface : receive " + e.Data);
    }
    
    protected override void OnOpen()
    {
        Debug.Log("WebUI Interface : client 立加");
        Clients.Add(new UIClient()
        {
            sessionID = ID
        });
    }

    
    
    protected override void OnClose(CloseEventArgs e)
    {
        Debug.Log("WebUI Interface : client 立加 秦力");
    }

}
