using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPandPort
{
    private string ip;
    private int port;

    public IPandPort(string ip, int port)
    {
        this.ip = ip;
        this.port = port;
    }

    public string getIP()
    {
        return this.ip;
    }

    public int getPort()
    {
        return this.port;
    }

}
