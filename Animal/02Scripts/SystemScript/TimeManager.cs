using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 시간 관리자 클래스
/// </summary>
public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    public GameObject BigTimerObject; // 큰 시계UI 배경
    public GameObject BigTimerInner;  // 큰 시계UI 내부원
    public GameObject SmallTimerObject; // 작은 시계UI 배경
    public GameObject SmallTimerInner;  // 작은 시계UI 내부원
    public Text timeText;   // 시간 표시텍스트
    public Image dayDarkness; // 시간별로 화면 어두움을 표시해줄 이미지

    RectTransform BigRt; // 큰시계의 트랜스폼
    RectTransform SmallRt; // 작은시계의 트랜스폼

    public int day { get; private set; }
    public float time { get; private set; }
    public const float onedayTime = 1440;
    public float nightTime { get { return onedayTime * 20 / 24; }}
    public float dawnTime { get { return 0; }}
    public float dayTime { get { return onedayTime * 4 / 24; } }

    string saveTime = "SAVETIMENAME";
    string saveDay = "SAVEDAYNAME";
    string saveState = "SAVESTATENAME";
    string timeString = "";

    Color daycolor = Color.clear;
    Color nightcolor = new Color(0.1243063f, 0, 0.3018868f, 0.3f);
    Color dawncolor = new Color(0, 0.1861124f, 0.3019608f, 0.2f);
    float colorChangeTime = 2f;
    public enum TIMEZONE
    {
        DAY, NIGHT, DAWN,
    }
    [SerializeField] TIMEZONE _timezone;
    public TIMEZONE timezone
    {
        get { return _timezone; }
        set
        {
            if(_timezone != value)
            {
                _timezone = value;
                switch (_timezone)
                {   // 시간대별로 실행되어야 할 행동목록
                    case TIMEZONE.DAY:  // 낮시간엔 화면이 밝음
                        StartCoroutine(DarknessEffect(daycolor));
                        break;
                    case TIMEZONE.NIGHT: // 밤시간엔 화면이 어둡고 추격전이 시작됨
                        StartCoroutine(DarknessEffect(nightcolor));
                        FindObjectOfType<ChasingAtNight>().StartChasing();
                        break;
                    case TIMEZONE.DAWN: // 새벽시간엔 화면이 조금밝고 
                        StartCoroutine(DarknessEffect(dawncolor));
                        if (FindObjectOfType<Quest2>().IsEndQuest())
                        { // 2퀘스트가 종료되었다면
                            if (FindObjectOfType<ChasingAtNight>().notToNight)
                                return; 
                            FindObjectOfType<Quest_DailyRepayment>().LaunchingQuest();
                            // 일일 상환 이벤트가 발생한다
                        }
                        break;
                }
            }
        }
    }

    enum TIME
    {
        STOP,
        FLOW,
    }
    TIME timestate = TIME.STOP;
    public bool flowTime = false;

    CreateMapData cmd;


    public void test()
    {
        SetTime(onedayTime * 19.8f / 24);
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        BigRt = BigTimerInner.GetComponent<RectTransform>();
        SmallRt = SmallTimerInner.GetComponent<RectTransform>();
        timestate = (TIME)PlayerPrefs.GetInt(saveState, 0);
        cmd = FindObjectOfType<CreateMapData>();

        if (timestate == TIME.FLOW)
        {
            time = PlayerPrefs.GetFloat(saveTime, 0);
            day = PlayerPrefs.GetInt(saveDay, 1);
            StartCoroutine(SaveTime());
            flowTime = true;
            SmallTimerObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (!flowTime)
            return;

        time += Time.deltaTime; // 실제 경과 시간을 time값에 넣는다
        if(time > onedayTime) // 만약 하루로 설정된 시간이 지나면
        {
            day++; // 날자가 지나고
            time = 0; // 시간은 초기화되고
            PlayerPrefs.SetInt(saveDay, day);
            cmd.TreeGrowPerDay(); // 나무와 풀이 자란다
        }
        CompareTimeZone(); // 현재 시간이 어느시간대인지 확인한다

        SmallRt.rotation = Quaternion.Euler(0, 0, time * 360 / onedayTime); //작은시계가 돌아감

        if (!BigTimerObject.activeSelf)
            return;

        BigRtView(); 
    }

    void BigRtView()
    {   // 큰 시계를 보여주는함수 
        BigRt.rotation = Quaternion.Euler(0, 0, time * 360 / onedayTime); // 큰시계 회전
        timeString = "";
        timeString += "Day " + day + "\n";
        if (time < onedayTime / 2)
            timeString += "AM ";
        else
            timeString += "PM ";

        int hour = (int)(time / (onedayTime / 24));
        timeString += hour.ToString("D2") + " : ";
        int minute = (int)((time - hour * (onedayTime / 24)) * (60 / (onedayTime / 24)));
        //time - hour*(onedayTime / 24);
        //에다가
        //60/(onedayTime/24)를 곱해주면 분
        timeString += minute.ToString("D2");

        timeText.text = timeString;
    }

    void CompareTimeZone()
    {
        if (dawnTime < time && time < dayTime)
            timezone = TIMEZONE.DAWN;
        else if (dayTime < time && time < nightTime)
            timezone = TIMEZONE.DAY;
        else if (nightTime < time)
            timezone = TIMEZONE.NIGHT;
    }

    IEnumerator DarknessEffect(Color color)
    {
        yield return null;
        float cool = 0;
        Color currColor = dayDarkness.color;
        while(cool < colorChangeTime)
        {
            dayDarkness.color = Color.Lerp(currColor, color, cool/colorChangeTime);
            cool += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator SaveTime()
    {   // 현재 시간을 저장하는 코루틴
        while (true)
        {
            PlayerPrefs.SetFloat(saveTime, time);
            yield return new WaitForSeconds(2f);
        }
    }

    public void SetActiveBigTimer()
    {
        BigTimerObject.SetActive(!BigTimerObject.activeSelf);
        if(BigTimerObject.activeSelf)
            BigRtView();
    }

    public void ActiveTimeManager()
    {
        time = PlayerPrefs.GetFloat(saveTime, 0);
        day = PlayerPrefs.GetInt(saveDay, 0);
        StartCoroutine(SaveTime());
        flowTime = true;
        timestate = TIME.FLOW;
        PlayerPrefs.SetInt(saveState, (int)timestate);

        SmallTimerObject.SetActive(true);
    }

    public void SetTime(float _time)
    {
        if (_time < time)
        {
            day++;
            PlayerPrefs.SetInt(saveDay, day);
        }
        time = _time;
    }
}
