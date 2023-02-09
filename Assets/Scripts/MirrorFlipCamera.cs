using System;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class MirrorFlipCamera : MonoBehaviour
{
    new Camera camera;
    public bool flipHorizontal;
    void Awake()
    {
        camera = GetComponent<Camera>();

        Vector3 bottomLeft = new Vector3(0, 0, 0);
        Vector3 topRight = new Vector3(1, 1, 0);

        //Debug.Log("top-right : " + camera.ViewportToWorldPoint(topRight));
        //Debug.Log("bottom-left : " + camera.ViewportToWorldPoint(bottomLeft));
    }
    void OnPreCull()
    {
        //camera.ResetWorldToCameraMatrix();
        //camera.ResetProjectionMatrix();
        //Vector3 scale = new Vector3(flipHorizontal ? -1 : 1, 1, 1);
        //camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(scale);
    }
    void OnPreRender()
    {
        //GL.invertCulling = flipHorizontal;
    }

    void OnPostRender()
    {
        //GL.invertCulling = false;
    }

    public void setPosition()
    {
        Invoke("SetCameraPosition", 0.3f);
    }

    private void SetCameraPosition()
    {
        var list = GameObject.FindGameObjectsWithTag("Vertex");

        float maxPositionX = -999;
        float maxPositionY = -999;
        float minPositionX = 999;
        float minPositionY = 999;

        foreach (GameObject obj in list)
        {
            if (obj.transform.position.x > maxPositionX)
                maxPositionX = obj.transform.position.x;
            if (obj.transform.position.x < minPositionX)
                minPositionX = obj.transform.position.x;
            if (obj.transform.position.y > maxPositionY)
                maxPositionY = obj.transform.position.y;
            if (obj.transform.position.y < minPositionY)
                minPositionY = obj.transform.position.y;
        }

        float width = maxPositionX - minPositionX;
        float height = maxPositionY - minPositionY;

        Camera camera = gameObject.GetComponent<Camera>();
        if (width > height)
            camera.orthographicSize = (float)(width * 0.35);
        else
            camera.orthographicSize = (float)(height * 0.35);

        Vector3 pos = new Vector3(minPositionX + width / 2, (minPositionY + height / 2), -1);
        camera.transform.position = pos;
    }


}