using System.Collections;
using UnityEngine;

public class PlayerFunctions : MonoBehaviour
{
    public static PlayerFunctions instance;

    CreateMapData CMD;
    MapManager MM;
    Canvas MinimapCanvas;  // 미니맵 캔버스
    int targetDisplayIdx;   // 미니맵을 감출때 사용할 카메라 인덱스
    [HideInInspector] public Transform bodyTr;    // 플레이어 캐릭터 트랜스폼
    ItemDatabase IDB;
    public bool interact = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CMD = FindObjectOfType<CreateMapData>();
        MM = FindObjectOfType<MapManager>();
        MinimapCanvas = GameObject.Find("MinimapCanvas").transform.GetComponent<Canvas>();
        bodyTr = GameObject.Find("Player").transform;
        IDB = FindObjectOfType<ItemDatabase>();
    }

    public void SetDefaultInput()
    {
        JoyStickManager.Instance.pressA = PlayerInterAction;
        JoyStickManager.Instance.pressX = ShowAndHideMinimap;
        JoyStickManager.Instance.pressB = B_Button;
    }
    void B_Button()
    {
        Inventory.instance.go.SetActive(true);
        Inventory.instance.ShowItem();
    }
    // 바닥에 프리팹을 생성하고 맵 데이터에 저장하는 함수
    public void InstanceOnFloor(GameObject prefab, Vector3 position, Item item, string mapcode)
    {
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.y);
        int z = Mathf.RoundToInt(position.z);
        int idx = x + (y * CMD.size * CMD.size) + (z * CMD.size);

        if (CMD.MapDataStr[idx - (y * CMD.size * CMD.size)].Contains("01")
            && CMD.MapDataStr[idx] == "")
        {
            MM.CreatePrefab(prefab, position, item.itemID, item.itemCount);
            CMD.MapDataStr[idx] = mapcode;
            CMD.SaveDataJson();
        }
    }

    // 미니맵을 보이거나 안보이게 하는 함수
    public void ShowAndHideMinimap()
    {
        targetDisplayIdx++;
        targetDisplayIdx = targetDisplayIdx % 2;

        MinimapCanvas.targetDisplay = targetDisplayIdx;
    }

    // 플레이어와 사물간의 상호작용 함수
    public void PlayerInterAction()
    {
        if (Player.instance.playerState.Contains(STATE.EQUIPED))
        {
            if (Player.instance.EquipItem != null)
                Player.instance.EquipItem.Use_Equip();
        }
        else
        {
            // 빈손일때 ItemPickup을 가진 물체면 줍는다
            BareHandInterAct();
        }
    }

    public void BareHandInterAct()
    {
        if (!interact)
            return;

        interact = false;

        Ray ray = new Ray(bodyTr.position + new Vector3(0, 0.5f, 0) - (bodyTr.forward / 2), bodyTr.forward);
        int layermask = (1 << LayerMask.NameToLayer("PLAYERINTERACTOBJECT"));
        if (Physics.Raycast(ray, out RaycastHit hit, 1.5f, layermask))
        {
            ItemPickup targetitem = hit.transform.GetComponent<ItemPickup>();
            IInterAct interactObject = hit.transform.GetComponent<IInterAct>();
            if (targetitem != null)
            {
                bool ajrdma;

                Inventory.instance.GetAnItem(targetitem.itemID, out ajrdma, targetitem._count);
                if (Inventory.instance.gameObject.activeSelf)
                    Inventory.instance.ShowItem();
                if (ajrdma)
                {
                    int idx = targetitem.x + (targetitem.y * CMD.size * CMD.size) + (targetitem.z * CMD.size);
                    if (CMD.MapDataStr[idx] == "21" || CMD.MapDataStr[idx] == "22" || CMD.MapDataStr[idx] == "23" || CMD.MapDataStr[idx] == "24")
                        CMD.grasscount--;
                    CMD.MapDataStr[idx] = "";
                    Destroy(targetitem.gameObject);
                    CMD.SaveDataJson();
                }
                BoolList bl = Comment.instance.CommentPrint("아이템을 습득했다", interact);
                StartCoroutine(Wait(bl));
            }
            else if (interactObject != null)
            {
                interactObject.InterActPlayer();
                interact = true;

            }
            else
            {
                interact = true;
            }
        }
        else
        {
            interact = true;
        }
    }

    IEnumerator Wait(BoolList bl)
    {
        if (bl.isDone)
        {
            yield return new WaitWhile(() => bl.isDone);
            interact = true;
        }
        else
        {
            yield return new WaitUntil(() => bl.isDone);
            interact = true;
        }
    }
}