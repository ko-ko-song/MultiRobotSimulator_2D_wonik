using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;
using Defective.JSON;
using System.IO;

public class EnvironmentManager : MonoBehaviour
{

    //public string runType = "editor";
    //public string runType = "build";
    public bool isEditor = true;
    public Dictionary<string, SensorActuatorModule> sensorActuatorModules;
    public GameObject vertexPrefab;
    public GameObject rectPrefab;
    public GameObject pentagonPrefab;
    public GameObject edgePrefab;
    public GameObject productPrefab;
    public GameObject rackPrefab;
    public List<Edge> edges = new List<Edge>();

    public Dictionary<string, EnvironmentObject> vertexes;

    public Dictionary<string, EnvironmentObject> ALLobj;
    private MirrorFlipCamera camera;
    //int nextDump = 1;

    private List<string> robotIDs;
    private List<string> doorIDs;

    public string id;

    public static EnvironmentManager instance = null;

    private void Awake()
    {
        Application.runInBackground = true;
        if (instance == null)
        {
            instance = this;
        }

        sensorActuatorModules = new Dictionary<string, SensorActuatorModule>();
        ALLobj = new Dictionary<string, EnvironmentObject>();
        camera = GameObject.Find("Main Camera").GetComponent<MirrorFlipCamera>();
        vertexes = new Dictionary<string, EnvironmentObject>();
        robotIDs = new List<string>();
        doorIDs = new List<string>();
        id = UnityEngine.Random.Range(0, 6000).ToString();

        
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Run Type build or editor : " + this.runType);
        //loadVertex();
        //setVertexName();
        //setRobotsName();
        //setEdges();
        

        //string pathInEdior = "./Assets/Resources/";
        //string pathInBuild = "./Models/";

        //loadLocalTextFile(pathInBuild);
        //loadLocalTextFile(pathInEdior);

        string path = "";
        if (!isEditor)
        {
            path = "./Models/";
        }
        else if (isEditor)
        {
            path = "./Assets/Resources/";
        }

            //if (runType.Equals("build"))
            //{
            //    path = "./Models/";
            //}
            //else if (runType.Equals("editor"))
            //{
            //    path = "./Assets/Resources/";
            //}


            convertVertexFormatToJson(path);
        
        extractEdges(path);
        
        loadLocalTextFile(path);
        
    }

    private void extractEdges(string path)
    {
        string filePath = path + "/environmentObjectModels/";
        string[] environmentObjectModels = Directory.GetFiles(path + "temp/", "*.json", SearchOption.AllDirectories);
        
        if (environmentObjectModels.Contains(path+"temp/edges.json"))
            return;
        
        StreamReader sr = new StreamReader(filePath + "map_cloud.txt");
        JSONObject objectModel = new JSONObject();
        JSONObject map = JSONObject.emptyArray;

        objectModel.AddField("edges", map);
        int edgeCount = 0;
        //stringmapString = sr.ReadToEnd();
        JSONObject json = new JSONObject();
        string vertex1Name = null;
        while (true)
        {
            string line = sr.ReadLine();

            if (line == null)
            {
                if (json.HasField("id"))
                    map.Add(json);
                break;
            }


            if (line.Contains("Vertex"))
            {
                //if (json != null)
                //    map.Add(json);
                //json = new JSONObject();

            }
            else if (line.Contains("name"))
            {
                line = line.TrimStart();
                char a = ' ';
                string[] st = line.Split(a);
                vertex1Name = st[1];

            }
            else if (line.Contains("edge"))
            {
                JSONObject obj = new JSONObject();
                obj.AddField("id", "edge" + edgeCount);
                obj.AddField("vertex1", vertex1Name);

                line = line.TrimStart();
                char a = ' ';
                string[] st = line.Split(a);
                string vertex2Name = st[1];
                obj.AddField("vertex2", vertex2Name);
                map.Add(obj);
                edgeCount++;
            }



        }
        
        StreamWriter sw = new StreamWriter(path + "temp/edges.json");
        sw.Write(objectModel.ToString(true));
        sw.Flush();
        sw.Close();
        Debug.Log("Extract edge done");
    }

    private void convertVertexFormatToJson(string path)
    {
        string filePath = path+"environmentObjectModels/";
        //이미 json으로 만들어놨으면 실행하지 않음
        string[] environmentObjectModelsJSON = Directory.GetFiles(filePath , "*.json", SearchOption.AllDirectories);

        if (environmentObjectModelsJSON.Contains(filePath + "vertexModel.json"))
            return;

        
        string[] environmentObjectModelsTxt = Directory.GetFiles(filePath, "*.txt", SearchOption.AllDirectories);
        if (!environmentObjectModelsTxt.Contains(filePath + "map_cloud.txt"))
        {
            Debug.Log("need map_cloud.txt file in Directory:   " + filePath);
            return;
        }
        
        StreamReader sr = new StreamReader(filePath + "map_cloud.txt");
        
        JSONObject objectModel = new JSONObject();
        JSONObject map = JSONObject.emptyArray;

        objectModel.AddField("environmentObject", map);

        //stringmapString = sr.ReadToEnd();
        JSONObject json = new JSONObject();

        while (true)
        {
            string line = sr.ReadLine();

            if (line == null)
            {
                map.Add(json);
                break;
            }


            if (line.Contains("Vertex"))
            {
                if (json.HasField("id"))
                    map.Add(json);
                json = new JSONObject();
            }
            else if (line.Contains("name"))
            {
                line = line.TrimStart();
                char a = ' ';
                string[] st = line.Split(a);
                json.AddField("id", st[1]);
                json.AddField("name", st[1]);
                json.AddField("type", "Vertex");


            }
            else if (line.Contains("pos"))
            {
                line = line.TrimStart();
                char a = ' ';
                string[] st = line.Split(a);
                JSONObject pos = new JSONObject();
                float x = float.Parse(st[1]);
                float y = float.Parse(st[3]);
                float z = float.Parse(st[2]);

                pos.AddField("x", Math.Round(x, 1));
                pos.AddField("y", Math.Round(y, 1));
                pos.AddField("z", Math.Round(z, 1));

                json.AddField("position", pos);

                JSONObject size = new JSONObject();
                size.AddField("x", 0.4);
                size.AddField("y", 0.4);
                size.AddField("z", 0.0001);

                json.AddField("size", size);

                json.AddField("color", "#000000");

                json.AddField("shape", "Rectangle");

                json.AddField("properties", JSONObject.emptyArray);
            }

        }

        StreamWriter sw = new StreamWriter(filePath + "vertexModel.json");
        sw.Write(objectModel.ToString(true));
        sw.Flush();
        sw.Close();
        Debug.Log("Vertex format converToJSON done");
    }

    private void LoadEdges()
    {
        TextAsset edgeModel = Resources.Load<TextAsset>("temp/edges");
        JSONObject edgeObjectModel = new JSONObject(edgeModel.ToString());
        JSONObject edgeObjects = edgeObjectModel[0];
        foreach (JSONObject edgeObj in edgeObjects.list)
        {
            string id = edgeObj.GetField("id").getStringValue();
            string vertex1Id = edgeObj.GetField("vertex1").getStringValue();
            string vertex2Id = edgeObj.GetField("vertex2").getStringValue();

            Edge edge = new Edge(id, vertex1Id, vertex2Id);
            edges.Add(edge);
        }
        DeleteDuplicatedEdge();
        MakeEdges();
    }

    private void DeleteDuplicatedEdge()
    {
        List<int> deleteIndex = new List<int>();

        for(int j=0; j<edges.Count; j++)
        {
            for(int i=j+1; i<edges.Count; i++)
            {
                if(edges[i].vertex1Id.Equals(edges[j].vertex2Id) && edges[i].vertex2Id.Equals(edges[j].vertex1Id))
                {
                    deleteIndex.Add(j);
                }
            }
        }
        deleteIndex.Reverse();
        foreach(int i in deleteIndex)
        {
            edges.RemoveAt(i);
        }
    }

    private void MakeEdges()
    {
        foreach(Edge edge in edges)
        {
            Vector3 position1 = this.getVertex(edge.vertex1Id).transform.position;
            Vector3 position2 = this.getVertex(edge.vertex2Id).transform.position;

            Vector3 edgePosition = new Vector3(
                (position1.x + position2.x) / 2,
                (position1.y + position2.y) / 2,
                (position1.z + position2.z) / 2);

            float sizeX = Vector3.Distance(position1, position2);
            float sizeY = 0.1f;
            float sizeZ = 0.1f;
            Vector3 directionVector = Vector3.zero;
            if (position2.y > position1.y)
                directionVector = position2 - position1;
            else
                directionVector = position1 - position2;
            
            float Dot = Vector3.Dot(directionVector, new Vector3(1, 0, 0));
            float angle = Mathf.Acos(Dot / (directionVector.magnitude * 1)) * Mathf.Rad2Deg;
            
            GameObject obj = Instantiate(edgePrefab, edgePosition, Quaternion.identity);
            obj.transform.localScale = new Vector3(sizeX, sizeY, sizeZ);
            obj.transform.Rotate(0, 0, angle);
            obj.transform.SetParent(GameObject.Find("Edges").transform);

        }
    }

    private void Temp()
    {
        Color color = Color.clear;
        ColorUtility.TryParseHtmlString("#ffa500", out color);
        for (int i = 1; i < 5; i++)
        {
            if (this.getVertex(i.ToString()) != null)
                this.getVertex(i.ToString()).gameObject.GetComponent<SpriteRenderer>().color = color;

        }
        for (int i = 10; i < 54; i++)
        {
            if (this.getVertex(i.ToString()) != null)
                this.getVertex(i.ToString()).gameObject.GetComponent<SpriteRenderer>().color = color;
        }

        ColorUtility.TryParseHtmlString("#81c147", out color);
        for (int i=100; i<161; i++){
            if(this.getVertex(i.ToString()) != null)
                this.getVertex(i.ToString()).gameObject.GetComponent<SpriteRenderer>().color = color;
        }
    }

    //private void LoadRack()
    //{
    //    List<string> vertexId = new List<string>();
    //    for(int i=10; i<54; i++)
    //    {
    //        Vector3 position = new Vector3(this.getVertex(i.ToString()).position.x, this.getVertex(i.ToString()).position.y, this.getVertex(i.ToString()).position.z);
    //        Instantiate(rackPrefab, position, Quaternion.identity);
    //    }
    //}

    private void loadLocalTextFile(string path)
    {
        string[] environmentObjectModels2 = Directory.GetFiles(path + "/environmentObjectModels", "*.json", SearchOption.AllDirectories);
        foreach (var file in environmentObjectModels2)
        {
            StreamReader sr = new StreamReader(file);
            string data = sr.ReadToEnd();
            JSONObject a = new JSONObject(data);
            loadObject(a);
        }
        string[] sensorActuatorModels2 = Directory.GetFiles(path + "/sensorActuatorModels", "*.json", SearchOption.AllDirectories);
        foreach (var file in sensorActuatorModels2)
        {
            StreamReader sr = new StreamReader(file);
            string data = sr.ReadToEnd();
            loadSensorActuatorModule(new JSONObject(data));
        }
    }

    public void loadObject(JSONObject jsonObject)
    {
        if (jsonObject.HasField("environmentObject"))
        {
            try
            {
                loadEnvironmentObjects(jsonObject);
                
            }
            catch
            {
                Debug.Log(jsonObject.ToString(true));
                Debug.Log("environmentObject parsing error");
            }

            if (jsonObject[0].list[0].GetField("type").getStringValue().Equals("Vertex"))
            {
                try
                {

                    Invoke("LoadEdges", 0.3f);
                }
                catch
                {
                    Debug.Log("load Edge error");
                }
                try
                {
                    Invoke("Temp", 0.3f);
                }
                catch
                {
                    Debug.Log("temp error");
                }
                camera.setPosition();
                setVertexName();
            }
            else if (jsonObject[0].list[0].GetField("type").getStringValue().Equals("Robot"))
                setRobotsName();
        }
        else if (jsonObject.HasField("sensorActuators"))
        {
            try
            {
                loadSensorActuatorModule(jsonObject);
            }
            catch
            {
                Debug.Log("sensorActuatorModule parsing error");
            }
        }
        else
        {
            Debug.Log("load object error");
        }
            
    }

    public void updateObject(JSONObject message)
    {
        string id = message.GetField("id").getStringValue();
        string property = message.GetField("propertyName").getStringValue();
        GameObject gobj = GameObject.Find(id);
        EnvironmentObject eobj = gobj.GetComponent<EnvironmentObject>();
        object exValue = eobj.getProperty(property);
        object newValue;
        System.Type type = exValue.GetType();

        if (type == typeof(float))
        {
            newValue = message.GetField("propertyName").floatValue;
        }
        else if(type == typeof(int))
        {
            newValue = message.GetField("propertyName").intValue;
        }
        else if(type == typeof(string))
        {
            newValue = message.GetField("propertyName").getStringValue();
        }
        else if(type == typeof(bool))
        {
            newValue = message.GetField("propertyName").boolValue;
        }
        else if(type == typeof(Vector3))
        {
            float x = message.GetField("propertyValue").GetField("x").floatValue;
            float y = message.GetField("propertyValue").GetField("y").floatValue;
            float z = message.GetField("propertyValue").GetField("z").floatValue;
            Vector3 p = new Vector3(x,y,z);
            newValue = p;
        }
        else
        {
            Debug.Log("type error  type : " + type);
            return;
        }
        eobj.updateProperty(property, newValue);
        
    }

    public void deleteObject(JSONObject message)
    {
        string id = message.GetField("id").getStringValue();
        GameObject gobj = GameObject.Find(id);
        Destroy(gobj);
        Debug.Log("delete obejct : " + id);
    }



    private void loadSensorActuatorModule(JSONObject sensorActuatorModuleModels)
    {
        List<JSONObject> sensorActuatorModelList = sensorActuatorModuleModels[0].list;
        buildSensorActuatorModules(sensorActuatorModelList);
    }

    private void buildSensorActuatorModules(List<JSONObject> sensorActuatorModelList)
    {
        foreach (JSONObject sensorActuatorModuleModel in sensorActuatorModelList)
        {
            buildSensorAcutatorModule(sensorActuatorModuleModel);
        }
    }

    private void buildSensorAcutatorModule(JSONObject sensorActuatorModuleModel)
    {
        GameObject gobj = new GameObject();
        
        SensorActuatorModule sensorActuatorModule = gobj.AddComponent< SensorActuatorModule>();
        
        sensorActuatorModule.id             = sensorActuatorModuleModel.GetField("id").getStringValue();
        sensorActuatorModule.name           = sensorActuatorModuleModel.GetField("name").getStringValue();
        sensorActuatorModule.type           = sensorActuatorModuleModel.GetField("type").getStringValue();
        sensorActuatorModule.targetObjectId = sensorActuatorModuleModel.GetField("targetObjectId").getStringValue();
        sensorActuatorModule.messageFormat  = sensorActuatorModuleModel.GetField("messageFormat").getStringValue();
        sensorActuatorModule.ip             = sensorActuatorModuleModel.GetField("ip").getStringValue();
        sensorActuatorModule.port           = sensorActuatorModuleModel.GetField("port").getStringValue();

        gobj.name = sensorActuatorModule.id;
        
        List<JSONObject> actionProtocolModels = sensorActuatorModuleModel.GetField("actionProtocols").list;

        if (actionProtocolModels != null)
            buildActionProtocols(sensorActuatorModule, actionProtocolModels, sensorActuatorModule.messageFormat);

        List<JSONObject> sensingProtocolModels = sensorActuatorModuleModel.GetField("sensingProtocols").list;
        if (sensingProtocolModels != null)
            buildSensingProtocols(sensorActuatorModule, sensingProtocolModels, sensorActuatorModule.messageFormat);

        sensorActuatorModules.Add(sensorActuatorModule.id, sensorActuatorModule);
        sensorActuatorModule.openServer();
        sensorActuatorModule.print();
    }
   
    private void buildSensingProtocols(SensorActuatorModule sensorActuatorModule, List<JSONObject> sensingProtocolModels, string messageFormat)
    {
        foreach(JSONObject sensingProtocolModel in sensingProtocolModels)
        {
            if (sensingProtocolModel == null)
                return;    
            SensingProtocol sensingProtocol = builidSensingProtocol(sensingProtocolModel, messageFormat);
            sensorActuatorModule.addSensingProtocol(sensingProtocol.protocolId, sensingProtocol);
        }
    }

    private SensingProtocol builidSensingProtocol(JSONObject sensingProtocolModel, string messageFormat)
    {
        SensingProtocol sensingProtocol = new SensingProtocol();
        sensingProtocol.protocolId = sensingProtocolModel.GetField("protocolId").getStringValue();
        sensingProtocol.protocolType = sensingProtocolModel.GetField("protocolType").getStringValue();
        sensingProtocol.messageFormat = messageFormat;
        sensingProtocol.period = sensingProtocolModel.GetField("period").floatValue;
        sensingProtocol.sensingMessageTemplate = sensingProtocolModel.GetField("sensingMessageTemplate");
        
        return sensingProtocol;
    }

    private void buildActionProtocols(SensorActuatorModule sensorActuatorModule, List<JSONObject> actionProtocolModels, string messageFormat)
    {
        foreach (JSONObject actionProtocolModel in actionProtocolModels)
        {
            ActionProtocol actionProtocol = builidActionProtocol(actionProtocolModel, messageFormat);

            //Debug.Log(actionProtocol.protocolId);
            sensorActuatorModule.addActionProtocol(actionProtocol.protocolId, actionProtocol);
        }
    }

    //private ActionProtocol builidActionProtocol(JSONObject actionProtocolModel, string messageFormat)
    //{
    //    ActionProtocol actionProtocol = new ActionProtocol();
    //    actionProtocol.protocolId = actionProtocolModel.GetField("protocolId").getStringValue();
    //    actionProtocol.protocolType = actionProtocolModel.GetField("protocolType").getStringValue();
    //    actionProtocol.messageFormat = messageFormat;
    //    actionProtocol.requestMessageTemplate = actionProtocolModel.GetField("requestMessageTemplate");
    //    actionProtocol.responseMessageTemplate = actionProtocolModel.GetField("responseMessageTemplate");
    //    actionProtocol.resultMessageTemplate = actionProtocolModel.GetField("resultMessageTemplete");

    //    if(actionProtocolModel.GetField("headerSize") != null)
    //    {
    //        actionProtocol.HEADER_SIZE = actionProtocolModel.GetField("headerSize").intValue;
    //    }

    //    List<JSONObject> actionModels = actionProtocolModel.GetField("actionList").list;
    //    foreach (JSONObject actionModel in actionModels)
    //    {
    //        Action action = buildAction(actionModel);
    //        actionProtocol.action = action;
    //    }

    //    return actionProtocol;
    //}
    private ActionProtocol builidActionProtocol(JSONObject actionProtocolModel, string messageFormat)
    {
        ActionProtocol actionProtocol = new ActionProtocol();
        actionProtocol.protocolId = actionProtocolModel.GetField("protocolId").getStringValue();
        actionProtocol.protocolType = actionProtocolModel.GetField("protocolType").getStringValue();
        actionProtocol.messageFormat = messageFormat;
        if(actionProtocol.protocolType =="result")
        {
            actionProtocol.requestMessageTemplate = actionProtocolModel.GetField("requestMessageTemplate");
            actionProtocol.responseMessageTemplate = actionProtocolModel.GetField("responseMessageTemplate");
            actionProtocol.resultMessageTemplate = actionProtocolModel.GetField("resultMessageTemplete");
            List<JSONObject> actionModels = actionProtocolModel.GetField("actionList").list;
            foreach (JSONObject actionModel in actionModels)
            {
                Action action = buildAction(actionModel);
                actionProtocol.action = action;
            }
        }
        
        if(actionProtocol.protocolType == "send")
        {
            actionProtocol.resultMessageTemplate = actionProtocolModel.GetField("resultMessageTemplete");
        }
        
        if(actionProtocol.protocolType == "request")
        {
            actionProtocol.requestMessageTemplate = actionProtocolModel.GetField("requestMessageTemplate");
            actionProtocol.responseMessageTemplate = actionProtocolModel.GetField("responseMessageTemplate");
            if(actionProtocolModel.GetField("actionList") != null)
            {
                List<JSONObject> actionModels = actionProtocolModel.GetField("actionList").list;
                foreach (JSONObject actionModel in actionModels)
                {
                    Action action = buildAction(actionModel);
                    actionProtocol.action = action;
                }
            }
        }

        if (actionProtocolModel.GetField("headerSize") != null)
        {
            actionProtocol.HEADER_SIZE = actionProtocolModel.GetField("headerSize").intValue;
        }

        return actionProtocol;
    }
    
    private Action buildAction(JSONObject actionModel)
    {
        string actionString = actionModel.getStringValue();
        int index1 = actionString.IndexOf("(");
        int index2 = actionString.IndexOf(")");
        string actionName = actionString.Substring(0, index1);
        string argsString = actionString.Substring(index1 + 1, index2 - index1 - 1);

        string[] args = argsString.Split(',');
        for (int i = 0; i < args.Length; i++)
        {
            args[i] = args[i].Replace(" ", String.Empty);
        }

        List<string> argList = new List<string>(args);
        
        Action action = new Action();

        action.ActionName = actionName;
        action.actionArgs = argList;
        return action;
    }


    

    private void setVertexName()
    {
        GameObject[] vertexes = GameObject.FindGameObjectsWithTag("Vertex");
        foreach (GameObject vertex in vertexes)
        {
            GameObject vertexName = new GameObject("name");
            TextMesh vertexNameTextMesh = vertexName.AddComponent<TextMesh>();

            vertexNameTextMesh.characterSize = 0.33f;
            vertexNameTextMesh.fontSize = 40;
            vertexNameTextMesh.fontStyle = FontStyle.Bold;
            vertexNameTextMesh.alignment = TextAlignment.Center;
            vertexNameTextMesh.anchor = TextAnchor.MiddleCenter;
            vertexNameTextMesh.color = Color.black;
            vertexNameTextMesh.text = vertex.GetComponent<EnvironmentObject>().name;

            vertexName.transform.SetParent(vertex.transform, false);
            vertexName.transform.position = new Vector3(
                vertex.transform.position.x, 
                vertex.transform.position.y + 0.4f, 
                vertex.transform.position.z);
        }
    }

    private void setRobotsName()
    {
        GameObject[] robotObjs = GameObject.FindGameObjectsWithTag("Robot");
        foreach (GameObject robot in robotObjs)
        {
            if(robot.transform.localScale.x < 0.1)
            {
                continue;
            }
            GameObject robotName = new GameObject("name");
            TextMesh robotNametextMesh = robotName.AddComponent<TextMesh>();
            
            robotNametextMesh.characterSize = 0.3f;
            robotNametextMesh.fontSize = 40;
            robotNametextMesh.fontStyle = FontStyle.Bold;
            robotNametextMesh.alignment = TextAlignment.Center;
            robotNametextMesh.anchor = TextAnchor.MiddleCenter;
            robotNametextMesh.color = Color.red;
            robotNametextMesh.text = robot.GetComponent<EnvironmentObject>().name;
            robotName.GetComponent<MeshRenderer>().sortingLayerName = "Text";

            robotName.AddComponent<NameText>().target = robot.transform;
        }
    }

    private Dictionary<string,Vertex> convertVertexPropertyType(NavigationVertex[] navigationVertices)
    {
        Dictionary<string, Vertex> vertices = new Dictionary<string, Vertex>();
        
        for (int i = 0; i < navigationVertices.Length; i++)
        {
            Vertex v = new Vertex();
            v.setEdges(navigationVertices[i].edges);
            v.setName(navigationVertices[i].name);
            v.setType(navigationVertices[i].type);
            v.setPosX(Convert.ToSingle(navigationVertices[i].pos[2]));
            v.setPosY(-Convert.ToSingle(navigationVertices[i].pos[0]));
            vertices.Add(v.name, v);
        }
        return vertices;
    }

    private void loadEnvironmentObjects(JSONObject jsonData)
    {
        List<JSONObject> environmentObjectsJSON = jsonData[0].list;

        foreach (JSONObject environmentObjectJSON in environmentObjectsJSON)
        {
            string type = environmentObjectJSON.GetField("type").getStringValue();
            string id = environmentObjectJSON.GetField("id").getStringValue();
            string name = environmentObjectJSON.GetField("name").getStringValue();
            string color = environmentObjectJSON.GetField("color").getStringValue();
            Vector3 position = new Vector3(
                environmentObjectJSON.GetField("position").GetField("x").floatValue,
                environmentObjectJSON.GetField("position").GetField("y").floatValue,
                environmentObjectJSON.GetField("position").GetField("z").floatValue
                );
            Vector3 size = new Vector3(
                environmentObjectJSON.GetField("size").GetField("x").floatValue,
                environmentObjectJSON.GetField("size").GetField("y").floatValue,
                environmentObjectJSON.GetField("size").GetField("z").floatValue
                );
            
            string shape = environmentObjectJSON.GetField("shape").getStringValue();
            List<string> simulationProperties = new List<string>();
            foreach (JSONObject property in environmentObjectJSON.GetField("properties"))
            {
                simulationProperties.Add(property.getStringValue());
            }
            GameObject gobj;
            if (type.Equals("Vertex"))
            {
                gobj = Instantiate(vertexPrefab);
                gobj.transform.SetParent(GameObject.Find("Vertexes").transform);
            }
            else if (type.Equals("Robot"))
            {
                gobj = Instantiate(pentagonPrefab);
            }
            else if (type.Equals("Product"))
            {
                gobj = Instantiate(productPrefab);
            }
            else if (type.Equals("Rack"))
            {
                gobj = Instantiate(rackPrefab);
            }
            else
            {
                gobj = Instantiate(rectPrefab);
            }
            EnvironmentObject eobj;
            if (type.Equals("Product"))
                eobj = gobj.AddComponent<Product>();
            else
                eobj = gobj.AddComponent<EnvironmentObject>();
            
            eobj.id = id;
            eobj.name = name;
            eobj.type = type;
            eobj.position = position;
            eobj.size = size;
            eobj.color = color;
            eobj.shape = shape;
            eobj.simulationProperties = simulationProperties;
            eobj.init();

            if (type.Equals("Vertex"))
                gobj.name = "Vertex" + id;
            else
                gobj.name = id;
            gobj.tag = type;
            gobj.transform.localScale = size;
            gobj.transform.position = position;
            if (type.Equals("Vertex"))
                this.vertexes.Add(id, eobj);

            if (simulationProperties.Contains("detectable"))
                gobj.AddComponent<Collidable>();
            if (simulationProperties.Contains("physical"))
                gobj.AddComponent<Physical>();

            SpriteRenderer spr = gobj.GetComponent<SpriteRenderer>();
            if (spr == null)
                spr = gobj.AddComponent<SpriteRenderer>();

            if (color != null)
            {
                Color colorObj = Color.clear;
                ColorUtility.TryParseHtmlString(color, out colorObj);
                spr.color = colorObj;
            }
            else
                spr.color = Color.gray;
            spr.sortingLayerName = type;
            if (type.Equals("Robot"))
            {
                gobj.AddComponent<Robot>().id = id;
                gobj.transform.SetParent(GameObject.Find("Controller").transform, false);
            }
            else if (type.Equals("Door"))
            {
                gobj.AddComponent<Door>().id = id;
                gobj.transform.SetParent(GameObject.Find("Controller").transform, false);
            }
        }
    }
    public EnvironmentObject getVertex(string vertexID)
    {
        if (vertexes.ContainsKey(vertexID))
            return vertexes[vertexID];

        return null;
    }

    public EnvironmentObject getVertexByPosition(Vector3 position)
    {
        foreach (EnvironmentObject vertex in vertexes.Values)
        {
            if (Mathf.Abs(vertex.position.x - position.x) < 0.1f && Mathf.Abs(vertex.position.y - position.y) < 0.1f)
            {
                return vertex;
            }
        }

        return null;
    }

    // Update is called once per frame

    //public bool isConnectedVertex(string firstVertexID, string secondVertexID)
    //{
    //    Vertex firstVertex = vertices[firstVertexID];
    //    Vertex secondVertex = vertices[secondVertexID];

    //    foreach (string edge in firstVertex.edges)
    //    {
    //        if (edge.Equals(secondVertex.name))
    //            return true;
    //    }
    //    return false;
    //}

    //public bool isPathContainNotDrivingTypeNode(List<string> path)
    //{
    //    foreach (string nodeID in path)
    //    {
    //        if (vertices[nodeID].type != 0)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    //public bool isPathContainDisconnectedNode(List<string> path)
    //{
    //    for (int i = 0; i < (path.Count - 1); i++)
    //    {
    //        if (!(isConnectedVertex(path[i], path[i + 1])))
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}


    //public bool isPathContainUndefinedNode(List<string> path)
    //{
    //    foreach (string nodeID in path)
    //    {
    //        if (vertices[nodeID] == null)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}


    //public bool isDrivingNode(string nodeID)
    //{
    //    Vertex v = vertices[nodeID];
    //    if (v.type == 0)
    //        return true;
    //    else
    //        return false;
    //}

    //public bool isChargingNode(string nodeID)
    //{
    //    Vertex v = vertices[nodeID];
    //    if (v.type == 2)
    //        return true;
    //    else
    //        return false;
    //}

    //public bool isUndefinedNode(string nodeID)
    //{
    //    Vertex v = vertices[nodeID];
    //    if (v == null)
    //        return true;
    //    else
    //        return false;
    //}


    //public bool isStationNode(string nodeID)
    //{
    //    Vertex v = vertices[nodeID];
    //    if (v.type == 1)
    //        return true;
    //    else
    //        return false;
    //}






    //private void loadVertex()
    //{
    //    TextAsset verticesText = Resources.Load<TextAsset>("vertex-model");
    //    NavigationVertex[] navigationVertices = JsonHelper.FromJson<NavigationVertex>(verticesText.ToString());
    //    var vertexesObjectTransform = GameObject.Find("Vertexes").transform;

    //    foreach (NavigationVertex vertex in navigationVertices)
    //    {
    //        GameObject vertexObj = Instantiate(vertexPrefab);
    //        vertexObj.name = vertex.name;
    //        vertexObj.layer = 7;
    //        vertexObj.transform.localScale = new Vector3(float.Parse("0.5"), float.Parse("0.5"), float.Parse("0.5"));
    //        Vector3 position = new Vector3(float.Parse(vertex.pos[2]), -float.Parse(vertex.pos[0]), float.Parse(vertex.pos[1]));
    //        vertexObj.transform.position = position;
    //        //vertexObj.GetComponent<SpriteRenderer>().sortingLayerName = "Vertex";
    //        vertexObj.transform.parent = vertexesObjectTransform;
    //        vertexObj.AddComponent<VertexComponent>().edges = vertex.edges;
    //        vertexObj.tag = "Vertex";
    //        //Debug.Log(vertexObj);
    //    }
    //    vertices = convertVertexPropertyType(navigationVertices);
    //}

    public bool CreateProduct(string nodeId)
    {
        EnvironmentObject vertex = getVertex(nodeId);
        if (vertex == null)
        {
            Debug.Log("vertex " + nodeId + " 찾을 수 없습니다");
            return false;
        }
            
        Vector3 position = vertex.transform.position;
        GameObject productObject = Instantiate(productPrefab, position, Quaternion.identity);
        productObject.name = "Product" + UnityEngine.Random.Range(1, 6000);

        productObject.GetComponent<Product>().id = productObject.name;
        productObject.GetComponent<Product>().name = productObject.name;
        if (productObject.GetComponent<Product>().simulationProperties.Contains("detectable"))
            productObject.AddComponent<Collidable>();
        if (productObject.GetComponent<Product>().simulationProperties.Contains("physical"))
            productObject.AddComponent<Physical>();
        return productObject.GetComponent<Product>().init();
    }
    
}
