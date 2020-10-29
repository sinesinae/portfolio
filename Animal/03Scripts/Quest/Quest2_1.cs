using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Quest2_1 : MonoBehaviour
{
    Ctrl ctrl;
    PlayerMovement playerMovement;
    GameObject panel;
    Image panelImage;
    Text panelText;
    TalkEffect talkEffect;

    private void OnEnable()
    {
        ctrl = FindObjectOfType<Ctrl>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        panel = GameObject.Find("ChattingCanvas").transform.GetChild(0).gameObject;
        panel.SetActive(true);
        panelImage = panel.gameObject.GetComponentInChildren<Image>();
        panelText = panel.gameObject.GetComponentInChildren<Text>();
        panel.SetActive(false);
        talkEffect = GetComponent<TalkEffect>();

        ctrl.enabled = false;
        playerMovement.enabled = false;

        StartCoroutine(QuestStart());

    }


    //퀘스트 1_2 움막을 짓자 진행 부분
    IEnumerator QuestStart()
    {
        yield return null;

        JoyStickManager.Instance.CanvasDisable();

        string comment = "'재수없는 다시마에게서 움막을 받았다. \n움막설치를 원하는 장소에서 \n퀘스트창을 열고 텐트를 눌러서 사용해보자'";
        int commentIdx = 0;
        bool isClick = true;
        //퀘스트 내용 부분
        panel.SetActive(true);

        yield return StartCoroutine(talkEffect.FadeInObject(panelImage, 0.5f));
        yield return null;

        while (commentIdx < comment.Length)
        {
            panelText.text += comment[commentIdx];
            commentIdx++;
            yield return new WaitForSeconds(0.03f);
        }

        while (isClick)
        {
            if (Input.GetMouseButton(0))
                isClick = false;
            yield return null;
        }

        isClick = true;
        panelText.text = "";
        panel.SetActive(false);

        yield return null;


        //퀘스트 내용이 다 나오고 움직임 재생부분
        ctrl.enabled = true;
        playerMovement.enabled = true;
        JoyStickManager.Instance.CanvasAble();

        QuestManager.instance.AddQuest("생존을 위한 움막을 짓자2", "움막설치를 원하는 장소에서 퀘스트창을 열고 텐트를 사용해보자", "itemIcon/80000011", "Quest1_2", "UsingQuest");
        PlayerFunctions.instance.SetDefaultInput();

    }

    public void UsingQuest()
    {
        Debug.Log("aaaa");
    }
}
