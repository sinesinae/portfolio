using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 튜토리얼 시작시 화면과 NPC움직임을 담당하는 클래스
/// </summary>
public class OpeningEffect : MonoBehaviour
{
    [Header("배경 검은색들")]
    public RectTransform Right; // 화면을 가린 가림막 오른쪽
    public RectTransform Left, Top, Bottom; // 가림막 하,좌,우
    Vector3 RightEndPos, LeftEndPos, TopEndPos, BottomEndPos; // 가림막들의 도착지점
    float speed = 0.5f; // 가림막 이동속도

    [Header("NPC 두명")]
    public Transform Chara1;    // NPC1
    public Transform Chara2, Chara1EndPos, Chara2EndPos; // NPC2, NPC1의도착점, NPC2의도착점
    float moveSpeed = 3;    // NPC이동속도

    [Header("터치버튼")]
    public GameObject TouchButton;  // 전체화면터치버튼
    PhaseCtrl pc;   // 페이즈컨트롤
    TalkEffect te;  // 토크이펙트

    void Start()
    {
        pc = GetComponent<PhaseCtrl>();     // 컴포넌트연결
        te = GetComponent<TalkEffect>();    // 컴포넌트연결

        pc.CreateFourMiniMap(); // 선택지로 보여줄 미니맵 생성

        RightEndPos = Right.position + (Vector3.right * Screen.width);  // 가림막 도착지점
        LeftEndPos = Left.position + (-Vector3.right * Screen.width);   // 가림막 도착지점
        TopEndPos = Top.position + (Vector3.up * Screen.height); // 가림막 도착지점
        BottomEndPos = Bottom.position + (-Vector3.up * Screen.height);  // 가림막 도착지점
       
    }

    // 오프닝씬 시작 함수
    public void StartEffect()
    {
        StartCoroutine(BlindDisappear());   // 가림막 움직임 코루틴 실행
        StartCoroutine(CharacterMoveRight());   // 캐릭터 움직임 코루틴 실행
        Destroy(Right.gameObject, 3f);  // 가림막 제거
        Destroy(Left.gameObject, 3f);   // 가림막 제거
        Destroy(Top.gameObject, 3f);    // 가림막 제거
        Destroy(Bottom.gameObject, 3f); // 가림막 제거
    }

    // 가림막의 움직임을 표현하는 코루틴함수
    IEnumerator BlindDisappear()
    {
        yield return new WaitForSeconds(0.5f); // 시작후 0.5초부터
        while(Right != null)
        {
            Right.position = Vector3.Lerp(Right.position, RightEndPos, Time.deltaTime*speed);
            Left.position = Vector3.Lerp(Left.position, LeftEndPos, Time.deltaTime* speed);
            Top.position = Vector3.Lerp(Top.position, TopEndPos, Time.deltaTime* speed);
            Bottom.position = Vector3.Lerp(Bottom.position, BottomEndPos, Time.deltaTime* speed);
            yield return null;
        }
    }

    // 캐릭터 움직임을 표현해줄 코루틴함수
    IEnumerator CharacterMoveRight()
    {
        yield return new WaitForSeconds(1.0f); // 시작후 1초부터
        while (Vector3.SqrMagnitude(Chara1.position - Chara1EndPos.position) > Mathf.Epsilon)
        {
            Chara1.position = Vector3.MoveTowards(Chara1.position, Chara1EndPos.position, Time.deltaTime*moveSpeed);
            Chara2.position = Vector3.MoveTowards(Chara2.position, Chara2EndPos.position, Time.deltaTime*moveSpeed);
            yield return null;
        }
        StartCoroutine(te.ScriptGo()); // 움직임이 끝나면 대사를 시작
    }
}
