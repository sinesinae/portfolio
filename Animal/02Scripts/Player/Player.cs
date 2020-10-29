using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 현재 상태 
public enum STATE
{
    BAREHAND,
    EQUIPED,
}

/// <summary>
/// 플레이어 상태 정보를 저장하는 클래스
/// </summary>
public class Player : MonoBehaviour
{
    public static Player instance;  // 클래스 스태틱화

    public List<STATE> playerState = new List<STATE>(); // 플레이어 상태 목록
    public ItemPrefabBase EquipItem = null; // 현재 장착중인 아이템

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerState.Add(STATE.BAREHAND);
    }
}

