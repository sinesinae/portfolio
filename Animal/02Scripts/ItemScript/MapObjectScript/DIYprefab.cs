using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 조합 목록에서 개별 목록을 관리하는 클래스
/// </summary>
public class DIYprefab : MonoBehaviour
{
    [HideInInspector] public Item item;
    [HideInInspector] public AseemblyItem Aitem;
    public Text nameText;
    public Text desText;
    public Image icon;
    GameObject parent;
    bool isInteract;

    public Slider slider;
    public Image fillImage;
    float successTime = 1f;
    float disapearTime = 0.5f;

    private void OnEnable()
    {
        parent = GameObject.Find("WorkbenchListContent");
        this.transform.SetParent(parent.transform);
        isInteract = true;
    }

    public void SetThis(Item _item, AseemblyItem _Aitem)
    {
        // 해당 목록의 정보를 읽어옴
        item = _item;
        Aitem = _Aitem;
        nameText.text = item.itemName;
        desText.text = Aitem.assemblyList;
        icon.sprite = item.itemIcon;
    }

    public void ClickThis() // 목록을 클릭했을때 실행되는 함수
    {
        if (!isInteract)
            return;

        if (item.itemType == Item.Itemtype.ETC)
        { // 도구 만들때 16개 이상 있으면 제작 불가능(인벤토리 갯수제한)
            int count = 0;

            for (int k = 0; k < Inventory.instance.inventoryItemList.Count; k++)
            {
                if (Inventory.instance.inventoryItemList[k].itemType == Item.Itemtype.EQUIP ||
                    Inventory.instance.inventoryItemList[k].itemType == Item.Itemtype.ETC)
                    count++;
            }
            if (count >= 16)
            {
                StartCoroutine(FailEffect());
                return;
            }
        }

        isInteract = false;
        Item[] materials;

        if (CheckMaterial(Aitem.assemblyState, Aitem.materialID, Aitem.materialCount, out materials))
        { // 재료 조건을 검사해서 조합이 가능한지 확인

            StartCoroutine(SuccessEffect()); // 조합성공 이펙트 실행

            //여기서 조건별로 감소갯수 바꿔준다
            switch (Aitem.assemblyState)
            {
                case AseemblyItem.STATE.ASSEMBLYMULTICOUNT: // 2개이상의 지정된 재료가 필요한경우
                    for (int i = 0; i < materials.Length; i++)
                    {
                        materials[i].itemCount -= Aitem.materialCount[i]; // 재료의 갯수를 차감하고
                        if (materials[i].itemCount <= 0)
                        {   // 재료가 0개가되면 해당 재료를 인벤토리에서 제거함
                            Inventory.instance.inventoryItemList.Remove(materials[i]);
                        }
                    }
                    Inventory.instance.GetAnItem(item.itemID, out bool get); // 인벤토리에 조합완성품을 추가
                    if (Inventory.instance.gameObject.activeSelf) // 인벤토리가 열려있는경우
                        Inventory.instance.ShowItem(); // 인벤토리를 다시 보여줌
                    Inventory.instance.saveinven(); // 인벤토리 데이터 저장
                    break;
                case AseemblyItem.STATE.ASSEMBLYMONOCOUNT: // 재료의 종류와 상관없이 수량만 충족하면 되는경우
                    int count = Aitem.materialCount[0]; // 총 필요한 재료의 수를 저장
                    for (int i = 0; i < materials.Length; i++)
                    {
                        if (materials[i] == null) // 해당 재료가 null이면 반복문 다음회차로 이동
                            continue;

                        if (count - materials[i].itemCount >=0) // 해당재료가 총 수량보다 적은경우
                        {
                            count -= materials[i].itemCount; // 총 수량에서 해당수량만큼 차감하고
                            Inventory.instance.inventoryItemList.Remove(materials[i]); // 해당재료 제거
                        }
                        else // 해당 재료가 총 수량보다 많을경우
                        {
                            materials[i].itemCount -= count; // 해당 재료를 필요한만큼 차감하고
                            break; // 반복문 종료
                        }
                    }
                    Inventory.instance.GetAnItem(item.itemID, out get); // 인벤토리에 완성품을 추가함
                    if (Inventory.instance.gameObject.activeSelf) // 인벤토리가 활성화되어있으면
                        Inventory.instance.ShowItem(); // 목록을 다시로드함
                    Inventory.instance.saveinven(); // 인벤토리를 저장함
                    break;  
            }
        }
        else // 조합이 불가능할경우
        {
            StartCoroutine(FailEffect()); // 조합 실패 이펙트실행
        }
    }

    IEnumerator SuccessEffect() // 조합성공이펙트 - 초록색 게이지가 올라가며 조합이 진행됨을 표시한다
    {
        yield return null;

        slider.value = 0;
        fillImage.color = Color.green;

        float time = 0;
        while (time < successTime) // 초록색 진행바가 채워짐
        {
            time += Time.deltaTime;
            slider.value = time / successTime;
            yield return null;
        }

        Comment.instance.CommentPrint("제작에 성공했다"); // 제작 성공 멘트가 화면에 표시됨
        time = 0;

        while (time < disapearTime) // 초록색으로 채워진것이 사라짐
        {
            time += Time.deltaTime;
            fillImage.color = Color.Lerp(Color.green, Color.clear, time / disapearTime);
            yield return null;
        }
        isInteract = true; // 다시 조합버튼을 누를수 있게됨
        slider.value = 0; // 슬라이더 초기화

    }

    IEnumerator FailEffect() // 조합 실패한 경우의 이펙트
    {
        yield return null;

        slider.value = 1; // 슬라이더를 가득채움

        fillImage.color = Color.red; // 슬라이더 색상은 빨강

        Comment.instance.CommentPrint("제작에 실패했다"); // 제작 실패멘트 출력
        float time = 0;
        while (time < disapearTime) // 빨간색이 사라지는 효과
        {
            time += Time.deltaTime;
            fillImage.color = Color.Lerp(Color.red, Color.clear, time / disapearTime);
            yield return null;
        }
        isInteract = true; // 다시 터치할수있음
        slider.value = 0; // 슬라이더 초기화

    }

    bool CheckMaterial(AseemblyItem.STATE state, int[] materialID, int[] materialCount, out Item[] materials)
    {   // 재료가 필요량만큼 있는지 확인하여 조합가능여부를 리턴하는 함수

        if (state == AseemblyItem.STATE.ASSEMBLYMULTICOUNT) // 두가지 이상의 재료가 필요한경우
        {
            bool[] isbool = new bool[materialID.Length]; // 재료의 갯수만큼 bool값을 생성
            materials = new Item[materialID.Length]; // out해줄 Item배열 생성

            for (int i = 0; i < materialID.Length; i++)
            {
                for (int j = 0; j < Inventory.instance.inventoryItemList.Count; j++)
                {
                    if (Inventory.instance.inventoryItemList[j].itemID == materialID[i])
                    {   // 만약 인벤토리에 해당 재료가 있으면
                        if (Inventory.instance.inventoryItemList[j].itemCount >= materialCount[i])
                        {   // 수량이 충분한지 확인한 뒤
                            isbool[i] = true; // bool값을 true로 해줌
                            materials[i] = Inventory.instance.inventoryItemList[j]; // out배열에 해당 아이템 추가
                        }
                    }
                }
            }
            if (isbool.Contains(false)) // bool배열에 false값이 있으면 false반환
                return false;
            else
                return true;
        }
        else if (state == AseemblyItem.STATE.ASSEMBLYMONOCOUNT) // 여러재료가 섞여도 되는경우
        {
            materials = new Item[materialID.Length] ; // out해줄 아이템배열
            int count = materialCount[0]; // 필요수량 저장

            for (int i = 0; i < materialID.Length; i++)
            {
                for (int j = 0; j < Inventory.instance.inventoryItemList.Count; j++)
                {
                    if (Inventory.instance.inventoryItemList[j].itemID == materialID[i])
                    {   // 인벤토리에 해당 아이템이 있으면
                        count -= Inventory.instance.inventoryItemList[j].itemCount; // 총수량에서 해당아이템수량을 뺌
                        materials[i] = Inventory.instance.inventoryItemList[j]; // out배열에 해당아이템 추가
                        if (count <= 0) // 총 수량이 0이하가 되면 true반환
                            return true;
                    }
                }
            }

            return false; // 총수량이 남는다면 false반환

        }
        else
        {   // 그외 실패함
            materials = new Item[materialID.Length];
            return false;
        }
    }
}
