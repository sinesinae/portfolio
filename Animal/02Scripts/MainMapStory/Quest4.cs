using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest4 : QuestBehavior
{
    PROGRESS progress = PROGRESS.YET;
    string save = "QUEST4PRGRESS";
    string questName = "낚시란 무엇인가?";
    string qst2save = "QUEST2PRGRESS";

    protected override void Start()
    {
        base.Start();
        progress = (PROGRESS)PlayerPrefs.GetInt(save, 0);
        if (PlayerPrefs.GetInt(qst2save) == 3)
            LaunchingQuest();
    }
    public override void LaunchingQuest()
    {
        if (progress != PROGRESS.YET)
            return;

        QuestManager.instance.AddQuest(questName, "섬에서 생존하기 위해 낚시를 해보자\n이곳을 터치하면 설명을 볼수있다", "", "Quest4", "StateYetClick");

        progress = PROGRESS.START;
        PlayerPrefs.SetInt(save, (int)progress);
    }

    public void StateYetClick()
    {
        if (progress != PROGRESS.START)
            return;

        if (SearchInventory(90000011) == null)
        {
            string[] comments = new string[]
            {
            "낚시를 해서 물고기를 잡으면 먹어서 배를 채울수도 있고 팔아서 돈을 벌수도있다",
            "그러나 낚시를 하려면 낚시대가 필요한 법 우선 낚시대를 만들어 보자",
            "낚시대는 나뭇가지 10개와 잡초 5개로 집안의 DIY제작대를 통해 만들수 있다",
            };
            Comment.instance.CommentPrint(comments);
        }
        else
        {
            progress = PROGRESS.GOING;
            PlayerPrefs.SetInt(save, (int)progress);
            Comment.instance.CommentPrint("낚시대가 생겼으니 이제 물고기를 잡아보자");

            QuestManager.instance.RemoveQueset(questName);
            QuestManager.instance.AddQuest(questName, "낚시를 해보자!\n낚시 방법은 이곳을 터치해서 볼수있다", "", "Quest4", "StateGoingClick");
            ResetQusetList();
        }
    }

    public void StateGoingClick()
    {
        if (progress != PROGRESS.GOING)
            return;

        if (SearchInventory(30000001) == null)
        {

            string[] comment = new string[] 
            {
            "인벤토리에서 낚시대를 착용하고 물고기를 향해 A버튼을 누르면 낚시를 시작할수있다",
            "물고기 '썩어'를 잡은후 퀘스트를 클릭해 퀘스트를 완료하자",
            };
            Comment.instance.CommentPrint(comment);
        }
        else
        {
            progress = PROGRESS.END;
            PlayerPrefs.SetInt(save, (int)progress);

            QuestManager.instance.RemoveQueset(questName);
            Comment.instance.CommentPrint("물고기를 많이 잡아서 빚을 탕감하도록 하자");
            ResetQusetList();
        }
    }
}
