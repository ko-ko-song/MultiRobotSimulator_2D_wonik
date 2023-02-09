using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameText : MonoBehaviour
{
    public Transform target;
    
    void Update()
    {
        if(target != null)
        {
            this.transform.position = new Vector3(
                target.transform.position.x,
                target.transform.position.y + 0.5f,
                target.transform.position.z
                );
        }
    }
}
