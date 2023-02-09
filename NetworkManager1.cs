using System;
using System.Collections;
using UnityEngine;


public class NetworkManager : MonoBehaviour
{
    private static NetworkManager instance = null;
    private ControllableThing controllableThing;

    private Adaptor adaptor;
    

    #region Modified
    private CommandScript commandScript;
    #endregion


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        #region Modified
        commandScript = CommandScript.Instance;
        controllableThing = GameObject.Find("ControllableThing").GetComponent<ControllableThing>();
        #endregion
        adaptor = new Adaptor();
        IPandPort ipandPort = new IPandPort("127.0.0.1", 9090);

        StartCoroutine("connect", ipandPort);
    }

    IEnumerator connect(IPandPort ipandPort)
    {
        yield return new WaitForSecondsRealtime(5);
        adaptor.connect(ipandPort.getIP(), ipandPort.getPort());
    }

    // Update is called once per frame
    void Update()
    {
        #region input test
        if (Input.GetKeyDown("enter"))
        {
            string inputCommnad = commandScript.getInputCommand();
            string[] splitCommand = inputCommnad.Split(' ');
            
            string command = splitCommand[0];
            string actorId = splitCommand[1];
            string vertexId = null;
            if (splitCommand.Length==3)
                vertexId = splitCommand[2];
            controllableThing.sendCommandMessgage(command, actorId, vertexId);
            
        }
        #endregion
    }

    public static NetworkManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            } else
            {
                return instance;
            }
        }
    }



 
    public void AckMove(GameObject gameObject)
    {
        Debug.Log("AckMove(" + gameObject.name + ")");
    }

    public void AckMoveEnd(GameObject gameObject)
    {
        Debug.Log("AckMoveEnd(" + gameObject.name + ")");
    }
    // Use this for initialization
  
    internal void AckGrab(GameObject gameObject, GameObject lockedObject)
    {
        Debug.Log("AckLoad(" + gameObject.name + ")");
    }

    internal void AckGrabEnd(GameObject gameObject, GameObject lockedObject)
    {
        Debug.Log("AckLoadEnd(" + gameObject.name + ")");
    }
}
