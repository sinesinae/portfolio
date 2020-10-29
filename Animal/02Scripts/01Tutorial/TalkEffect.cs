using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 대사 진행에 사용되는 효과를 표현해줄 코루틴함수를 관리하는 클래스
/// </summary>
public class TalkEffect : MonoBehaviour
{
    public GameObject scriptPannel; // 대사 표현 패널
    public GameObject FullscreenButton; // 전체 화면 터치 버튼
    CharacterTalk ct;

    private void Awake()
    {
        ct = GetComponent<CharacterTalk>();
    }

    //스크립트가 진행하게 하는 코루틴
    public IEnumerator ScriptGo()
    {
        if (scriptPannel != null && FullscreenButton != null && ct != null)
        {
            yield return null;
            scriptPannel.SetActive(true);
            yield return StartCoroutine(FadeInObject(scriptPannel.transform.GetChild(0).GetComponent<Image>(), 0.5f));
            scriptPannel.SetActive(true);
            FullscreenButton.SetActive(true);
            ct.ScriptGoFunc();
        }
    }

    // 대사 진행을 멈추는 코루틴함수
    public IEnumerator ScriptNoGo()
    {
        if (scriptPannel != null && FullscreenButton != null && ct != null)
        {
            FullscreenButton.SetActive(false);
            yield return StartCoroutine(FadeOutObject(scriptPannel.transform.GetChild(0).GetComponent<Image>(), 0.5f));
            scriptPannel.SetActive(false);
        }
    }

    // 서서히 나타나는 효과 코루틴함수
    public IEnumerator FadeInObject(Image target, float speed)
    {
        float i = 0;
        while (i <= speed)
        {
            i += Time.deltaTime;
            target.color = Color.Lerp(Color.clear, Color.white, i / speed);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        target.color = Color.white;
    }

    // 서서히 사라지는 효과 코루틴 함수
    public IEnumerator FadeOutObject(Image target, float speed)
    {
        float i = 0;
        while (i <= speed)
        {
            i += Time.deltaTime;
            target.color = Color.Lerp(Color.white, Color.clear, i / speed);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        target.color = Color.clear;
    }
}
