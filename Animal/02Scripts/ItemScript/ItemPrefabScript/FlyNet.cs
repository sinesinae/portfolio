using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyNet : ItemPrefabBase
{
    // 잠자리채 프리펩에 적용되는 스크립트
    protected override void Start()
    {
        base.Start();   // 필요컴포넌트와 프리펩위치 지정
        base.CopyItemData();    // 해당 아이템의 데이터를 가져옴
    }

    // 해당 아이템 착용중 사용했을때 실행될 함수
    public override void Use_Equip()
    {
        base.Use_Equip();

    }
}
