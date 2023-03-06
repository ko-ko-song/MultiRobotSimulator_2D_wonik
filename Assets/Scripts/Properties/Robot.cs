using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public RobotStatusEnum.RobotStatus robotStatus;
    public RobotStatusEnum.RobotStatus previousRobotStatus;
    public string id;
    public float speed = 1f;
    public float turningSpeed = 18f;
    //public float speed = 6f;
    //public float turningSpeed = 80f;
    public string locatedVertexName;
    public EnvironmentObject locatedVertex;
    public List<GameObject> aroundObjects;
    public GameObject loadedObject;
    public int remainingBattery = 80;
    public int maxinumBattery = 100;
    public bool loading = false;

    #region coroutine
    public IEnumerator behaviorCoroutine;
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        aroundObjects = new List<GameObject>();
        robotStatus = RobotStatusEnum.RobotStatus.Ready;
        Invoke("InitObject", 0.3f);
        //StartCoroutine("RTSR");
    }

    void Update()
    {

        if (remainingBattery == 0)
        {
            Debug.Log(gameObject.name + " : 배터리 방전");
            behaviorCoroutine = null;
        }
    }
    
    private void InitObject()
    {
        EnvironmentObject v = EnvironmentManager.instance.getVertexByPosition(transform.position);

        //if (EnvironmentManager.instance.runType.Equals("build"))
        //{
        //    speed = 6f;
        //    turningSpeed = 80f;
        //}
        //else if (EnvironmentManager.instance.runType.Equals("editor"))
        //{
        //    speed = 0.3f;
        //    turningSpeed = 12f;
        //}
        if(gameObject.transform.localScale.x > 0.1)
            initLocatedVertex(v);
    }

    private void initLocatedVertex(EnvironmentObject vertex)
    {
        if(vertex == null)
        {
            Invoke("InitObject", 0.5f);
        }

        locatedVertex = vertex;
        gameObject.transform.position = new Vector3(
            locatedVertex.position.x,
            locatedVertex.position.y,
            gameObject.transform.position.z);
    }

   
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(gameObject.name + " : 충돌 발생");
 
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Door"))
        {
            //if (!coll.gameObject.GetComponent<Door>().isOpend())
            //{
            //    if (behaviorCoroutine != null)
            //        StopCoroutine(behaviorCoroutine);
            //    Debug.Log(gameObject.name + " : door is closed");
            //    if (robotStatus == RobotStatusEnum.RobotStatus.ChargeIn)
            //    {
            //        SendMessageUpwards("sendAckMessage", new AckEndCharge(this.id, 12));
            //    }
            //    else if (robotStatus == RobotStatusEnum.RobotStatus.ChargeOut)
            //    {
            //        SendMessageUpwards("sendAckMessage", new AckEndChargeStop(this.id, 17));
            //    }
            //    else if (robotStatus == RobotStatusEnum.RobotStatus.Loading)
            //    {
            //        SendMessageUpwards("sendAckMessage", new AckEndLoad(this.id, 13));
            //    }
            //    else if (robotStatus == RobotStatusEnum.RobotStatus.Unloading)
            //    {
            //        SendMessageUpwards("sendAckMessage", new AckEndUnload(this.id, 14));
            //    }
            //    robotStatus = RobotStatusEnum.RobotStatus.EmergencyStop;
            //}
        }
        else
        {
            aroundObjects.Add(coll.gameObject);
        }
    }
 
    void OnTriggerExit2D(Collider2D coll)
    {
        if (aroundObjects.Contains(coll.gameObject))
        {
            aroundObjects.Remove(coll.gameObject);
        }

    }

 

}
