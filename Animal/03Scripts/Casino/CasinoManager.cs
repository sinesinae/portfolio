using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CasinoManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static CasinoManager instance
    {
        get
        {
            if (cm_instance == null)
            {
                cm_instance = FindObjectOfType<CasinoManager>();
            }
            return cm_instance;
        }
    }

    private static CasinoManager cm_instance;
    private int money = 0;
    private int diamond = 0;

    public GameObject playerPrefab;

    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Vector3 randomSpawnPos = Random.insideUnitSphere * 2f;
        randomSpawnPos.y = 0f;
        //PhotonNetwork.Instantiate("Prefabs/"+playerPrefab.name, new Vector3(0,0,0) , Quaternion.identity).transform.parent = player.transform;
        PhotonNetwork.Instantiate("Prefabs/"+playerPrefab.name, randomSpawnPos, Quaternion.identity);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MainMap");
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}
