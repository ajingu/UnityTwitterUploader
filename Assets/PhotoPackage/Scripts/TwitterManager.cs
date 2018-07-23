using UnityEngine;

using TwitterKit.Unity;

public class TwitterManager : Singleton<TwitterManager> {

    private string FileName;

    public void startLogin(string fileName)
    {
        UnityEngine.Debug.Log("startLogin()");
        FileName = fileName;
        // To set API key navigate to tools->Twitter Kit
        Twitter.Init();

        Twitter.LogIn(LoginCompleteWithCompose, (ApiError error) => {
            UnityEngine.Debug.Log(error.message);
        });
    }

    public void LoginCompleteWithEmail(TwitterSession session)
    {
        // To get the user's email address you must have "Request email addresses from users" enabled on https://apps.twitter.com/ (Permissions -> Additional Permissions)
        UnityEngine.Debug.Log("LoginCompleteWithEmail()");
        Twitter.RequestEmail(session, RequestEmailComplete, (ApiError error) => { UnityEngine.Debug.Log(error.message); });
    }

    public void RequestEmailComplete(string email)
    {
        UnityEngine.Debug.Log("email=" + email);
        LoginCompleteWithCompose(Twitter.Session);
    }

    public void LoginCompleteWithCompose(TwitterSession session)
    {
        //ScreenCapture.CaptureScreenshot("Screenshot.png");
        UnityEngine.Debug.Log("Screenshot location=" + Application.persistentDataPath + "/" + FileName);
        string imageUri = "file://" + Application.persistentDataPath + "/" + FileName;
        Twitter.Compose(session, imageUri, "Welcome to", new string[] { "#TwitterKitUnity" },
            (string tweetId) => { UnityEngine.Debug.Log("Tweet Success, tweetId=" + tweetId); },
            (ApiError error) => { UnityEngine.Debug.Log("Tweet Failed " + error.message); },
            () => { Debug.Log("Compose cancelled"); }
         );
    }
}
