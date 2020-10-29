using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    ATMManager ATM;//엔딩을 호출할 조건
    Player Player;//신너굴의 움직임

    public GameObject Bronze;//동갈치움직임 조정
    public GameObject Silver;//은갈치움직임 조정
    public GameObject EndingPanel;//엔딩 선택 패널
    public GameObject BalancePanel;//ATM상 코인패널
    public GameObject BlackPanel;//엔딩을보여줄패널

    public Transform Light;

    public Image PanelImage;//BlackPanel의 이미지
    public Image image;//앤딩에 나올 이미지

    public Text Text;//엔딩에 나올 글
    float talkspeed = 0.05f;//엔딩의 글씨나 이미지의 속도조절패널



    void Start()
    {
        ATM = FindObjectOfType<ATMManager>();
        Player = FindObjectOfType<Player>();

    }

    void Update()
    {

    }
    private void ending()//엔딩을 위한 조건검사
    {
        QuestManager.instance.AddQuest("#빛청산을 완료했다", "이제 어떻게되는거지?", "ENDING", "ending_");

    }
    public void NPCManager()//npc를 원하는 위치에 생성해줄 함수
    {
        Light.Rotate(0, -240, 0);
        BalancePanel.SetActive(false);
       GameObject _Bronze=Instantiate(Bronze, ATM.transform.position + new Vector3(0, 1, -0.4f), Quaternion.identity);
       GameObject _Silver=Instantiate(Silver, ATM.transform.position + new Vector3(0, 1, 1.7f), Quaternion.identity);
        _Bronze.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        _Bronze.transform.Rotate(0, 90f, 0f);
        _Silver.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        _Silver.transform.Rotate(0, 90f, 0f);

        JoyStickManager.Instance.CanvasDisable();
    }
    public void ending_()
    {
        StartCoroutine(ENDING());
    }
    public IEnumerator ENDING()//엔딩조건을 만족하면 실행할함수
    {
        yield return null;
        Comment.instance.CommentPrint("드디어 채무를 전액탕감했다!\n긴 시간이였지만 보람찼어\n뭔가 시원섭섭한 기분이 드는것같아......");
        NPCManager();
        BoolList bl= Comment.instance.CommentPrint("\"신너굴님 채무를 전액상환하신걸 축하드립니다.\n신너굴님께서 저희와 함께 섬을 나가시겠어요 ?\"");
        yield return new WaitUntil(() => bl.isDone);
        EndingPanel.SetActive(true);

    }
    public void Exit()//버튼을 누르면 호출할함수
    {
        EndingPanel.SetActive(false);
        Comment.instance.CommentPrint("\"그렇다면 더 지체할것도 없겠네요~\n바로떠나시죠!!\"");
        StartCoroutine(EndingManager());

    }

    IEnumerator EndingManager()//검은 엔딩 화면과 스토리를 보여줄 함수
    {
        yield return new WaitForSeconds(3);
        BlackPanel.SetActive(true);//엔딩화면을 보여줄패널
        for (float a = 0; a <= 1; a += Time.deltaTime)//이미지가 서서히 나타나게함
        {
            yield return null;
            PanelImage.color = Color.Lerp(Color.clear, Color.black, a);
        }
        PanelImage.color = Color.black;

        yield return new WaitForSeconds(0.02f);

        for (float a = 0; a <= 1; a += Time.deltaTime)
        {
            yield return null;
            image.color = Color.Lerp(Color.clear, Color.white, a);
        }
        image.color = Color.white;
        yield return StartCoroutine(TalkStaccato("섬을 탈출하다니 아직 실감이 나질않는군.... \n은갈치와 동갈치의 배웅을 받으며 \n배에오른 신너굴\n배에 혼자 남겨진 그는 \n연명의 숲에서의 일들을 회상하며 \n잠이든다......."));
        yield return new WaitForSeconds(3);
        Destroy(image);
        Text.text = "";
        yield return new WaitForSeconds(3);
        for (float a = 0; a <= 1; a += Time.deltaTime)
        {
            yield return null;
            Text.text = "피신해요 연명의숲\n";
            Text.color = Color.Lerp(Color.clear, Color.red, a);
        }
        yield return new WaitForSeconds(3);
        Text.fontSize = 300;
        Text.resizeTextMaxSize = 300;
        yield return StartCoroutine(TalkStaccato("\nEND"));
        yield return new WaitForSeconds(10);
        File.Delete(Application.persistentDataPath + "/datajson.json");//계속하기 파일삭제
        SceneManager.LoadScene(0);//처음 화면으로 돌림



    }
    IEnumerator TalkStaccato(string data)//한글자씩 텍스트를 추가해주는 함수
    {
        yield return null;

        for (int i = 0; i < data.Length; i++)
        {
            Text.text += data[i];
            if (talkspeed > 0)
            {
                yield return new WaitForSeconds(talkspeed);
            }

        }
        Text.color = Color.white;

    }
}


