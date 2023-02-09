using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObject : MonoBehaviour
{
    public string id;
    public string name;
    public string type;
    public Vector3 position;
    public Vector3 size;
    public string color;
    public string shape;
    public List<string> simulationProperties;
    public Dictionary<string, object> properties;

    void Awake()
    {
        simulationProperties = new List<string>();
        properties = new Dictionary<string, object>();
    }
    
    public void updateProperty(string property, object newValue)
    {
        if (properties.ContainsKey(property))
        {
            properties[property] = newValue;
            if(property.Equals("position"))
            {
                transform.position = (Vector3)newValue;
            }
            else if(property.Equals("size"))
            {
                transform.localScale = (Vector3)newValue;
            }
        }
        else
        {
            return;
        }
    }

    public object getProperty(string property)
    {
        if (!properties.ContainsKey(property))
        {
            Debug.Log("environmentObject doen't have property : " + property);
            return null;
        }
        return properties[property];
    }

    public virtual bool init()
    {
        this.properties.Add("id", this.id);
        this.properties.Add("name", this.name);
        this.properties.Add("type", this.type);
        this.properties.Add("position", this.position); 
        this.properties.Add("size", this.size);
        this.properties.Add("color", this.color);
        this.properties.Add("shape", this.shape);
        this.properties.Add("simulationProperties", this.simulationProperties);
        return true;
    }

}

