using UnityEngine;

public class SleepManager : MonoBehaviour
{
    public GameObject panel;


    public void sleep()//대출갚기 퀘스트를 완료했으면 잠을잘수있게하고 아니면 못자게하는 함수 
    {
        Debug.Log(TimeManager.instance.day);

        ChasingAtNight chasingAtNight = FindObjectOfType<ChasingAtNight>();
        if (chasingAtNight.notToNight)//퀘스트 완료시
        {
            Ctrl ctrl = FindObjectOfType<Ctrl>();

            TimeManager.instance.SetTime(TimeManager.onedayTime * 4 / 24);//다음날 4시로 진행
            panel.SetActive(false);//패널 끔
            ctrl.transform.Rotate(270, 180, 0);
            ctrl.transform.position = new Vector3(-300f, 1, -300f);
            Comment.instance.CommentPrint("푹잤더니 개운한걸");
        }
        else//아니면 대화창 실행
        {
            panel.SetActive(false);
            Comment.instance.CommentPrint("돈도 못갚은 주제에 잠이오냐?!!");
        }
    }
    public void dontsleep()//안잔다고하면 패널 끔
    {
        panel.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)//침대에 접근하면
    {
        Player player = other.GetComponent<Player>();//플레이어가 널이아닐때
        if (player != null)
        {
            panel.SetActive(true);//패널보여줌
        }
    }
    private void OnTriggerExit(Collider other)//침대에서 떨어지면 패널끔
    {
        dontsleep();
    }
}
