using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrologueScript
{   // Json파싱을 위한 클래스 구조
    public int PhaseNum;
    public string[] phase1;
    public string[] phase2;
    public string[] phase3;
    public string[] phase4;
    public string[] phase5;
    public string[] phase6;
    public string[] phase7;
}

/// <summary>
/// 프롤로그 진행상황 스크립트
/// </summary>
public class Prologue : MonoBehaviour
{
    public Text prol_text;
    PrologueScript PS = new PrologueScript();
    float talkspeed = 0.05f;

    bool click = true;
    bool skip = true;

    OpeningEffect OE;
    MovingCamera mc;

    private void Start()
    {
        OE = FindObjectOfType<OpeningEffect>();
        TextAsset json = Resources.Load("00Json/TutorialPrologue", typeof(TextAsset)) as TextAsset;
        PS = JsonUtility.FromJson<PrologueScript>(json.text);
        mc = FindObjectOfType<MovingCamera>();

        StartCoroutine(TitleEffect(2));
    }

    // 프롤로그를 스킵하는 버튼함수
    public void Skip(GameObject button)
    {
        StopAllCoroutines();
        prol_text.text = "";
        OE.StartEffect();
        StartCoroutine(mc.MovingCam());
        button.SetActive(false);
    }

    // 프롤로그 진행하는 코루틴함수
    IEnumerator TitleEffect(float time)
    {
        prol_text.text = "피신해요 연명의숲";
        prol_text.color = Color.clear;

        for (float i = 0; i <= time; i += Time.deltaTime)
        {
            prol_text.color = Color.Lerp(Color.clear, Color.white, i / time);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        prol_text.color = Color.white;
        yield return new WaitForSeconds(time);

        for (float i = 0; i <= time; i += Time.deltaTime)
        {
            prol_text.color = Color.Lerp(Color.white, Color.clear, i / time);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        prol_text.text = "";
        prol_text.color = Color.white;

        yield return StartCoroutine(TalkStaccato(PS.phase1[0]));
        while (click)
        {
            if (Input.GetMouseButtonDown(0))
                click = false;
            yield return null;
        }

        click = true;
        prol_text.text = "";
        yield return null;


        for (int i = 0; i < PS.phase2.Length; i++)
        {
            StartCoroutine(StopStatcatto());
            yield return StartCoroutine(TalkStaccato(PS.phase2[i]));
            prol_text.text += "\n";
            talkspeed = 0.05f;
            yield return new WaitForSeconds(talkspeed);
        }

        skip = false;
        yield return null;


        while (click)
        {
            if (Input.GetMouseButtonDown(0))
                click = false;
            yield return null;
        }

        click = true;
        prol_text.text = "";

        yield return StartCoroutine(TalkStaccato(PS.phase3[0]));

        while (click)
        {
            if (Input.GetMouseButtonDown(0))
                click = false;
            yield return null;
        }
        
        click = true;
        prol_text.text = "";

        skip = true;
        for (int i = 0; i < PS.phase4.Length; i++)
        {
            StartCoroutine(StopStatcatto());
            yield return StartCoroutine(TalkStaccato(PS.phase4[i]));
            prol_text.text += "\n";
            talkspeed = 0.05f;
            yield return new WaitForSeconds(talkspeed);
        }
        skip = false;
        yield return null;


        while (click)
        {
            if (Input.GetMouseButtonDown(0))
                click = false;
            yield return null;
        }

        click = true;
        prol_text.text = "";

        skip = true;
        for (int i = 0; i < PS.phase5.Length; i++)
        {
            StartCoroutine(StopStatcatto());
            yield return StartCoroutine(TalkStaccato(PS.phase5[i]));
            prol_text.text += "\n";
            talkspeed = 0.05f;
            yield return new WaitForSeconds(talkspeed);
        }
        skip = false;
        yield return null;


        while (click)
        {
            if (Input.GetMouseButtonDown(0))
                click = false;
            yield return null;
        }

        click = true;
        prol_text.text = "";

        skip = true;
        for (int i = 0; i < PS.phase6.Length; i++)
        {
            StartCoroutine(StopStatcatto());
            yield return StartCoroutine(TalkStaccato(PS.phase6[i]));
            prol_text.text += "\n";
            talkspeed = 0.05f;
            yield return new WaitForSeconds(talkspeed);
        }
        skip = false;
        yield return null;

        while (click)
        {
            if (Input.GetMouseButtonDown(0))
                click = false;
            yield return null;
        }

        click = true;
        prol_text.text = "";

        skip = true;
        for (int i = 0; i < PS.phase7.Length; i++)
        {
            StartCoroutine(StopStatcatto());
            yield return StartCoroutine(TalkStaccato(PS.phase7[i]));
            prol_text.text += "\n";
            talkspeed = 0.05f;
            yield return new WaitForSeconds(talkspeed);
        }
        skip = false;
        yield return null;

        while (click)
        {
            if (Input.GetMouseButtonDown(0))
                click = false;
            yield return null;
        }
        prol_text.text = "";
        OE.StartEffect();
        StartCoroutine(mc.MovingCam());
    }

    // 클릭시 대사가 빠르게 진행됨
    IEnumerator StopStatcatto()
    {
        while (skip)
        {
            if(Input.GetMouseButtonDown(0))
                talkspeed = 0;
            yield return null;
        }
    }

    // 한글자씩 화면에 출력되게 하는 함수
    IEnumerator TalkStaccato(string data)
    {
        yield return null;

        for (int i = 0; i < data.Length; i++)
        {
            prol_text.text += data[i]; // 한글자씩 텍스르를 추가함
            if (talkspeed > 0)
            {
                yield return new WaitForSeconds(talkspeed);
            }
            
        }
    }
}
