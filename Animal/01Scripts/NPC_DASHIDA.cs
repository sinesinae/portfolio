using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Reflection;

[Serializable]
public class ActObject
{
    public string className;
    public string methodName;
}
/// <summary>
/// 다시다가 실행할 함수목록을 만들고 다시다에게 접근하면 함수를 순차적으로 실행함
/// </summary>
public class NPC_DASHIDA : MonoBehaviour
{
    public Queue<Action> actQueue = new Queue<Action>();
    public List<ActObject> actobject = new List<ActObject>();//제이슨으로 저장할수있는값이 스트링이라서 액트오브젝트 클래스를 따로선언함 

    private void Start() 
    {
        LoadObject();//가지고있던 제이슨파일의 퀘스트 목록(액트오브젝트)을 가져옴
        ResetQueue();//가지고있던 리스트(액트오브젝트)를 액션형태의 큐로 저장함(변환)
    }

    public void RunAction()//다시다에게 말을 걸었을때 실행되는 함수
    {
        if (actQueue.Count <= 0)
            return;

        actQueue.Dequeue()();//큐목록에있는 액션을 하나실행하고 삭제
        actobject.RemoveAt(0);//큐목록과 동기화하기위해 하나삭제
        JsonData questData = JsonMapper.ToJson(actobject);
        File.WriteAllText(Application.persistentDataPath + "/actqueue.json", questData.ToString());
    }

    public void AddObject(string className, string questName)
        //새로운 액트오브젝트를 생성하고 기존액트오브젝트의 리스트에 추가하고 제이슨으로 저장
        
    {
        for (int i = 0; i < actobject.Count; i++)
            if (actobject[i].className == className && actobject[i].methodName == questName)
                return;

        ActObject aobj = new ActObject();
        aobj.className = className;
        aobj.methodName = questName;

        actobject.Add(aobj);
        JsonData questData = JsonMapper.ToJson(actobject);
        File.WriteAllText(Application.persistentDataPath + "/actqueue.json", questData.ToString());

        ResetQueue();
    }

    void LoadObject()//액트오브젝트를 제이슨파싱하여 불러오는함수
    {
        if (!File.Exists(Application.persistentDataPath + "/actqueue.json"))
            return;
        string json = File.ReadAllText(Application.persistentDataPath + "/actqueue.json");
        actobject = JsonMapper.ToObject<List<ActObject>>(json);
    }

    void ResetQueue()//큐목록을 불러오는함수
    {
        actQueue.Clear();
        foreach(ActObject ao in actobject)//액트오브젝트를 한바퀴다돌겠다.
            EnqueueAction(ao);
    }

    void EnqueueAction(ActObject aobj)//엑트오브젝트형자료를 액션큐로변환하는 함수
    {
        Type type = Type.GetType(aobj.className);
        object obj = FindObjectOfType(type);
        MethodInfo method = type.GetMethod(aobj.methodName);

        actQueue.Enqueue(() => method.Invoke(obj, null));


        
    }

}