using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 육지 프리팹을 관리하는 클래스
/// </summary>
public class Ground : MonoBehaviour
{
    public GameObject x1;   // x축을 가로막는 벽1
    public GameObject x2;   // x축을 가로막는 벽2
    public GameObject z1;   // z축을 가로막는 벽1
    public GameObject z2;   // z축을 가로막는 벽2

    MapManager mm;  // 맵 매니저 연결

    private void Awake()
    {
        mm = GameObject.Find("MapManager").GetComponent<MapManager>();  // 맵매니저를 찾아옴
        mm.GroundEndCheck += hasGroundbyVoid;   //델리게이트에 함수 연결
    }

    // 육지에서 사방으로 1길이의 레이를 쏴서
    // 육지의 끝에 콜라이더를 활성화해 육지 밖으로 떨어지지 않게함
    public void hasGroundbyVoid()
    {
        Vector3 origin = transform.position + new Vector3(0, 0.8f, 0);
        if(!Physics.Raycast(origin, Vector3.forward, 1))
        {
             z1.SetActive(true);
        }
        if(!Physics.Raycast(origin, -Vector3.forward, 1))
        {
             z2.SetActive(true);
        }
        if(!Physics.Raycast(origin, Vector3.right, 1))
        {
             x1.SetActive(true);
        }
        if(!Physics.Raycast(origin, -Vector3.right, 1))
        {
             x2.SetActive(true);
        }
    }

    // 육지가 삭제 될때 델리게이트에서 해당 함수를 뺌
    private void OnDestroy()
    {
        mm.GroundEndCheck -= hasGroundbyVoid;
    }
}
