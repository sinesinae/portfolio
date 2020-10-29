using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishLod : ItemPrefabBase
{
    public Transform startPos;
    public LineRenderer LR;
    int layer;
    bool isFishing;
    Mulgogi mulgogi;

    // 낚시대 프리펩에 적용되는 스크립트
    protected override void Start()
    {
        base.Start();   // 필요컴포넌트와 프리펩위치 지정
        base.CopyItemData();    // 해당 아이템의 데이터를 가져옴

        LR.positionCount = 2;
        LR.enabled = false;
        layer = 1 << LayerMask.NameToLayer("PLAYERINTERACTOBJECT");
    }

    // 해당 아이템 착용중 사용했을때 실행될 함수
    public override void Use_Equip()
    {
        base.Use_Equip();

        Ray ray = new Ray(PlayerFunctions.instance.bodyTr.position + new Vector3(0, 0.5f, 0), PlayerFunctions.instance.bodyTr.forward);
        if (Physics.Raycast(ray, out RaycastHit hitt, 4f, layer))
        {
            mulgogi = hitt.transform.GetComponent<Mulgogi>(); 
            if(mulgogi != null)
            {
                isFishing = true;
                StartCoroutine(FishLineEffect(hitt.transform.position));
                JoyStickManager.Instance.CanvasDisable();
                
                FindObjectOfType<MinigameFish>().StartFishing();
            }
        }
    }

    public void EndFish(bool iscatch)
    {
        isFishing = false;
        LR.enabled = false;

        if (iscatch)
        {
            Inventory.instance.GetAnItem(mulgogi.thisMulgogi.itemID, out bool ajrdma);
            string comment = mulgogi.thisMulgogi.itemName + "!! 잡았다!!";
            Comment.instance.CommentPrint(comment);
        }
        else
        {
            Comment.instance.CommentPrint("생선을 놓치고 말았다..");
        }
        if (mulgogi != null) 
            Destroy(mulgogi.gameObject);

        base.ItemDuration(-1);
    }

    IEnumerator FishLineEffect(Vector3 target)
    {
        while (isFishing)
        {
            LR.SetPosition(0, startPos.position);
            LR.SetPosition(1, target);
            LR.enabled = true;
            yield return null;
        }
    }
}
