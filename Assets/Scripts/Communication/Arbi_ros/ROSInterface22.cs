
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using UnityEngine;

public class ROSInterface22 : MonoBehaviour
{
    
    ROSConnection ros;
    public string topicName = "pos_rot";

    // The game object
    public GameObject cube;
    // Publish the cube's position and rotation every N seconds
    public float publishMessageFrequency = 0.5f;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;

    private Type messageType;

    void Start()
    {


        //Debug.Log("type : " + type);

        //ROSMessageManager rm = ROSMessageManager.Instance();
        //Type type = rm.getMessageType("PosRotMsg");

        
        messageType = Type.GetType("RosMessageTypes.UnityRoboticsDemo.PosRotMsg");
        Debug.Log("type : " + messageType.FullName);

        

        //start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        //ros.RegisterPublisher(topicName, "PosRotMsg");
        RegisterPublisher(ros, messageType, topicName);

    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {
            cube.transform.rotation = UnityEngine.Random.rotation;

            object instance = Activator.CreateInstance(messageType);

            FieldInfo posXField = messageType.GetField("pos_x");
            FieldInfo posYField = messageType.GetField("pos_y");
            FieldInfo posZField = messageType.GetField("pos_z");

            if (posXField != null && posYField != null && posZField != null)
            {
                posXField.SetValue(instance, cube.transform.position.x);
                posYField.SetValue(instance, cube.transform.position.y);
                posZField.SetValue(instance, cube.transform.position.z);

                // Set rotation values
                FieldInfo rotXField = messageType.GetField("rot_x");
                FieldInfo rotYField = messageType.GetField("rot_y");
                FieldInfo rotZField = messageType.GetField("rot_z");
                FieldInfo rotWField = messageType.GetField("rot_w");

                if (rotXField != null && rotYField != null && rotZField != null && rotWField != null)
                {
                    rotXField.SetValue(instance, cube.transform.rotation.x);
                    rotYField.SetValue(instance, cube.transform.rotation.y);
                    rotZField.SetValue(instance, cube.transform.rotation.z);
                    rotWField.SetValue(instance, cube.transform.rotation.w);

                    ros.Publish(topicName, (Message)instance);
                }
                else
                {
                    Debug.LogError("Rotation fields not found in PosRotMsg.");
                }
            }
            else
            {
                Debug.LogError("Position fields not found in PosRotMsg.");
            }

            timeElapsed = 0;
        }
    }

    public void RegisterPublisher(ROSConnection ros, Type messageType, string topicName)
    {
        MethodInfo method = typeof(ROSConnection).GetMethods().First(
            m => m.Name == "RegisterPublisher" && m.IsGenericMethod
        );
        MethodInfo generic = method.MakeGenericMethod(messageType);
        generic.Invoke(ros, new object[] { topicName, null, null });
    }
    
}
