//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;

namespace RosMessageTypes.Core
{
    [Serializable]
    public class GoalMsg : Message
    {
        public const string k_RosMessageName = "core_msgs/Goal";
        public override string RosMessageName => k_RosMessageName;

        public HeaderMsg header;
        public Geometry.PoseMsg pose;
        public string type;
        public string name;

        public GoalMsg()
        {
            this.header = new HeaderMsg();
            this.pose = new Geometry.PoseMsg();
            this.type = "";
            this.name = "";
        }

        public GoalMsg(HeaderMsg header, Geometry.PoseMsg pose, string type, string name)
        {
            this.header = header;
            this.pose = pose;
            this.type = type;
            this.name = name;
        }

        public static GoalMsg Deserialize(MessageDeserializer deserializer) => new GoalMsg(deserializer);

        private GoalMsg(MessageDeserializer deserializer)
        {
            this.header = HeaderMsg.Deserialize(deserializer);
            this.pose = Geometry.PoseMsg.Deserialize(deserializer);
            deserializer.Read(out this.type);
            deserializer.Read(out this.name);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.header);
            serializer.Write(this.pose);
            serializer.Write(this.type);
            serializer.Write(this.name);
        }

        public override string ToString()
        {
            return "GoalMsg: " +
            "\nheader: " + header.ToString() +
            "\npose: " + pose.ToString() +
            "\ntype: " + type.ToString() +
            "\nname: " + name.ToString();
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize);
        }
    }
}
