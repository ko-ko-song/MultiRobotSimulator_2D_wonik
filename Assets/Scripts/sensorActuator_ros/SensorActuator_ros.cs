using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorActuator_ros : MonoBehaviour
{
    private Graph graph;

    private void Awake()
    {
    }

    private void Start()
    {
        graph = new Graph();
    }

    public void StopRobot(string robotID)
    {
        GameObject robotObj = GameObject.Find(robotID);
        if (robotObj == null)
        {
            Debug.Log("robot not found   | received : " + robotID);
            return;
        }
        Robot robot = robotObj.GetComponent<Robot>();
        robot.robotStatus = RobotStatusEnum.RobotStatus.IDLE;
        if (robot.behaviorCoroutine != null)
            StopCoroutine(robot.behaviorCoroutine);
    }
    public void MoveRobot(string robotID, Vector3 position, float velocity)
    {

        GameObject robotObj = GameObject.Find(robotID);
        EnvironmentObject vertex = EnvironmentManager.instance.getVertexByPosition(position);

        if (robotObj == null)
        {

            Debug.Log("robot not found   | received : " +robotID);
            return;
        }
        if (vertex == null)
        {
            Debug.Log("vertex not found    | received : " +position);
            return;
        }
        
        Robot robot = robotObj.GetComponent<Robot>();
        robot.speed = velocity;

        List<string> path = FindPath(robot.locatedVertex.id, vertex.id);
        IEnumerator coroutine = MoveThroughPath(robot, path);
        
        if (robot.behaviorCoroutine != null)
        {
            StopCoroutine(robot.behaviorCoroutine);
        }

        robot.behaviorCoroutine = coroutine;
        StartCoroutine(coroutine);
    }

    private List<string> FindPath(string locatedVertex, string goalVertex)
    {
        return graph.FindPah(locatedVertex, goalVertex);
    }


    private IEnumerator MoveThroughPath(Robot robot, List<string> path)
    {
        robot.robotStatus = RobotStatusEnum.RobotStatus.RUNNING;
        foreach (string nodeID in path)
        {
            EnvironmentObject nextVertex = EnvironmentManager.instance.getVertex(nodeID);
            float angle = calcEastThetaFromLoactedVertex(robot, nextVertex);
            if (!isRightFacingWay(robot, angle))
            {
                while (!isRightFacingWay(robot, angle))
                {
                    //while (robot.robotStatus == RobotStatusEnum.RobotStatus.Paused)
                    //{
                    //    yield return null;
                    //}
                    turnRobot(robot, angle);
                    yield return null;
                }
            }
            
            while (!isRightPosition(robot, nextVertex))
            {
                //while (robot.robotStatus == RobotStatusEnum.RobotStatus.Paused)
                //{
                //    yield return null;
                //}
                move(robot, nextVertex.transform);
                yield return null;
            }

            robot.locatedVertex = nextVertex;
            robot.locatedVertexName = nextVertex.name;
            
        }

        robot.robotStatus = RobotStatusEnum.RobotStatus.IDLE;
    }
    private float calcEastThetaFromLoactedVertex(Robot robot, EnvironmentObject nextVertex)
    {
        if (isRightPosition(robot, nextVertex))
        {
            return robot.transform.eulerAngles.z;
        }
        Vector3 targetPosition = new Vector3(nextVertex.position.x, nextVertex.position.y, robot.transform.position.z);
        Vector3 diff = targetPosition - robot.transform.position;

        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        if (angle < 0f)
            angle = angle + 360;

        return angle;
    }


    private bool isRightFacingWay(Robot robot, float angle)
    {
        float angleDiff = Mathf.Abs(angle - robot.transform.rotation.eulerAngles.z);
        if (angleDiff > 359)
            angleDiff = angleDiff - 360;

        if (angleDiff < 0.01)
            return true;
        else
            return false;
    }

    private bool isRightPosition(Robot robot, EnvironmentObject vertex)
    {
        if ((Math.Abs(vertex.position.x - robot.transform.position.x) < 0.01f)
           && (Math.Abs(vertex.position.y - robot.transform.position.y) < 0.01f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void turnRobot(Robot robot, float toAngle)
    {
        Quaternion fromRotation = robot.transform.rotation;
        Quaternion toRotation = Quaternion.Euler(0f, 0f, toAngle);
        robot.transform.rotation = Quaternion.RotateTowards(fromRotation, toRotation, Time.deltaTime * robot.turningSpeed);

    }
    
    private void move(Robot robot, Transform target)
    {
        robot.transform.position = Vector3.MoveTowards(robot.transform.position, target.position, robot.speed * Time.deltaTime);

        if (robot.loadedObject != null)
        {
            robot.loadedObject.transform.position = new Vector3(
                robot.transform.position.x,
                robot.transform.position.y,
                robot.loadedObject.transform.position.z
            );
        }

        //float speedX = direction.x * Time.deltaTime;
        //float speedY = direction.y * Time.deltaTime;

        //robot.transform.position = new Vector3(
        //    robot.transform.position.x + speedX,
        //    robot.transform.position.y + speedY,
        //    robot.transform.position.z
        //);

        //if (robot.loadedObject != null)
        //{
        //    robot.loadedObject.transform.position = new Vector3(
        //        robot.loadedObject.transform.position.x + speedX,
        //        robot.loadedObject.transform.position.y + speedY,
        //        robot.loadedObject.transform.position.z
        //    );
        //}

    }
}
