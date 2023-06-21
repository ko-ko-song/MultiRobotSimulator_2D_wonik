using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class MirrorFlipCamera : MonoBehaviour
{
    new Camera camera;
    public bool flipHorizontal;
    public float movementSpeed = 10f; // 카메라 이동 속도 조절 변수
    public float zoomSpeed = 7f; // 카메라 확대/축소 속도 조절 변수
    void Awake()
    {
        camera = GetComponent<Camera>();
        

        Vector3 bottomLeft = new Vector3(0, 0, 0);
        Vector3 topRight = new Vector3(1, 1, 0);

        //Debug.Log("top-right : " + camera.ViewportToWorldPoint(topRight));
        //Debug.Log("bottom-left : " + camera.ViewportToWorldPoint(bottomLeft));
    }
    void Update()
    {
        // 방향키를 누르고 있으면 카메라 이동
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * movementSpeed * Time.deltaTime;
        transform.Translate(movement);

        // + 버튼을 누르면 확대
        if (Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.KeypadPlus))
        {
            Camera.main.orthographicSize -= zoomSpeed * Time.deltaTime;
        }

        // - 버튼을 누르면 축소
        if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
        {
            Camera.main.orthographicSize += zoomSpeed * Time.deltaTime;
        }
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
        //SetCameraAspectRatio();
        Invoke("SetCameraPosition", 0.4f);
    }

    private void SetCameraPosition()
    {
        GameObject[] activeObjects = FindObjectsOfType<GameObject>();

        var list = new List<GameObject>();

        foreach (GameObject obj in activeObjects)
        {
            if (obj.activeSelf)
            {
                list.Add(obj);
            }
        }

        //var list = GameObject.FindGameObjectsWithTag("Vertex");

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
            camera.orthographicSize = (float)(width * 0.55);
        else
            camera.orthographicSize = (float)(height * 0.55);

        Vector3 pos = new Vector3((minPositionX + width / 2), (minPositionY + height / 2), -1);
        camera.transform.position = pos;
    }

    private void SetCameraAspectRatio()
    {
        float targetAspectRatio = 16f / 9f; // 원하는 비율을 설정합니다.

        float currentAspectRatio = (float)Screen.width / Screen.height;
        float scaleHeight = currentAspectRatio / targetAspectRatio;

        Camera camera = GetComponent<Camera>();

        if (scaleHeight < 1f)
        {
            Rect rect = camera.rect;
            rect.width = 1f;
            rect.height = scaleHeight;
            rect.x = 0f;
            rect.y = (1f - scaleHeight) / 2f;
            camera.rect = rect;
        }
        else
        {
            float scaleWidth = 1f / scaleHeight;

            Rect rect = camera.rect;
            rect.width = scaleWidth;
            rect.height = 1f;
            rect.x = (1f - scaleWidth) / 2f;
            rect.y = 0f;
            camera.rect = rect;
        }
    }


}