using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 낚시를 미니게임으로 구현한 클래스
/// </summary>
public class MinigameFish : MonoBehaviour
{
    public GameObject FishGame;
    public RectTransform ProgressBar;
    public RectTransform jji;

    public RectTransform progressOrigin;
    public RectTransform progressEnd;

    Image progressimg;
    Color originColor = new Color(1,1,1,0.3f);

    enum STATE
    {
        START,
        BITE,
        MISS,
        CATCH,
    }
    STATE state;

    public void StartFishing()
    {   // 낚시 시작 함수
        StartCoroutine(StartDelay());
    }

    IEnumerator StartDelay()
    {   // 1초를 기다린뒤 미니게임 활성화
        yield return new WaitForSeconds(1f);
        FishGame.SetActive(true);

        progressimg = ProgressBar.transform.GetComponent<Image>();
        progressimg.color = originColor;
        ProgressBar.position = progressOrigin.position;
        jji.position = progressEnd.position;

        state = STATE.START;
        StartCoroutine(MoveJJI());
    }

    IEnumerator MoveJJI()
    {   // 미니게임상에서 찌를 흔드는 코루틴
        yield return null;

        // 몇번 흔들지 정함 idx
        int idx = Random.Range(3, 10);
        for (int i = 0; i < idx; i++)
        {
            // 그 횟수만큼 흔드는데 흔들때마다 어느강도로 흔들지 정함
            float tense = Random.Range(50, 120);
            yield return StartCoroutine(ShakeJJI(tense));
        }
        // 정해진 횟수만큼 흔들면 생선이 꽉물은 상태가 됨
        float time = 0;
        Vector3 origin = jji.position;
        Vector3 destination = new Vector3(0, -250, 0) + origin;
        while (time < 0.2f)
        {
            jji.position = Vector3.Lerp(origin, destination, time / 0.2f);
            time += Time.deltaTime;
            yield return null;
        }
        jji.position = destination;
        StartCoroutine(FishRoutine());
    }

    IEnumerator FishRoutine()
    {
        state = STATE.BITE;
        // 0.5초 내에 터치하면 낚시 성공
        float time = 0;
        while (time < 0.5f)
        {
            ProgressBar.position = Vector3.Lerp(progressOrigin.position, progressEnd.position, time / 0.5f);
            time += Time.deltaTime;
            yield return null;
        }
        ProgressBar.position = progressEnd.position;
        yield return null;
        state = STATE.MISS;
        // 그외 모든 순간에 터치하면 낚시 실패
        CatchFish();
    }

    // 정해진 강도로 흔들어주는 코루틴 만듬
    IEnumerator ShakeJJI(float tense)
    {
        yield return null;
        float time = 0;
        Vector3 origin = jji.position;
        Vector3 destination = new Vector3(0, -tense, 0) + origin;
        while (time < 0.2f) // 찌가 팍 내려갔다가
        {
            jji.position = Vector3.Lerp(origin, destination, time / 0.2f);
            time += Time.deltaTime;
            yield return null;
        }
        time = 0;
        while (time < 0.8f) // 서서히 올라옴
        {
            jji.position = Vector3.Lerp(jji.position, origin, time / 0.8f);
            time += Time.deltaTime;
            yield return null;
        }
        jji.position = origin;
    }


    public void CatchFish()
    {
        switch (state)
        {
            case STATE.BITE:
                // 잡은상황
                StopAllCoroutines();
                StartCoroutine(Colorize(new Color(0,1,0,0.5f)));
                state = STATE.CATCH;
                FindObjectOfType<FishLod>().EndFish(true);
                break;
            default:
                // 놓친상황
                state = STATE.MISS;
                StopAllCoroutines();
                StartCoroutine(Colorize(new Color(1, 0, 0, 0.5f)));
                FindObjectOfType<FishLod>().EndFish(false);
                break;
        }
    }

    IEnumerator Colorize(Color color)
    {
        yield return null;

        ProgressBar.position = progressEnd.position;

        float time = 0;
        Color origin = progressimg.color;
        while (time < 0.5f)
        {
            progressimg.color = Color.Lerp(origin, color, time / 0.5f);
            time += Time.deltaTime;
            yield return null;
        }
        progressimg.color = color;

        yield return new WaitForSeconds(1f);
        FishGame.SetActive(false);
    }
}
