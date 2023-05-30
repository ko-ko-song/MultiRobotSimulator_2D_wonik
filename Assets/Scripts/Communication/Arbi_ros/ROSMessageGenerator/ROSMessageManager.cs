using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

public class ROSMessageManager
{
    string inPath = "Assets/Scripts/Communication/Arbi_ros/unity_robotics_demo_msgs";
    string outPath = "Assets/GeneratedMessages";
    public static ROSMessageManager instance;

    private Dictionary<string, Assembly> rosMessageAssemblies;

    private ROSMessageManager() { }

    public static ROSMessageManager Instance()
    {
        if (instance == null)
        {
            instance = new ROSMessageManager();
            instance.init();
        }
            
        return instance;
    }
    
    private void init()
    {
        if (EnvironmentManager.instance.isEditor)
        {
            inPath = "Assets/Scripts/Communication/Arbi_ros/unity_robotics_demo_msgs";
            outPath = "Assets/GeneratedMessages";
        }
        else
        {
            inPath = "./Models/ros_msgs";
            outPath = "./Models/ros_msgs/generated_msgs";
        }

        rosMessageAssemblies = new Dictionary<string, Assembly>();
        createROSMessages();
        AssembleMessages();
    }
    
    private void createROSMessages()
    {
        ROSMessageGenerator rmg = new ROSMessageGenerator();
        rmg.GenerateROSMessages(inPath, outPath);
    }
    
    private void AssembleMessages()
    {
        //yield return new WaitForSeconds(0.2f);
        Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();
        
        string path = "./" + outPath + "/";
        ROSMessageCompiler rmc = new ROSMessageCompiler();
        string[] fileNames = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);
        foreach (string fileName in fileNames)
        {
            StreamReader sr = new StreamReader(fileName);
            string fileString = sr.ReadToEnd();
            Assembly assembly = rmc.compileROSMessage(fileString);
            assemblies.Add(Path.GetFileNameWithoutExtension(fileName),assembly);
        }

        rosMessageAssemblies = assemblies;
    }
    
    private Assembly getROSMessageAssembly(string messageName)
    {
        if(rosMessageAssemblies == null)
        {
            rosMessageAssemblies = new Dictionary<string, Assembly>();
            AssembleMessages();
        }
        if (rosMessageAssemblies.ContainsKey(messageName))
            return rosMessageAssemblies[messageName];
        else
            return null;
    }
    private string getNamespaceValue(string data)
    {
        int namespaceStartIndex = data.IndexOf("namespace");
        int namespaceEndIndex = data.IndexOf("{", namespaceStartIndex);

        if (namespaceStartIndex != -1 && namespaceEndIndex != -1)
        {
            string namespaceLine = data.Substring(namespaceStartIndex, namespaceEndIndex - namespaceStartIndex);
            string[] parts = namespaceLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                string namespaceValue = parts[1];
                string modifiedString = namespaceValue.Replace(" ", "").Replace("\r", "").Replace("\n", "");
                return modifiedString;
            }
            else
                return null;
        }
        else
        {
            return null;
        }
    }
    
    public Type getMessageType(string messageName)
    {
        Assembly assembly = getROSMessageAssembly(messageName);
        string namespcaeValue = "";
        string[] fileNames = Directory.GetFiles(outPath+"/", messageName + ".cs", SearchOption.AllDirectories);
        if (fileNames.Length > 1)
        {
            Debug.LogError("file names duplicated error");
            return null;
        }
        if(fileNames.Length == 1)
        {
            StreamReader sr = new StreamReader(fileNames[0]);
            string fileString = sr.ReadToEnd();
            namespcaeValue = getNamespaceValue(fileString);
        }
        Type type = assembly.GetType(namespcaeValue + "." + messageName);
        if (type != null)
        {
            //Debug.Log("Type found: " + type.FullName);
        }
        else
        {
            Debug.LogError("Type not found!");
        }
        
        return type;
    }
}

