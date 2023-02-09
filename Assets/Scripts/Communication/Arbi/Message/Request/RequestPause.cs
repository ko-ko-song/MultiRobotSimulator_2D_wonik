using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestPause : RequestMessage
{
	private int robotID;

	public RequestPause(int robotID)
	{
		this.messageType = (int)MessageTypeEnum.MessageType.ReqPause;
		this.robotID = robotID;
	}

	public int getRobotID()
	{
		return robotID;
	}
}
