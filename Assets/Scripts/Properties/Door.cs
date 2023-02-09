using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public string id;
    
    public bool opend = false;
    //public void RequestDoorOpen(RequestMessage requestMessage)
    //{
    //    RequestDoorOpen message = (RequestDoorOpen) requestMessage;

    //    if (message.getDoorID() != this.id)
    //    {
    //        return;
    //    }
    //    SendMessageUpwards("sendAckMessage", new AckDoorOpen(this.id));

    //    if(opend) 
    //    {
    //        SendMessageUpwards("sendAckMessage", new AckEndDoorOpen(this.id, 2));
    //        return;
    //    }
    //    else
    //    {
    //        SendMessageUpwards("sendLogMessage", new LogMessage(transform.name, "openDoor", transform.position.x, transform.position.y));

    //        opend = true;
    //        ChangeColor(opend);
    //        SendMessageUpwards("sendAckMessage", new AckEndDoorOpen(this.id, 0));
    //    }

    //}

    //public void RequestDoorClose(RequestMessage requestMessage)
    //{
    //    RequestDoorClose message = (RequestDoorClose)requestMessage;

    //    if (message.getDoorID() != this.id)
    //    {
    //        return;
    //    }
    //    SendMessageUpwards("sendAckMessage", new AckDoorClose(this.id));

    //    if (!opend)
    //    {
    //        SendMessageUpwards("sendAckMessage", new AckEndDoorClose(this.id, 3));
    //        return;
    //    }

    //    else
    //    {
    //        SendMessageUpwards("sendLogMessage", new LogMessage(transform.name, "closeDoor", transform.position.x, transform.position.y));

    //        opend = false;
    //        ChangeColor(opend);
    //        SendMessageUpwards("sendAckMessage", new AckEndDoorClose(this.id, 0));
    //    }
    //}

    public bool isOpend()
    {
        return this.opend;
    }

    public void ChangeColor(bool opend)
    {
        if (opend)
        {
            Color color = Color.clear;
            ColorUtility.TryParseHtmlString("#ffffff", out color);
            gameObject.GetComponent<SpriteRenderer>().color = color;
        }
        else
        {
            Color color = Color.clear;
            ColorUtility.TryParseHtmlString("#000000", out color);
            gameObject.GetComponent<SpriteRenderer>().color = color;
        }
    }


}
