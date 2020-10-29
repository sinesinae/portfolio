using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 밤시간의 추격전을 구현하는 클래스
/// </summary>
public class ChasingAtNight : QuestBehavior
{
    public GameObject[] chaserPrefab;
    GameObject[] chaser;
    Transform bodyTr;
    public bool notToNight // 오늘밤에 추격전이 일어나는가 false:추격함 true:추격안함
    {
        get
        {
            if (PlayerPrefs.GetInt("SAVENOTTONIGHT",0) == 0) // 저장된값이 0이면 false반환
                return false;
            else
            {   // 저장된값이 0이 아닌경우
                if (PlayerPrefs.GetInt("SAVENOTTONIGHTDAY") != TimeManager.instance.day)
                {   // 저장된 날자가 오늘인지 확인후 오늘이 아니면 false반환하고 0으로 저장
                    PlayerPrefs.SetInt("SAVENOTTONIGHT", 0);
                    return false;
                }
                // 저장값이 0이 아니고 저장된날자가 오늘인경우 true
                return true;
            }
        }
        set
        {
            if (value == false)
                PlayerPrefs.SetInt("SAVENOTTONIGHT", 0); //false이면 0으로 저장
            else
            {
                PlayerPrefs.SetInt("SAVENOTTONIGHT", 1); //true이면 1로 저장하고
                PlayerPrefs.SetInt("SAVENOTTONIGHTDAY", TimeManager.instance.day); // 오늘날자를 저장함
            }
        }
    }


    public void StartChasing()
    {
        if (notToNight) // 추격이 일어나는지 조건확인
            return;

        TimeManager.instance.flowTime = false; // 시간흐름을 멈춤
        StartCoroutine(_StartChasing()); // 추격전 시작
        StartCoroutine(DetectEndNight()); // 밤이 끝나는지 확인함
    }

    IEnumerator DetectEndNight()
    {   // 현재시간대가 밤이 아닐때까지 대기
        yield return new WaitUntil(() => TimeManager.instance.timezone != TimeManager.TIMEZONE.NIGHT);
        StartCoroutine(EndChase());
    }

    public void _EndChase()
    {   // 외부에서 추격전 종료를 호출하는 함수
        StartCoroutine(EndChase());
    }

    public IEnumerator EndChase()
    {   // 추격전 종료 코루틴
        notToNight = true;
        yield return null;

        TimeManager.instance.flowTime = false; // 시간흐름을 멈추고

        foreach (GameObject go in chaser) // 추격 오브젝트의 EndChase를 호출함
            go.GetComponent<ChasingKnifeFish>().EndChase();

        string[] comments = new string[] // 대화목록
        {
            "은갈치 : \"오늘은 이만 돌아 가도록 하지...\"",
            "동갈치 : \"가도록 하지!\"",
            "은갈치 : \"채무 이행에 성실히 임해주세요 고객님!\"",
            "동갈치 : \"고객님!\"",
            "신너굴 : ...'한숨도 못잤더니 너무 피곤해 '",
        };
        BoolList bl = Comment.instance.CommentPrint(comments); // 대화목록을 출력함
        yield return new WaitUntil(() => bl.isDone); // 대화목록이 다 표시될때까지 대기

        TimeManager.instance.flowTime = true; // 시간이 흐름

        foreach (GameObject go in chaser) // 추격오브젝트들 파괴
            Destroy(go);

        StopAllCoroutines(); // 모든 코루틴 종료
    }

    IEnumerator _StartChasing()
    {   // 추격이 시작되는 코루틴
        yield return null;

        if (bodyTr == null)
            bodyTr = FindObjectOfType<Player>().transform; // 추격할 플레이어

        if(chaser == null)
            chaser = new GameObject[chaserPrefab.Length]; // 추격자 배열

        for (int i = 0; i < chaser.Length; i++)
        {
            chaser[i] = Instantiate(chaserPrefab[i]); //추격자 생성
            int offset = i > 0 ? 3 : -3; // 추격자별 offset설정
            Vector3 pos = bodyTr.position + bodyTr.right * offset; // 추격자 포지션 저장
            chaser[i].transform.position = new Vector3(pos.x, 2, pos.z); // 추격자를 플레이어 좌우에 배치
            chaser[i].AddComponent<ChasingKnifeFish>(); // 추격자에게 추격구현 클래스 부여
            Rigidbody rb = chaser[i].AddComponent<Rigidbody>(); // Rigidbody컴포넌트 부여
            rb.useGravity = false; // 중력의 영향을 받지 않음
        }


        string[] comments = new string[] // 코멘트 목록
        {
            "은갈치 : \"이런 신너굴이 같은!! 돈을 빌렸으면 제때제때 갚아야할것이아이냐?!\"",
            "동갈치 : \"아이냐?!\"",
            "은갈치 : \"이자는 조상님이 갚아주나 앙?!\"",
            "동갈치 : \"앙?!\"",
            "신너굴 : \"돈이 없어서 못드린걸 어떻게합니까? 굽신굽신\"",
            "은갈치 : \"누군 땅파서 장사하냐 우리도 다 월급받는 처지야!\"",
            "동갈치 : \"처지야!\"",
            "은갈치 : \"뒤져서나오면 대당 10벨씩이다!!\"",
            "'이런 돈없는것도서러운데 달밤에 체조라니!!'",
            "채무불이행으로 은갈치와 동갈치의 협박을 이겨내야한다.",
            "갈치들에게 한푼도 뺏기고 싶지않다면 이밤을 죽도록 달리자!!!!",
            "코인을 빼돌리거나 숨겨놓는다해도 귀신같이 찾는녀석들이니 헛된짓은 하지말도록하자!",
        };

        BoolList bl = Comment.instance.CommentPrint(comments); // 코멘트 출력
        yield return new WaitUntil(() => bl.isDone); // 코멘트가 다 출력될때까지 대기

        TimeManager.instance.flowTime = true; // 시간이 흐름
    }
}
