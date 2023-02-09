using kr.ac.uos.ai.mcmonitor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public static Controller instance = null;
    NetworkManager networkManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        networkManager = NetworkManager.instance;
    }
    
    public void sendReqMessage(RequestMessage message)
    {
        switch (message.getType())
        {
            case (int)MessageTypeEnum.MessageType.ReqMove:
                BroadcastMessage("RequestMove", message);
                break;
            case (int)MessageTypeEnum.MessageType.ReqCancelMove:
                BroadcastMessage("RequestCancleMove", message);
                break;
            case (int)MessageTypeEnum.MessageType.ReqLoad:
                BroadcastMessage("RequestLoad", message);
                break;
            case (int)MessageTypeEnum.MessageType.ReqUnload:
                BroadcastMessage("RequestUnload", message);
                break;
            case (int)MessageTypeEnum.MessageType.ReqCharge:
                BroadcastMessage("RequestCharge", message);
                break;
            case (int)MessageTypeEnum.MessageType.ReqChargeStop:
                BroadcastMessage("RequestChargeStop", message);
                break;
            case (int)MessageTypeEnum.MessageType.ReqPause:
                BroadcastMessage("RequestPause", message);
                break;
            case (int)MessageTypeEnum.MessageType.ReqResume:
                BroadcastMessage("RequestResume", message);
                break;
            case (int)MessageTypeEnum.MessageType.ReqDoorOpen:
                BroadcastMessage("RequestDoorOpen", message);
                break;
            case (int)MessageTypeEnum.MessageType.ReqDoorClose:
                BroadcastMessage("RequestDoorClose", message);
                break;
        }
    }
    
    //public void sendAckMessage(AckMessage message)
    //{
    //    networkManager.sendAckMessage(message);
    //}

    public void sendLogMessage(LogMessage logMessage) 
    {
        networkManager.sendLogMessage(logMessage);
    }

}





