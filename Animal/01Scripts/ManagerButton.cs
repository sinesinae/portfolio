using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.Video;
using UnityEngineInternal;

public class ManagerButton : MonoBehaviour
{
    public GameObject seleted_panel;//
    public GameObject EquipPos;
    
    


    public void Start()
    {
        EquipPos = GameObject.Find("EquipPos");
    }
    public void OnButton(GameObject button)//버튼활성화 함수
    {   
        button.SetActive(!button.activeSelf);//버튼동작
        Inventory.instance.DeSelectedTab();//설명패널 비활성화
    }

    public void UseItem(GameObject PANEL)//아이템사용
    {
        if (Inventory.instance.targetslot_item == null)//아이템이 널이면 함수종료
            return;

        Item thisItem = Inventory.instance.targetslot_item;//아이템정보 가져옴

        switch (thisItem.itemType)//아이템타입이
        {
            case Item.Itemtype.ETC://도구이면
                if (EquipPos.transform.childCount > 0)//손에 다른 도구를 들고있으면
                    Destroy(EquipPos.transform.GetChild(0).gameObject);//도구제거

                string itemidx = thisItem.itemID.ToString();//아이템아이디를 스트링으로저장
                GameObject prefab = Resources.Load("Prefabs/equipprefabs/" + itemidx, typeof(GameObject)) as GameObject;//리소스폴더에서 프리팹에 저장된내용을 읽어옴
                GameObject item = Instantiate(prefab);//아이템생성
                Player.instance.playerState.Remove(STATE.BAREHAND);//플레이어상태에서 맨손상태를 제거
                Player.instance.playerState.Add(STATE.EQUIPED);//플레이어 상태에 장착상태추가
                Player.instance.EquipItem = item.GetComponent<ItemPrefabBase>();//장착중인 아이템정보를 변수에저장
                seleted_panel.SetActive(false);//패널비활성화
                break;

            case Item.Itemtype.USE://채집품이면
                FindObjectOfType<Inventory>().ExitItem(PANEL);//아이템 버리기함수호출
                FindObjectOfType<FoodManager>().addFOOD(thisItem.hunger);//푸드에 포만감추가
                FindObjectOfType<HealthManager>().AddHealth(thisItem.hunger);//체력바에 포만감추가

                break;

        }
    }
}
