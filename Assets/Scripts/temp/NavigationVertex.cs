using System.Collections.Generic;

[System.Serializable]
public class NavigationVertex : VirtualObj
{
    public string[] edges;
    public int type;
    public string[] pos;

    public NavigationVertex(string[] edges, int type)
    {
        this.edges = edges;
        this.type = type;
    }

    public NavigationVertex()
    {
        name = "vertex" + name;
    }
}

