using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 튜토리얼 진행상황을 컨트롤하는 클래스
/// </summary>
public class PhaseCtrl : MonoBehaviour
{
    public GameObject Tutorial; // 튜토리얼 전체 하이어라키
    public GameObject Customization;    // 커스터마이징 전체 하이어라키
    [SerializeField]
    private int _phaseNum=0;
    public int phaseNum // 페이즈 번호가 설정될때 해당 페이즈에 맞는 동작이 실행되는 프로퍼티
    {
        get
        {
            return _phaseNum;
        }
        set
        {
            _phaseNum = value;
            switch (_phaseNum)
            {
                case 1:
                    StartCoroutine(te.ScriptNoGo());
                    StartCoroutine(ShowNamePannel(inputNamePannel.GetComponent<Image>()));
                    break;
                case 2:
                    //커스터마이징 호출
                    //GoNextScript();
                    Tutorial.SetActive(false);
                    Customization.SetActive(true);
                    break;
                case 3:
                    StartCoroutine(te.ScriptNoGo());
                    ShowMiniMap();
                    break;
                case 4:
                    GoNextScript();
                    break;
                case 5:
                    string questData = "{}";
                    File.WriteAllText(Application.persistentDataPath + "/questdata.json", questData);
                    File.WriteAllText(Application.persistentDataPath + "/invendata.json", questData);
                    PlayerPrefs.DeleteAll();
                    StartCoroutine(GoMainScene());
                    break;
                case 6:
                    ct.textTarget.text = "잠시만 기다려주세요";
                    te.FullscreenButton.SetActive(false);
                    break;
                default:
                    break;
            }
        }
    }
    CharacterTalk ct;
    TalkEffect te;
    
    public GameObject inputNamePannel; // 이름 입력을 받는 UI  

    [Header("미니맵 관련")]
    public MinimapCtrl mmc; 
    public CreateMapData CM;
    CreateMapData[] cm = new CreateMapData[4];
    public GameObject[] SelectMapButton;    // 미니맵 선택용 버튼 4개를 넣는 배열, 활성화 비활성화에 사용
    int selectedMapIDX; // 선택한 미니맵 인덱스를 저장
    public GameObject ConfirmPannel;    // 확인창 UI
    public GameObject MiniMapCanvas;    // 미니맵을 들고있는 캔버스
    public Image[] minimapButton; // 미니맵버튼
    

    private void Start()
    {
        ct = GetComponent<CharacterTalk>();
        te = GetComponent<TalkEffect>();
    }

    // 메인신으로 로딩되기전 저장이 완료 되지 않으면 대기하는 코루틴함수
    IEnumerator GoMainScene()
    {
        string[] waiting = new[] { "준비중.", "준비중..", "준비중..." };
        int idx = 0;
        while (!cm[selectedMapIDX].saveDone) // 저장이 완료되지 않으면
        {
            ct.textTarget.text = waiting[idx%3]; // 대사를 3가지 돌림
            idx++;
            yield return new WaitForSeconds(0.25f); // 0.25초마다
        }
        ct.textTarget.text = "갑니다"; // 저장이 완료되면 갑니다 출력

        
        SceneManager.LoadScene("MainMap",LoadSceneMode.Single); // 매인맵을 로딩
    }

    // 선택한 맵을 확정하는 버튼을 누를때 실행되는 함수
    public void SelectMap_Confirm()
    {
        for (int i = 0; i < SelectMapButton.Length; i++)
        {   // 선택버튼들을 비활성화
            SelectMapButton[i].SetActive(false);
        }
        MiniMapCanvas.SetActive(false); // 미니맵들을 비활성화
        
        cm[selectedMapIDX].SaveDataJson();
        ConfirmPannel.SetActive(false); // 확인창을 비활성화
        
        GoNextScript(); // 다음 대사 실행
        te.scriptPannel.SetActive(true);    // 대시 패널 활성화
    }

    // 맵선택 확인창에서 취소버튼에 호출되는 함수
    public void SelectMap_Cancel()
    {
        ConfirmPannel.SetActive(false); // 확인창 비활성화
        // 선택되었던 맵 테두리 색 초기화
        SelectMapButton[selectedMapIDX].GetComponent<Image>().color = Color.white;
    }

    // 맵을 선택할때 눌러지는 버튼에 호출되는 함수
    public void SelectMap(int idx)
    {
        selectedMapIDX = idx; // 선택한 맵번호 지정
        // 선택한 맵 버튼을 초록색으로 고정
        SelectMapButton[idx].GetComponent<Image>().color = new Color(0, 0.6117647f, 0, 1);
        ConfirmPannel.SetActive(true); // 확인창 활성화
    }

    // 선택할 미니맵을 보여주는 함수
    public void ShowMiniMap()
    {
        for (int i = 0; i < SelectMapButton.Length; i++)
        {   // 선택버튼 활성화
            SelectMapButton[i].SetActive(true);
        }
        // 미니맵 캔버스를 현재 디스플레이에 표시하게 함
        // 미니맵 캔버스를 비활성화 상태에서 활성화로 변경할 시
        // 4만개의 미니맵 오브젝트 활성화 때문에 렉이발생
        // 그 현상을 대처하기 위해 안보이게 두었던 미니맵을 보이게 함
        MiniMapCanvas.GetComponent<Canvas>().targetDisplay = 0;
    }

    // 4개 선택지의 미니맵을 생성하는 함수
    public void CreateFourMiniMap()
    {
        mmc.default_x = 0;  // 인게임과 튜토리얼의 미니맵 표시 위치가 다르므로 초기화
        mmc.default_y = 0;
        cm[0] = CM.CreateNewMapData("minimap1");
        mmc.ShowMiniMap(cm[0].MapDataStr, -450, 50,0,minimapButton[0]);
        cm[1] = CM.CreateNewMapData("minimap2");
        mmc.ShowMiniMap(cm[1].MapDataStr, 50, 50,0, minimapButton[1]);
        cm[2] = CM.CreateNewMapData("minimap3");
        mmc.ShowMiniMap(cm[2].MapDataStr, -450, -450,0, minimapButton[2]);
        cm[3] = CM.CreateNewMapData("minimap4");
        mmc.ShowMiniMap(cm[3].MapDataStr, 50, -450,0, minimapButton[3]);
    }

    // 이름 입력 패널 등장 이펙트
    IEnumerator ShowNamePannel(Image target)
    {
        target.color = Color.clear; // 패널 투명하게
        inputNamePannel.SetActive(true);    // 패널 활성화
        GameObject inputField = inputNamePannel.transform.GetChild(0).gameObject;// 인풋필드
        inputField.SetActive(false);    // 인풋필드 비활성화
        yield return StartCoroutine(te.FadeInObject(target, 0.5f)); // 서서히 나타나는 효과
        inputField.SetActive(true); // 인풋필드 활성화
        inputField.transform.GetComponent<InputField>().Select(); // 인풋필드 커서 활성화
        yield return null;
    }

    // 이름입력의 결정버튼에 호출되는 함수
    public void NamePannelButton(GameObject namePannel)
    {
        Text[] Text = namePannel.GetComponentsInChildren<Text>();   // 인풋필드의 모든 Text컴포넌트를 가져옴
        StartCoroutine(NamePannelSubmit(Text[1], Text[0], namePannel));
        StartCoroutine(te.ScriptNoGo());
    }

    // 결정버튼 눌렀을때 조건을 비교하여 동작을 결정하는 함수
    public IEnumerator NamePannelSubmit(Text nameText, Text backText, GameObject namePannel)
    { 
        if (nameText.text == "신너굴") // 이름을 입력한경우
        {
            yield return StartCoroutine(te.FadeOutObject(namePannel.GetComponent<Image>(), 0.5f)); // 서서히 사라짐효과
            namePannel.SetActive(false); // 이름입력패널 비활성화
            yield return new WaitForSeconds(0.5f);
            GoNextScript(); // 다음 스크립트로 진행
        }
        else
        {
            backText.text = "신너굴 입력하라고!"; // 이름이 입력되지 않으면 대기
            inputNamePannel.transform.GetChild(0).gameObject.transform.GetComponent<InputField>().Select();
            inputNamePannel.transform.GetChild(0).gameObject.transform.GetComponent<InputField>().text = "";


        }
    }

    // 다음 스크립트로 진행하는 함수
    public void GoNextScript()
    {
        ct.TutorialScript.PhaseNum++; // 스크립트의 페이즈 증가
        ct.clickScreen = false;
        ct.currentTextIdx = 0;  // 대사 인덱스를 0번째로 초기화
        StartCoroutine(te.ScriptGo());  // 스크립트 진행
    }
}
