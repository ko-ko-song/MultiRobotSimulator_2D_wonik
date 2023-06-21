using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SensingActions : MonoBehaviour
{
    private float loadTime = 0.1f;
    private float packingTime = 5f;
    private List<string> palletizerPackingNode = new List<string>() { "1", "2" };
    
    // Start is called before the first frame update
    void Start()
    {
    }

    //public void test()
    //{
    //    RequestMoveElevator(null, null, new List<string> { "1" });
    //}

    // Update is called once per frame
    void Update()
    {
        
    }


    #region actions
    public void executeAction(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance)
    {
        string functionName = "";
        List<string> functionArgs = new List<string>();
        if (actionProtocolInstance.actionInstance != null)
        {
            functionName = actionProtocolInstance.actionInstance.actionName;
            functionArgs = actionProtocolInstance.actionInstance.actionArgs;
        }
             
        StringBuilder sb = new StringBuilder();
        foreach(string arg in functionArgs)
        {
            sb.Append(arg);
            sb.Append("\t");
        }
        if(sb.Length!= 0)
        {
            Debug.Log("execute action : \n" +functionName);
        }
        
        //Debug.Log("action : " + functionName + "\t args :   " + sb.ToString());

        switch (functionName)
        {
            case "move":
                RequestMove(sensorActuatorModule, actionProtocolInstance, functionArgs);
                break;
            case "cancelMove":
                RequestCancelMove(sensorActuatorModule, actionProtocolInstance, functionArgs);
                break;
            case "load":
                RequestLoad(sensorActuatorModule, actionProtocolInstance, functionArgs);
                break;
            case "unload":
                RequestUnload(sensorActuatorModule, actionProtocolInstance, functionArgs);
                break;
            case "charge":
                RequestCharge(sensorActuatorModule, actionProtocolInstance, functionArgs);
                break;
            case "chargeStop":
                RequestChargeStop(sensorActuatorModule, actionProtocolInstance, functionArgs);
                break;
            case "pause":
                RequestPause(sensorActuatorModule, actionProtocolInstance, functionArgs);
                break;
            case "resume":
                RequestResume(sensorActuatorModule, actionProtocolInstance, functionArgs);
                break;
            case "doorOpen":
                RequestOpenDoor(sensorActuatorModule, actionProtocolInstance, functionArgs);
                break;
            case "doorClose":
                RequestCloseDoor(sensorActuatorModule, actionProtocolInstance, functionArgs);
                break;
            case "personCall":
                break;
            case "login":
                RequestLogin(sensorActuatorModule, actionProtocolInstance, functionArgs);
                break;
            case "guideMove":
                RequestPreciseMove(sensorActuatorModule, actionProtocolInstance, functionArgs);
                break;
            case "preciseMove":
                RequestPreciseMove(sensorActuatorModule, actionProtocolInstance, functionArgs);
                break;
            case "straightBackMove":
                RequestStraightBackMove(sensorActuatorModule, actionProtocolInstance, functionArgs);
                break;
            case "palletizerStart":
                RequestPalletizerStart(sensorActuatorModule, actionProtocolInstance, functionArgs);
                break;
            case "palletizerStop":
                RequestPalletizerStop(sensorActuatorModule, actionProtocolInstance, functionArgs);
                break;
            case "openDoor":
                RequestOpenDoor(sensorActuatorModule, actionProtocolInstance, functionArgs);
                break;
            case "closeDoor":
                RequestCloseDoor(sensorActuatorModule, actionProtocolInstance, functionArgs);
                break;
            case "openElevatorDoor":
                RequestOpenElevatorDoor(sensorActuatorModule, actionProtocolInstance, functionArgs);
                break;
            case "closeElevatorDoor":
                RequestCloseElevatorDoor(sensorActuatorModule, actionProtocolInstance, functionArgs);
                break;
            case "moveElevator":
                RequestMoveElevator(sensorActuatorModule, actionProtocolInstance, functionArgs);
                break;
            default:
                Debug.Log("funtion name undefined   may be palletizer Enter, Exit" + functionName);
                break;
        }
        
    }

    private void RequestCloseElevatorDoor(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, List<string> functionArgs)
    {
        string doorId = functionArgs[0];
        string floor = functionArgs[1];
        
        GameObject gobj = GameObject.Find(doorId+"_"+floor);
        if (gobj == null)
        {
            Debug.Log("해당 id의 elevator door을 찾을 수 없음");
            return;
        }
        Robot door = gobj.GetComponent<Robot>();

        IEnumerator coroutine = closeElevatorDoor(sensorActuatorModule, actionProtocolInstance, door, functionArgs);

        if (door.behaviorCoroutine != null)
        {
            StopCoroutine(door.behaviorCoroutine);
        }

        door.behaviorCoroutine = coroutine;
        StartCoroutine(coroutine);
    }

    private IEnumerator closeElevatorDoor(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, Robot door, List<string> functionArgs)
    {
        float closingTime = 3f;

        float originalSizeX = door.gameObject.GetComponent<EnvironmentObject>().size.x;
        float originalSizeY = door.gameObject.GetComponent<EnvironmentObject>().size.y;

        if (actionProtocolInstance.getProtocolType().Equals("result"))
        {
            sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(3));
        }

        while (door.transform.localScale.x < originalSizeX)
        {
            float increasingRatio = originalSizeX / closingTime * Time.deltaTime;
            door.transform.localScale = new Vector2(door.transform.localScale.x + increasingRatio, originalSizeY);
            door.transform.position = new Vector2(door.transform.position.x + increasingRatio / 2, door.transform.position.y);

            yield return null;
        }

        while (door.transform.localScale.y < originalSizeY)
        {
            float increasingRatio = originalSizeY / closingTime * Time.deltaTime;
            door.transform.localScale = new Vector2(originalSizeX, door.transform.localScale.y + increasingRatio);
            door.transform.position = new Vector2(door.transform.position.x , door.transform.position.y + increasingRatio / 2);
            
            yield return null;
        }

        if (actionProtocolInstance.getProtocolType().Equals("result"))
        {
            sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(2));
        }

        door.robotStatus = RobotStatusEnum.RobotStatus.Ready;
    }

    private IEnumerator openElevatorDoor(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, Robot door, List<string> functionArgs)
    {
        float openingTime = 3f;
        float originalSizeX = door.transform.localScale.x;
        float originalSizeY = door.transform.localScale.y;

        if (actionProtocolInstance.getProtocolType().Equals("result"))
        {
            sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(1));
        }

        if (door.transform.localScale.x > door.transform.localScale.y)
        {
            while (door.transform.localScale.x > 0.1f)
            {
                float decreasingRatio = originalSizeX / openingTime * Time.deltaTime;
                door.transform.localScale = new Vector2(door.transform.localScale.x - decreasingRatio, originalSizeY);
                door.transform.position = new Vector2(door.transform.position.x - decreasingRatio / 2, door.transform.position.y);
                yield return null;
            }
        }
        else
        {
            while (door.transform.localScale.y > 0.1f)
            {
                float decreasingRatio = originalSizeY / openingTime * Time.deltaTime;
                door.transform.localScale = new Vector2(originalSizeX, door.transform.localScale.y - decreasingRatio);
                door.transform.position = new Vector2(door.transform.position.x, originalSizeY + decreasingRatio / 2);
                yield return null;

            }
        }

        if (actionProtocolInstance.getProtocolType().Equals("result"))
        {
            sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(0));
        }
        
        door.robotStatus = RobotStatusEnum.RobotStatus.Ready;
    }

    private void RequestOpenElevatorDoor(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, List<string> functionArgs)
    {
        string doorId = functionArgs[0];
        string floor = functionArgs[1];

        GameObject gobj = GameObject.Find(doorId + "_" + floor);
        if (gobj == null)
        {
            Debug.Log("해당 id의 elevator door을 찾을 수 없음");
            return;
        }
        Robot door = gobj.GetComponent<Robot>();

        IEnumerator coroutine = openElevatorDoor(sensorActuatorModule, actionProtocolInstance, door, functionArgs);

        if (door.behaviorCoroutine != null)
        {
            StopCoroutine(door.behaviorCoroutine);
        }

        door.behaviorCoroutine = coroutine;
        StartCoroutine(coroutine);
    }

    private void RequestMoveElevator(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, List<string> functionArgs)
    {
        string elevatorID = functionArgs[0];
        string goalFloor = functionArgs[1];
        GameObject elevator_vertex = null;
        List<GameObject> robots = new List<GameObject>();
        GameObject elevator = GameObject.Find(elevatorID);

        if (goalFloor == "1")
        {
            elevator_vertex = GameObject.Find("elevator2_in_vertex");
        }
        else if(goalFloor == "2")
        {
            elevator_vertex = GameObject.Find("elevator1_in_vertex");
        }
        else
        {
            Debug.Log("floor err : " + goalFloor);
        }

        if (elevator_vertex == null)
            return;
        
        foreach (GameObject gobj in GameObject.FindGameObjectsWithTag("Robot"))
        {
            float distance = Vector2.Distance(elevator_vertex.transform.position, gobj.transform.position);
            if (distance < 1.5f)
            {
                robots.Add(gobj);
            }
        }

        


        IEnumerator coroutine = moveElevator(sensorActuatorModule, actionProtocolInstance, elevator, robots, goalFloor);
        StartCoroutine(coroutine);
    }
    
    private IEnumerator moveElevator(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, GameObject elevator, List<GameObject> robots, string floor)
    {
        if(elevator == null)
        {
            Debug.Log("can't find elevator object");
            yield break;
        }
        float floorFloat = float.Parse(floor);
        
        if (Mathf.Abs(elevator.transform.position.z - floorFloat) < 0.1f)
        {
            if (actionProtocolInstance.getProtocolType().Equals("result"))
            {
                sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(Int32.Parse(floor)));
            }

            yield break;
        }
        
        float elapsedTime = 0f;
        float movementTime = 3f;
        Vector3 startPosition = elevator.transform.position;
        float differenceZ = Mathf.RoundToInt(floorFloat - elevator.transform.position.z);
        
        Vector3 targetPosition = new Vector3(elevator.transform.position.x, elevator.transform.position.y + Utility.zOffset*differenceZ, float.Parse(floor));
        
        while (elapsedTime < movementTime)
        {
            elevator.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / movementTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        foreach (GameObject robotObj in robots)
        {
            Robot robot = robotObj.GetComponent<Robot>();
            Vector3 robotPosition = robotObj.transform.position;
            if (floor == "1")
            {
                robotObj.transform.position = new Vector3(robotPosition.x, robotPosition.y - Utility.zOffset, 1);
                robot.locatedVertex = EnvironmentManager.instance.getVertexByPosition(robot.transform.position);
            }
            else if (floor == "2")
            {
                robotObj.transform.position = new Vector3(robotPosition.x, robotPosition.y + Utility.zOffset, 2);
                robot.locatedVertex = EnvironmentManager.instance.getVertexByPosition(robot.transform.position);
            }
        }
       
        if (actionProtocolInstance.getProtocolType().Equals("result"))
        {
            sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(Int32.Parse(floor)));
        }

    }

    private void RequestOpenDoor(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, List<string> functionArgs)
    {        
        string doorId = functionArgs[0];
        GameObject gobj = GameObject.Find(doorId);
        
        if (gobj == null)
        {
            Debug.Log("해당 id의 door을 찾을 수 없음");
            return;
        }
        Robot door = gobj.GetComponent<Robot>();
        
        
        IEnumerator coroutine = openDoor(sensorActuatorModule, actionProtocolInstance, door, functionArgs);
        
        if (door.behaviorCoroutine != null)
        {
            StopCoroutine(door.behaviorCoroutine);
        }
        
        door.behaviorCoroutine = coroutine;
        StartCoroutine(coroutine);
    }

    private IEnumerator openDoor(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, Robot door, List<string> functionArgs)
    {

        float openingTime = 3f;
        float originalSizeX = door.transform.localScale.x;
        float originalSizeY = door.transform.localScale.y;

        if (door.transform.localScale.x > door.transform.localScale.y)
        {
            while (door.transform.localScale.x > 0.1f)
            {
                float decreasingRatio = originalSizeX / openingTime * Time.deltaTime;
                door.transform.localScale = new Vector2(door.transform.localScale.x - decreasingRatio, originalSizeY);
                door.transform.position= new Vector2(door.transform.position.x - decreasingRatio/2, door.transform.position.y);
                yield return null;
            }
        }
        else
        {
            while (door.transform.localScale.y > 0.1f)
            {
                float decreasingRatio = originalSizeY / openingTime * Time.deltaTime;
                door.transform.localScale = new Vector2(originalSizeX, door.transform.localScale.y - decreasingRatio);
                door.transform.position = new Vector2(door.transform.position.x, originalSizeY - decreasingRatio / 2);
                yield return null;
                
            }
        }

        if (actionProtocolInstance.getProtocolType().Equals("result"))
        {
            sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(0));
        }
        
        door.robotStatus = RobotStatusEnum.RobotStatus.Ready;
    }
    
    private void RequestCloseDoor(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, List<string> functionArgs)
    {
        string doorId = functionArgs[0];
        GameObject gobj = GameObject.Find(doorId);

        if (gobj == null)
        {
            Debug.Log("해당 id의 door을 찾을 수 없음");
            return;
        }
        Robot door = gobj.GetComponent<Robot>();

        IEnumerator coroutine = closeDoor(sensorActuatorModule, actionProtocolInstance, door, functionArgs);

        if (door.behaviorCoroutine != null)
        {
            StopCoroutine(door.behaviorCoroutine);
        }

        door.behaviorCoroutine = coroutine;
        StartCoroutine(coroutine);
    }

    private IEnumerator closeDoor(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, Robot door, List<string> functionArgs)
    {
        float closingTime = 3f;

        float originalSizeX = door.gameObject.GetComponent<EnvironmentObject>().size.x;
        float originalSizeY = door.gameObject.GetComponent<EnvironmentObject>().size.y;

        while (door.transform.localScale.x < originalSizeX)
        {
            float increasingRatio = originalSizeX / closingTime * Time.deltaTime;
            door.transform.localScale = new Vector2(door.transform.localScale.x + increasingRatio, originalSizeY);
            door.transform.position = new Vector2(door.transform.position.x + increasingRatio / 2, door.transform.position.y);


            yield return null;
        }
        
        while (door.transform.localScale.y < originalSizeY)
        {
            float increasingRatio = originalSizeY / closingTime * Time.deltaTime;
            door.transform.localScale = new Vector2(originalSizeX , door.transform.localScale.y + increasingRatio);
            door.transform.position = new Vector2(door.transform.position.x, originalSizeY + increasingRatio / 2);

            yield return null;
        }
        
        if (actionProtocolInstance.getProtocolType().Equals("result"))
        {
            sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(0));
        }

        door.robotStatus = RobotStatusEnum.RobotStatus.Ready;
    }
    
    private void RequestPalletizerStop(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, List<string> functionArgs)
    {
        string robotId = functionArgs[0];
        GameObject gobj = GameObject.Find(robotId);
        if (gobj == null)
        {
            return;
        }
        Robot robot = gobj.GetComponent<Robot>();
        functionArgs.RemoveAt(0);

        if (robot.behaviorCoroutine != null)
            StopCoroutine(robot.behaviorCoroutine);

        robot.robotStatus = RobotStatusEnum.RobotStatus.Ready;

        if (actionProtocolInstance.getProtocolType().Equals("result"))
        {
            sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(0));
        }
    }
    
    private void RequestPalletizerStart(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, List<string> functionArgs)
    {
        
        string robotId = functionArgs[0];
        GameObject gobj = GameObject.Find(robotId);

        if (gobj == null)
        {
            return;
        }
        Robot robot = gobj.GetComponent<Robot>();
        functionArgs.RemoveAt(0);
        
        IEnumerator coroutine = startPacking(sensorActuatorModule, actionProtocolInstance, robotId, functionArgs);
        if (robot.behaviorCoroutine != null)
        {
            StopCoroutine(robot.behaviorCoroutine);
        }

        robot.behaviorCoroutine = coroutine;
        StartCoroutine(coroutine);

        if (actionProtocolInstance.getProtocolType().Equals("result"))
        {
            sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(0));
        }
    }

    private IEnumerator startPacking(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, string robotId, List<string> functionArgs)
    {
        GameObject gobj = GameObject.Find(robotId);
        Robot robot = gobj.GetComponent<Robot>();
        foreach (string nodeId in palletizerPackingNode)
        {
            robot.robotStatus = RobotStatusEnum.RobotStatus.Packing;
            yield return new WaitForSecondsRealtime(packingTime);
            if (robot.robotStatus != RobotStatusEnum.RobotStatus.Packing)
                StopCoroutine(robot.behaviorCoroutine);
            SimulatorEventGenerator seg = GameObject.Find("EventGenerator").GetComponent<SimulatorEventGenerator>();
            
            ActionProtocol actionProtocol = sensorActuatorModule.getActionProtocol("PalletizerPackingFinish");
            if (actionProtocol == null)
            {
                Debug.Log("PalletizerPackingFinish protocol not found ");
                StopCoroutine(robot.behaviorCoroutine);
            }
            ActionProtocolInstance actionProtocolInstance2 = actionProtocol.getInstance(new byte[0]);
            
            bool isCreated = seg.CreateProduct(nodeId);
            if (isCreated) {
                sensorActuatorModule.sendMessgae(actionProtocolInstance2.getResultMessage(Int32.Parse(robotId),Int32.Parse(nodeId)));
            }
        }
        robot.robotStatus = RobotStatusEnum.RobotStatus.Ready;
    }

    private void RequestLogin(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, List<string> functionArgs)
    {
        string robotId = functionArgs[0];
        sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(0));
    }
    private void RequestDoorClose(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, List<string> functionArgs)
    {
        
    }

    private void RequestDoorOpen(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, List<string> functionArgs)
    {
        
    }

    private void RequestMove(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, List<string> functionArgs)
    {
        
        string robotId = functionArgs[0];
        GameObject gobj = GameObject.Find(robotId);
        
        //GameObject[] robotGobjs = GameObject.FindGameObjectsWithTag("Robot");
        //foreach(GameObject robotGobj in robotGobjs)
        //{
        //    if (robotGobj.GetComponent<Robot>().id == robotId)
        //    {
        //        gobj = robotGobj;
        //        break;
        //    }
        //}
        
        if (gobj == null)
        {
            return;
        }
        Robot robot = gobj.GetComponent<Robot>();
        functionArgs.RemoveAt(0);

        IEnumerator coroutine = moveGoalVertex(sensorActuatorModule, actionProtocolInstance, robotId, functionArgs);

        if (robot.behaviorCoroutine != null)
        {
            StopCoroutine(robot.behaviorCoroutine);
        }
        
        robot.behaviorCoroutine = coroutine;
        StartCoroutine(coroutine);
    }

    private void RequestCancelMove(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, List<string> functionArgs)
    {
        string robotId = functionArgs[0];
        GameObject gobj = GameObject.Find(robotId);
        if (gobj == null)
        {
            return;
        }
        Robot robot = gobj.GetComponent<Robot>();
        functionArgs.RemoveAt(0);

        if (!(robot.robotStatus == RobotStatusEnum.RobotStatus.Move))
        {
            Debug.Log(robot.gameObject.name + " : 현재 " + robot.robotStatus + " 상태입니다");
            return;
        }
        if (robot.behaviorCoroutine != null)
            StopCoroutine(robot.behaviorCoroutine);
        
        robot.robotStatus = RobotStatusEnum.RobotStatus.Ready;

        if (actionProtocolInstance.getProtocolType().Equals("result"))
        {
            sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(0));
        }
        
    }


    #region move
    private IEnumerator moveGoalVertex(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, string robotId, List<string> functionArgs)
    {
        GameObject gobj = GameObject.Find(robotId);
        Robot robot = gobj.GetComponent<Robot>();
        robot.robotStatus = RobotStatusEnum.RobotStatus.Move;
        List<string> path = functionArgs;
        foreach (string nodeID in path)
        {
            EnvironmentObject nextVertex = EnvironmentManager.instance.getVertex(nodeID);
            float angle = calcEastThetaFromLoactedVertex(robot, nextVertex);
            //float diffAngleFacinwayWithNextVertex = calcDiffAngle(angle);
            bool turningClockwiseDirection = isClockwiseDirection(robot, angle);

            //float turnX = turningSpeed * Mathf.Cos(Mathf.Deg2Rad * diffAngleFacinwayWithNextVertex);
            //float turnY = turningSpeed * Mathf.Sin(Mathf.Deg2Rad * diffAngleFacinwayWithNextVertex);
            if (!isRightFacingWay(robot, angle))
            {
                if (turningClockwiseDirection)
                {
                    SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "turn", robot.transform.position.x, robot.transform.position.y, -robot.turningSpeed*Time.deltaTime, angle));
                }
                else
                {
                    SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "turn", robot.transform.position.x, robot.transform.position.y, robot.turningSpeed*Time.deltaTime, angle));
                }
                while (!isRightFacingWay(robot, angle))
                {
                    while (robot.robotStatus == RobotStatusEnum.RobotStatus.Paused)
                    {
                        yield return null;
                    }
                    turnRobot(robot, angle);
                    yield return null;
                }
                SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "endTurn", robot.transform.position.x, robot.transform.position.y, 0, robot.transform.eulerAngles.z));
            }

            Vector3 moveVector = calcVector(robot, nextVertex);
            SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "move", nextVertex.position.x, nextVertex.position.y, moveVector.x, moveVector.y));
            if (robot.loadedObject != null)
            {
                SendMessageUpwards("sendLogMessage", new LogMessage(robot.loadedObject.name, "move", nextVertex.position.x, nextVertex.position.y, moveVector.x, moveVector.y));
            }
            while (!isRightPosition(robot, nextVertex))
            {
                while (robot.robotStatus == RobotStatusEnum.RobotStatus.Paused)
                {
                    yield return null;
                }
                move(robot, nextVertex.transform);
                yield return null;
            }
            SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "endMove", robot.transform.position.x, robot.transform.position.y, 0, 0));
            if (robot.loadedObject != null)
            {
                SendMessageUpwards("sendLogMessage", new LogMessage(robot.loadedObject.name, "endMove", robot.transform.position.x, robot.transform.position.y, 0, 0));
            }

            robot.locatedVertex = nextVertex;
            robot.locatedVertexName = nextVertex.name;

            //remainingBattery = remainingBattery - 1;
        }

        if (actionProtocolInstance.getProtocolType().Equals("result"))
        {
            sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(0));
        }

        robot.robotStatus = RobotStatusEnum.RobotStatus.Ready;
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

    private bool isClockwiseDirection(Robot robot, float angle)
    {
        float facinWayAngle;
        if (robot.transform.eulerAngles.z <= 180f)
        {
            facinWayAngle = robot.transform.eulerAngles.z;
        }
        else
        {
            facinWayAngle = robot.transform.eulerAngles.z - 360f;
        }
        if (angle > 180f)
        {
            angle = angle - 360;
        }
        float differenceAngle = angle - facinWayAngle;
        if (differenceAngle < 0)
        {
            differenceAngle = differenceAngle + 360;
        }
        if (differenceAngle < 180)
        {
            return false;
        }
        else
        {
            return true;
        }

    }


    private bool isRightFacingWay(Robot robot, float angle)
    {

        if (Mathf.Abs(angle - robot.transform.rotation.eulerAngles.z) < 0.01) { 

            return true;
        }
        else
        {
            return false;
        }
        
        //float facinWayAngle;
        //if (robot.transform.eulerAngles.z <= 180f)
        //{
        //    facinWayAngle = robot.transform.eulerAngles.z;
        //}
        //else
        //{
        //    facinWayAngle = robot.transform.eulerAngles.z - 360f;
        //}
        //if (angle > 180f)
        //{
        //    angle = angle - 360;
        //}
        //float diffAngle = angle - facinWayAngle;
        
        //if (Math.Abs(diffAngle) < 0.75f)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
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

    //private void turnRobot(Robot robot, bool isClockwise)
    //{

    //    if (isClockwise)
    //    {
    //        robot.transform.Rotate(0.0f, 0.0f, -robot.turningSpeed*Time.deltaTime);
    //    }
    //    else
    //    {
    //        robot.transform.Rotate(0.0f, 0.0f, robot.turningSpeed*Time.deltaTime);
    //    }
    //}
    private Vector3 calcVector(Robot robot, EnvironmentObject vertex)
    {
        Vector3 v = new Vector3(vertex.position.x - robot.transform.position.x, vertex.position.y - robot.transform.position.y, 0);

        Vector3 direction = v / Vector3.Magnitude(v);

        Vector3 vector = direction * robot.speed;

        return vector;
    }

    private IEnumerator moveBack(Robot robot, string vertexID)
    {
        EnvironmentObject vertex = EnvironmentManager.instance.getVertex(vertexID);

        Vector3 moveVector = calcVector(robot, vertex);

        SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "move", vertex.position.x, vertex.position.y, moveVector.x, moveVector.y));
        if (robot.loadedObject != null)
        {
            SendMessageUpwards("sendLogMessage", new LogMessage(robot.loadedObject.name, "move", vertex.position.x, vertex.position.y, moveVector.x, moveVector.y));
        }
        while (!isRightPosition(robot, vertex))
        {
            while (robot.robotStatus == RobotStatusEnum.RobotStatus.Paused)
            {
                yield return null;
            }
            move(robot, vertex.transform);
            yield return null;
        }
        SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "endMove", robot.transform.position.x, robot.transform.position.y, 0, 0));
        if (robot.loadedObject != null)
        {
            SendMessageUpwards("sendLogMessage", new LogMessage(robot.loadedObject.name, "endMove", robot.transform.position.x, robot.transform.position.y, 0, 0));
        }
        robot.locatedVertex = vertex;
        robot.locatedVertexName = vertex.name;

        //remainingBattery = remainingBattery - 1;

        robot.robotStatus = RobotStatusEnum.RobotStatus.Ready;
    }

    private IEnumerator moveOneVertex(Robot robot, string nodeID)
    {
        EnvironmentObject vertex = EnvironmentManager.instance.getVertex(nodeID);

        float angle = calcEastThetaFromLoactedVertex(robot, vertex);
        //float diffAngleFacinwayWithNextVertex = calcDiffAngle(angle);

        bool turningClockwiseDirection = isClockwiseDirection(robot, angle);

        //float turnX = turningSpeed * Mathf.Cos(Mathf.Deg2Rad * diffAngleFacinwayWithNextVertex);
        //float turnY = turningSpeed * Mathf.Sin(Mathf.Deg2Rad * diffAngleFacinwayWithNextVertex);

        if (!isRightFacingWay(robot, angle))
        {
            //SendMessageUpwards("sendLogMessage", new LogMessage(transform.name, "turn", transform.position.x, transform.position.y, turnX, turnY));
            if (turningClockwiseDirection)
            {
                SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "turn", robot.transform.position.x, robot.transform.position.y, -robot.turningSpeed*Time.deltaTime, angle));
            }
            else
            {
                SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "turn", robot.transform.position.x, robot.transform.position.y, robot.turningSpeed*Time.deltaTime, angle));
            }


            while (!isRightFacingWay(robot, angle))
            {
                while (robot.robotStatus == RobotStatusEnum.RobotStatus.Paused)
                {
                    yield return null;
                }
                turnRobot(robot, angle);
                yield return null;
            }
            SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "endTurn", robot.transform.position.x, robot.transform.position.y, 0, robot.transform.eulerAngles.z));
        }

        Vector3 moveVector = calcVector(robot, vertex);

        SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "move", vertex.position.x, vertex.position.y, moveVector.x, moveVector.y));
        if (robot.loadedObject != null)
        {
            SendMessageUpwards("sendLogMessage", new LogMessage(robot.loadedObject.name, "move", vertex.position.x, vertex.position.y, moveVector.x, moveVector.y));
        }

        while (!isRightPosition(robot, vertex))
        {
            while (robot.robotStatus == RobotStatusEnum.RobotStatus.Paused)
            {
                yield return null;
            }
            move(robot, vertex.transform);
            yield return null;
        }
        SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "endMove", robot.transform.position.x, robot.transform.position.y, 0, 0));
        if (robot.loadedObject != null)
        {
            SendMessageUpwards("sendLogMessage", new LogMessage(robot.loadedObject.name, "endMove", robot.transform.position.x, robot.transform.position.y, 0, 0));
        }
        robot.locatedVertex = vertex;
        robot.locatedVertexName = vertex.name;

        //remainingBattery = remainingBattery - 1;
    }

    private void move(Robot robot, Transform target)
    {

        
        robot.transform.position = Vector2.MoveTowards(robot.transform.position, target.position, robot.speed * Time.deltaTime);

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
    #endregion

    private void RequestLoad(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, List<string> functionArgs)
    {
        string robotId = functionArgs[0];
        GameObject gobj = GameObject.Find(robotId);
        if (gobj == null)
        {
            return;
        }
        Robot robot = gobj.GetComponent<Robot>();
        functionArgs.RemoveAt(0);

        IEnumerator coroutine = Load(sensorActuatorModule, actionProtocolInstance, robotId, functionArgs);
        if (robot.behaviorCoroutine != null)
        {
            StopCoroutine(robot.behaviorCoroutine);
        }
        robot.behaviorCoroutine = coroutine;
        StartCoroutine(coroutine);

    }
    public IEnumerator Load(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, string robotId, List<string> functionArgs)
    {
        GameObject gobj = GameObject.Find(robotId);
        Robot robot = gobj.GetComponent<Robot>();
        robot.robotStatus = RobotStatusEnum.RobotStatus.Loading;

        string nodeID = functionArgs[0];
        
        foreach (GameObject aroundGOBJ in robot.aroundObjects)
        {
            EnvironmentObject eobj = aroundGOBJ.GetComponent<EnvironmentObject>();

            if (eobj.simulationProperties.Contains("carryable"))
            {
                robot.loadedObject = aroundGOBJ;
                robot.loading = true;
                SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "load", robot.transform.position.x, robot.transform.position.y));
                yield return new WaitForSeconds(loadTime);
                SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "endLoad", robot.transform.position.x, robot.transform.position.y));
                SendMessageUpwards("sendLogMessage", new LogMessage(robot.loadedObject.name, "loaded", robot.transform.position.x, robot.transform.position.y));
                Debug.Log(robot.gameObject.name + " : load " + robot.loadedObject.name);
                break;

            }
        }

        if (robot.loadedObject == null)
        {
            if (actionProtocolInstance.getProtocolType().Equals("result"))
            {
                sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(13));
                Debug.Log(robot.gameObject.name + " : can't find loadable object");
            }
        }

        if (robot.loadedObject != null)
        {
            if (actionProtocolInstance.getProtocolType().Equals("result"))
            {
                sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(0));
            }
        }
        robot.robotStatus = RobotStatusEnum.RobotStatus.Ready;
    }

    private void RequestUnload(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, List<string> functionArgs)
    {
        string robotId = functionArgs[0];
        GameObject gobj = GameObject.Find(robotId);
        if (gobj == null)
        {
            return;
        }
        Robot robot = gobj.GetComponent<Robot>();
        functionArgs.RemoveAt(0);

        IEnumerator coroutine = Unload(sensorActuatorModule, actionProtocolInstance, robotId, functionArgs);
        if (robot.behaviorCoroutine != null)
        {
            StopCoroutine(robot.behaviorCoroutine);
        }
        robot.behaviorCoroutine = coroutine;
        StartCoroutine(coroutine);
    }

    public IEnumerator Unload(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, string robotId, List<string> functionArgs)
    {
        GameObject gobj = GameObject.Find(robotId);
        Robot robot = gobj.GetComponent<Robot>();
        robot.robotStatus = RobotStatusEnum.RobotStatus.Unloading;
       

        string startVertexID = robot.locatedVertex.name;
        string nodeID = functionArgs[0];

        SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "unload", robot.transform.position.x, robot.transform.position.y));
        yield return new WaitForSeconds(loadTime);
        SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "endUnload", robot.transform.position.x, robot.transform.position.y));
        Debug.Log(robot.gameObject.name + " : unload : " + robot.loadedObject.name);
        SendMessageUpwards("sendLogMessage", new LogMessage(robot.loadedObject.name, "unloaded", robot.transform.position.x, robot.transform.position.y));
        robot.loading = false;
        robot.loadedObject = null;
            
        if (robot.loadedObject != null)
        {
            Debug.Log(robot.gameObject.name + " : unload fail");
            if (actionProtocolInstance.getProtocolType().Equals("result"))
            {
                sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(4));
            }
        }

        if (robot.loadedObject == null)
        {
            if (actionProtocolInstance.getProtocolType().Equals("result"))
            {
                sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(0));
            }
        }

        robot.robotStatus = RobotStatusEnum.RobotStatus.Ready;
    }

    private void RequestCharge(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, List<string> functionArgs)
    {
        string robotId = functionArgs[0];
        GameObject gobj = GameObject.Find(robotId);
        if (gobj == null)
        {
            return;
        }
        Robot robot = gobj.GetComponent<Robot>();
        functionArgs.RemoveAt(0);

        IEnumerator coroutine = charge(sensorActuatorModule, actionProtocolInstance, robotId, functionArgs);
        if (robot.behaviorCoroutine != null)
        {
            StopCoroutine(robot.behaviorCoroutine);
        }
        robot.behaviorCoroutine = coroutine;
        StartCoroutine(coroutine);
    }

    private IEnumerator charge(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, string robotId, List<string> functionArgs)
    {
        GameObject gobj = GameObject.Find(robotId);
        Robot robot = gobj.GetComponent<Robot>();
        robot.robotStatus = RobotStatusEnum.RobotStatus.Unloading;

        string nodeID = functionArgs[0];

        string startVertexID = robot.locatedVertex.name;
        robot.robotStatus = RobotStatusEnum.RobotStatus.ChargeIn;

        yield return moveOneVertex(robot, nodeID);

        //if (!checkObjectIsAround("Charger"))
        //{
        //    SendMessageUpwards("sendAckMessage", new AckEndCharge(this.id, 15));
        //    Debug.Log("충전가능한 Charger가 없습니다");
        //    moveBack(startVertexID);
        //}
        SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "charge", robot.transform.position.x, robot.transform.position.y));

        Debug.Log(robot.gameObject.name + " : charge start");
        robot.robotStatus = RobotStatusEnum.RobotStatus.Charging;

        if (actionProtocolInstance.getProtocolType().Equals("result"))
        {
            sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(0));
        }

        while (robot.maxinumBattery != robot.remainingBattery)
        {
            while (robot.robotStatus == RobotStatusEnum.RobotStatus.Paused)
            {
                yield return null;
            }
            robot.remainingBattery = robot.remainingBattery + 1;
            yield return new WaitForSecondsRealtime(0.5f);
        }

        Debug.Log(robot.gameObject.name + " : charge success");
        robot.robotStatus = RobotStatusEnum.RobotStatus.Ready;
    }

    private void RequestChargeStop(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, List<string> functionArgs)
    {
        string robotId = functionArgs[0];
        GameObject gobj = GameObject.Find(robotId);
        if (gobj == null)
        {
            return;
        }
        Robot robot = gobj.GetComponent<Robot>();
        functionArgs.RemoveAt(0);

        IEnumerator coroutine = chargeStop(sensorActuatorModule, actionProtocolInstance, robotId, functionArgs);
        if (robot.behaviorCoroutine != null)
        {
            StopCoroutine(robot.behaviorCoroutine);
        }
        robot.behaviorCoroutine = coroutine;
        StartCoroutine(coroutine);
    }

    private IEnumerator chargeStop(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, string robotId, List<string> functionArgs)
    {
        GameObject gobj = GameObject.Find(robotId);
        Robot robot = gobj.GetComponent<Robot>();
        robot.robotStatus = RobotStatusEnum.RobotStatus.Unloading;

        string nodeID = functionArgs[0];

        if (!(robot.robotStatus == RobotStatusEnum.RobotStatus.Charging))
        {
            if (actionProtocolInstance.getProtocolType().Equals("result"))
            {
                sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(16));
            }
            yield break;
        }

        StopCoroutine(robot.behaviorCoroutine);
        SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "EndCharge", robot.transform.position.x, robot.transform.position.y));
        Debug.Log(robot.gameObject.name + " : charge stop");
        robot.robotStatus = RobotStatusEnum.RobotStatus.ChargeOut;
        yield return moveBack(robot, nodeID);
        if (actionProtocolInstance.getProtocolType().Equals("result"))
        {
            sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(0));
        }
        robot.robotStatus = RobotStatusEnum.RobotStatus.Ready;
    }

    private void RequestPause(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, List<string> functionArgs)
    {
        string robotId = functionArgs[0];
        GameObject gobj = GameObject.Find(robotId);
        if (gobj == null)
        {
            return;
        }
        Robot robot = gobj.GetComponent<Robot>();
        functionArgs.RemoveAt(0);

        robot.previousRobotStatus = robot.robotStatus;
        robot.robotStatus = RobotStatusEnum.RobotStatus.Paused;
        Debug.Log(robot.gameObject.name + " : pause");

        if (actionProtocolInstance.getProtocolType().Equals("result"))
        {
            sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(0));
        }

    }
    private void RequestResume(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, List<string> functionArgs)
    {
        string robotId = functionArgs[0];
        GameObject gobj = GameObject.Find(robotId);
        if (gobj == null)
        {
            return;
        }
        Robot robot = gobj.GetComponent<Robot>();
        functionArgs.RemoveAt(0);

        robot.robotStatus = robot.previousRobotStatus;
        Debug.Log(robot.gameObject.name + " : resume");
        
        if (actionProtocolInstance.getProtocolType().Equals("result"))
        {
            sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(0));
        }

    }

    private void RequestPreciseMove(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, List<string> functionArgs)
    {

        string robotId = functionArgs[0];
        GameObject gobj = GameObject.Find(robotId);

        if (gobj == null)
        {
            return;
        }
        Robot robot = gobj.GetComponent<Robot>();
        functionArgs.RemoveAt(0);

        IEnumerator coroutine = preciseMove(sensorActuatorModule, actionProtocolInstance, robotId, functionArgs);

        if (robot.behaviorCoroutine != null)
        {
            StopCoroutine(robot.behaviorCoroutine);
        }

        robot.behaviorCoroutine = coroutine;
        StartCoroutine(coroutine);
    }

    private IEnumerator preciseMove(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, string robotId, List<string> functionArgs)
    {
        GameObject gobj = GameObject.Find(robotId);
        Robot robot = gobj.GetComponent<Robot>();
        robot.robotStatus = RobotStatusEnum.RobotStatus.Move;

        string nodeID = functionArgs[0];
        EnvironmentObject nextVertex = EnvironmentManager.instance.getVertex(nodeID);

        float angle = calcEastThetaFromLoactedVertex(robot, nextVertex);

        bool turningClockwiseDirection = isClockwiseDirection(robot, angle);

        if (!isRightFacingWay(robot, angle))
        {
            if (turningClockwiseDirection)
            {
                SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "turn", robot.transform.position.x, robot.transform.position.y, -robot.turningSpeed*Time.deltaTime, angle));
            }
            else
            {
                SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "turn", robot.transform.position.x, robot.transform.position.y, robot.turningSpeed*Time.deltaTime, angle));
            }
            while (!isRightFacingWay(robot, angle))
            {
                while (robot.robotStatus == RobotStatusEnum.RobotStatus.Paused)
                {
                    yield return null;
                }
                turnRobot(robot, angle);
                yield return null;
            }
            SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "endTurn", robot.transform.position.x, robot.transform.position.y, 0, robot.transform.eulerAngles.z));
        }

        Vector3 moveVector = calcVector(robot, nextVertex);

        SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "move", nextVertex.position.x, nextVertex.position.y, moveVector.x, moveVector.y));
        if (robot.loadedObject != null)
        {
            SendMessageUpwards("sendLogMessage", new LogMessage(robot.loadedObject.name, "move", nextVertex.position.x, nextVertex.position.y, moveVector.x, moveVector.y));
        }
        while (!isRightPosition(robot, nextVertex))
        {
            while (robot.robotStatus == RobotStatusEnum.RobotStatus.Paused)
            {
                yield return null;
            }
            move(robot, nextVertex.transform);
            yield return null;
        }
        SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "endMove", robot.transform.position.x, robot.transform.position.y, 0, 0));
        if (robot.loadedObject != null)
        {
            SendMessageUpwards("sendLogMessage", new LogMessage(robot.loadedObject.name, "endMove", robot.transform.position.x, robot.transform.position.y, 0, 0));
        }


        robot.locatedVertex = nextVertex;
        robot.locatedVertexName = nextVertex.name;

        if (actionProtocolInstance.getProtocolType().Equals("result"))
        {
            sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(0));
        }

        robot.robotStatus = RobotStatusEnum.RobotStatus.Ready;
    }

    private void RequestStraightBackMove(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, List<string> functionArgs)
    {

        string robotId = functionArgs[0];
        GameObject gobj = GameObject.Find(robotId);

        if (gobj == null)
        {
            return;
        }
        Robot robot = gobj.GetComponent<Robot>();
        functionArgs.RemoveAt(0);

        IEnumerator coroutine = straightBackMove(sensorActuatorModule, actionProtocolInstance, robotId, functionArgs);

        if (robot.behaviorCoroutine != null)
        {
            StopCoroutine(robot.behaviorCoroutine);
        }

        robot.behaviorCoroutine = coroutine;
        StartCoroutine(coroutine);
    }

    private IEnumerator straightBackMove(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance, string robotId, List<string> functionArgs)
    {
        GameObject gobj = GameObject.Find(robotId);
        Robot robot = gobj.GetComponent<Robot>();
        robot.robotStatus = RobotStatusEnum.RobotStatus.Move;

        string nodeID = functionArgs[0];
        EnvironmentObject nextVertex = EnvironmentManager.instance.getVertex(nodeID);
        
        Vector3 moveVector = calcVector(robot, nextVertex);

        SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "move", nextVertex.position.x, nextVertex.position.y, moveVector.x, moveVector.y));
        if (robot.loadedObject != null)
        {
            SendMessageUpwards("sendLogMessage", new LogMessage(robot.loadedObject.name, "move", nextVertex.position.x, nextVertex.position.y, moveVector.x, moveVector.y));
        }
        while (!isRightPosition(robot, nextVertex))
        {
            while (robot.robotStatus == RobotStatusEnum.RobotStatus.Paused)
            {
                yield return null;
            }
            move(robot, nextVertex.transform);
            yield return null;
        }
        SendMessageUpwards("sendLogMessage", new LogMessage(robot.transform.name, "endMove", robot.transform.position.x, robot.transform.position.y, 0, 0));
        if (robot.loadedObject != null)
        {
            SendMessageUpwards("sendLogMessage", new LogMessage(robot.loadedObject.name, "endMove", robot.transform.position.x, robot.transform.position.y, 0, 0));
        }

        robot.locatedVertex = nextVertex;
        robot.locatedVertexName = nextVertex.name;

        if (actionProtocolInstance.getProtocolType().Equals("result"))
        {
            sensorActuatorModule.sendMessgae(actionProtocolInstance.getResultMessage(0));
        }

        robot.robotStatus = RobotStatusEnum.RobotStatus.Ready;
    }






    #endregion



    #region sensing 



    #endregion








}
