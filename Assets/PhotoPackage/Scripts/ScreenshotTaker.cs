using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Zenject;

public class ScreenshotTaker : Singleton<ScreenshotTaker>, ITaker
{
    [Inject]
    Canvas _uICanvas;

    [Inject]
    TwitterManager _twitterManager;

    [Inject]
    Button _takeButton;

    void Start()
    {
        _takeButton.onClick.AddListener(Take);
    }

    public void Take()
    {
        var fileName = DateTime.Now.ToString("Cap_yyyy-MM-dd_HH.mm.ss") + ".png";
        var filePath = Application.persistentDataPath + "/" + fileName;
        Debug.Log(filePath);


        Observable
        .FromCoroutine(_ => Screenshot(fileName, filePath))
        .Timeout(TimeSpan.FromSeconds(5))
        .Subscribe(
            _ =>
            {
                _twitterManager.startLogin(filePath);
                _uICanvas.enabled = true;
            },

            ex =>
            {
            Debug.Log(ex.Message);
                _uICanvas.enabled = true;
#if UNITY_EDITOR
                filePath = Directory.GetParent(Application.dataPath).FullName + "/" + fileName;
#endif
                DeleteFile(filePath);
                
            }
        ).AddTo(this);
    }

    IEnumerator Screenshot(string fileName, string filePath)
    {
        _uICanvas.enabled = false;
        ScreenCapture.CaptureScreenshot(fileName);
        yield return new WaitUntil(() => { return File.Exists(filePath); });
    }

    void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            UnityEngine.Debug.Log("File Deleted");
            File.Delete(filePath);
        }
    }
}
