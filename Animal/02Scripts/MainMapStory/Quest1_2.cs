using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Quest1_2 : QuestBehavior
{
    PROGRESS progress = PROGRESS.YET;
    string save = "QUEST1_2PRGRESS";
    string questName = "생존을 위한 움막을 짓자2";

    protected override void Start()
    {
        base.Start();
        progress = (PROGRESS)PlayerPrefs.GetInt(save, 0);
    }

    //퀘스트 시작 호출함수
    public override void LaunchingQuest()
    {
        if (progress != PROGRESS.YET)
            return;
        
        progress = PROGRESS.START;
        PlayerPrefs.SetInt(save, (int)progress);
        StartCoroutine(QuestStart());
    }


    //퀘스트 1_2 움막을 짓자 진행 부분
    IEnumerator QuestStart()
    {
        yield return null;

        if (progress != PROGRESS.START)
            yield break;

        StopInput();

        Comment.instance.CommentPrint("'재수없는 다시마에게서 움막을 받았다. \n움막설치를 원하는 장소에서 \n퀘스트창을 열고 텐트를 눌러서 사용해보자'");

        //퀘스트 내용이 다 나오고 움직임 재생부분
        StartInput();

        QuestManager.instance.AddQuest(questName, "움막설치를 원하는 장소에서 퀘스트창을 열고 텐트를 사용해보자", "itemIcon/80000011", "Quest1_2", "UsingQuest");
        PlayerFunctions.instance.SetDefaultInput();

        progress = PROGRESS.GOING;
        PlayerPrefs.SetInt(save, (int)progress);
    }

    public void UsingQuest()
    {
        if (goingonQuest)
            return;

        if (progress != PROGRESS.GOING)
            return;

        goingonQuest = true;

        ClearCurrentGridPixels();

        BuildingGridCanvasCtrl.instance.DrawGrid(ctrl.transform.position, 4, 3);

        StopInput();

        for (int i = 0; i < BuildingGridCanvasCtrl.instance.child.Count; i++)
            if (BuildingGridCanvasCtrl.instance.child[i].GetComponent<Image>().color == BuildingGridCanvasCtrl.instance.notpermit)
            {
                StartCoroutine(NotAllowedBuilding());
                return;
            }

        StartCoroutine(AllowedBuilding());
    }

    IEnumerator AllowedBuilding()
    {
        yield return null;

        string comment = "이곳에 설치할까?\nA버튼 : 설치한다\nB버튼 : 취소";
        int commentIdx = 0;

        panel.SetActive(true);

        yield return StartCoroutine(talkEffect.FadeInObject(panelImage, 0.5f));
        yield return null;


        while (commentIdx < comment.Length)
        {
            panelText.text += comment[commentIdx];
            commentIdx++;
            yield return new WaitForSeconds(0.03f);
        }

        JoyStickManager.Instance.pressA = Abutton;
        JoyStickManager.Instance.pressB = Bbutton;
        confirm = true;
        GameObject.Find("ChattingCanvas").transform.GetComponent<Canvas>().sortingOrder = -1;
        JoyStickManager.Instance.CanvasAble();

        while (confirm)
        {
            yield return null;
        }
        panelText.text = "";
        panel.SetActive(false);

        yield return null;

        PlayerFunctions.instance.SetDefaultInput();
    }

    public void Abutton()
    {
        confirm = false;
        ClearCurrentGridPixels();
        StartInput();

        Vector3 pos = BuildingGridCanvasCtrl.instance.transform.position;
        GameObject tent = Instantiate(tentPrefab, this.transform);
        tent.transform.position = pos;

        
        int y = (int)pos.y;

        CreateMapData cmd = FindObjectOfType<CreateMapData>();
        int size = cmd.size;

        for(int x = (int)pos.x ; x < (int)pos.x + 4; x++)
        {
            for (int z = (int)pos.z;  z < (int)pos.z + 3; z++)
            {
                cmd.MapDataStr[x + (z * size) + ((y-1) * size * size)] = "09"; 
            }
        }
        cmd.MapDataStr[(int)pos.x + ((int)pos.z * size) + ((int)pos.y * size * size)] = "98";
        cmd.SaveDataJson();

        progress = PROGRESS.END;
        PlayerPrefs.SetInt(save, (int)progress);

        QuestManager.instance.RemoveQueset(questName);
        ResetQusetList();

        StartCoroutine(AbuttonComplete());
    }

    IEnumerator AbuttonComplete()
    {
        yield return null;
        BoolList bl = Comment.instance.CommentPrint("'우와 움막설치를 해냈어!'");
        yield return new WaitUntil(() => bl.isDone);

        yield return new WaitForSeconds(1);

        GetComponent<Quest1_3>().LaunchingQuest();
    }

    public void Bbutton()
    {
        confirm = false;
        ClearCurrentGridPixels();
        StartInput();
        goingonQuest = false;
    }

    IEnumerator NotAllowedBuilding()
    {
        yield return null;

        BoolList bl = Comment.instance.CommentPrint("이곳은 장애물이있어\n다른 설치할곳을 알아보자");
        yield return new WaitUntil(() => bl.isDone);

        ClearCurrentGridPixels();
        StartInput();
        goingonQuest = false;
    }

    void ClearCurrentGridPixels()
    {
        for (int i = 0; i < BuildingGridCanvasCtrl.instance.child.Count; i++)
        {
            Destroy(BuildingGridCanvasCtrl.instance.child[i]);
        }
        BuildingGridCanvasCtrl.instance.child.Clear();
    }
}
