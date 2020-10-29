using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 튜토리얼 시작시 카메라의 움직임을 담당하는 클래스
/// </summary>
public class MovingCamera : MonoBehaviour
{
    public Transform StartCamPos;   // 시작하는 카메라위치
    public Camera currCam;  // 현재 카메라위치
    public Transform EndCamPos; // 카메라 이동의 도착지점

    void Start()
    {
        // 시작지점에 카메라 위치를 맞추고 움직임 코루틴 실행
        currCam.transform.position = StartCamPos.position;
        currCam.transform.rotation = StartCamPos.rotation;
    }

    // 카메라가 이동하는 코루틴 실행
    public IEnumerator MovingCam()
    {
        while (Vector3.SqrMagnitude(currCam.transform.position - EndCamPos.position) > 0.0001f)
        {   // 종료지점에 거의 근접할때까지 이동을 진행
            currCam.transform.position = Vector3.Lerp(currCam.transform.position, EndCamPos.position, Time.deltaTime );
            currCam.transform.rotation = Quaternion.Lerp(currCam.transform.rotation, EndCamPos.rotation, Time.deltaTime);

            yield return null;
        }
    }
}