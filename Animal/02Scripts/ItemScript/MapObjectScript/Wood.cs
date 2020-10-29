using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


/// <summary>
/// 맵에서 나무와 플레이어간의 상호작용을 담당하는 클래스
/// </summary>
public class Wood : MonoBehaviour, IInterAct
{
    Player player;
    bool canInteract;

    private void Start()
    {
        canInteract = true;
    }

    public void InterActPlayer()
    {
        if (!canInteract) // 상호작용이 안되는 상태일때는 함수리턴
            return;

        canInteract = false; // 상호작용이 안됨으로 설정하여 연속입력으로 인한 버그 방지

        if (player == null)
            player = FindObjectOfType<Player>();

        if (player.EquipItem == null)
        {
            BoolList bl = Comment.instance.CommentPrint("나무는 빈손으로 캘수 없다", canInteract);
            StartCoroutine(Wait(bl));
            return; // 빈손이면 리턴
        }

        if (player.EquipItem != null)
        {   // 현재 장비가 도끼가 아닌경우
            if (player.EquipItem.itemdata.itemID != 90000031
                && player.EquipItem.itemdata.itemID != 90000032
                && player.EquipItem.itemdata.itemID != 90000033
                && player.EquipItem.itemdata.itemID != 90000034)
            {
                // 멘트를 출력하고
                BoolList bl = Comment.instance.CommentPrint("나무는 도끼로 캘수 있다", canInteract);
                StartCoroutine(Wait(bl)); // 해당 멘트가 실행된후 canInteract를 true로 바꿔줌
                return; 
            }
        }

        // 도끼를 사용할 경우 아래 실행
        player.EquipItem.ItemDuration(-1); // 내구도 1 감소

        int chance = Random.Range(0, 100); // 확률 생성

        if (chance < 30)
        {   // 30퍼센트 확률로 나뭇가지를 획득함
            Inventory.instance.GetAnItem(10000024, out bool ajrdma, 1);
            BoolList bl = Comment.instance.CommentPrint("나뭇가지를 획득했다.", canInteract);
            StartCoroutine(Wait(bl));
        }
        else
        {   // 실패한경우 실패멘트 출력
            BoolList bl = Comment.instance.CommentPrint("나무에서 아무것도 나오지 않았다", canInteract);
            StartCoroutine(Wait(bl));
        }
    }

    IEnumerator Wait(BoolList bl)
    {
        if (bl.isDone)
        {
            yield return new WaitWhile(() => bl.isDone); // bool값이 false일때까지 대기
            canInteract = true;
        }
        else
        {
            yield return new WaitUntil(() => bl.isDone); // bool값이 true일때까지 대기
            canInteract = true;
        }
    }
}
