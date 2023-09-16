using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


public class CaputerImageEditor 
{
    private const string path = "Assets/MapCapture/";
    private const string screenshotPath = path + "map.png";
    private const string infoFilePath = path + "mapMetaData.txt";
    
    [MenuItem("UnityTools/CaptureImage", true)]
    private static bool ValidateGetOriginInImageAndCapture()
    {
        return Application.isPlaying; // �÷��� ����� ���� �޴� �׸� Ȱ��ȭ
    }

    [MenuItem("UnityTools/CaptureImage")]
    private static void GetOriginInImageAndCapture()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera)
        {
            Vector2 imagePosInPixels = GetUnityOriginInImage(mainCamera);
            float resolution = (2 * mainCamera.orthographicSize) / mainCamera.pixelHeight;
            Vector2 imagePosInMeters = imagePosInPixels * resolution;

            Debug.Log($"Unity Origin in Image (meters): {imagePosInMeters}");

            CaptureScreenshot();
            Debug.Log($"Screenshot saved at: {screenshotPath}");

            SaveInfoToFile(mainCamera, imagePosInMeters, resolution);
            Debug.Log($"Info saved at: {infoFilePath}");
        }
        else
        {
            Debug.LogError("Main camera not found.");
        }
    }

    private static Vector2 GetUnityOriginInImage(Camera camera)
    {
        // ����Ƽ ������ ��ũ�� ��ǥ ���
        Vector3 screenPos = camera.WorldToScreenPoint(Vector3.zero);

        // �̹����� �ػ󵵴� ī�޶��� ���� �ػ󵵿� �����ϹǷ� ������ ������ �ʿ� �����ϴ�.
        Vector2 imagePos = new Vector2(screenPos.x, screenPos.y);

        return imagePos;
    }

    private static void CaptureScreenshot()
    {
        ScreenCapture.CaptureScreenshot(screenshotPath);
    }

    private static void SaveInfoToFile(Camera camera, Vector2 imagePosInMeters, float resolution)
    {
        string content = $"originX X: -{imagePosInMeters.x}\n";
        content += $"originX Y: -{imagePosInMeters.y}\n";
        content += $"Resolution: {resolution}\n";
        content += $"Image width: {camera.pixelWidth}\n";
        content += $"Image height: {camera.pixelHeight}\n";

        File.WriteAllText(infoFilePath, content);
    }
}
