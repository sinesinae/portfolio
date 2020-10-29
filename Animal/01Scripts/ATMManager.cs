using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ATMManager : MonoBehaviour
{
    public class saveATM//atm코인을 저장할 클래스
    {
        public int ATMmoney;
    }

    public int ATM = 10000000;//빚잔액
    int input;//입력받을 금액

    CoinManager CoinManager;//코인정보를 가지고올 클래스
    Ending Ending;//엔딩클래스

    public GameObject CCTV;//atm모드를 꾸며주는cctvimage
    public GameObject EXIT;//atm모드를 벗어나게할 버튼
    public GameObject COINPANEL;//ui에 보여질패널

    public InputField InputField;//입출금금액을 입력받을부분
    
    public Text atmtext;//atm화면에 나오는 텍스트
    public Text cointext;//보유중인 코인을 표시할 텍스트
    
    void Start()
    {
        CoinManager = FindObjectOfType<CoinManager>();//코인정보를 불러옴
        Ending = FindObjectOfType<Ending>();//엔딩정보를 불러옴
        loadATMcoin();//저장된 atmcoin을 불러옴
    }

    private void LateUpdate()
    {
        if (CCTV.activeSelf)
            JoyStickManager.Instance.CanvasDisable();
    }
    public void MinusATM()//입력받은 input을 atm에 더해주고 보유코인을 감소하는 함수(대출증가/)
    {
        ScanATM();
        ATM += input;
        CoinManager.ATMminuscoin(input);
        CheckATM();
        CheckCOIN();
    }
    public void PlusATM()//입력받은 input을 atm에 빼주고 보유코인을 감소하는 함수(대출증가)
    {
        ScanATM();
        if (CoinManager.coin < input)//보유중인 코인이 입력받은 input보다 작거나 같으면
        {
            Comment.instance.CommentPrint("돈없어서 빚못갚아");//대화ui실행
        }
        else if(ATM<input)
        {
      
            if (ATM < 0)
            {
                Comment.instance.CommentPrint("대출금이 전액 상환 되었습니다.");
            }
            else if(ATM > 0 )
            {
                CoinManager.coin -= ATM;
                string atm = ATM.ToString();
                Comment.instance.CommentPrint(atm + "원 출금");
                ATM -= ATM;
            }
            CheckATM();
            CheckCOIN();
        }
        else//아니면 대출증가
        {
            ATM -= input;
            CoinManager.ATMaddcoin(input);
            CheckATM();
            CheckCOIN();
        }

    }
    public void CheckATM()//atm코인을 저장하고 atm잔액을 보여주는함수(잔액확인)
    {
        atmtext.text ="대출잔액\n"+ATM;
        SaveATMcoin();
        if(ATM==0)
        {
            Ending.ending_();
        }
    }
    public void CheckCOIN()//보유코인정도를 저자아고 보유코인잔액을 보여주는함수(보유코인확인)
    {
        cointext.text = "보유잔액\n" + CoinManager.coin;
        CoinManager.SaveCoin();
    }
    public void ScanATM()//atm입출금 금액을 입력하는함수
    {
        
        if (String.IsNullOrEmpty(InputField.text))
            return;
        input = Convert.ToInt32(InputField.text);//텍스트를 인트로변환 인풋에저장함
        InputField.text = "0";//저장후 텍스트는0
    }
    private void OnTriggerEnter(Collider other)//플레이어가 atm콜라이더에 들어오면 atm모드진입
    {
        Player player = other.GetComponent<Player>();//플레이어정보를 불러옴

        if (player != null)//플레이어가널이아니면 atm모드실행
        {
            atmtext.text = "금액입력후 입금 출금 버튼을 눌러주세요";
            FindObjectOfType<FollowCamera>().Target = this.transform;//카메라타겟을 플레이어에서 atm으로 변경
            FindObjectOfType<FollowCamera>().Offset = new Vector3(-2, 2.5f, 0.05f);//카메라오프셋을 플레이어에서 atm위치로 변경
            JoyStickManager.Instance.CanvasDisable();//조이스틱 멈춤
            CCTV.SetActive(true);//cctv켬
            EXIT.SetActive(true);//나가기버튼활성화
            COINPANEL.SetActive(true);//코인패널활성화
            CheckCOIN();//코인정보출력
            atmtext.gameObject.SetActive(true);//에이티엠화면출력
        }
    }
    public void OnTriggerExit(Collider other)//플레이어가 atm콜라이더에서 벗어나면 atm모드벗어남
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            Exitbutton();
        }
    }
    public void Exitbutton()//atm모드중 나가기버튼을 누르면 atm모드를 종료하는 함수
    {
        Player player = FindObjectOfType<Player>();

        FindObjectOfType<FollowCamera>().Target = player.transform.GetChild(0);//타겟을 플레이어로 조정
        FindObjectOfType<FollowCamera>().Offset = new Vector3(0, 3, -3);//오프셋을 플레이어위치로 조정
        JoyStickManager.Instance.CanvasAble();//조이스틱활성화
        CCTV.SetActive(false);//씨씨티비 비활성화
        EXIT.SetActive(false);//나가기버튼 비활성화
        COINPANEL.SetActive(false);//코인패널 비활성화
        atmtext.text = "김깍둑 캐피탈";//atm화면에 출력할글자변경
    }
    public void SaveATMcoin()//atm코인정보를 저장하는 함수
    {
        saveATM save = new saveATM();//saveATM클래스를 생성하여 변수에할당
        save.ATMmoney = ATM;//atmmoney변수에 atm잔액저장
        string json = JsonUtility.ToJson(save);//저장된내용을 텍스트로 저장

        File.WriteAllText(Application.persistentDataPath + "/ATMdata.json", json);//atmdata.json텍스트파일생성
    }
    public void loadATMcoin()//atm코인정보를 불러올 함수
    {
        try
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/ATMdata.json");//생성된 atmdata.json텍스트파일을 읽어온다
            saveATM load = new saveATM();//saveATM클래스를 생성하여 변수에할당
            load = JsonUtility.FromJson<saveATM>(json);//저장된내용을 로드에 불러옴

            ATM = load.ATMmoney;//atm잔액에 저장된atm잔액을 대입
        }
        catch { }
    }
}
