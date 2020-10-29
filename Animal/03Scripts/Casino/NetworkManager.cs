using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("GetCasinoButton")]
    public Button GetCasinoButton;
    public GameObject HansoonCanvases;
    public GameObject CasinoPannel;
    public GameObject DisconnectPannel;
    [Header("DisconnectPanel")]
    public InputField NickNameInput;

    [Header("LobbyPanel")]
    public GameObject LobbyPanel;
    //public InputField RoomInput;
    public Text WelcomeText;
    public Text LobbyInfoText;
    public Button[] CellBtn;
    public Button PreviousBtn;
    public Button NextBtn;

    [Header("RoomPanel")]
    public GameObject RoomPanel;
    public Text ListText;
    public Text RoomInfoText;
    public Text[] ChatText;
    public InputField ChatInput;

    [Header("ETC")]
    public Text StatusText;
    public PhotonView PV;

    [Header("CreateRoomPanel")]
    public GameObject CreateRoomPanel;
    public InputField RoomInput;
    public InputField RoomPassword;
    public string gameType;
    public InputField BetMoney;
    public List<Button> numberofPeople = new List<Button>();
    public byte maxPlayer;
    public List<Button> RoomPublicOX = new List<Button>();
    public List<Button> GameTypeButtons = new List<Button>();
    private bool isRoompublic = true;
    public Button CreateRoomButton;
    public GameObject RoomChoiceErrorPannel;
    public Text RoomCreateErrorText;

    [Header("PasswordPanel")]
    public GameObject passwordPanel;
    public InputField passwordInput;


    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;

    #region 카지노 버튼 클릭 이벤트
    public void CasinoButtonClick()
    {
        if (CasinoPannel.activeSelf == true)
        {
            CasinoPannel.SetActive(false);
            JoyStickManager.Instance.joystickCanvas.SetActive(true);
        }
        else
        {
            CasinoPannel.SetActive(true);
            JoyStickManager.Instance.joystickCanvas.SetActive(false);
        }
    }

    #endregion



    //public void GetCasinoButtonPress()
    //{
    //    if (DisconnectPannel.activeSelf == true)
    //    {
    //        DisconnectPannel.SetActive(false);
    //        JoyStickManager.Instance.joystickCanvas.SetActive(true);
    //    }
    //    else
    //    {
    //        DisconnectPannel.SetActive(true);
    //        JoyStickManager.Instance.joystickCanvas.SetActive(false);
    //    }
    //}
    #region 방리스트 갱신
    // ◀버튼 -2 , ▶버튼 -1 , 셀 숫자
    public void MyListClick(int num)
    {
        if (num == -2) --currentPage;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
        else if (num == -1) ++currentPage;
        else{
            //RoomInfo[] roominfo = PhotonNetwork.GetCustomRoomList();
            PhotonNetwork.JoinRoom(myList[multiple + num].Name);
        }
        MyListRenewal();
    }

    void MyListRenewal()
    {
        // 최대페이지
        maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;

        // 이전, 다음버튼
        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // 페이지에 맞는 리스트 대입
        multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
            CellBtn[i].transform.GetChild(0).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
            CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
            
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }
        MyListRenewal();
    }
    #endregion


    #region 서버연결
    //void Awake() => Screen.SetResolution(2280, 1080, false);

    void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
        LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "로비 / " + PhotonNetwork.CountOfPlayers + "접속";
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby()
    {
        
        LobbyPanel.SetActive(true);
        RoomPanel.SetActive(false);
        DisconnectPannel.SetActive(false);
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        WelcomeText.text = PhotonNetwork.LocalPlayer.NickName + "님 환영합니다";
        myList.Clear();
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public void CreateRoomifNotExist()
    {
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers =4;
        roomOption.IsOpen = true;
        roomOption.CustomRoomProperties = new Hashtable()
        {
            {"roomname", "Casino Lobby"},
        };

        roomOption.CustomRoomPropertiesForLobby = new string[]
        {
            "roomname",
        };
        //PhotonNetwork.JoinOrCreateRoom("Casino Lobby", new RoomOptions { MaxPlayers = 4 }, TypedLobby.Default);
        PhotonNetwork.JoinOrCreateRoom("Casino Lobby", roomOption, TypedLobby.Default);
        
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        LobbyPanel.SetActive(false);
        //RoomPanel.SetActive(false);
    }
    #endregion


    #region 방
    //public void CreateRoom() => PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Room" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 4 });

    public void CreateRoomOption()
    {
        CreateRoomPanel.SetActive(true);
    }
    System.Collections.IEnumerator CreateRoomError(int type)
    {
        if (type == 1)
        {
            RoomCreateErrorText.text = "게임 이름을 적어 주세요";
        }

        if (type == 2)
        {
            RoomCreateErrorText.text = "게임 종류를 선택하여 주세요";
            
        }
        RoomChoiceErrorPannel.SetActive(true);
        yield return new WaitForSeconds(1f);
        RoomChoiceErrorPannel.SetActive(false);
    }

    [PunRPC]
    public void CreateRoom()
    {
        string roomName;
        if (RoomPassword.text.Length != 0)
        {
           roomName = RoomInput.text + "_" + RoomPassword.text;
        }
        else
        {
            roomName = RoomInput.text;
        }
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = this.maxPlayer;
        //Debug.Log(maxPlayer);
        roomOption.IsVisible = this.isRoompublic;
        roomOption.IsOpen = true;
        roomOption.CustomRoomProperties = new Hashtable()
        {
            {"roomname",  RoomInput.text == "" ? "Room" + Random.Range(0, 100) : RoomInput.text},
            {"password" , RoomPassword.text},
            {"gameType", gameType},
            //{"betMoney" ,BetMoney},
            
        };
        if(RoomInput.text.Length == 0)
        {
            StartCoroutine(CreateRoomError(1));
            return;
        }else if(gameType.Length == 0)
        {
            StartCoroutine(CreateRoomError(2));
            return;
        }

        roomOption.CustomRoomPropertiesForLobby = new string[]
        {
            "roomname",
            "password"
        };
        if (RoomPassword.text.Length != 0)
        {
            //비번 있음
            PhotonNetwork.CreateRoom(RoomInput.text, roomOption);
        }
        else
        {
            //비번없음
            PhotonNetwork.CreateRoom(roomName, roomOption);
        }
    }
    
    public void GameTypeButton(int idx)
    {
        foreach (Button button in GameTypeButtons)
        {
            ColorBlock unselectedcb = button.colors;
            unselectedcb.normalColor = Color.white;
            button.colors = unselectedcb;
        }
        ColorBlock cb = GameTypeButtons[idx-1].colors;
        cb.normalColor = Color.green;
        GameTypeButtons[idx-1].colors = cb;



        if (idx == 1)
        {
            gameType = "Logic";

        }
    }
    public void CancleNotExistGameType()
    {
        StopAllCoroutines();
        RoomChoiceErrorPannel.SetActive(false);
    }
    public void NumberofPlayerButton(int idx)
    {
       
        maxPlayer = (byte)idx;
        //ColorBlock cb = numberofPeople[idx - 2].colors;
        //cb.normalColor = Color.white;
        //numberofPeople[idx - 2].colors = cb;
        foreach (Button button in numberofPeople)
        {
            ColorBlock unselectedcb = button.colors;
            unselectedcb.normalColor = Color.gray;
            button.colors = unselectedcb;
        }
        ColorBlock cb = numberofPeople[idx - 2].colors;
        cb.normalColor = Color.white;
        numberofPeople[idx - 2].colors = cb;

        //Debug.Log(maxPlayer + "bb");
        //int maxPlayer
    }
    public void RoomPublicOXButton(int idx)
    {
        foreach (Button button in RoomPublicOX)
        {
            ColorBlock unselectedcb = button.colors;
            unselectedcb.normalColor = Color.gray;
            button.colors = unselectedcb;
        }
        ColorBlock cb = RoomPublicOX[idx ].colors;
        cb.normalColor = Color.white;
        RoomPublicOX[idx].colors = cb;

        if(idx == 0)
        {
            isRoompublic = true;
        }
        else
        {
            isRoompublic = false;
        }

        
    }

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    
    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom);
        Hashtable cp = PhotonNetwork.CurrentRoom.CustomProperties;
        Debug.Log(cp["gameType"]);
        //Debug.Log(PhotonNetwork.CurrentRoom);
        //Debug.Log(cp["roomname"]);
        //Debug.Log(cp["roomname"].Equals("Casnio Lobby"));
        //RoomPanel.SetActive(true);
        //RoomRenewal();
        //ChatInput.text = "";
        if (cp["roomname"].Equals("Casino Lobby") == true)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("Casino");
                Debug.Log(PhotonNetwork.CurrentRoom);
                return;
            }
            else
            {
                PhotonNetwork.LoadLevel("Casino");
            }
        }

        if (cp["gameType"].ToString() == "Logic")
        {
            if (cp["password"].ToString() != string.Empty)
            {
                //비밀번호가 있고 게스트
                if (!PhotonNetwork.IsMasterClient)
                {
                    passwordPanel.SetActive(true);
                }

                //비밀번호가 있고 방주인
                else
                {
                    PhotonNetwork.LoadLevel("LogicNet");
                }

            }
            else
            {
                //비밀번호가 없는 방
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.LoadLevel("LogicNet");
                }

                else
                {
                    PhotonNetwork.LoadLevel("LogicNet");
                }
            }
        }
       
    }

    public void passwordSubmitButton()
    {
        Hashtable cp = PhotonNetwork.CurrentRoom.CustomProperties;
        if (passwordInput.text == cp["password"].ToString() && cp["gameType"].ToString() == "Logic")
        {
            PhotonNetwork.LoadLevel("LogicNet");
        }
        else
        {
            Debug.Log("패스워드 오류");
            passwordPanel.SetActive(false);
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); }

    public override void OnJoinRandomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); }


    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        RoomRenewal();
        ChatRPC("<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        RoomRenewal();
        ChatRPC("<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
    }


    void RoomRenewal()
    {
        ListText.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            ListText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", ");
        RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + " / " + PhotonNetwork.CurrentRoom.PlayerCount + "명 / " + PhotonNetwork.CurrentRoom.MaxPlayers + "최대";
    }
    #endregion

    
    #region 채팅
    public void Send()
    {
        PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + ChatInput.text);
        ChatInput.text = "";
    }

    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < ChatText.Length; i++)
            if (ChatText[i].text == "")
            {
                isInput = true;
                ChatText[i].text = msg;
                break;
            }
        if (!isInput) // 꽉차면 한칸씩 위로 올림
        {
            for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
            ChatText[ChatText.Length - 1].text = msg;
        }
    }
    #endregion
}
