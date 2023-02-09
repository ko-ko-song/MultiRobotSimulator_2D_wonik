using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandScript : MonoBehaviour
{
    public InputField inputField;
    public GameObject[] robotObejcts;
    public List<Robot> robots;

    private GameObject MOS;
    private SensorActuatorModule sensorActuatorModule;

    void Start()
    {
        Invoke("InitObj", 1f);
    }

    public void InitObj()
    {
        robots = new List<Robot>();
        robotObejcts = GameObject.FindGameObjectsWithTag("Robot");

        if(robotObejcts == null)
        {
            Invoke("InitObj", 1f);
            return;
        }

        MOS = GameObject.Find("MOS");
        if (MOS == null)
            MOS = GameObject.Find("MOS_test");

        if(MOS == null)
        {
            Invoke("InitObj", 1f);
            return;
        }

        for (int i = 0; i < robotObejcts.Length; i++)
        {
            robots.Add(robotObejcts[i].GetComponent<Robot>());
        }

        sensorActuatorModule = MOS.GetComponent<SensorActuatorModule>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            try
            {
                string[] commandArr = inputField.text.Split(' ');

                string command = commandArr[0];
                string robotId = commandArr[1];
                string pathSize = "";
                List<string> path = new List<string>();
                string targetNode = "";

                if (command.Equals("Move"))
                {
                    pathSize = commandArr[2];

                    for (int i = 0; i < int.Parse(pathSize); i++)
                    {
                        path.Add(commandArr[3 + i]);
                    }
                }
                else
                {
                    targetNode = commandArr[2];
                }

                inputField.text = string.Empty;
                inputField.placeholder.GetComponent<Text>().text = string.Empty;

                switch (command)
                {
                    case "Move":
                        string pathString = "[";
                        for (int i = 0; i < path.Count; i++)
                        {
                            if (i == path.Count - 1)
                                pathString += path[i];
                            else
                                pathString += path[i] + ",";
                        }
                        pathString += "]";

                        sensorActuatorModule.arbiInterfaceJSON.parseMessageForTest("{\"robotID\" : \"" + robotId + "\",\"mType\" : \"requestMove\",\"path\" : " + pathString + "}");
                        return;
                    case "Load":
                        sensorActuatorModule.arbiInterfaceJSON.parseMessageForTest("{\"robotID\" : \"" + robotId + "\",\"mType\" : \"requestLoad\",\"nodeID\" : " + targetNode + "}");
                        return;
                    case "unLoad":
                        sensorActuatorModule.arbiInterfaceJSON.parseMessageForTest("{\"robotID\" : \"" + robotId + "\",\"mType\" : \"requestUnload\",\"nodeID\" : " + targetNode + "}");
                        return;
                }

                inputField.text = string.Empty;
                inputField.placeholder.GetComponent<Text>().text = string.Empty;
            }
            catch(Exception e)
            {
                Debug.Log(e.ToString());

            }

        }
    }



    public void upRobotVelocity()
    {
        foreach (Robot robot in robots)
        {
            robot.speed+=0.5f;
            robot.turningSpeed = robot.turningSpeed + 3;
        }
            


    }
    public void downRobotVelocity()
    {
        foreach (Robot robot in robots)
        {
            robot.speed-=0.5f;
            robot.turningSpeed = robot.turningSpeed - 3;
        }
    }

}
