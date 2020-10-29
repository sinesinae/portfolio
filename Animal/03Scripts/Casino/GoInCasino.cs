using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class GoInCasino : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1"; //게임 버전
    public Button CasinoButton;
    public GameObject CasinoCanvas;

    public Text connectionInfoText;
    public Button joinButton;
    public InputField nickName;


    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void GetCasinoButton()
    {
        CasinoCanvas.SetActive(true);
        PhotonNetwork.GameVersion = gameVersion;
        //설정한 정보로 마스터 서버 접속 시도
        PhotonNetwork.ConnectUsingSettings();
        //룸 접속 버튼 잠시 비활성화
        joinButton.interactable = false;
        //접속 시도 중임을 텍스트로 표시
        connectionInfoText.text = "마스터 서버에 접속 중...";
        JoyStickManager.Instance.joystickCanvas.SetActive(false);
    }

    public void CancleButton()
    {
        CasinoCanvas.SetActive(false);
        JoyStickManager.Instance.joystickCanvas.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        //룸 접속 버튼 다시 활성화
        joinButton.interactable = true;
        connectionInfoText.text = "마스터 서버와 연결됨";
    }
    //마스터 서버 접속 실패 시 자동실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;
        connectionInfoText.text = "마스터 서버와 연결되지 않음\n접속 재시도 중...";
        //서버 연결 재시도
        PhotonNetwork.ConnectUsingSettings();
    }

    //룸 접속 시도 함수
    public void Connect()
    {
        joinButton.interactable = false;

        //서버에 접속 중인 상태라면
        if (PhotonNetwork.IsConnected && nickName.text.Length != 0 && nickName.text != "" && nickName.text != null)
        {
            connectionInfoText.text = "룸에 접속...";
            //접속이 되고나면 룸에 접속 시도
            PhotonNetwork.JoinRandomRoom();
            PhotonNetwork.LocalPlayer.NickName = nickName.text;
        }
        else if (nickName.text.Length == 0 || nickName.text == "" || nickName.text == null)
        {
            nickName.GetComponentInChildren<Text>().text = "닉네임을 입력하지 않았습니다";
            joinButton.interactable = true;
        }
        else
        {
            connectionInfoText.text = "마스터 서버와 연결되지 않음\n접속 재시도 중...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    //(빈 방이 없어) 랜덤 룸 참가에 실패한 경우 자동 실행
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "빈 방이 없음, 새로운 방 생성...";
        //접속할 방이 없는 경우 최대 4명이 접속 가능한 방을 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
        //RoomOptions option = new RoomOptions();
        //option.IsOpen = true;
        //option.IsVisible = true;
        //option.CleanupCacheOnLeave = true;
        //option.MaxPlayers = 8;

        //TypedLobby lobby = new TypedLobby("Lobby", LobbyType.Default);

        //PhotonNetwork.CreateRoom("Casino Lobby", option, lobby);

    }
    //룸에 참가 완료된 경우 자동 실행
    public override void OnJoinedRoom()
    {
        //접속 상태 표시
        connectionInfoText.text = "방에 참가 성공";
        //모든 룸 참가자가 Main 씬을 로드하게 함
        PhotonNetwork.LoadLevel("Casino");
        Debug.Log(PhotonNetwork.CurrentRoom);
    }
}
