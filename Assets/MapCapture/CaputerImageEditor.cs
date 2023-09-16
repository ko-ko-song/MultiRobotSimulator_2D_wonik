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
        return Application.isPlaying; // 플레이 모드일 때만 메뉴 항목 활성화
    }

    [MenuItem("UnityTools/CaptureImage")]
    private static void GetOriginInImageAndCapture()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera)
        {
            Vector2 imagePos = GetUnityOriginInImage(mainCamera);
            Debug.Log($"Unity Origin in Image: {imagePos}");

            CaptureScreenshot();
            Debug.Log($"Screenshot saved at: {screenshotPath}");

            SaveInfoToFile(mainCamera, imagePos);
            Debug.Log($"Info saved at: {infoFilePath}");
        }
        else
        {
            Debug.LogError("Main camera not found.");
        }
    }

    private static Vector2 GetUnityOriginInImage(Camera camera)
    {
        // 유니티 원점의 스크린 좌표 얻기
        Vector3 screenPos = camera.WorldToScreenPoint(Vector3.zero);

        // 이미지의 해상도는 카메라의 현재 해상도와 동일하므로 별도의 조정이 필요 없습니다.
        Vector2 imagePos = new Vector2(screenPos.x, screenPos.y);

        return imagePos;
    }

    private static void CaptureScreenshot()
    {
        ScreenCapture.CaptureScreenshot(screenshotPath);
    }

    private static void SaveInfoToFile(Camera camera, Vector2 imagePos)
    {
        float resolution = (2 * camera.orthographicSize) / camera.pixelHeight;
        
        string content = $"originX X: {imagePos.x}\n";
        content += $"originX Y: {imagePos.y}\n";
        content += $"Resolution: {resolution}\n";
        content += $"Image width: {camera.pixelWidth}\n";
        content += $"Image height: {camera.pixelHeight}\n";
        
        File.WriteAllText(infoFilePath, content);
    }
}
