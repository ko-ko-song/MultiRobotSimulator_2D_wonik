using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable: MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        var box = gameObject.GetComponent<BoxCollider2D>();
        if (box == null)
        {
            box = gameObject.AddComponent<BoxCollider2D>();
        }
        box.isTrigger = true;
        box.enabled = true;
        //box.size = new Vector2(1.6f, 1.6f);
    }

}
