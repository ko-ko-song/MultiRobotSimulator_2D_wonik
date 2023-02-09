using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physical: MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var box = gameObject.GetComponent<BoxCollider2D>();
        var body = gameObject.GetComponent<Rigidbody2D>();
        if (box == null)
        {
            box = gameObject.AddComponent<BoxCollider2D>();
            body = gameObject.AddComponent<Rigidbody2D>();
        }
        box.isTrigger = false;
        box.enabled = true;
        body.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
