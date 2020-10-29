using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 집안 내부 출구에서 외부로 이동시키기 위한 클래스
/// </summary>
public class TentCtrl_inside : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        // 출구에 위치한 트리거에 플레이어가 닿으면 집 밖으로 이동시킴
        Player player = other.transform.GetComponent<Player>();
        if (player != null)
        {
            FindObjectOfType<MinimapCtrl>().TracePlayer = true; // 미니맵에서 플레이어를 추적하게함

            Vector3 pos = FindObjectOfType<TentCtrl>().transform.position + new Vector3(0, 0, -2);
            player.transform.position = pos;
        }
    }
}
