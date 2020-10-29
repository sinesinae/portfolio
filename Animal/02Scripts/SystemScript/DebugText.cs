using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 모바일상에서 디버그 내용을 출력하기 위한 클래스
/// </summary>
public class DebugText : MonoBehaviour
{
    public static DebugText instance;
    string _text;
    int lineNum;
    RectTransform thisRt;
    public RectTransform parentRt;

    public string text
    {
        get { return _text; }
        set
        {
            _text = value;
            SetText();
        }
    }
    Text target;

    private void Awake()
    {
        instance = this;
        target = GetComponent<Text>();
        thisRt = GetComponent<RectTransform>();
    }

    void SetText()
    {
        target.text += _text + "\n";
        lineNum++;
        thisRt.sizeDelta = new Vector2(492, 33.5f * lineNum);
        parentRt.sizeDelta = new Vector2(492, 33.5f * lineNum);
    }
}
