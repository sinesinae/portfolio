using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[Serializable]
public class saveinventory//인벤토리 정보를 저장할 클래스
{
    public List<Item> inventoryList;//인벤토리정보를 저장할 아이템리스트
    public int num;//슬롯을 몇번 증가시켰는지 저장할 변수
}   
public class Inventory : MonoBehaviour
{
    public static Inventory instance;//스태틱선언

    [HideInInspector]
    public Item targetslot_item;//타겟아이템정보를 불러올 변수
    public CoinManager Coin;//코인정보를 불러올변수
    public Market_buy market;//마켓정보를 불러올변수
    public ItemDatabase DB;//아이템정보를 불러올 변수
    public InventorySlot[] slots;//인벤토리슬롯들

    public List<Item> inventoryItemList;//플레이어가 소지한 전체 아이템리스트
    public List<Item> inventoryTabList;//선택한 탭에 따라 다르게 보여질 아이템 리스트

    public RectTransform Infor;//설명패널(커서를 올렸을때 보여질)
    public RectTransform[] MenuButton_tr;//메뉴버튼의 트랜스폼을 저장할 배열
    public RectTransform content_tr;//뷰포트의 컨텐트 조정변수

    public Transform tf;//slot의 부모객체
    public GameObject go;//인벤토리 활성화 비활성화
    public GameObject addslot;//슬롯추가 패널 
    public GameObject Addslot_;//슬롯추가 버튼 

    public Text addslottext;//슬롯패널에 보여줄 텍스트
    public Text Description_Text;//부연설명
    public string[] tabDescription;//탭 부연설명.

    public int selectedTab = 2;//선택된 탭
    public int num=1;//슬롯증가시 컨텐트사이즈를 조절해줄 변수
    public int slotbuyprice;//슬롯증가금액확인변수

    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        DB = FindObjectOfType<ItemDatabase>();//아이템정보를 불러옴
        inventoryItemList = new List<Item>();//아이템리스트할당
        inventoryTabList = new List<Item>();//아이템리스트할당
        slots = tf.GetComponentsInChildren<InventorySlot>();//tf 자식에 인벤토리슬롯정보를불러옴
        Coin = FindObjectOfType<CoinManager>();//코인정보를 불러옴
        market = FindObjectOfType<Market_buy>();//마켓정보를 불러옴
        loadinven();//저장된 인벤을 불러옴

    }
    public void GetAnItem(int _itemID, out bool ajrdma, int _count = 1)//아이템을 인벤토리슬롯에 추가해주는 함수
    {
        ajrdma = false;
        for (int i = 0; i < DB.itemList.Count; i++)//데이터베이스 아이템검색
        {
            if (_itemID == DB.itemList[i].itemID)//데이터베이스의 아이템발견
            {
                for (int j = 0; j < inventoryItemList.Count; j++)//소지품에 같은아이템이 있는지 확인
                {
                    if (inventoryItemList[j].itemID == _itemID)//소지품에 같은아이템이 있다면 갯수만증감시켜줌
                    {
                        if (inventoryItemList[j].itemType == Item.Itemtype.USE)//해당아이템의 타입이 채집품이면
                        {
                            if (inventoryItemList[j].itemCount < 99)//해당아이템 수량이99보다 작을땐
                            {
                                inventoryItemList[j].itemCount += _count;//아이템수량을 1씩만증가
                                ShowItem();//인벤토리 갱신
                                ajrdma = true;//아이템습득
                                return;
                            }

                        }
                        else//해당아이템의 타입이 채집품이 아니면
                        {
                            int count = 0;//카운트는 영

                            for (int k = 0; k < inventoryItemList.Count; k++)//아이템리스트갯수만큼돈다
                            {
                                if (inventoryItemList[k].itemType == Item.Itemtype.EQUIP ||
                                    inventoryItemList[k].itemType == Item.Itemtype.ETC)//해당아이템타입이 꾸미기거나 도구일때
                                    count++;//카운트증가
                                ShowItem();//인벤토리 갱신

                            }

                            if (count < 16)//카운트가 16보다 작으면
                            {
                                inventoryItemList.Add(CopyItem(DB.itemList[i]));//인벤토리에 추가하고
                                ajrdma = true;//아이템습득
                                return;
                            }
                            else//아니면 코르틴실행
                            {
                                Comment.instance.CommentPrint("인벤이 가득찼어");
                                ajrdma = false;//아이템습득안함
                                return;
                            }

                        }
                    }
                }
                int countidx = 16 + (num * 4);//아이템의 갯수가 16이상일때 슬롯을 4개씩증가시켜줄 변수
                if (CheckTaplistCount(countidx))//아이템 슬롯의 갯수가 countidx이면
                {
                    inventoryItemList.Add(CopyItem(DB.itemList[i]));//소지품에 해당아이템 추가
                    inventoryItemList[inventoryItemList.Count - 1].itemCount += _count - 1;//소지품카운트 갯수조정
                    ShowItem();//인벤토리 갱신

                }
                else//아니면 코르틴실행 아이템습득안함
                {
                    Comment.instance.CommentPrint("인벤이 가득찼어");
                    Coin.coin += targetslot_item.buyPrice;
                    ajrdma = false;
                    return;
                }
                slots[i].itemCount_Text.text = "x" + inventoryItemList[inventoryItemList.Count - 1].itemCount;//해당슬롯의 텍스트에 수량저장
                ShowItem();//인벤토리 갱신
                ajrdma = true;//아이템습등
                return;
            }
        }
    }

    bool CheckTaplistCount(int idx)//채집품인지 검사하고 인덱스보다 크면 false반환하는 함수
    {
        int count = 0;
        for (int i = 0; i < inventoryItemList.Count; i++)
        {
            if (Item.Itemtype.USE == inventoryItemList[i].itemType)//해당 아이템타입이 채집품이면
                count++;//카운트증가
        }
        if (count > idx - 1)
            return false;

        return true;
    }

    Item CopyItem(Item target)//아이템 복사하고 반환
    {
        Item itemdata = new Item();
        itemdata.itemID = target.itemID;
        itemdata.itemName = target.itemName;
        itemdata.itemDescription = target.itemDescription;
        itemdata.itemType = target.itemType;
        itemdata.itemCount = target.itemCount;
        itemdata.itemDurable = target.itemDurable;
        itemdata.sellPrice = target.sellPrice;
        itemdata.buyPrice = target.buyPrice;
        itemdata.itemIcon = target.itemIcon;
        itemdata.hunger = target.hunger;
        return itemdata;
    }
    public void RemoveSlot()//인벤토리 슬롯 초기화함수
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();//해당슬롯삭제
            slots[i].gameObject.SetActive(false);//슬롯 비활성화
        }
    }
    public void SelectedTab(int a)//탭선택시 패널을보여주게할 함수 
    {
        Infor.gameObject.SetActive(true);//설명패널활성화

        Infor.position = new Vector3(MenuButton_tr[a].position.x, MenuButton_tr[a].position.y + 100, 0);//설명패널의 포지션을 해당버튼의 포지션값으로 저장
        Description_Text.text = tabDescription[a];//설명패널의 해당아이템 정보 저장

    }
    public void DeSelectedTab()//탭선택해제시
    {
        Infor.gameObject.SetActive(false);//설명패널 비활성화
        Description_Text.text = "";//설명패널 텍스트 null
    }

    public void settabnumber(int a)//탭지정함수
    {
        selectedTab = a;
    }

    public void ExitItem(GameObject panel)//아이템1개버리기 함수
    {
        if (targetslot_item == null)//아이템이 널이면 함수종료
            return;

        if (targetslot_item.itemCount > 1)//해당아이템의 카운트가1보다크면 카운트1개 빼기
            targetslot_item.itemCount--;
        else//아니면 삭제
            inventoryItemList.Remove(targetslot_item);

        panel.SetActive(false);// 해당 버튼패널 비활성화
        ShowItem();//인벤토리 갱신
    }
    public void ExitItem2(GameObject panel)//아이템 여러개 버리기 함수
    {
        int num = 99;//한슬롯당 아이템 카운트지정
        if (targetslot_item == null)//해당아이템이 없으면 함수종료
            return;

        if (targetslot_item.itemCount > num)//해당 아이템수량이99보다크면
            targetslot_item.itemCount -= num;//99개삭제
        else//아니면 전부삭제
            inventoryItemList.Remove(targetslot_item);

        panel.SetActive(false);//해당 버튼패널 비활성화
        ShowItem();//인벤토리 갱신
    }
    public void SellItem(GameObject panel)//아이템 1개판매 함수
    {
        if (targetslot_item == null)//해당아이템이 없으면 함수종료
            return;

        Coin.addcoin(1);//1개의 가격만큼 코인추가
        targetslot_item.itemCount -= 1;//해당아이템갯수 1개 감소
        if (targetslot_item.itemCount == 0)//아이템 카운트가 0이면
        {
            inventoryItemList.Remove(targetslot_item);//해당아이템삭제
        }

        panel.SetActive(false);//해당 버튼패널 비활성화
        ShowItem();//인벤토리 갱신
    }
    public void SellItem2(GameObject panel)//여러개판매(한슬롯판매)
    {
        if (targetslot_item == null)//아이템이 널이면 함수종료
            return;

        Coin.addcoin(targetslot_item.itemCount);//해당슬롯의 아이템카운트*아이템가격 만큼 코인증가
        targetslot_item.itemCount -= targetslot_item.itemCount;//해당아이템 카운트 0
        inventoryItemList.Remove(targetslot_item);//해당아이템 삭제

        panel.SetActive(false);//해당 버튼패널 비활성화
        ShowItem();///인벤토리 갱신
    }

    public void ShowItem()//인벤토리 갱신 및 탭에따른 탭분류을 인벤토리 탭리스트에 추가, 인벤토리 탭리스트의 내용을 인벤토리 슬롯에 추가해주는 함수

    {
        inventoryTabList.Clear();//해당 슬롯에있는 아이템 클리어
        RemoveSlot();//슬롯삭제
        switch (selectedTab)//탭넘버에따라
        {

            case 0://꾸미기창이면
                for (int i = 0; i < inventoryItemList.Count; i++)//해당탭에 아이템타입이 장비인 아이템을 추가
                {

                    if (Item.Itemtype.EQUIP == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);

                }
                e(16);//꾸미기탭의  슬롯 활성화 개수는16
                content_tr.sizeDelta = new Vector2(0, 360);//16개인슬롯에 대한 컨텐트 사이즈지정
                break;
            case 1://도구창이면

                for (int i = 0; i < inventoryItemList.Count; i++)//해당탬에 아이템타입이 도구인 아이템 추가
                {
                    if (Item.Itemtype.ETC == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                e(16);//도구탭의 슬롯 활성화 개수는 16
                content_tr.sizeDelta = new Vector2(0, 360);//16개인슬롯에 대한 컨텐트 사이즈지정

                break;
            case 2://채집창이면
                for (int i = 0; i < inventoryItemList.Count; i++)//해당탭에 아이템타입이 채집인 아이템 추가
                {
                    if (Item.Itemtype.USE == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                e(16 + (num * 4));//채집탭의 슬롯 활성화 개수는 기본 16+(num*4)만큼 증가
                content_tr.sizeDelta = new Vector2(0, 360 + (90 * num));// 16+(num*4)에 따른 갯수만큼 컨텐트사이즈 조정

                break;

        }
        saveinven();//인벤토리 저장
    }
    public void e(int a)//해당탭의 슬롯개수 활성화및 조정
    {
        for (int i = 0; i < inventoryTabList.Count; i++)//해당탭의 카운트의 슬롯만큼 해당아이템추가
        {
            slots[i].Additem(inventoryTabList[i]);
        }
        for (int j = 0; j < a; j++)//해당슬롯 활성화
        {
            slots[j].gameObject.SetActive(true);
        }
    }
    public void panel()
    {
        selectedTab = 2;
        ShowItem();
        Addslot_.SetActive(true);
        int idx = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].gameObject.activeSelf)
                idx++;
        }
        num = (idx / 4) - 4;
        
        Debug.Log(num);
        switch (num)
        {
            case 0:
                slotbuyprice = 100;
                break;
            case 1:
                slotbuyprice = 500;
                break;
            case 2:
                slotbuyprice = 700;
                break;
            case 3:
                slotbuyprice = 1500;
                break;
            case 4:
                slotbuyprice = 3500;
                break;
            case 5:
                slotbuyprice = 5500;
                break;
            case 6:
                slotbuyprice = 20000;
                break;
            case 7:
                slotbuyprice = 50000;
                break;
            default:
                addslot.SetActive(false);
                Addslot_.SetActive(false);
                break;
        }
                addslottext.text = slotbuyprice + "원";
    }
    public void scancoin(int a)//슬롯을 추가하고 가격을 빼주는함수
    {
        if (Coin.coin >= a)
        {
            Coin.coin -= a;
            num++;
            if (num > 7)//넘이 7보다 크면 계속 7번추가가 max
            {
                num = 7;
                addslot.SetActive(false);
            }
            ShowItem();//인벤토리 갱신
        }
        else
            Comment.instance.CommentPrint("돈없잖아!!");
        Addslot_.SetActive(false);
    }
    public void buyslot()
    {
        scancoin(slotbuyprice);
    }

    public void saveinven()//인벤토리정보 저장함수
    {
        saveinventory save = new saveinventory();//인벤토리저장할 변수에 해당클래스 할당
        save.inventoryList = inventoryItemList;//인벤토리 저장
        save.num = num;
        string json = JsonUtility.ToJson(save);//제이슨에 인벤토리 정보 저장

        File.WriteAllText(Application.persistentDataPath + "/invendata.json", json);//인벤데이타텍스트 생성

    }
    public void loadinven()//인벤토리 정보를 불러올함수
    {
        try
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/invendata.json");//인벤데이터텍스트 읽어옴
            saveinventory load = new saveinventory();//로드에 해당클래스 할당
            load = JsonUtility.FromJson<saveinventory>(json);//로드에 인벤데이타 저장
            for (int i = 0; i < load.inventoryList.Count; i++)//인벤토리 아이콘과 스트라이트 불러오기
            {
                load.inventoryList[i].itemIcon = Resources.Load("itemIcon/" + load.inventoryList[i].itemID.ToString(), typeof(Sprite)) as Sprite;
            }
            inventoryItemList = load.inventoryList;//인벤토리에 인벤토리 정보 저장
            num = load.num;//넘정보를 넘에저장

        }
        catch { }
    }
}
