using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class SensingActions : MonoBehaviour
{
    private float loadTime = 0.1f;
    private float packingTime = 5f;
    private List<string> palletizerPackingNode = new List<string>() { "1", "2" };
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region actions
    public void executeAction(SensorActuatorModule sensorActuatorModule, ActionProtocolInstance actionProtocolInstance)
    {

        string functionName = actionProtocolInstance.actionInstance.actionName;
        List<string> functionArgs = actionProtocolInstance.actionInstance.actionArgs;

        StringBuilder sb = new StringBuilder();
        foreach(string arg in functionArgs)
        {
            sb.Append(arg);
            sb.Append("\t");
        }
        if(sb.Length!= 0)
        {
            Debug.Log("execute action : " + sb[0] + " : " +functionName);
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
                RequestDoorOpen(sensorActuatorModule, actionProtocolInstance, functionArgs);
                break;
            case "doorClose":
                RequestDoorClose(sensorActuatorModule, actionProtocolInstance, functionArgs);
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
            default:
                Debug.Log("funtion name undefined " + functionName);
                break;
        }

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
            Debug.Log(robot.gameObject.name + " : ���� " + robot.robotStatus + " �����Դϴ�");
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
                    turnRobot(robot, turningClockwiseDirection);
                    yield return new WaitForSeconds(0.001f);
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
                move(robot, moveVector);
                yield return new WaitForSeconds(0.001f);
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
        float diffAngle = angle - facinWayAngle;

        if (Math.Abs(diffAngle) < 0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool isRightPosition(Robot robot, EnvironmentObject vertex)
    {
        if ((Math.Abs(vertex.position.x - robot.transform.position.x) < 0.05f)
           && (Math.Abs(vertex.position.y - robot.transform.position.y) < 0.05f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void turnRobot(Robot robot, bool isClockwise)
    {
        
        if (isClockwise)
        {
            robot.transform.Rotate(0.0f, 0.0f, -robot.turningSpeed*Time.deltaTime);
        }
        else
        {
            robot.transform.Rotate(0.0f, 0.0f, robot.turningSpeed*Time.deltaTime);
        }
    }
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
            move(robot, moveVector);
            yield return new WaitForSeconds(0.001f);
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
                turnRobot(robot, turningClockwiseDirection);
                yield return new WaitForSeconds(0.001f);
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
            move(robot, moveVector);
            yield return new WaitForSeconds(0.001f);
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

    private void move(Robot robot, Vector3 direction)
    {
 
        float speedX = direction.x * Time.deltaTime;
        float speedY = direction.y * Time.deltaTime;
        
        robot.transform.position = new Vector3(
            robot.transform.position.x + speedX,
            robot.transform.position.y + speedY,
            robot.transform.position.z
        );

        if (robot.loadedObject != null)
        {
            robot.loadedObject.transform.position = new Vector3(
                robot.loadedObject.transform.position.x + speedX,
                robot.loadedObject.transform.position.y + speedY,
                robot.loadedObject.transform.position.z
            );
        }

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
        //    Debug.Log("���������� Charger�� �����ϴ�");
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
                turnRobot(robot, turningClockwiseDirection);
                yield return new WaitForSeconds(0.001f);
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
            move(robot, moveVector);
            yield return new WaitForSeconds(0.001f);
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
            move(robot, moveVector);
            yield return new WaitForSeconds(0.001f);
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