using kr.ac.uos.ai.mcmonitor;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance = null;
    private Controller controller;
    private IDEInterface ideInterface;
    private ProtoInterface protoInterface;
    
    public GameObject serverGroup;
    public InputField ideInterfaceIPInputField;
    public InputField ideInterfacePortInputField;
    public InputField webUIServerIPInputFiled;
    public InputField webUIServerPortInputFiled;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        
    }
    
    void Start()
    {
        //controller = GameObject.Find("Controller").GetComponent<Controller>();
        //arbiInterface = new ArbiInterface2(this);
        protoInterface = new ProtoInterface(this);
        ideInterface = new IDEInterface(GameObject.Find("EnvironmentManager").GetComponent<EnvironmentManager>());
        
        string localIP = GetLocalIPAddress();
        ideInterfaceIPInputField.text = localIP;
        webUIServerIPInputFiled.text = localIP;
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown("a"))
            //protoInterface.sendInitialEnvironment();
    }

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
    public void clickIDEInterfaceOepn()
    {
        ideInterface.connect(ideInterfaceIPInputField.text, ideInterfacePortInputField.text);
        StartCoroutine("checkIDEMessage");
    }

    public void clickWebUIServerOpen()
    {
        protoInterface.connect(webUIServerIPInputFiled.text, webUIServerPortInputFiled.text);
        StartCoroutine("checkUIMessage");
        StartCoroutine("Dump");
        destroyServerImage();
    }

    private void destroyServerImage()
    {
        foreach (Transform child in serverGroup.transform)
        {
            Destroy(child.gameObject);
        }
        Destroy(serverGroup);
    }
    private IEnumerator checkIDEMessage()
    {
        while (true)
        {
            //arbiInterface.CheckMessage();
            ideInterface.checkMessage();
            yield return null;
        }
    }
    
    private IEnumerator checkUIMessage()
    {
        while (true)
        {
            //arbiInterface.CheckMessage();
            protoInterface.checkClient();
            yield return null;
        }
    }
    
    //public void sendReqMessage(RequestMessage message)
    //{
    //    controller.sendReqMessage(message);
    //}

    //public void sendAckMessage(AckMessage message)
    //{
    //    arbiInterface.sendAckMessage(message);
    //}

    public void sendLogMessage(LogMessage logMessage)
    {
        protoInterface.sendLogMessage(logMessage);
    }
    
    //public void sendInitialEnvironment()
    //{
    //    protoInterface.sendInitialEnvironment();
    //}

    private IEnumerator Dump()
    {
        while (true)
        {
            protoInterface.sendDumpEnvironment();
            yield return new WaitForSeconds(3.0f);
        }
        
        //테스트용
        //yield return null;

    }


    private void OnApplicationQuit()
    {
        ideInterface.quit();
        protoInterface.quit();
    }


}
