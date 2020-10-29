using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 조합대 활성화여부를 담당하는 클래스
/// </summary>
public class DIYworkbench : MonoBehaviour, IInterAct
{
    public GameObject DIYlistUI;
    bool stopCoroutine;

    public void InterActPlayer() // 플레이어와 상호작용 함수
    {
        DIYlistUI.SetActive(true); // 조합 목록을 활성화하고
        StartCoroutine(DistancePlayer()); // 비활성화코루틴을 실행
    }

    IEnumerator DistancePlayer()
    {   // 조합대와 플레이어의 거리가 멀어지면 조합목록을 닫는 코루틴
        stopCoroutine = true;
        Player player = FindObjectOfType<Player>();
        while (stopCoroutine)
        {
            if((player.transform.position - transform.position).sqrMagnitude > 2)
            {   // 플레이어와 조합대의 거리가 멀어지면
                DIYlistUI.SetActive(false); // 조합목록이 비활성화됨
                stopCoroutine = false;  // 현재 이 코루틴이 종료됨
            }
            yield return new WaitForSeconds(0.2f); // 0.2초마다 해당 조건을 확인
        }
    }
}
