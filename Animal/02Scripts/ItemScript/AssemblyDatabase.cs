using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 조합아이템 데이터를 관리하는 클래스
/// </summary>
[System.Serializable]
public class AseemblyItem
{
    public int itemID;  // 조합할 아이템의 ID
    public STATE assemblyState; // 조합 가능여부 인덱스 0 : 불가능 1: 복합재료복합수량 2: 복합재료단일수량
    public string assemblyList; // 조합재료 목록
    public int[] materialID; // 재료 목록
    public int[] materialCount; //재료 수량

    public enum STATE
    {
        NOASSEMBLY,
        ASSEMBLYMULTICOUNT,
        ASSEMBLYMONOCOUNT,
    }

    public AseemblyItem(int ItemID ,STATE AssemblyState, string AssemblyList, int[] MaterialID, int[] MaterialCount)    
    {
        itemID = ItemID;
        assemblyState = AssemblyState;
        assemblyList = AssemblyList;
        materialID = MaterialID;
        materialCount = MaterialCount;
    }
}

public class AssemblyDatabase : MonoBehaviour
{
    public List<AseemblyItem> assemList = new List<AseemblyItem>();

    private void Awake()
    {
        //엉성한 도끼 90000031
        assemList.Add(new AseemblyItem(90000031, AseemblyItem.STATE.ASSEMBLYMULTICOUNT, "나뭇가지x10 잡초x5", new int[] { 10000024, 10000021 }, new int[] { 10, 5 }));
        //엉성한 낚시대
        assemList.Add(new AseemblyItem(90000011, AseemblyItem.STATE.ASSEMBLYMULTICOUNT, "나뭇가지x10 잡초x5", new int[] { 10000024, 10000021 }, new int[] { 10, 5 }));
        //버섯구이
        assemList.Add(new AseemblyItem(20000011, AseemblyItem.STATE.ASSEMBLYMULTICOUNT, "나뭇가지x1 버섯x1", new int[] { 10000024, 10000023 }, new int[] { 1, 1 }));
        //버섯 슾
        assemList.Add(new AseemblyItem(20000012, AseemblyItem.STATE.ASSEMBLYMULTICOUNT, "나뭇가지x3 버섯x3", new int[] { 10000024, 10000023 }, new int[] { 3, 3 }));
        //생선구이
        assemList.Add(new AseemblyItem(40000002, AseemblyItem.STATE.ASSEMBLYMONOCOUNT, "생선x1", new int[] { 30000012, 30000011, 30000010, 30000009, 30000008, 30000007, 30000006, 30000005, 30000004, 30000003, 30000002, 30000001 }, new int[] { 1 }));
        //생선회
        assemList.Add(new AseemblyItem(40000001, AseemblyItem.STATE.ASSEMBLYMONOCOUNT, "생선x5", new int[] { 30000012, 30000011, 30000010, 30000009, 30000008, 30000007, 30000006, 30000005, 30000004, 30000003, 30000002, 30000001 }, new int[] { 5 }));
    }
}
