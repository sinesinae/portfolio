using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest3 : QuestBehavior
{
    PROGRESS progress = PROGRESS.YET;
    string save = "QUEST3PRGRESS";
    string questName = "생존을 위한 식량을 구해보자";

    protected override void Start()
    {
        base.Start();
        progress = (PROGRESS)PlayerPrefs.GetInt(save, 0);
    }
    public override void LaunchingQuest()
    {
        if(progress == PROGRESS.YET)
            StartCoroutine(LaunchingComment());
    }

    IEnumerator LaunchingComment()
    {

        string[] comments = new string[]
        {
            "\"뭐라도 드셔야 빛을 갚을테니??\n근처에서 버섯이라도 찾아드세요\"",
            "'하 우선 살아남아야하니 \n다시다의 말에따라 식량을 구해보자'",
            "버섯은 섬곳곳에서 자라고 있다\n독버섯일수도 있지만 포만감이 떨어져\n기절하는것보단 낫겠지.....또르르",
        };
        BoolList bl = Comment.instance.CommentPrint(comments);
        yield return new WaitUntil(() => bl.isDone);

        QuestManager.instance.AddQuest(questName, "버섯을 10개 모아서 다시다에게 가져가자","","Quest3", "RemindQuest");
        NPCdashida.AddObject("Quest3", "Dashida_Answer");
        ResetQusetList();
        progress = PROGRESS.START;
        PlayerPrefs.SetInt(save, (int)progress);
    }

    public void RemindQuest()
    {
        Comment.instance.CommentPrint("섬 곳곳에 흩어져 있는 버섯을 10개 모아서 다시다에게 가져가자. 버섯은 A버튼으로 얻을수있다.");
    }

    public void Dashida_Answer()
    {
        if (progress != PROGRESS.START)
            return;

        bool isbool = false;
        int idx = 0;
        for (int i = 0; i < Inventory.instance.inventoryItemList.Count; i++)
        {
            if (Inventory.instance.inventoryItemList[i].itemID == 10000023)
            {
                if (Inventory.instance.inventoryItemList[i].itemCount >= 10)
                {
                    isbool = true;
                    idx = i;
                }
            }
        }
        if (isbool)
        {
            StartCoroutine(DoneComment());

            GetComponent<Quest3_1>().LaunchingQuest();
            GetComponent<Quest3_2>().LaunchingQuest();

            progress = PROGRESS.END;
            PlayerPrefs.SetInt(save, (int)progress);
            QuestManager.instance.RemoveQueset(questName);
            ResetQusetList();
        }
        else
        {
            Comment.instance.CommentPrint("\"10개가 무슨말인지 모르시나요?\n버섯 10개를 모아오세요\"");
            NPCdashida.AddObject("Quest3", "Dashida_Answer");
        }
    }

    IEnumerator DoneComment()
    {
        string[] comments = new string[]
        {
            "\"버섯을 다 모아오셨군요. \nDIY키트로 음식을 만들수있어요\"",
            "\"버섯구이는 나뭇가지1개와 버섯1개\n버섯슾은 나뭇가지3개와 버섯3개로\n만들수있어요..그정도는 하실수있겠죠?\"",
        };
        BoolList bl = Comment.instance.CommentPrint(comments);
        yield return new WaitUntil(() => bl.isDone);

        yield return new WaitForSeconds(60f);

        FindObjectOfType<Quest2>().LaunchingQuest();
    }
}