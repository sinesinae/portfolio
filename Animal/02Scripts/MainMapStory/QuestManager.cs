using LitJson;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Quest
{
    public string QuestName;
    public string QuestDescription;
    public string QuestItemSpritePath;
    public string QuestClassName;
    public string QuestMethodName;
}



public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public List<Quest> questList = new List<Quest>();
    public GameObject questView;
    public GameObject parent;
    public GameObject listPrefab;
    public RawImage questIcon;
    public Texture[] textures;


    private void Awake()
    {
        instance = this;
        LoadQuest();

        // 아래는 퀘스트 클래스 사용법이다.
        //AddQuest(퀘스트이름, 퀘스트설명, 아이콘리소스경로, 퀘스트함수가포함된클래스이름, 퀘스트함수이름)

    }

    // 퀘스트 이름과 설명 아이콘 리스스 위치퀘스트 클릭했을때 발생하는 이벤트까지 등록하는 함수
    //AddQuest(퀘스트이름, 퀘스트설명, 아이콘리소스경로, 퀘스트함수가포함된클래스이름, 퀘스트함수이름)
    public void AddQuest(string QuestName, string QuestDescription, string QuestItemSprite = "", string questClassName = "", string questMethodName = "")
    {
        for (int i = 0; i < questList.Count; i++)
            if (questList[i].QuestName == QuestName)
                return;
        questList.Add(new Quest { QuestName = QuestName, QuestDescription = QuestDescription, QuestItemSpritePath = QuestItemSprite, QuestClassName = questClassName, QuestMethodName = questMethodName});
        questIcon.texture = textures[1];
        SaveQuest();
    }

    // 퀘스트를 목록에서 제거한다. 퀘스트 이름만 입력해주면 된다
    public void RemoveQueset(string QuestName)
    {
        for (int i = 0; i < questList.Count; i++)
        {
            if (questList[i].QuestName == QuestName)
                questList.Remove(questList[i]);
        }
        SaveQuest();
    }

    public void SaveQuest()
    {
        JsonData questData = JsonMapper.ToJson(questList);
        File.WriteAllText(Application.persistentDataPath + "/questdata.json", questData.ToString());
        
    }

    public void LoadQuest()
    {
        if (File.Exists(Application.persistentDataPath + "/questdata.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/questdata.json");
            questList = JsonMapper.ToObject<List<Quest>>(json);
        }
    }

    // 퀘스트 버튼을 눌렀을때 목록을 펼쳐 보이는 함수이다.
    // 닫을때는 퀘스트 리스트를 제거 한다.
    public void questListView()
    {
        if (!questView.activeSelf)
        {
            questView.SetActive(true);
            parent.GetComponent<RectTransform>().sizeDelta = new Vector2(800, questList.Count * 110);


            for (int i = 0; i < questList.Count; i++)
            {
                GameObject inst = Instantiate(listPrefab, parent.transform);
                QuestListPanelCtrl qlpc = inst.GetComponent<QuestListPanelCtrl>();
                qlpc.SetQuestList(questList[i]);
            }
        }
        else
        {
            RectTransform[] children = parent.GetComponentsInChildren<RectTransform>();
            for(int i = 1; i<children.Length; i++)
            {
                Destroy(children[i].gameObject);
            }
            questView.SetActive(false);
        }
    }

    public void SetDefaultTexture()
    {
        questIcon.texture = textures[0];
    }
}
