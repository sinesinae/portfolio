using UnityEngine;

/// <summary>
/// 플레이어 컨트롤러 클래스
/// </summary>
public class Ctrl : MonoBehaviour
{
    MapManager MM;

    ItemDatabase IDB;

    void Start()
    {
        //transform.position = new Vector3(85, 2, 15);    // 플레이어 시작위치 할당
        //transform.Rotate(270, 0, 0);

        MM = FindObjectOfType<MapManager>();
        IDB = FindObjectOfType<ItemDatabase>();

        PlayerFunctions.instance.SetDefaultInput();
    }

    void Update()
    {
        // 미니맵 보이기 안보이기
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayerFunctions.instance.ShowAndHideMinimap();
        }

        // 상호작용 키
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayerFunctions.instance.PlayerInterAction();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerFunctions.instance.InstanceOnFloor(MM.flowerPrefabs[0], transform.position, IDB.itemList[24], "22");
        }

        // 모든 도구 얻기 버튼
        if (Input.GetKeyDown(KeyCode.Q))
        {
            bool ajrdma;
            Inventory.instance.GetAnItem(90000011, out  ajrdma, 1);
            Inventory.instance.GetAnItem(90000021, out  ajrdma, 1);
            Inventory.instance.GetAnItem(90000031, out  ajrdma, 1);
            Inventory.instance.GetAnItem(90000041, out  ajrdma, 1);
            Inventory.instance.GetAnItem(90000051, out  ajrdma, 1);
            Inventory.instance.GetAnItem(90000061, out  ajrdma, 1);

            if (Inventory.instance.gameObject.activeSelf)
                Inventory.instance.ShowItem();
        }
    }
}