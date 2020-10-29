using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest2 : QuestBehavior
{
    PROGRESS progress = PROGRESS.YET;
    string save = "QUEST2PRGRESS";
    string questName = "생존을 위해 빚과 이자를 탕감하자";
    string qst3save = "QUEST3PRGRESS";

    protected override void Start()
    {
        base.Start();
        progress = (PROGRESS)PlayerPrefs.GetInt(save, 0);
        if (PlayerPrefs.GetInt(qst3save) == 3)
            LaunchingQuest();
    }

    public override void LaunchingQuest()
    {
        if (progress != PROGRESS.YET)
            return;

        StartCoroutine(StartQuest());
    }

    IEnumerator StartQuest()
    {
        yield return null;

        string[] comments = new string[]
        {
            "다시다 : \"...신너굴님 잠시 저좀 뵐까요?\"",
            "내주제에 오라면 가야지....\n왠지 가지않으면 안좋은일이 생길것같은 예감이다.\n어서 다시다에게 가보자!",
        };
        BoolList bl = Comment.instance.CommentPrint(comments);
        yield return new WaitUntil(() => bl.isDone);

        QuestManager.instance.AddQuest(questName, "다시다에게 가서 말을 들어보자");
        NPCdashida.AddObject("Quest2", "TalkDashida_Start");

        progress = PROGRESS.START;
        PlayerPrefs.SetInt(save, (int)progress);
    }

    public void TalkDashida_Start()
    {
        if (progress != PROGRESS.START)
            return;
        StartCoroutine(TalkDashda_start());
    }

    IEnumerator TalkDashda_start()
    {
        yield return null;
        string[] comments = new string[]
        {
            "\"오홍 역시 재빠르시군요\n다름이아니라 신너굴님의 채무상환스케쥴을\n알려드리려고 뵙자고했어요.\"",
            "\"우선 돈을 빌리셨으면 갚으셔야하는게 맞는거니까 매일 PM 8시전까지 원금및 이자상환부탁드릴게용\"",
            "\"혹시나 그럴일은 없겠지만 PM 8시전까지 상환하지않으신다면 무서운분들을 만나실꺼에요 험한꼴 당하고 싶으시면 뭐 상관없겠지만요~\"",
            "\"현재 신너굴님의 대출금액은 10.000.000벨, 대출이자는 1.000.000벨입니다. \"",
            "\"상환스케쥴은 변동될수 있는점 양해부탁드리구요 신너굴님의 노력여하에따라 이자율은 늘어날수도 줄어들수도 있으니 알아서 잘해보세용. 헿\"",
            "\"상점을 이용해서 채집한 아이템이나 제작한아이템들을 판매해서 티끌모아 빛청산!도 가능하시지 않을까요? 찡긋\"",
            "\"오늘의 상환스케쥴\n원금 100벨 이자10벨 토탈110벨\"",
        };
        BoolList bl = Comment.instance.CommentPrint(comments);
        yield return new WaitUntil(() => bl.isDone);

        QuestManager.instance.RemoveQueset(questName);
        QuestManager.instance.AddQuest(questName, "오늘의 상환스케쥴\n원금 100벨 이자10벨 토탈110벨","", "Quest2", "RepaymentToday");
        ResetQusetList();
        NPCdashida.AddObject("Quest2", "RepaymentTodashida");
        StartTimer();

        progress = PROGRESS.GOING;
        PlayerPrefs.SetInt(save, (int)progress);
    }

    public void RepaymentToday()
    {
        if (progress != PROGRESS.GOING)
            return;

        CoinManager coinmanager = FindObjectOfType<CoinManager>();
        if (coinmanager.coin <= 110)
        {
            Comment.instance.CommentPrint("상환금액은 110벨이다\n얼른 돈을 모으도록 하자\n현재 소지금은 인벤토리에서 확인가능하다");
        }
        else
        {
            Comment.instance.CommentPrint("상환금액을 다 모았다\n다시다에게 가자");
        }
    }

    public void RepaymentTodashida()
        //다시다에게 붙여줄 다시다 상환 함수
    {
        if (progress != PROGRESS.GOING)
            return;

        if (TimeManager.instance.timezone == TimeManager.TIMEZONE.NIGHT)
        {
            Comment.instance.CommentPrint("상환 가능한 시간이 아닙니다");
            NPCdashida.AddObject("Quest2", "RepaymentTodashida");
            return;
        }

        CoinManager coinmanager = FindObjectOfType<CoinManager>();
        if (coinmanager.coin >= 110)
        {
            coinmanager.coin -= 110;
            Comment.instance.CommentPrint("다행히 시간안에 가져오셨군요\n오늘은 무서운일이 일어나지 않을거에요");
            FindObjectOfType<ChasingAtNight>().notToNight = true;
            QuestManager.instance.RemoveQueset(questName);
            ResetQusetList();
            progress = PROGRESS.END;
            PlayerPrefs.SetInt(save, (int)progress);

            StartCoroutine(WaitNextQuest());
        }
        else
        {
            Comment.instance.CommentPrint("아직 110벨을 모으지 못했어요\n시간이 계속 흐른답니다\n늦으면 큰일난다구요!");
            NPCdashida.AddObject("Quest2", "RepaymentTodashida");
        }
    }

    IEnumerator WaitNextQuest()
    {
        yield return new WaitForSeconds(60f);
        GetComponent<Quest4>().LaunchingQuest();
    }

    void  StartTimer()
    {
        TimeManager.instance.ActiveTimeManager();
        TimeManager.instance.SetTime(900);
    }

    public bool IsEndQuest()
    {
        if (progress == PROGRESS.END)
            return true;

        return false;
    }
}
