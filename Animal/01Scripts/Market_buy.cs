using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Market_buy : MonoBehaviour
{
    public ItemDatabase DB;//아이템정보 변수
    public CoinManager CM;


    public int BUY_selectedTab;//선택탭의 번호를 지정할 변수
    public string[] BUY_tabDescription;//탭 부연설명.
    public int BUY_content_num;//컨텐트 사이즈 관여변수
    public Text BUY_Description_Text;//부연설명

    public RectTransform BUY_content_tr;//뷰포트 컨텐트사이즈 조정변수

    public GameObject go;//마켓활성화 변수
    public GameObject selected_panel;//설명창 


    public InventorySlot[] BUY_slots;//슬롯정보를 반영할 변수

    public List<Item> BUY_ItemList;//아이템 변수
    public List<Item> BUY_tabList;//탭에따른 아이템분류변수



    void Start()
    {
        CM = FindObjectOfType<CoinManager>();
        DB = FindObjectOfType<ItemDatabase>();
        BUY_ItemList = new List<Item>();
        BUY_tabList = new List<Item>();

    }
    public void BuyItem()//마켓아이템창에 아이템추가
    {
        for (int i = 0; i < DB.itemList.Count; i++)//데이터베이스 아이템검색
        {
            if (!BUY_ItemList.Contains(DB.itemList[i]))
            {
                BUY_ItemList.Add(DB.itemList[i]);//소지품에 해당아이템 추가
                slotup();//해당아이템만큼 컨텐트 사이즈 조정
                Inventory.instance.ShowItem();//아이템을 보여준다.
            }
        }
    }

    public void buy_RemoveSlot()//슬롯 비활성화 함수
    {
        for (int i = 0; i < BUY_slots.Length; i++)
        {
            BUY_slots[i].RemoveItem();//아이템안보이게하고
            BUY_slots[i].gameObject.SetActive(false);//슬롯비활성화
        }
    }
    public void Buy_ShowItem()//탭에따른 탭분류 그것을 인벤토리 탭리스트에 추가 인벤토리 탭리스트의 내용을 인벤토리 슬롯에 추가하는 함수
    {
        BUY_tabList.Clear();
        buy_RemoveSlot();


        switch (BUY_selectedTab)
        {
            case 0:
                for (int i = 0; i < BUY_ItemList.Count; i++)
                {
                    if (Item.Itemtype.EQUIP == BUY_ItemList[i].itemType)
                        BUY_tabList.Add(BUY_ItemList[i]);
                }
                buy(BUY_tabList.Count <= 16 ? 16: BUY_tabList.Count);
                slotup();
                BUY_content_tr.sizeDelta = new Vector2(0, 90 + (90 * BUY_content_num));
                break;
            case 1:
                for (int i = 0; i < BUY_ItemList.Count; i++)
                {
                    if (Item.Itemtype.ETC == BUY_ItemList[i].itemType)
                        BUY_tabList.Add(BUY_ItemList[i]);
                }
                buy(BUY_tabList.Count <= 16 ? 16 : BUY_tabList.Count);
                slotup();
                BUY_content_tr.sizeDelta = new Vector2(0, 90 + (90 * BUY_content_num));
                break;
            case 2:
                for (int i = 0; i < BUY_ItemList.Count; i++)
                {
                    if (Item.Itemtype.USE == BUY_ItemList[i].itemType)
                        BUY_tabList.Add(BUY_ItemList[i]);
                }
                buy(BUY_tabList.Count <= 16 ? 16 : BUY_tabList.Count);
                slotup();
                BUY_content_tr.sizeDelta = new Vector2(0, 90 + (90 * BUY_content_num));
                break;
        }

    }
    public void slotup()//탭분류하고 그만큼 스크롤뷰 커넥트 변경
    {
        if (BUY_tabList.Count % 4 == 0)
        {
            BUY_content_num = BUY_tabList.Count / 4;
        }
        else
        {
            BUY_content_num = BUY_tabList.Count / 4 + 1;
        }
    }

    public void buy(int a)//구매창에 아이템 추가및 슬롯활성화 함수
    {
        for (int i = 0; i < BUY_tabList.Count; i++)//마켓에 아이템 추가
        {
            BUY_slots[i].Additem(BUY_tabList[i]);
        }
        for (int j = 0; j < a; j++)
        {
            BUY_slots[j].gameObject.SetActive(true);//슬롯활성화
        }

    }

    public void settabnumber(int a)//해당탭을 숫자로 지정해주는함수
    {
        BUY_selectedTab = a;
    }

    public void BUY_button()
    {
        StartCoroutine(CM.minuscoin());
        selected_panel.SetActive(false);//구매패널닫기
    }
 
    public void Buy_slot_button()//클릭한 슬롯아이템정보저장
    {
        if (EventSystem.current.currentSelectedGameObject.GetComponent<InventorySlot>() != null)//슬롯이 널이아니면
            Inventory.instance.targetslot_item = EventSystem.current.currentSelectedGameObject.GetComponent<InventorySlot>().temp;//아이템에 슬롯정보를 가져옴
    }
    public void exitbutton()//마켓&인벤토리 닫기
    {
        Inventory.instance.DeSelectedTab();//infor설명창닫기
        go.SetActive(false);//마켓끄기
        Inventory.instance.go.SetActive(false);//인벤토리끄기
    }
}
