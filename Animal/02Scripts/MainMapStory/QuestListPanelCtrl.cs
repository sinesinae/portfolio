using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 퀘스트 목록을 담당하는 클래스
/// </summary>
public class QuestListPanelCtrl : MonoBehaviour
{
    public Text QuestName { get; private set; }
    Text QuestDescription;
    Image QuestItem;
    Sprite defaultSprite;
    object QuestObject;
    MethodInfo Questmethod;

    public bool isClick { get; private set; }

    private void OnEnable()
    {   // 필요한 컴포넌트를 찾는다.
        QuestName = transform.GetChild(0).transform.GetComponent<Text>();
        QuestDescription = transform.GetChild(1).transform.GetComponent<Text>();
        QuestItem = transform.GetChild(2).transform.GetComponent<Image>();
        defaultSprite = QuestItem.sprite;
    }

    // 해당 리스트에 퀘스트 정보를 세팅한다
    public void SetQuestList(Quest quest)
    {
        QuestName.text = quest.QuestName;
        QuestDescription.text = quest.QuestDescription;
        if (quest.QuestItemSpritePath != "")
        {
            QuestItem.sprite = Resources.Load(quest.QuestItemSpritePath, typeof(Sprite)) as Sprite;
        }
        if (quest.QuestClassName != "")
        {
            Type type = Type.GetType(quest.QuestClassName);
            QuestObject = FindObjectOfType(type);
            Questmethod = type.GetMethod(quest.QuestMethodName);
            
            isClick = true;
        }
    }

    // 퀘스트 목록을 클릭하면 실행되는 함수
    public void OnClickQuestList()
    {
        if (!isClick)
            return;
        
            Questmethod.Invoke(QuestObject, null);
        
    }
}
