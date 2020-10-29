using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Quest1 : MonoBehaviour
{
    Ctrl ctrl;//플레이어 동작입력클래스
    PlayerMovement playerMovement;//플레이어 동작구현클래스

    public GameObject ScriptPannel;//대화창
    public Text ScriptText;//대화내용
    public Image ScriptImage;//퀘스트창에 보여질이미지
    public TalkEffect talkEffect; // 대화창의 이펙트 구현 클래스

    public Transform DASHI_tr;//다시다의 위치

    int isClear = 0;
    string saveStr = "QUEST1ISCLEAR";

    void Start()
    {
        ctrl = FindObjectOfType<Ctrl>();
        playerMovement = FindObjectOfType<PlayerMovement>();

        ctrl.transform.Rotate(270, 180, 0);//플레이어 시작로테이트
        ctrl.transform.position = new Vector3(-300, 1, -300);//플레이어위치
        ctrl.transform.GetChild(0).GetComponent<Animator>().enabled = false;//에니메이터 멈춤
        ctrl.enabled = false;//동작멈춤
        playerMovement.enabled = false;//동작구현멈춤

        isClear = PlayerPrefs.GetInt(saveStr, 0);
        Debug.Log(isClear);
        talkEffect = GetComponent<TalkEffect>();
        if (isClear < 10)
        {
            ctrl.transform.position = new Vector3(85, 1, 15);//플레이어위치
            StartCoroutine(QuestPlay());
            FindObjectOfType<NPC_DASHIDA>().AddObject("Quest1", "DASHI_A_start");
        }
        else if(isClear == 10)
        {
            ctrl.transform.position = new Vector3(85, 1, 15);
            FindObjectOfType<NPC_DASHIDA>().AddObject("Quest1", "DASHI_A_start");
            ctrl.enabled = true;
            playerMovement.enabled = true;
        }
        else
        {
            ctrl.enabled = true;
            playerMovement.enabled = true;
        }
    }
    


    IEnumerator QuestPlay()// 대사와 동작를 지정해주는 함수
    {
        yield return null;
        for (int i = 0; i < QuestManager.instance.questList.Count; i++)
        {
            if (QuestManager.instance.questList[i].QuestName == "#생존을 위한 움막을 짓자1")
            {
                ctrl.transform.GetChild(0).GetComponent<Animator>().enabled = true;

                ctrl.enabled = true;
                playerMovement.enabled = true;
                ctrl.transform.Rotate(90, 0, 0);
                ctrl.transform.Rotate(0, 180, 0);//플레이어 시작로테이트
                ctrl.transform.position = new Vector3(85, 1, 11);

                StopAllCoroutines();
            }
            yield return null;
        }

        JoyStickManager.Instance.CanvasDisable();//조이스틱멈추고

        FindObjectOfType<HealthManager>().AddHealth(10);//체력10
        FindObjectOfType<FoodManager>().addFOOD(10);//배고픔10


        string quest1 = "으.....머리야 눈부셔 여긴어디지???";//대화추가
        bool click = true;
        ScriptPannel.SetActive(true);//대화창활성화
        yield return StartCoroutine(talkEffect.FadeInObject(ScriptImage, 0.5f)); //이코르틴이 끝날때까지 기다림 yield return
        for (int i = 0; i < quest1.Length; i++)
        {
            ScriptText.text += quest1[i];//한글자씩 ScriptText.text에 집어넣어줌 
            yield return new WaitForSeconds(0.03f);
        }
        while (click)//클릭했을때 대화를 빠르게 넘김
        {
            if (Input.GetMouseButtonDown(0))
                click = false;
            yield return null;
        }
        click = true;
        ScriptText.text = "";
        yield return null;
        yield return StartCoroutine(talkEffect.FadeOutObject(ScriptImage, 0.5f));
        ScriptPannel.SetActive(false);

        ctrl.transform.Rotate(90, 0, 0);
        

        string[] comments_a = new string[]
        {
             "신너굴 그가 정신을 차렸을땐\n태어나서 처음보는 아름다운 에메랄드빛의\n바다가 펼쳐져있었다.",
             "멀리로는 수평선과 \n하늘, 바다가 구분되지 않을만큼의\n선명하고 맑은 하늘이 투영되고있었다.",
             "대한민국에선 보기힘든 늪지대의 향연,\n유리같이 반짝이는 시멘트와 찰떡인\n거무죽죽한 버섯과 잡초들",
             "저멀리로 보이는 선명하고 푸르른 능선과\n사파이어 만큼 영롱한 물길이\n신너굴의 주위를 둘러싸고 있었다.",
             "태어나서 처음보는 광경에\n넋을 잃은것도 잠시\n신너굴은 정신을 차렸다.",
             "'하아.......\n내 너굴생이 어쩌다 이렇게 되버린거지???'"
        };

        BoolList bl = Comment.instance.CommentPrint(comments_a);
        yield return new WaitUntil(() => bl.isDone);

        yield return null;

        yield return StartCoroutine(PLAYER_tr());//플레이어가 움직이고나서 대화 출력

        string[] comments_b = new string[]
        {
            "'꾸륵꾸륵꾸륵'",
            "'정면에 보이는 바다의 물살이 갈라지며\n흡사 다시마가 승천하는듯한\n무언가가 솟아났다.'"
        };
        BoolList bl_b = Comment.instance.CommentPrint(comments_b);
        yield return new WaitUntil(() => bl_b.isDone);

        yield return StartCoroutine(DASHIDA());

        string[] comments_c = new string[]
        {
            "\"신너굴님 맞으신가요 ??\"",
            "\"네에에???...제가 신너굴입니다만....\"",
            "\"아하! 안녕하세요\n저는 이섬의 관리를 맡고있는'다시다'입니다.",
            "궁금하신사항이나 필요하신게 있으시면\n해드릴진 모르겠지만 내키면 도와드릴게요~ \"",
            "'정말 다시다였구나.....'",
            "\"우선 빛을갚으려면 생존부터 해야겠죠?\n주변에있는 나뭇가지나 잡초들을 구해서 \n움막이라도 지어보세요\""
        };
        BoolList bl_c = Comment.instance.CommentPrint(comments_c);
        yield return new WaitUntil(() => bl_c.isDone);


        StartCoroutine(FindObjectOfType<NPC_DASHIDA_TRIGGER>().DownDASHIDA());//다시다 내려감



        ctrl.transform.GetChild(0).GetComponent<Animator>().enabled = true;
        ctrl.enabled = true;
        playerMovement.enabled = true;//플레이어 동작가능

        JoyStickManager.Instance.CanvasAble();//조이스틱 활성화
        QuestManager.instance.AddQuest("#생존을 위한 움막을 짓자1", "'나뭇가지 x5'\n'잡초 x10’", "", "Quest1", "QUESTONCLICK");//퀘스트추가

        FindObjectOfType<NPC_DASHIDA>().AddObject("Quest1", "DASHI_A_start");//다시다에게 퀘스트추가
        isClear = 10;
        PlayerPrefs.SetInt(saveStr, isClear);
    }
    public void DASHI_A_start()
    {
        StartCoroutine(DASHI_A());
    }

    public IEnumerator DASHI_A()//퀘스트 완료후 다시다에게 갔을때 구현할 함수
    {
        bool japco = false;
        bool namu = false;
        int japco_idx = 0;
        int namu_idx = 0;
        for (int i = 0; i < Inventory.instance.inventoryItemList.Count; i++)
        {
            if (Inventory.instance.inventoryItemList[i].itemID == 10000021)//인벤토리에 잡초가 10개 이상이면 트루
            {
                if (Inventory.instance.inventoryItemList[i].itemCount >= 10)
                {
                    japco = true;
                    japco_idx = i;
                }

            }
        }
        for (int i = 0; i < Inventory.instance.inventoryItemList.Count; i++)//인벤토리에 나무가 5개이상이면 트루
        {
            if (Inventory.instance.inventoryItemList[i].itemID == 10000024)
            {
                if (Inventory.instance.inventoryItemList[i].itemCount >= 5)
                {
                    namu = true;
                    namu_idx = i;
                }
            }

        }
        if (japco && namu)//잡초10개 나무가지5개이상가지고있으면 퀘스트 진행 
        {
            JoyStickManager.Instance.CanvasDisable();

            string[] comment_a = new string[]
            {
                "\"굼뜬줄 알았더니 생각보다 빨리왔네,,,,\n입돌아가긴 싫은가보지?\n...아 혼잣말이니 신경쓰지마세요 하핫!\"",
                "\"움막으로 바로 교환해드릴게요\n허접하긴하지만 맨몸보단 낫겠죠 ?? \""
            };
            BoolList bl_a = Comment.instance.CommentPrint(comment_a);
            yield return new WaitUntil(() => bl_a.isDone);

            JoyStickManager.Instance.CanvasAble();

            Inventory.instance.inventoryItemList[japco_idx].itemCount -= 10;
            Inventory.instance.inventoryItemList[namu_idx].itemCount -= 5;
            QuestManager.instance.RemoveQueset("#생존을 위한 움막을 짓자1");
            isClear = 11;
            PlayerPrefs.SetInt(saveStr, isClear);
            FindObjectOfType<Quest1_2>().LaunchingQuest();
        }
        else//아니면 다른대화창 호출
        {
            string[] comment_b = new string[]
            {
                "\"신너굴님 재료가 부족한거같네요\n입돌아가기 싫으시면\n해지기전에 얼른모아오세요\"",
                "\"역시 굼뜨구만..빛은언제갚니..하...내실적...\n아! 혼잣말이니 신경쓰지마세요 하핫!\n다녀오세요~\""
            };
            BoolList bl_b = Comment.instance.CommentPrint(comment_b);
            yield return new WaitUntil(() => bl_b.isDone);

            FindObjectOfType<NPC_DASHIDA>().AddObject("Quest1", "DASHI_A_start");
        }
    }

    IEnumerator PLAYER_tr()//퀘스트에 따른 플레이어 동작구현함수
    {
        yield return null;
        float i = 0;
        Vector3 origine = ctrl.transform.position;
        while (i <= 2f)
        {
            i += Time.deltaTime;
            ctrl.transform.position = Vector3.Lerp(origine, new Vector3(85, 1, 11), i / 2f);
            yield return new WaitForSeconds(Time.deltaTime);
        }

    }
    IEnumerator DASHIDA()//다시다 동작구현함수
    {
        yield return null;
        float i = 0;
        int a = -1;
        Vector3 origine = DASHI_tr.transform.position;

        while (i <= 2f)
        {
            i += Time.deltaTime;
            DASHI_tr.position = Vector3.Lerp(origine, new Vector3(87, 1.5f, 10), i / 2f);

            a = a * -1;
            DASHI_tr.rotation = Quaternion.Euler(new Vector3(0, 0, 15 * a));

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public void QUESTONCLICK()//퀘스트창의 해당 퀘스트를 클릭하면
    {
        StartCoroutine(QuestPlay2());
    }



    IEnumerator QuestPlay2()//퀘스트창을 클릭했을때 보여질 대화창
    {
        yield return null;
        JoyStickManager.Instance.CanvasDisable();

        string[] comments = new string[]
        {
            "하 우선 살아남아야하니\n다시다의 말에따라 집지을 재료를 구해보자\n",
            "나뭇가지와 잡초등은 섬 곳곳에서 자라고 있다.\n화면 오른쪽하단의 버튼을 사용해 채집해보자"
        };
        BoolList bl = Comment.instance.CommentPrint(comments);
        yield return new WaitUntil(() => bl.isDone);

        yield return null;

        ctrl.transform.GetChild(0).GetComponent<Animator>().enabled = true;

        ctrl.enabled = true;
        playerMovement.enabled = true;

        JoyStickManager.Instance.CanvasAble();
    }
}