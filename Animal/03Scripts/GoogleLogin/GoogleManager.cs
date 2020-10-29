using GooglePlayGames;
using UnityEngine;

public class GoogleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        LogIn();
    }

    public void LogIn()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success) Debug.Log(Social.localUser.id + "\n" + Social.localUser.userName);
            else Debug.Log("구글 로그인 실패");
        });
    }

    public void Logout()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        Debug.Log("구글 로그아웃");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
