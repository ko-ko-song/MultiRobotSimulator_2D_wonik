using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : EnvironmentObject
{

    private void Awake()
    {
        id = transform.name;
        name = transform.name;
        type = "Product";
        position = transform.position;
        size = transform.localScale;
        color = "brown";
        shape = "Rectangle";
        simulationProperties = new List<string>();
        properties = new Dictionary<string, object>();

        simulationProperties.Add("detectable");
    }
    
    public override bool init()
    {
        base.init();
        GameObject[] racks = GameObject.FindGameObjectsWithTag("Rack");
        foreach(GameObject rack in racks)
        {
            if(Mathf.Abs(this.position.x - rack.transform.position.x) < 0.2f && 
                Mathf.Abs(this.position.y - rack.transform.position.y) < 0.2f)
            {
                if(rack.transform.childCount != 0)
                {
                    Debug.Log("rack이 가득 차 있습니다");
                    Destroy(gameObject);
                    return false;
                }
                else
                {
                    gameObject.transform.parent = rack.transform;
                    return true;
                }
                
            }
        }
        
        Debug.Log("product가 생성 될 rack을 찾을 수 없습니다");
        Destroy(gameObject);
        return false;
    }
}
