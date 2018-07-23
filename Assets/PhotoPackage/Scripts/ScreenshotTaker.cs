using UnityEngine;

public class ScreenshotTaker : Singleton<ScreenshotTaker>, ITaker
{
    public void Take()
    {
        var fileName = System.DateTime.Now.ToString("スクリーンショット_yyyy-MM-dd_HH.mm.ss") + ".png";
        ScreenCapture.CaptureScreenshot(fileName);
        TwitterManager.Instance.startLogin(fileName);
    }
}
