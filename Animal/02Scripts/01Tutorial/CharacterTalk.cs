using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

[System.Serializable]
public class TalkScript
{   // Json파싱을 위한 클래스 구조
    public int PhaseNum;
    public string[] phase1;
    public string[] phase2;
    public string[] phase3;
    public string[] phase4;
    public string[] phase5;
}

/// <summary>
/// 튜토리얼씬 대사집을 불러와서 대사를 출력하는 클래스
/// </summary>
public class CharacterTalk : MonoBehaviour
{
    [HideInInspector]
    public TalkScript TutorialScript;  // 튜토리얼씬 대사
    public Text textTarget; // 대사가 표현될 텍스트UI
    public int currentTextIdx; // 현재 페이즈의 몇번째 대사인지 표기해줄 인덱스
    PhaseCtrl pc;   // 튜토리얼 진행 페이즈를 관리하는 페이즈컨트롤러
    
    public TalkEffect talkEffect;   // 대화창의 이펙트 구현 클래스
    public bool clickScreen;

    void Start()
    {
        // 대사집을 불러옴
        TextAsset TalkAsset = Resources.Load("00Json/CharaterScript") as TextAsset;
        string jsonData = TalkAsset.text;
        TutorialScript = JsonUtility.FromJson<TalkScript>(jsonData);
        
        //필요한 컴포넌트 연결
        pc = GetComponent<PhaseCtrl>();
        talkEffect = GetComponent<TalkEffect>();
    }

    public void ScriptGoFunc()
    {
        string[] temp = SetTutorialStrings();
        if (currentTextIdx < temp.Length)
        {
            StopAllCoroutines(); // 진행중이던 대사 표시 코루틴 정지
            StartCoroutine(TalkStaccato(temp[currentTextIdx])); // 새로운 대사표시 코루틴 실행
        }
        else // 대사 인덱스가 대사 목록의 길이를 넘어서면
        {
            StopAllCoroutines();    // 모든 대사 표시 코루틴 정지
            textTarget.text = "";   // 대사를 빈칸으로 표시
            pc.phaseNum++;  // 튜토리얼을 다음페이즈로 진행
        }
    }

    // 화면 터치를 받아서 대사를 넘길수 있는 함수
    public void ClickScreen()
    {
        string[] temp = SetTutorialStrings();
        if (currentTextIdx < temp.Length)
        {
            if (!clickScreen)
            {
                StopAllCoroutines(); // 진행중이던 대사 표시 코루틴 정지
                textTarget.text = ""; 
                textTarget.text = temp[currentTextIdx];
                currentTextIdx++;
                clickScreen = true;
                return;
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(TalkStaccato(temp[currentTextIdx])); // 새로운 대사표시 코루틴 실행
                clickScreen = false;
                return;
            }
        }
        else // 대사 인덱스가 대사 목록의 길이를 넘어서면
        {
            StopAllCoroutines();    // 모든 대사 표시 코루틴 정지
            textTarget.text = "";   // 대사를 빈칸으로 표시
            pc.phaseNum++;  // 튜토리얼을 다음페이즈로 진행
        }
    }

    // 현재 페이즈에 사용될 대사를 지정해주는 함수
    public string[] SetTutorialStrings()
    {
        string[] currScript;
        switch (TutorialScript.PhaseNum)
        {
            case 1:
                currScript = TutorialScript.phase1;
                return currScript;
            case 2:
                currScript = TutorialScript.phase2;
                return currScript;
            case 3:
                currScript = TutorialScript.phase3;
                return currScript;
            case 4:
                currScript = TutorialScript.phase4;
                return currScript;
            case 5:
                currScript = TutorialScript.phase5;
                return currScript;
            default:
                return null;
        }
    }

    // 대사를 한글자 한글자 표시해주는 함수
    IEnumerator TalkStaccato(string data)
    {
        yield return null;
        textTarget.text = "";

        for (int i = 0; i < data.Length; i++)
        {
            int count = 0;
            if (data[i] == '<') // Rich텍스트 시작구문을 만나면
            {
                while (count < 2)
                {
                    textTarget.text += data[i];
                    i++;
                    if (data[i] == '>') // Rich텍스트가 종료 될때까지 한번에 표시
                        count++;
                }
            }
            else if(data[i] == '@') // 정해둔 @기호를 만나면
            {   // 시안색상의 플레이어 이름을 출력
                textTarget.text += "<color=red>"+PlayerPrefs.GetString("PLAYERNAME")+"</color>";
                i++;
            }
            textTarget.text += data[i]; // 한글자씩 텍스르를 추가함
            yield return new WaitForSeconds(0.03f);
        }
        clickScreen = true;
        currentTextIdx++;
    }
}
