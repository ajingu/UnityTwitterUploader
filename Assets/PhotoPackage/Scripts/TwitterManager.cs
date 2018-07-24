using System;
using System.IO;
using TwitterKit.Unity;

public class TwitterManager{

    private string FilePath;

    public void startLogin(string filePath)
    {
        UnityEngine.Debug.Log("startLogin()");
        FilePath = filePath;
        
        Twitter.Init();

        Twitter.LogIn(LoginCompleteWithCompose, (ApiError error) => {
            UnityEngine.Debug.Log(error.message);
        });
    }

    public void LoginCompleteWithEmail(TwitterSession session)
    {
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
        UnityEngine.Debug.Log("Screenshot location=" + FilePath);
        string imageUri = "file://" + FilePath;
        Twitter.Compose(
            session,
            imageUri,
            "Welcome to", new string[] { "#TwitterKitUnity" },
            (string tweetId) => { UnityEngine.Debug.Log("Tweet Success, tweetId=" + tweetId); DeleteFile(); },
            (ApiError error) => { UnityEngine.Debug.Log("Tweet Failed " + error.message); DeleteFile(); },
            () => { UnityEngine.Debug.Log("Compose cancelled"); DeleteFile(); }
         );
    }

    void DeleteFile()
    {
        if (File.Exists(FilePath))
        {
            UnityEngine.Debug.Log("File Deleted");
            File.Delete(FilePath);
        }
    }
}
