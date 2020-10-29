using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest1_3 : QuestBehavior
{
    PROGRESS progress = PROGRESS.YET;
    string save = "QUEST1_3PRGRESS";
    string questName = "생존을 위한 움막을 짓자3";

    protected override void Start()
    {
        base.Start();
        progress = (PROGRESS)PlayerPrefs.GetInt(save, 0);
    }
    public override void LaunchingQuest()
    {
        if (progress != PROGRESS.YET)
            return;

        Comment.instance.CommentPrint("'움막설치를 완료했다. \n다시다에게 가볼까??'");
        QuestManager.instance.AddQuest(questName, "다시다에게 말을 걸어 보자", "", "", "");
        ResetQusetList();
        progress = PROGRESS.START;
        PlayerPrefs.SetInt(save, (int)progress);

        NPCdashida.AddObject("Quest1_3", "TalkDashida");
    }

    public void TalkDashida()
    {
        if (progress != PROGRESS.START)
            return;
        StartCoroutine(TalkDashida_Coroutine());
    }

    IEnumerator TalkDashida_Coroutine()
    {
        string[] comments = new string[]
        {
            "움막 구경은 잘 하셨나요??\n허접하긴해도 있을건 다있답니다.\n움막안의 DIY창은 열어보셨나용??",
            "DIY창에서 각종 생존도구와 장비들을 만들수\n있어요! 신너굴님같은 가난쟁이에겐\n자급자족이 도움되지않을까요?",
        };
        BoolList bl = Comment.instance.CommentPrint(comments);
        yield return new WaitUntil(() => bl.isDone);

        progress = PROGRESS.END;
        PlayerPrefs.SetInt(save, (int)progress);
        QuestManager.instance.RemoveQueset(questName);
        ResetQusetList();

        yield return new WaitForSeconds(1f);

        // 다음퀘스트로 연결
        GetComponent<Quest3>().LaunchingQuest();
    }
}
