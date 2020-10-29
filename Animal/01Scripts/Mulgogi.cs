using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mulgogi : MonoBehaviour
{
    public bool A;//반복조정 불
    public Item thisMulgogi;//물고기


    private void Start()
    {
        A = true;
        transform.Rotate(45, 0, 0);//물고기 로테이트조정
        StartCoroutine(MulgogiCtrl());//반복
    }

    IEnumerator MulgogiCtrl()//물고기가 파닥하는 모습구현 함수
    {
        yield return null;
        int a = -1;
        while (A)
        {
            a = a * -1;
            transform.Rotate(0, 0, a * 6f);

            yield return new WaitForSeconds(Time.deltaTime);
        }

    }


}
