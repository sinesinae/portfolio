using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_DASHIDA_TRIGGER : MonoBehaviour
{
    NPC_DASHIDA dashida;

    private void Start()
    {
        dashida = FindObjectOfType<NPC_DASHIDA>();
    }

    IEnumerator UpDASHIDA()//다시다가 올라올때 구현함수
    {
        yield return null;
        float i = 0;
        int a = -1;
        Vector3 origine = dashida.transform.position;//다시다 위치가져옴

        while (i <= 2f)
        {
            i += Time.deltaTime;
            dashida.transform.position = Vector3.Lerp(origine, new Vector3(87, 1.5f, 10), i / 2f);//다시다 트랜스폼을 위로올림

            a = a * -1;
            dashida.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 15 * a));//파닥파닥

            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return null;
        dashida.RunAction();
    }

    public IEnumerator DownDASHIDA()//다시다가 내려가는함수
    {
        yield return null;
        float i = 0;
        int a = -1;
        Vector3 origine = dashida.transform.position;

        while (i <= 2f)
        {
            i += Time.deltaTime;
            dashida.transform.position = Vector3.Lerp(origine, new Vector3(87, -1.0f, 10), i / 2f);//안보이게 내려감

            a = a * -1;
            dashida.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 15 * a));//파닥파닥

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)//다시다에게 접근하면
    {
        StopAllCoroutines();//모든코르틴멈추고
        StartCoroutine(UpDASHIDA());//다시다 올라오는코르틴 시작
    }

    private void OnTriggerExit(Collider other)//다시다에서 떨어지면
    {
        StopAllCoroutines();
        StartCoroutine(DownDASHIDA());//내려가는 코르틴 시작
    }
}
