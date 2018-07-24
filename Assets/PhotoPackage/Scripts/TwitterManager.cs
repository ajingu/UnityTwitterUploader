using TwitterKit.Unity;
using Zenject;

public class TwitterManager{

    string filePath;
    public string FilePath { get; set; }

    [Inject]
    FileManager _fileManager;

    public void startLogin()
    {
        UnityEngine.Debug.Log("startLogin()");
        
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
            (string tweetId) => { UnityEngine.Debug.Log("Tweet Success, tweetId=" + tweetId); _fileManager.DeleteFile(FilePath); },
            (ApiError error) => { UnityEngine.Debug.Log("Tweet Failed " + error.message); _fileManager.DeleteFile(FilePath); },
            () => { UnityEngine.Debug.Log("Compose cancelled"); _fileManager.DeleteFile(FilePath); }
         );
    }
}
