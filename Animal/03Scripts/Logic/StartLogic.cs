using Logic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using JetBrains.Annotations;

public class StartLogic : MonoBehaviourPun,IPunObservable
{
    public LogicStarter logicstart;
    public Button GameStartBtn;
    public GameObject RoomPanel;
    
    // Start is called before the first frame update
    void Start()
    {

        if (!PhotonNetwork.IsMasterClient)
        {
            GameStartBtn.interactable = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    [PunRPC]
    public void startBtn()
    {
        photonView.RPC("StarGame", RpcTarget.All);
    }
    [PunRPC]
    public void StarGame()
    {
        RoomPanel.SetActive(false);
        logicstart.StartGame();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(RoomPanel.active);
        }
        else
        {
            RoomPanel.active = (bool)stream.ReceiveNext();
        }
    }
}

