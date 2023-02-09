using UnityEngine;

[System.Serializable]
public class VirtualObj
{
    public string id;
    public string name;
    public string[] position;
    public string shape;
    public string[] size;
    public string[] properties;
    public string color;
    public string tag;
    override public string ToString()
    {
        return name;
    }
}