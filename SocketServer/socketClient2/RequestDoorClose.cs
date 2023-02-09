using System.Collections;
using System.Collections.Generic;
namespace socketClient2
{
	public class RequestDoorClose : RequestMessage
	{
		private int doorID;

		public RequestDoorClose(int doorID)
		{
			this.messageType = (int)MessageTypeEnum.MessageType.ReqDoorClose;
			this.doorID = doorID;
		}

		public int getDoorID()
		{
			return this.doorID;
		}
	}
}