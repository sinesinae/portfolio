using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest3_2 : QuestBehavior
{
    PROGRESS progress = PROGRESS.YET;
    string save = "QUEST3_2PRGRESS";
    string questName = "버섯슾을 만들자";

    CoinManager coin;

    protected override void Start()
    {
        base.Start();
        progress = (PROGRESS)PlayerPrefs.GetInt(save, 0);
        coin = FindObjectOfType<CoinManager>();
    }
    public override void LaunchingQuest()
    {
        if (progress != PROGRESS.YET)
            return;

        QuestManager.instance.AddQuest(questName, "버섯슾은 나뭇가지x3 버섯x3개로 만들수있다\n완성후 퀘스트를 클릭해 완료할수있다", "", "Quest3_2", "CheckDone");
        ResetQusetList();
        progress = PROGRESS.START;
        PlayerPrefs.SetInt(save, (int)progress);
    }

    public void CheckDone()
    {
        if (progress != PROGRESS.START)
            return;

        bool isbool = false;
        for (int i = 0; i < Inventory.instance.inventoryItemList.Count; i++)
        {
            if (Inventory.instance.inventoryItemList[i].itemID == 20000012)
            {
                if (Inventory.instance.inventoryItemList[i].itemCount >= 1)
                {
                    isbool = true;
                }
            }
        }
        if (isbool)
        {
            Comment.instance.CommentPrint("'버섯슾을 완성했어'\n보상으로 30코인을 얻었다");
            coin.coin += 30;
            progress = PROGRESS.END;
            PlayerPrefs.SetInt(save, (int)progress);
            QuestManager.instance.RemoveQueset(questName);
            ResetQusetList();
        }
        else
        {
            Comment.instance.CommentPrint("'버섯슾은 아직인가'");
        }
    }


}
