using UnityEngine;

/// <summary>
/// 개별 아이템 정보 클래스
/// </summary>
[System.Serializable]
public class Item
{
    public int itemID;  // 고유번호
    public string itemName; // 이름
    public string itemDescription;  // 설명
    public int itemCount;   // 수량
    public int itemDurable; // 내구도
    public int sellPrice;   // 판매가
    public int buyPrice;    // 구매가
    public int hunger;  // 배고픔회복

    public Sprite itemIcon; // 아이템이미지
    public Itemtype itemType;
    
    public enum Itemtype
    {
        USE,
        EQUIP,
        QUEST,
        ETC,
    }

    public Item() { }

    public Item(int _itemID, string _itemName, string itemDes, Itemtype _itemtype, int _itemcount = 1)
    {
        itemID = _itemID;
        itemName = _itemName;
        itemDescription = itemDes;
        itemType = _itemtype;
        itemCount = _itemcount;
        itemIcon = Resources.Load("itemIcon/" + _itemID.ToString(), typeof(Sprite)) as Sprite;
    }

    public Item(int _itemID, string _itemName, string itemDes, Itemtype _itemtype, int _itemDu, int _sellPrice, int _buyPrice,int _itemcount= 1, int _hunger = 0)
    {
        itemID = _itemID;
        itemName = _itemName;
        itemDescription = itemDes;
        itemType = _itemtype;
        itemCount = _itemcount;
        itemDurable = _itemDu;
        sellPrice = _sellPrice;
        buyPrice = _buyPrice;
        hunger = _hunger;
        itemIcon = Resources.Load("itemIcon/" + _itemID.ToString(), typeof(Sprite)) as Sprite;
    }

    
}
