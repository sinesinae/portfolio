using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Xml.Serialization;
using System.IO;

[Serializable]
public class savedia//다이아정보를 저장할 클래스
{
    public int dia;//다이아 정보를 저장할 변수
}
public class Diamond_Manager : MonoBehaviour
{
    private int _Diamond;//다이아갯수
    public Text Diamond_Text;//다이아갯수를 ui에 보여지게할 텍스트


    [HideInInspector]
    public int Diamond//다이아저장프라퍼티
    {
        get { return _Diamond; }
        set
        {
            _Diamond = value;
            Diamond_Text.text = "x "+_Diamond.ToString();
            savedia();
        }
    }


    void Start()
    {
        loaddia();//저장된 다이아정보를 불러온다
    }

    public void adddia(int a)//다이아 더하기
    {

        Diamond += a;
    }
    //public void minusdia(int a)//다이아몬드 빼기
    //{
    //    if(Diamond<=0)//소지한다이아 갯수가 조건보다 작거나같으면 코르틴실행
    //    {
    //        Comment.instance.CommentPrint("다이아가 없어!!");
    //    }
    //    else//아니면 다이아 감소
    //    {
    //        Diamond -= a;
    //    }


    //}

    public void savedia()//다이아 저장 함수
    {
        savedia save = new savedia();//다이아정보를 저장할 다이아 변수생성 할당
        save.dia = Diamond;//다이아정보를 저장
        string json = JsonUtility.ToJson(save);//저장된 다이아정보를 스트링으로 저장


        File.WriteAllText(Application.persistentDataPath + "/Diadata.json", json);//다이아데이터 텍스트파일생성

    }
    public void loaddia()//저장된다이아정보를 불러오는 함수
    {
        try
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/Diadata.json");//다이아데이터 텍스트파일을 읽고 제이슨에 저장
            savedia load = new savedia();//로드변수에 클래스할당
            load = JsonUtility.FromJson<savedia>(json);//로드에 저장된 다이아정보를 저장
            Diamond = load.dia;//다이아에 다이아정보를 저장
        }
        catch { }
    }
}
