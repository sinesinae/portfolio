using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 텐트 오브젝트의 미니맵상 위치와 출입구 기능구현을 하는 클래스
/// </summary>
public class TentCtrl : MonoBehaviour
{
    GameObject homePos;
    public RectTransform HomePos;

    private void Start()
    {
        // 비활성화된 게임오브젝트를 찾기위해 자식을 찾아 들어감
        homePos = GameObject.Find("MinimapPos").transform.GetChild(0).transform.GetChild(1).gameObject;
        
        homePos.SetActive(true); // 미니맵에서 집위치 표시가 활성화됨

        // 건설한 집의 위치를 미니상에 알맞게 위치시킴
        HomePos = homePos.GetComponent<RectTransform>();
        HomePos.anchoredPosition = new Vector3((transform.position.x * 3) , (transform.position.z * 3) , 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 집 입구에 트리거가 있고 그곳에 캐릭터가 오게되면 집 좌표로 이동시킴
        Player player = other.transform.GetComponent<Player>();
        if(player != null)
        {
            FindObjectOfType<MinimapCtrl>().TracePlayer = false; // 미니맵표시가 플레이어를 추적하는것을 멈춤

            FindObjectOfType<MinimapCtrl>().rt.position = new Vector3(HomePos.position.x + (transform.position.x * 3), HomePos.position.y + (transform.position.z * 3), 1);
            player.transform.position = new Vector3(-300, 0.05f, -302);
        }
    }
}
