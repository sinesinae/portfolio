using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 아이템 데이터를 저장하는 클래스
/// </summary>
public class ItemDatabase : MonoBehaviour
{
    public List<Item> itemList = new List<Item>();
    List<string> itemDes = new List<string>();
    
    private void Awake()
    {
        //낚시대 설명 index 0
        itemDes.Add("주로 물고기를 낚아 올리는데 사용하는 도구");
        //잠자리채 설명 index 1
        itemDes.Add("곤충이나 흩날리거나 공중에 돌아다니는 무언가(특정 계절의 소재 아이템, 깨빈의 영혼 등)를 잡는데 사용하는 도구");
        //도끼 설명 index 2
        itemDes.Add("나무에서 목재를 얻거나 베어서 제거할 때 사용하는 도구. 나무를 벨 때 필요한 타격 수는 기존 작과 동일한 3회");
        //물뿌리개 설명 index 3
        itemDes.Add("꽃에 물을 주기 위한 도구");
        //새총 설명 index 4
        itemDes.Add("일정시간마다 섬을 가로질러 날아가는 선물 상자가 달린 풍선을 터트리기 위한 도구");
        //삽 설명 index 5
        itemDes.Add("땅 속에 묻혀있는 아이템을 발굴하거나, 땅에 구멍을 내거나 매꿀때 사용할 수 있는 도구");
        

        //낚시대 index 0~3
        itemList.Add(new Item(90000011, "엉성한 낚시대", itemDes[0], Item.Itemtype.ETC, 10, 100, 400));
        itemList.Add(new Item(90000012, "낚시대", itemDes[0], Item.Itemtype.ETC, 30, 600, 0));
        itemList.Add(new Item(90000013, "금 낚시대", itemDes[0], Item.Itemtype.ETC, 90, 0, 0));
        itemList.Add(new Item(90000014, "기성품 낚시대", itemDes[0], Item.Itemtype.ETC, 30, 625, 2500));
        //잠자리채 index 4~7
        itemList.Add(new Item(90000021, "엉성한 잠자리채", itemDes[1], Item.Itemtype.ETC, 10, 100, 400));
        itemList.Add(new Item(90000022, "잠자리채", itemDes[1], Item.Itemtype.ETC, 30, 600, 0));
        itemList.Add(new Item(90000023, "금 잠자리채", itemDes[1], Item.Itemtype.ETC, 90, 0, 0));
        itemList.Add(new Item(90000024, "기성품 잠자리채", itemDes[1], Item.Itemtype.ETC, 30, 625, 2500));
        //도끼 index 8~11
        itemList.Add(new Item(90000031, "엉성한 도끼", "나무를 캘수있는 도끼", Item.Itemtype.ETC, 40, 200, 400));
        itemList.Add(new Item(90000032, "도끼", itemDes[2], Item.Itemtype.ETC, 100, 560, 0));
        itemList.Add(new Item(90000033, "금 도끼", itemDes[2], Item.Itemtype.ETC, 200, 0, 0));
        itemList.Add(new Item(90000034, "기성품 도끼", itemDes[2], Item.Itemtype.ETC, 100, 625, 2500));
        //물뿌리개 index 12~15
        itemList.Add(new Item(90000041, "엉성한 물뿌리개", itemDes[3], Item.Itemtype.ETC, 20, 200, 400));
        itemList.Add(new Item(90000042, "물뿌리개", itemDes[3], Item.Itemtype.ETC, 60, 600, 0));
        itemList.Add(new Item(90000043, "금 물뿌리개", itemDes[3], Item.Itemtype.ETC, 180, 10675, 0));
        itemList.Add(new Item(90000044, "기성품 물뿌리개", itemDes[3], Item.Itemtype.ETC, 60, 625, 2500));
        //새총 index 16~18
        itemList.Add(new Item(90000051, "새총", itemDes[4], Item.Itemtype.ETC, 20, 225, 900));
        itemList.Add(new Item(90000052, "금 새총", itemDes[4], Item.Itemtype.ETC, 60, 0, 0));
        itemList.Add(new Item(90000053, "기성품 새총", itemDes[4], Item.Itemtype.ETC, 20, 625, 2500));
        //삽 index 19~22
        itemList.Add(new Item(90000061, "엉성한 삽", itemDes[5], Item.Itemtype.ETC, 40, 200, 800));
        itemList.Add(new Item(90000062, "삽", itemDes[5], Item.Itemtype.ETC, 100, 600, 0));
        itemList.Add(new Item(90000063, "금 삽", itemDes[5], Item.Itemtype.ETC, 200, 0, 0));
        itemList.Add(new Item(90000064, "기성품 삽", itemDes[5], Item.Itemtype.ETC, 100, 625, 2500));
        //잡초 index 23
        itemList.Add(new Item(10000021, "잡초", "쓸모없는 잡초", Item.Itemtype.USE, 1, 10, 10));
        //꽃 index 24
        itemList.Add(new Item(10000022, "꽃", "하얀 꽃", Item.Itemtype.USE, 1, 10, 10));
        //꽃 index 25
        itemList.Add(new Item(10000023, "버섯", "마시쪙", Item.Itemtype.USE, 1, 10, 10));
        //나뭇가지 index 26
        itemList.Add(new Item(10000024, "나뭇가지", "유용한 나무막대기", Item.Itemtype.USE, 1, 10, 10));
        //버섯구이 index 27
        itemList.Add(new Item(20000011, "버섯구이", "포만감을 3 회복한다", Item.Itemtype.USE, 1, 30, 30, 1, 3));
        //버섯슾 index 28
        itemList.Add(new Item(20000012, "버섯슾", "포만감을 10 회복한다", Item.Itemtype.USE, 1, 90, 90, 1, 10));
        //생선종류 index 29~40
        itemList.Add(new Item(30000001, "썩어", "썩은것같아보이는'썩어'다", Item.Itemtype.USE, 1, 400, 500));
        itemList.Add(new Item(30000002, "우럭", "'우럭'매운탕먹고싶다", Item.Itemtype.USE, 1, 400, 500));
        itemList.Add(new Item(30000003, "전어", "가을'전어'다", Item.Itemtype.USE, 1, 400, 500));
        itemList.Add(new Item(30000004, "미꾸라지", "미끌미끌'미꾸라지'다", Item.Itemtype.USE, 1, 400, 500));
        itemList.Add(new Item(30000005, "피래미", "멸치만한'피래미'다", Item.Itemtype.USE, 1, 400, 500));
        itemList.Add(new Item(30000006, "돌돔", "돌같이생긴'돔'이다", Item.Itemtype.USE, 1, 400, 500));
        itemList.Add(new Item(30000007, "참돔", "참하게생긴'돔'이다", Item.Itemtype.USE, 1, 400, 500));
        itemList.Add(new Item(30000008, "열대어", "독이있을거같이생긴'열대어'다", Item.Itemtype.USE, 1, 400, 500));
        itemList.Add(new Item(30000009, "광어", "지느러미가맛있는'광어'다", Item.Itemtype.USE, 1, 400, 500));
        itemList.Add(new Item(30000010, "돌고래", "고래같이생겼지만착각이다'돌고래'다", Item.Itemtype.USE, 1, 400, 500));
        itemList.Add(new Item(30000011, "고래", "내가진짜'고래'다", Item.Itemtype.USE, 1, 400, 500));
        itemList.Add(new Item(30000012, "향어", "향어가 왜바다에????", Item.Itemtype.USE, 1, 50000, 9999999));
        //생선음식 index 41~42
        itemList.Add(new Item(40000001, "생선회", "정성을 가득들인 회한접시", Item.Itemtype.USE, 1, 50000, 9999999, 1, 5));
        itemList.Add(new Item(40000002, "구운생선", "굽기가 레어인듯하다", Item.Itemtype.USE, 1, 50000, 9999999, 1, 5));

    }
}