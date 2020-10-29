using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public class savecoin//코인정보를 저장할 클래스
    {
        public int COIN;//코인정보를 저장할 변수
    }
    public ItemDatabase DB;//아이템정보를 불러올변수

    private int _coin;//코인변수
    
    public Text coin_Text;//인벤토리에서 코인을 보여주는 텍스트
    //public Quest1 quest;

    [HideInInspector] public int coin//코인 프라퍼티
    {
        get { return _coin; }
        set { _coin = value;
            coin_Text.text = _coin.ToString();//코인의 금액을 텍스트로 저장
            SaveCoin();//코인정보저장
        }
    }
    void Start()
    {
        DB = FindObjectOfType<ItemDatabase>();//db변수에 아이템정보 불러옴
        loadCoin();//저장된 코인정보를 읽어온다
    }
    public void addcoin(int a)//아이템정보를 불러오고 아이템가격*매개변수로 받은 수량을 코인에 더해주는함수(판매관련)
    {
        Item thisItem = Inventory.instance.targetslot_item;
        coin += thisItem.sellPrice*a;
    }
    public IEnumerator minuscoin( )//아이템정보를 불러오고 아이템가격만큼 코인에 빼주는 함수(구매관련)
    {
        Item item = Inventory.instance.targetslot_item;
        if (coin < item.buyPrice)//소지한 코인이 아이템구매가격보다 작거나 같으면
        {
           BoolList bl = Comment.instance.CommentPrint("돈 없는데?");//대화출력코르틴실행 bl(comment코루틴,bool정보=false)
            yield return new WaitUntil(() => bl.isDone);//bool이 트루가 될때까지 기다림
        }
        else//아니면 코인에서 구매금엑을 빼고 인벤토리에 아이템을 추가
        {
            coin -= item.buyPrice;
            Inventory.instance.GetAnItem(Inventory.instance.targetslot_item.itemID, out bool ajrdma);
        }
    }
    public void ATMminuscoin(int a)//매개변수로 받은 금액만큼 코인증가
    {
        coin += 1 * a;
    }
    public void ATMaddcoin(int a)//매개변수로 받은 금액만큼 코인감소
    {
        coin -= 1 * a;
    }
    public void SaveCoin()//코인을 저장하는 함수
    {
        savecoin save = new savecoin();//세이브 코인 클래스 변수 세이브생성하고 할당
        save.COIN = coin;//저장할 코인변수에 코인정보저장
        string json = JsonUtility.ToJson(save);//텍스트로 코인정보저장

        File.WriteAllText(Application.persistentDataPath + "/coindata.json", json);//코인데이터텍스트파일 생성

    }
    public void loadCoin()//코인정보를 불러올 함수
    {
        try
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/coindata.json");//코인데이터 스트링으로 저장
            savecoin load = new savecoin();//생성한 로드변수에 클래스할당
            load = JsonUtility.FromJson<savecoin>(json);//로드에 저장된 코인정보저장

            coin = load.COIN;//코인에 저장된코인정보 저장
        }
        catch { }
    }
}

