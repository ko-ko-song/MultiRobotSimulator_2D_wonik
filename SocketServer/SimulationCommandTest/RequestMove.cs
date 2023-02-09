using System.Collections;
using System.Collections.Generic;
namespace socketClient
{
	public class RequestMove : RequestMessage
	{
		private int robotID;
		private int pathSize;
		private List<int> path;


		public RequestMove(int robotID, int pathSize, List<int> path)
		{
			this.messageType = (int)MessageTypeEnum.MessageType.ReqMove;
			this.robotID = robotID;
			this.pathSize = pathSize;
			this.path = path;
		}

		public int getRobotID()
		{
			return robotID;
		}

		public int getPathSize()
		{
			return pathSize;
		}

		public void appendPath(int nodeID)
		{
			this.path.Add(nodeID);
		}

		public List<int> getPath()
		{
			return path;
		}
	}
}