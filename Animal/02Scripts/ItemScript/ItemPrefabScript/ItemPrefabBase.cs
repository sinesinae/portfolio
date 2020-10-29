using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 아이템 프리팹이 지녀야 하는 속성 클래스
/// </summary>
public class ItemPrefabBase : MonoBehaviour
{
    public ItemDatabase IDB;    // 아이템데이터베이스를 가져옴
    public GameObject EquipHand;    // 아이템 프리팹의 하이어라키상 부모 위치
    public Item itemdata = new Item();  // 해당 아이템 정보
    protected RaycastHit hit;

    protected virtual void Start()
    {
        // 필요한 컴포넌트를 연결하고
        // 프리팹의 위치를 장착위치로 이동시킴
        IDB = FindObjectOfType<ItemDatabase>();
        EquipHand = GameObject.Find("EquipPos").gameObject;
        gameObject.transform.SetParent(EquipHand.transform);
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(1, 1, 1);
        transform.rotation = EquipHand.transform.rotation;
    }

    // 아이템 정보를 가져옴
    protected virtual void CopyItemData()
    {
        // 장착하기전 인벤토리에서 선택된 아이템의 정보를 해당아이템에 가져옴
        itemdata = Inventory.instance.targetslot_item;
    }

    public virtual void Use_Equip() 
    {
        PlayerFunctions.instance.BareHandInterAct();
        
    }

    public virtual void ItemDuration(int num)
    {
        itemdata.itemDurable += num;
        if (itemdata.itemDurable <= 0)
        {
            Inventory.instance.inventoryItemList.Remove(Player.instance.EquipItem.itemdata);
            Destroy(Player.instance.EquipItem.gameObject);
            Player.instance.EquipItem = null;
            Player.instance.playerState.Remove(STATE.EQUIPED);
            Player.instance.playerState.Add(STATE.BAREHAND);
            Comment.instance.CommentPrint("내구도가 다해 장비가 파괴되었어!");
        }
    }
}