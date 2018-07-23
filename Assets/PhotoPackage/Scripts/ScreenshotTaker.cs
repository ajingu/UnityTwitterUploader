using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UniRx;

public class ScreenshotTaker : Singleton<ScreenshotTaker>, ITaker
{
    [SerializeField]
    Canvas UICanvas;

    public void Take()
    {
        var fileName = System.DateTime.Now.ToString("Cap_yyyy-MM-dd_HH.mm.ss") + ".png";
        var filePath = Application.persistentDataPath + "/" + fileName;
        Debug.Log(filePath);


        Observable
            .FromCoroutine(_ => Screenshot(fileName, filePath))
            .Timeout(TimeSpan.FromSeconds(5))
            .Subscribe(
                _ =>
                {
                    TwitterManager.Instance.startLogin(filePath);
                    UICanvas.enabled = true;
                },

                ex => 
                {
                    Debug.Log(ex.Message);
                    UICanvas.enabled = true;
                    
                }
            ).AddTo(this);
    }

    IEnumerator Screenshot(string fileName, string filePath)
    {
        UICanvas.enabled = false;
        ScreenCapture.CaptureScreenshot(fileName);
        yield return new WaitUntil(() => { return File.Exists(filePath); });
    }
}
