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
    public float movementSpeed = 10f; // ī�޶� �̵� �ӵ� ���� ����
    public float zoomSpeed = 7f; // ī�޶� Ȯ��/��� �ӵ� ���� ����
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
        // ����Ű�� ������ ������ ī�޶� �̵�
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * movementSpeed * Time.deltaTime;
        transform.Translate(movement);

        // + ��ư�� ������ Ȯ��
        if (Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.KeypadPlus))
        {
            Camera.main.orthographicSize -= zoomSpeed * Time.deltaTime;
        }

        // - ��ư�� ������ ���
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
        float targetAspectRatio = 16f / 9f; // ���ϴ� ������ �����մϴ�.

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