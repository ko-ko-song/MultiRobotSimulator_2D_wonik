using System.Collections;
using System.Collections.Generic;
namespace socketClient2
{
	public class RequestResume : RequestMessage
	{
		private int robotID;
		public RequestResume(int robotID)
		{
			this.messageType = (int)MessageTypeEnum.MessageType.ReqResume;
			this.robotID = robotID;
		}

		public int getRobotID()
		{
			return robotID;
		}
	}
}