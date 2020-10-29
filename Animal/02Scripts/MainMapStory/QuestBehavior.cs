using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 퀘스트 구현에 필요한 변수와 함수를 정의해둔 클래스
/// </summary>
public class QuestBehavior : MonoBehaviour
{
    protected Ctrl ctrl;
    protected PlayerMovement playerMovement;
    protected GameObject panel;
    protected Image panelImage;
    protected Text panelText;
    protected TalkEffect talkEffect;
    protected bool goingonQuest;
    protected bool confirm;
    protected NPC_DASHIDA NPCdashida;

    protected enum PROGRESS
    {
        YET,
        START,
        GOING,
        END,
    }
    
    protected GameObject tentPrefab;

    protected virtual void Start()
    {
        ctrl = FindObjectOfType<Ctrl>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        panel = GameObject.Find("ChattingCanvas").transform.GetChild(0).gameObject;
        panel.SetActive(true);
        panelImage = panel.gameObject.GetComponentInChildren<Image>();
        panelText = panel.gameObject.GetComponentInChildren<Text>();
        panel.SetActive(false);
        talkEffect = GetComponent<TalkEffect>();
        tentPrefab = Resources.Load("Prefabs/buildings/tent", typeof(GameObject)) as GameObject;
        NPCdashida = FindObjectOfType<NPC_DASHIDA>();
    }

    protected void StopInput()
    {
        JoyStickManager.Instance.CanvasDisable();
        ctrl.enabled = false;
        playerMovement.enabled = false;
    }

    protected void StartInput()
    {
        ctrl.enabled = true;
        playerMovement.enabled = true;
        JoyStickManager.Instance.CanvasAble();
    }

    public virtual void LaunchingQuest() { }

    protected void ResetQusetList()
    {
        QuestManager.instance.questListView();
        QuestManager.instance.questListView();
    }

    protected Item SearchInventory(int itemId)
    {
        Item item = null;

        for (int i = 0; i < Inventory.instance.inventoryItemList.Count; i++)
        {
            if (Inventory.instance.inventoryItemList[i].itemID == itemId)
            {
                item = Inventory.instance.inventoryItemList[i];
                break;
            }
        }

        return item;
    }
}
