using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 아이템 조합 목록을 보여줄 클래스
/// </summary>
public class DIYlist : MonoBehaviour
{
    List<Item> diyList = new List<Item>();
    ItemDatabase idb;
    AssemblyDatabase adb;
    public GameObject diyPrefab;
    public GameObject content;

    private void OnEnable()
    {
        if (idb == null)
            idb = FindObjectOfType<ItemDatabase>();

        if (adb == null)
            adb = FindObjectOfType<AssemblyDatabase>();

        // 조합 목록에 있는 아이템을 아이템목록과 비교하여 조합가능한 아이템 정보를 리스트에 추가시킴
        for (int j = 0; j < adb.assemList.Count; j++)
        {
            for (int i = 0; i < idb.itemList.Count; i++)
            {
                if(adb.assemList[j].itemID == idb.itemList[i].itemID) 
                { 
                    diyList.Add(idb.itemList[i]);
                    break;
                }
            }
        }

        // 스크롤뷰 컨텐트의 크기를 리스트 목록수에 맞게 조절함
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 160 * diyList.Count);

        // 목록을 생성함
        for (int i = 0; i < diyList.Count; i++)
        {
            GameObject prefab = Instantiate(diyPrefab, content.transform);
            prefab.GetComponent<DIYprefab>().SetThis(diyList[i], adb.assemList[i]);
        }
    }

    private void OnDisable()
    {
        // 목록 보는것이 비활성화 될때 목록아래의 객체들을 제거함
        for (int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
            diyList.Clear();
        }
    }
}
