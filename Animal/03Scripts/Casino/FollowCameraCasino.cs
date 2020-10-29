using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class FollowCameraCasino : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        //만약 자신이 로컬플레이어라면
        //즉, 내 캐릭터와 타인의 캐릭터를 구분짓기 위함
        if (photonView.IsMine)
        {
            //씬에 존재하는 시네머신 가상 카메라를 찾음
            CinemachineVirtualCamera foolowCam =
                FindObjectOfType<CinemachineVirtualCamera>();

            //가상 카메라의 추적 대상을 자신의 트랜스폼으로 변경
            //타인의 플레이어 캐릭터를 추적할 수 있으므로
            //isMine을 통해서 자신의 캐릭터를 추적하도록 설정해줌
            foolowCam.Follow = transform;
            foolowCam.LookAt = transform;
        }
    }
}
