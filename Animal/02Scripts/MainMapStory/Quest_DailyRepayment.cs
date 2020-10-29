using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 매일 업데이트 되는 상환퀘스트 클래스
/// </summary>
public class Quest_DailyRepayment : QuestBehavior
{
    PROGRESS progress = PROGRESS.YET;
    string save = "QUESTDAILYREPAYMENT";
    string questName = "오늘의 상환 스케줄";
    string questDes = "";
    int loanBase = 100;
    int loanInt = 10;
    string loanB = "LOANBASE";
    string loanI = "LOANINTEREST";


    protected override void Start()
    {
        base.Start();
        progress = (PROGRESS)PlayerPrefs.GetInt(save, 0);
        if (progress == PROGRESS.START) // 퀘스트가 시작상태일때
            if (CheckQuest()) // NPC에게 해당 퀘스트가 등록되어있는지 확인후
                NPCdashida.AddObject("Quest_DailyRepayment", "RepaymentTodashida");
    }

    bool CheckQuest()
    {   // NPC에게 같은 퀘스트가 존재하는지 확인하는 함수
        foreach (ActObject ao in NPCdashida.actobject)
            if (ao.className == "Quest_DailyRepayment" && ao.methodName == "RepaymentTodashida")
                return false;

        return true;
    }

    public override void LaunchingQuest()
    {   // 퀘스트 시작 함수
        if (progress == PROGRESS.START || progress == PROGRESS.GOING)
            return;

        SetLoanAndDescription(); // 오늘의 상환금액 설정
        StartCoroutine(StartQuest()); // 퀘스트 코루틴실행
    }
    

    IEnumerator StartQuest()
    {
        yield return null;

        QuestManager.instance.AddQuest(questName, questDes, "", "Quest_DailyRepayment", "ClickQuest");
        Comment.instance.CommentPrint("새로운 날이 밝았다\n새로운 상환 스케줄을 확인하자");
        NPCdashida.AddObject("Quest_DailyRepayment", "RepaymentTodashida");

        progress = PROGRESS.START;
        PlayerPrefs.SetInt(save, (int)progress);
    }

    void SetLoanAndDescription()
    {
        // 상환금액이 매번 올라간다 - 게임 난이도의 점증적 증가
        loanBase = PlayerPrefs.GetInt(loanB, 100) + 50; 
        PlayerPrefs.SetInt(loanB, loanBase);
        loanInt = PlayerPrefs.GetInt(loanI, 10) + 5; 
        PlayerPrefs.SetInt(loanI, loanInt);
        questDes = "오늘 상환 금액은\n" + "원금 : " + loanBase.ToString() + " / 이자 : " + loanInt.ToString();
    }

    public void ClickQuest()
    {
        string[] comments = new string[]
        {
            questDes,
            "상환 금액이 모이면 다시다에게 가져가자",
        };
        Comment.instance.CommentPrint(comments);
    }

    public void RepaymentTodashida()
    {
        if (progress != PROGRESS.START)
            return;

        if(TimeManager.instance.timezone == TimeManager.TIMEZONE.NIGHT)
        {
            Comment.instance.CommentPrint("상환 가능한 시간이 아닙니다");
            NPCdashida.AddObject("Quest_DailyRepayment", "RepaymentTodashida");
            return;
        }

        CoinManager coinmanager = FindObjectOfType<CoinManager>();
        if (coinmanager.coin >= loanBase + loanInt)
        {
            coinmanager.coin -= loanBase + loanInt;

            Comment.instance.CommentPrint("다행히 시간안에 가져오셨군요\n오늘은 무서운일이 일어나지 않을거에요");
            FindObjectOfType<ChasingAtNight>().notToNight = true;
            QuestManager.instance.RemoveQueset(questName);
            ResetQusetList();
            progress = PROGRESS.END;
            PlayerPrefs.SetInt(save, (int)progress);
            ATMManager atm = FindObjectOfType<ATMManager>();
            atm.ATM -= loanBase;
            FindObjectOfType<CoinManager>().ATMaddcoin(loanBase);
            atm.CheckATM();
            atm.CheckCOIN();
            atm.SaveATMcoin();
        }
        else
        {
            Comment.instance.CommentPrint("아직 상환액을 모으지 못했어요\n시간이 계속 흐른답니다\n늦으면 큰일난다구요!");
            NPCdashida.AddObject("Quest_DailyRepayment", "RepaymentTodashida");
        }
    }
}
