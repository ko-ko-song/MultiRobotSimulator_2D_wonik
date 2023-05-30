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

public class RosPublisher : MonoBehaviour
{

    string robotID = "";
    GameObject robotObj;

    ROSConnection ros;
    public string initPoseTopicName = "";
    public string goalTopicName = "";

    public string mapChangeRequestTopicName = "";
    public string currentPoseTopicName = "";
    public string robotStatusTopicName = "";

    public float messageFrequency = 1f;

    public RosPublisher(string robotID)
    {
        this.robotID = robotID;
    }
    
    
    void Start()
    {
        initPoseTopicName = robotID + "/initialpose";
        goalTopicName = robotID + "/map_request";
        mapChangeRequestTopicName = robotID+ "/navifra/goal_id";
        currentPoseTopicName = robotID+"/localization/robot_pos";
        robotStatusTopicName = robotID+"/status";
        

        robotObj = GameObject.Find(robotID);

        // start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        InitPublisher();
        InitSubscriber();

    }


    private void InitPublisher()
    {
        ros.RegisterPublisher<StringMsg>(mapChangeRequestTopicName);



        ros.RegisterPublisher<PoseWithCovarianceStampedMsg>(currentPoseTopicName);
        StartCoroutine(PublishCurrentPose());

        ros.RegisterPublisher<StringMsg>(robotStatusTopicName);
        StartCoroutine(PublishStatus());
    }

    public void PublishRequestMapOnce(string data)
    {
        StringMsg mapRquestMsg = new StringMsg
        {
            data = data
        };

        ros.Publish(mapChangeRequestTopicName, mapRquestMsg);
    }

    private IEnumerator PublishCurrentPose()
    {
        while (true)
        {
            Vector3 position = robotObj.transform.position;
            Quaternion rotation = robotObj.transform.rotation;

            // Convert Unity position & rotation to ROS position & rotation
            Vector3 rosPosition = (Vector3)position.To<FLU>();
            rosPosition = Utility.ChangeExitPosition(rosPosition);

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

            ros.Publish(robotStatusTopicName, statusMsg);

            yield return new WaitForSeconds(messageFrequency);
        }
    }

    





    private void InitSubscriber()
    {
        ros.Subscribe<PoseWithCovarianceStampedMsg>(initPoseTopicName, CallBackInitPose);
        ros.Subscribe<GoalMsg>(goalTopicName, CallbackGoal);
    }

    private void CallBackInitPose(PoseWithCovarianceStampedMsg msg)
    {
        Vector3 position = new Vector3((float)msg.pose.pose.position.x, (float)msg.pose.pose.position.y, (float)msg.pose.pose.position.z);
        position = Utility.ChangeEntrancePosition(position);

        robotObj.transform.position = position;

        Debug.Log("call back init pose");
    }
    
    private void CallbackGoal(GoalMsg msg)
    {
        Vector3 position = new Vector3((float)msg.pose.position.x, (float)msg.pose.position.y, (float)msg.pose.position.z);
        position = Utility.ChangeEntrancePosition(position);

        string id = msg.id;
        string name = msg.name;
        string type = msg.type;

        Debug.Log("call back goal");
    }
    
    





}
