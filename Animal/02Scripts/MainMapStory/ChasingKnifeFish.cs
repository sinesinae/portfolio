using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 추격자의 추격을 구현하는 클래스
/// </summary>
public class ChasingKnifeFish : MonoBehaviour
{
    Transform playerTr;
    Rigidbody rb;
    bool isChase = false;
    CoinManager coin;
    Image effect;


    private void Start()
    {
        playerTr = FindObjectOfType<Player>().transform;
        rb = GetComponent<Rigidbody>();
        isChase = true;
        StartCoroutine(WaitForComment());
    }

    IEnumerator WaitForComment()
    {   // 추격자가 생성된후 대화출력동안 대기하게 함
        yield return null;

        while (!TimeManager.instance.flowTime)
        {
            yield return null;
        }

        StartCoroutine(MoveAtStart());
    }

    IEnumerator MoveAtStart()
    {   // 추격시작
        while (isChase)
        {
            Vector3 pos = playerTr.position;
            Vector3 force = new Vector3(pos.x, 2, pos.z) - transform.position;
            int speed = Random.Range(10, 40);
            rb.AddForce(force.normalized * speed, ForceMode.Impulse);
            float time = Random.Range(1.0f, 3.0f);
            yield return new WaitForSeconds(time);
            rb.velocity = Vector3.zero;
            if ((playerTr.position - transform.position).sqrMagnitude > 100)
                MoveRandomPos();
        }
    }

    void MoveRandomPos()
    {
        Vector2 rpos = Random.insideUnitCircle;
        rpos = rpos.normalized * 10+ new Vector2(playerTr.position.x, playerTr.position.z);
        transform.position = new Vector3(rpos.x, 2, rpos.y);
    }

    public void EndChase()
    {   // 해당함수가 호출되면 추격 코루틴이 종료됨
        isChase = false;
    }

    private void OnTriggerEnter(Collider other)
    {   // 추격자가 플레이어와 충돌하면
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null && isChase)
        {
            //코인매니저에서 돈을뺀다
            if (coin == null)
                coin = FindObjectOfType<CoinManager>();

            if (coin.coin >= 10)
                coin.coin -= 10;
            else
                coin.coin = 0;

            StartCoroutine(ImpactEffect()); // 피격 이펙트 출력
            FindObjectOfType<CoinManager>().SaveCoin(); // 코인값 저장

            //코인이 0원이되면 체이스 종료
            if (coin.coin <= 0)
               FindObjectOfType<ChasingAtNight>()._EndChase();
        }
    }

    IEnumerator ImpactEffect()
    {   // 피격 이펙트
        yield return null;
        
        // 시간을 표시하기위한 UI를 재활용하여 피격이펙트를 구현
        if (effect == null)
            effect = GameObject.Find("DayDarkness").GetComponent<Image>();

        float time = 0;

        while (time < 0.3f)
        {
            effect.color = Color.Lerp(Color.red, new Color(0.1243063f, 0, 0.3018868f, 0.3f), time / 0.3f);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
