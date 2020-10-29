using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상점입구에 가면 상점화면을 호출하는 클래스
/// </summary>
public class MegaMallMarket : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            Market_buy mb = FindObjectOfType<Market_buy>();

            if (!Inventory.instance.go.activeSelf)
            {
                mb.go.SetActive(true);
                mb.BUY_selectedTab = 2;
                mb.settabnumber(2);
                mb.BuyItem();
                mb.Buy_ShowItem();
                Inventory.instance.go.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            Market_buy mb = FindObjectOfType<Market_buy>();

            mb.go.SetActive(false);
            Inventory.instance.go.SetActive(false);
            Inventory.instance.DeSelectedTab();

        }
    }
}