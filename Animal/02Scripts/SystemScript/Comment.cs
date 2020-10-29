using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 코멘트 출력이 완료 되었는지 확인해줄 클래스
/// </summary>
public class BoolList
{
    public IEnumerator boolCoroutine;
    public bool isDone;

    public BoolList(IEnumerator corouTine, bool BOOL = false)
    {
        boolCoroutine = corouTine;
        isDone = BOOL;
    }
}

/// <summary>
/// 게임 화면내에 코멘트를 출력해주는 클래스
/// </summary>
public class Comment : MonoBehaviour
{
    public static Comment instance;

    GameObject panel;
    GameObject button;
    Image panelImage;
    Text panelText;
    TalkEffect talkEffect;

    List<BoolList> boolList = new List<BoolList>();

    public bool stopComment = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(PrintInOrder());
    }

    IEnumerator PrintInOrder()
    {   // 코멘트가 중복으로 출력되지 않도록 순차적으로 출력되게 해주는 코루틴
        while (true)
        {
            for (int i = 0; i < boolList.Count; i++)
            {
                yield return null;
                yield return StartCoroutine(boolList[0].boolCoroutine);
                yield return null;
                boolList[0].isDone = !boolList[0].isDone;
                boolList.RemoveAt(0);
            }
            yield return null;
        }
    }

    public void StopComment()
    {   // 스킵버튼을 누르면 현재출력중인 코멘트가 종료됨
        stopComment = true;
    }

    public BoolList CommentPrint(string comment, bool isDone = false)
    {   // 출력할 멘트를 입력받아 출력할 목록에 추가 시키는 함수
        IEnumerator currComment = CommentPrinter(comment); // 코루틴함수를 바로 실행하지 않고
        BoolList boollist = new BoolList(currComment, isDone); 
        boolList.Add(boollist); // 리스트에 저장함
        return boollist;
    }

    public BoolList CommentPrint(string[] comment, bool isDone = false)
    {   // 오버로드 함수
        IEnumerator currComment = CommentPrinter(comment);
        BoolList boollist = new BoolList(currComment, isDone);
        boolList.Add(boollist);
        return boollist;
    }

    IEnumerator CommentPrinter(string comment)
    {   // 입력받은 코멘트를 순차적으로 출력하는 코루틴
        yield return null;
        JoyStickManager.Instance.CanvasDisable(); // 조이스틱을 가려 채팅이 가려지지않게함

        if (panel == null)
            panel = GameObject.Find("ChattingCanvas").transform.GetChild(0).gameObject;
        if(button == null)
            button = GameObject.Find("ChattingCanvas").transform.GetChild(1).gameObject;

        // 필요한 변수가 null이 아닌지 체크
        panel.SetActive(true);  
        if (panelImage == null)
            panelImage = panel.gameObject.GetComponentInChildren<Image>();
        if (panelText == null)
            panelText = panel.gameObject.GetComponentInChildren<Text>();
        panel.SetActive(false);
        if (talkEffect == null)
            talkEffect = FindObjectOfType<TalkEffect>();

        int commentIdx = 0;
        bool isClick = true;

        panel.SetActive(true); // 텍스트 패널 활성화
        button.SetActive(true); // 스킵버튼 활성화

        yield return StartCoroutine(talkEffect.FadeInObject(panelImage, 0.5f)); // 패널이 서서히 나타남
        yield return null;

        panelText.text = "";
        while (commentIdx < comment.Length && !stopComment)
        {
            if (!panel.activeSelf)
                panel.SetActive(true); // 패널이 꺼진경우 다시 켬

            panelText.text += comment[commentIdx]; // 텍스트를 한글자씩 추가함
            commentIdx++;
            if (Input.GetMouseButtonDown(0))
            {   // 사용자 입력이 있는경우 텍스트가 끝까지 한번에 출력됨
                panelText.text = comment;
                commentIdx = comment.Length;
            }
            yield return new WaitForSeconds(0.03f);
        }

        yield return null;

        while (isClick && !stopComment)
        {   // 출력이 완료되면 사용자 입력을 기다림
            if (Input.GetMouseButtonDown(0))
                isClick = false;
            yield return null;
        }
        panelText.text = "";
        panel.SetActive(false); // 패널 비활성화
        button.SetActive(false); // 버튼 비활성화

        stopComment = false;

        yield return null;
        JoyStickManager.Instance.CanvasAble();

    }

    IEnumerator CommentPrinter(string[] comment)
    {   // 오버로드함수
        yield return null;
        JoyStickManager.Instance.CanvasDisable();

        if (panel == null)
            panel = GameObject.Find("ChattingCanvas").transform.GetChild(0).gameObject;
        if (button == null)
            button = GameObject.Find("ChattingCanvas").transform.GetChild(1).gameObject;

        panel.SetActive(true);
        if (panelImage == null)
            panelImage = panel.gameObject.GetComponentInChildren<Image>();
        if (panelText == null)
            panelText = panel.gameObject.GetComponentInChildren<Text>();
        panel.SetActive(false);
        if (talkEffect == null)
            talkEffect = FindObjectOfType<TalkEffect>();

        bool isClick = true;

        panel.SetActive(true);
        button.SetActive(true);

        yield return StartCoroutine(talkEffect.FadeInObject(panelImage, 0.5f));
        yield return null;

        for (int i = 0; i < comment.Length; i++)
        {
            int commentIdx = 0;
            panelText.text = "";
            while (commentIdx < comment[i].Length && !stopComment)
            {
                if (!panel.activeSelf)
                    panel.SetActive(true);

                panelText.text += comment[i][commentIdx];
                commentIdx++;
                if (Input.GetMouseButtonDown(0))
                {
                    panelText.text = comment[i];
                    commentIdx = comment[i].Length;
                }
                yield return new WaitForSeconds(0.03f);
            }
            yield return null;

            while (isClick && !stopComment)
            {
                if (Input.GetMouseButtonDown(0))
                    isClick = false;
                yield return null;
            }
            isClick = true;
        }

        panelText.text = "";
        panel.SetActive(false);
        button.SetActive(false);

        stopComment = false;

        yield return null;
        JoyStickManager.Instance.CanvasAble();

    }

    // 아래는 사용 예시
    //BoolList bl = Comment.instance.CommentPrint("아이템을 습득했다", interact);
    //StartCoroutine(Wait(bl));
    //IEnumerator Wait(BoolList bl)
    //{
    //    if (bl.isBool)
    //    {
    //        yield return new WaitWhile(() => bl.isDone);
    //        interact = true;
    //    }
    //    else
    //    {
    //        yield return new WaitUntil(() => bl.isDone);
    //        interact = true;
    //    }
    //}
}
