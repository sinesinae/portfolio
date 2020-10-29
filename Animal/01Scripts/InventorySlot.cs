using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    [HideInInspector] 
    //public Item BUY_targetslot_item;//타겟아이템슬롯
    public Item temp;//타겟아이템
    
   // public List<Item> inventoryTabList;
   // public List<Item> inventoryItemList;
   // public List<Item> BUY_inventoryTabList;
    
    public RectTransform Infor;//아이템 정보창
    
    public Text[] nameselected_text;//각패널에 보여질 아이템이름
    public Text[] etcselected_text;//각패널에 보여질 아이템설명
    
    public Text Description_Text;//설명을 보여줄 텍스트
    public Text itemCount_Text;//슬롯아이템의 카운트를 보여줄 텍스트
    public Image icon;//슬롯버튼이미지
    public Sprite toomyung;//슬롯버튼 이미지를 투명하게할png

    public GameObject market;//마켓엑티브
    public GameObject inven;//인벤액티브
    public GameObject[] selected_panel;//각 상황에맞는 패널

    public string itemName_Text = null;//아이템이름을 저장할 변수

    public void Start()
    {
        Infor = Inventory.instance.Infor;//인포에 인벤토리클래스의 인포할당
        Description_Text = Inventory.instance.Description_Text;//디스크립션텍스트에 인벤토리 디스크립션텍스트 할당
    }
    public void itemButton(GameObject button)//아이템 선택시 목적에 맞는 패널을 정해주는 함수
    {

        if (EventSystem.current.currentSelectedGameObject.GetComponent<InventorySlot>() != null)//해당 인벤토리슬롯이 널이아니면
            Inventory.instance.targetslot_item = EventSystem.current.currentSelectedGameObject.GetComponent<InventorySlot>().temp;//해당아이템슬롯에 아이템 정보할당
        if (temp != null)//해당아이템이 널이면
        {
            if (market.activeSelf && inven.activeSelf)//마켓이열렸을때
            {
                button = selected_panel[2];//마켓아이템 선택시 판매패널이 뜸
            }
            else //인벤토리만 열렸을때
            {
                if (Inventory.instance.selectedTab == 0 || Inventory.instance.selectedTab == 1)//꾸미기,도구탭의 아이템선택시
                {
                    button = selected_panel[0];//사용하기 패널뜸
                }
                else if (Inventory.instance.selectedTab == 2)//채집품이면
                {
                    if (temp.itemID== 40000001|| temp.itemID == 40000002||temp.itemID == 20000011|| temp.itemID == 20000012)//포만감이 있는 아이템의경우
                    {
                        button = selected_panel[3];//먹기 패널이 뜸
                    }
                    else//아니면
                    {
                        button = selected_panel[1];//버리기패널이 뜸
                    }
                }
            }
            button.SetActive(true);//패널 활성화
        }
        else//아닐땐
        {
            button.SetActive(false);//패널비활성화
        }
    }
    public void close_button(GameObject button)
    {
        if (temp != null)//아이템이 있으면
            button.SetActive(!button.activeSelf);//패널의 액티브 반전
        else
            button.SetActive(false);//없으면 무조건 비활성화
    }

    public void Additem(Item _item)//해당아이템슬롯의 수량텍스트 반영및 활성화 비활성화
    {
        temp = _item;//아이템 정보 저장
        itemName_Text = _item.itemName;//아이템이름저장
        icon.sprite = _item.itemIcon;//아이템 이미지저장
        if (Item.Itemtype.USE == _item.itemType)//아이템타입이 같으면
        {
            if (_item.itemCount > 0)//아이템의 수량이 0보다 클때
                itemCount_Text.text = "x" + _item.itemCount.ToString();//수량표시
            else
                itemCount_Text.text = "";//아니면 널
        }
    }
    public void Market_Selecteditem()
    {
        Selected_Text();//선택한 패널에맞는 텍스트를 보여줌
      
        if (market.activeSelf && inven.activeSelf)//마켓이랑 인벤토리가 같이열리면
        {
            if (temp != null)//아이템이 없지않으면
            {
                
                Infor.gameObject.SetActive(true);//설명창 활성화
                Infor.position = new Vector3(transform.position.x, transform.position.y + 100, transform.position.z + 1);//설명창을 해당아이템기준으로 위로100 앞으로 1에 표시
                if (Infor.position.x >=Camera.main.pixelWidth/2)//마켓의 오른쪽(인벤)
                {
                    Description_Text.text =  temp.itemName + "\n" + "판매가 : " + temp.sellPrice;//판매가 
                }
                else
                {
                    Description_Text.text =   temp.itemName + "\n" + "구매가 : " + temp.buyPrice;//왼쪽상점이면 구매가

                }
            }
            else//아이템이 없으면

            {
                Infor.gameObject.SetActive(false);//설명창 비활성화
                Infor.position = new Vector3(transform.position.x, transform.position.y + 100, transform.position.z + 1);
                Description_Text.text = "";//텍스트 널
            }
        }
        else//인벤토리와 상점중 하나만 열리고
        {
            if (temp != null)//아이템이 널이아니면
            {
                Infor.gameObject.SetActive(true);//설명창활성화
                Infor.position = new Vector3(transform.position.x, transform.position.y + 100, transform.position.z + 1);
                Description_Text.text = temp.itemName;//설명내용은 아이템이름
            }
            else//아이템이 널이면

            {
                Infor.gameObject.SetActive(false);//설명창 비활성화
                Infor.position = new Vector3(transform.position.x, transform.position.y + 100, transform.position.z + 1);
                Description_Text.text = "";//설명내용 없음
            }

        }

    }
 
    public void DeSelectedTab()//탭선택해제시
    {
        Infor.gameObject.SetActive(false);//설명창 비활성화
        Description_Text.text = "";//설명내용 없음
    }
    public void Selected_Text()//각목적에 맞는 패널에 따른 설명과 이름을 보여주는 함수
    {
        if (temp == null)//아이템이 없으면 종료
            return;
        for (int a = 0; a < 5; a++)//패널갯수가 4개임
        {
            nameselected_text[a].text = temp.itemName;//해당 아이템이름을 저장
            etcselected_text[a].text = temp.itemDescription;//설명을 저장
        }
    }
    public void RemoveItem()//인벤에서 아이템및 정보를 없애는 함수
    {
        itemCount_Text.text = "";//아이템 카운트는 널
        icon.sprite = toomyung;//아이콘 스프라이트 는 안보이게
        temp = null;//아이템 없음
    }
}
