using System.Collections;
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
    FileManager _fileManager;

    [Inject]
    Button _takeButton;

    void Start()
    {
        _takeButton.onClick.AddListener(Take);
    }

    public void Take()
    {
        var fileName = System.DateTime.Now.ToString("Cap_yyyy-MM-dd_HH.mm.ss") + ".png";
        var filePath = Application.persistentDataPath + "/" + fileName;
        Debug.Log(filePath);


        Observable
        .FromCoroutine(_ => Screenshot(fileName, filePath))
        .Timeout(System.TimeSpan.FromSeconds(5))
        .Subscribe(
            _ =>
            {
                _twitterManager.FilePath = filePath;
                _twitterManager.startLogin();
                _uICanvas.enabled = true;
            },

            ex =>
            {
                Debug.Log("Login Failed: " + ex.Message);
#if UNITY_EDITOR
                filePath = _fileManager.GetProjectDirectoryPath() + "/" + fileName;
#endif
                _fileManager.DeleteFile(filePath);
                _uICanvas.enabled = true;
                
            }
        ).AddTo(this);
    }

    IEnumerator Screenshot(string fileName, string filePath)
    {
        _uICanvas.enabled = false;
        ScreenCapture.CaptureScreenshot(fileName);
        yield return new WaitUntil(() => { return _fileManager.isFileExist(filePath); });
    }
}
