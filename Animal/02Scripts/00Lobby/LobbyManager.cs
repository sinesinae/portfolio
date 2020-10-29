using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// 메인 로비 화면 버튼 구성을 위한 클래스
/// </summary>
public class LobbyManager : MonoBehaviour
{
    public GameObject IsOkPanel;    // 정말 초기화 할건지 묻는 패널
    public Text messegeText;    // 불러오기 실패등 메세지를 출력하는 텍스트

    // 메세지 초기화 
    void InitMsg()
    {
        messegeText.text = ""; 
    }

    // 계속 하기 버튼 클릭시 호출되는 함수
    public void ContinueBtn()
    {
        // 맵 데이터가 존재하는지 확인 한뒤 없으면 실행되지 않음
        try
        {
            string mapstr = File.ReadAllText(Application.persistentDataPath + "/datajson.json");
        }
        catch
        {
            messegeText.text = "저장된 게임이 없습니다";
            Invoke("InitMsg", 2f);
            return;
        }
        
        SceneManager.LoadScene("MainMap");
    }

    // 새로하기 버튼 클릭시 실행되는 함수
    public void NewGameBtn()
    {
        IsOkPanel.SetActive(true);
    }
    
    // 종료버튼 클릭시 실행되는 함수
    public void ExitBtn()
    {
        Application.Quit();
    }

    // 새로하기 확인창의 OK버튼 함수
    public void NewGameOK()
    {
        SceneManager.LoadScene("Tutorial");
    }

    // 새로하기 확인창의 NO버튼 함수
    public void NewGameNo()
    {
        IsOkPanel.SetActive(false);
    }
}
