using RosMessageTypes.Geometry;
using RosMessageTypes.Std;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;
using RosMessageTypes.BuiltinInterfaces;
using RosMessageTypes.Core;

public class ROSInterface : MonoBehaviour
{

    public string robotID = "";
    GameObject robotObj;

    ROSConnection ros;
    public string initPoseTopicName = "";
    public string goalTopicName = "";

    public string mapChangeRequestTopicName = "";
    public string currentPoseTopicName = "";
    public string robotStatusTopicName = "";
    public string robotStopTopicName = "";
    
    public float messageFrequency = 1.0f;

    public SensorActuator_ros sensorActuator;

    public ROSInterface(string robotID)
    {
        this.robotID = robotID;
    }

    void Start()
    {
        initPoseTopicName = robotID + "/initialpose";
        goalTopicName = robotID + "/navifra/goal_id";
        mapChangeRequestTopicName = robotID + "/map_request";
        currentPoseTopicName = robotID + "/localization/robot_pos";
        robotStatusTopicName = robotID + "/navigation/state";
        robotStopTopicName = robotID + "/stop";
        messageFrequency = 1.0f;


        robotObj = GameObject.Find(robotID);

        // start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        InitPublisher();
        InitSubscriber();
        this.sensorActuator = gameObject.AddComponent<SensorActuator_ros>();
    }


    private void InitPublisher()
    {
        ros.RegisterPublisher<PoseWithCovarianceStampedMsg>(currentPoseTopicName);
        StartCoroutine(PublishCurrentPose());

        ros.RegisterPublisher<StringMsg>(robotStatusTopicName);
        StartCoroutine(PublishStatus());

        Debug.Log(robotID + " : initPublisher");
    }



    private IEnumerator PublishCurrentPose()
    {
        while (true)
        {
            Vector3 position = robotObj.transform.position;
            Quaternion rotation = robotObj.transform.rotation;

            // Convert Unity position & rotation to ROS position & rotation
            
            Vector3 rosPosition = Utility.ChangeExitPosition(position);
            
            Quaternion rosRotation = (Quaternion)rotation.To<FLU>();

            DateTime currentTime = DateTime.Now;
            double seconds = currentTime.TimeOfDay.TotalSeconds;
            long nanoseconds = currentTime.Ticks % TimeSpan.TicksPerSecond * 100;

            // Create a ROS PoseWithCovarianceStampedMsg message
            PoseWithCovarianceStampedMsg currentPoseMsg = new PoseWithCovarianceStampedMsg
            {
                header = new HeaderMsg
                {
                    stamp = new TimeMsg
                    {
                        sec = (uint)seconds,
                        nanosec = (uint)nanoseconds
                    },
                    frame_id = "map"
                },
                pose = new PoseWithCovarianceMsg
                {
                    pose = new PoseMsg
                    {
                        position = new PointMsg(rosPosition.x, rosPosition.y, rosPosition.z),
                        orientation = new QuaternionMsg(rosRotation.x, rosRotation.y, rosRotation.z, rosRotation.w)
                    },
                    // Assume some example covariance (replace with actual values)
                    covariance = new double[36]
                }
            };

            ros.Publish(currentPoseTopicName, currentPoseMsg);
            Debug.Log("[" + robotID + "]  " + "\tx : " + position.x  + "\ty : " + position.y);
            // Wait for the next publish
            yield return new WaitForSeconds(messageFrequency);
        }
    }

    private IEnumerator PublishStatus()
    {
        while (true)
        {
            // Create a ROS message based on the current transform
            StringMsg statusMsg = new StringMsg
            {
                data = robotObj.GetComponent<Robot>().robotStatus.ToString()
            };

            Debug.Log("[" + robotID + "]  " + robotObj.GetComponent<Robot>().robotStatus.ToString());
            // Debug.Log("[" + robotID + "]  " + statusMsg);

            ros.Publish(robotStatusTopicName, statusMsg);

            yield return new WaitForSeconds(messageFrequency);
        }
    }



    private void InitSubscriber()
    {
        ros.Subscribe<PoseWithCovarianceStampedMsg>(initPoseTopicName, CallBackInitPose);
        ros.Subscribe<GoalMsg>(goalTopicName, CallbackGoal);
        ros.Subscribe<StringMsg>(mapChangeRequestTopicName, CallBackChangeMap);
        ros.Subscribe<StringMsg>(robotStopTopicName, CallBackStopRobot);
    }

    private void CallBackStopRobot(StringMsg msg)
    {
        sensorActuator.StopRobot(robotID);
    }

    private void CallBackInitPose(PoseWithCovarianceStampedMsg msg)
    {
        Vector3 position = new Vector3((float)msg.pose.pose.position.x, (float)msg.pose.pose.position.y, (float)msg.pose.pose.position.z);
        position = Utility.ChangeEntrancePosition(position);
        EnvironmentObject vertex = EnvironmentManager.instance.getVertexByPosition(position);
        robotObj.GetComponent<Robot>().locatedVertex = vertex;
        robotObj.GetComponent<Robot>().locatedVertexName = vertex.name;
        robotObj.GetComponent<Robot>().robotStatus = RobotStatusEnum.RobotStatus.IDLE;
        
        robotObj.transform.position = position;

        Debug.Log("[ROS] " + robotID + "  call back init pose " + position);
    }
    
    private void CallbackGoal(GoalMsg msg)
    {
        Vector3 position = new Vector3((float)msg.pose.position.x, (float)msg.pose.position.y, (float)msg.pose.position.z);
        position = Utility.ChangeEntrancePosition(position);
        

        string type = msg.type;

        float velocity = 0.0f;
        if (type == "normal")
        {
            velocity = 1f;
        }
        else if (type == "elevator")
        {
            velocity = 0.5f;
        }
        else if (type == "door")
        {
            velocity = 0.5f;
        }
        Debug.Log("[ROS] " + robotID + "  move to pose " + position);
        
        sensorActuator.MoveRobot(robotID, position, velocity);
    }


    private void CallBackChangeMap(StringMsg obj)
    {
        string floor = obj.data;
    }




}