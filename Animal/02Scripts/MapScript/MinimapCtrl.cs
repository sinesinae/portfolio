using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 미니맵을 그리고 미니맵에 오브젝트 위치를 표시해주는 클래스
/// </summary>
public class MinimapCtrl : MonoBehaviour
{
    [Header("맵그리는부분")]
    public GameObject blueDot;  // 물위치의 파란색점
    public GameObject greenDot; // 육지 위치의 초록색점
    public Transform canvasParent;  // 물과 육지점의 하이어라키상의 부모객체

    [Header("플레이어위치표시")]
    public GameObject PlayerPos;    // 플레이어 위치를 표시해줄 점
    public Transform playerTr; // 플레이어 위치를 읽어올 플레이어 컨트롤러
    public RectTransform rt;   // 플레이어위치점의 렉트트랜스폼
    Vector3 origin; // rt의 초기 위치좌표
    public float default_x = -450;   // 미니맵 위치의 초기 보정값
    public float default_y = -550;  // 미니맵 위치의 초기 보정값

    [Header("미니맵위 오브젝트")]
    public RectTransform AirPortPos;    // 미니맵위의 항구 위치

    [HideInInspector] public bool TracePlayer;

    private void Start()
    {
        if (PlayerPos != null)
            rt = PlayerPos.GetComponent<RectTransform>();
        if (rt != null)
            origin = rt.position;

        try
        {
            playerTr = FindObjectOfType<Player>().transform;
            TracePlayer = true;
        }
        catch
        {

        }
    }


    public void Update()
    {
        //미니맵 위에서 플레이어 위치를 표시해줌
        float xratio = Screen.width / 2280.0f;
        float yratio = Screen.height / 1080.0f;
        if (rt != null && playerTr != null && TracePlayer)
            rt.anchoredPosition = new Vector3((playerTr.position.x * 3), (playerTr.position.z * 3), 1);
        else if(rt != null && playerTr != null && !TracePlayer)
            rt.position = FindObjectOfType<TentCtrl>().HomePos.position;
    }

    /// <summary>
    /// 미니맵을 캔버스에 그려주는 함수
    /// </summary>
    /// <param name="data">맵데이터</param>
    /// <param name="x">맵 x축 보정값</param>
    /// <param name="y">맵 y축 보정값</param>
    public void ShowMiniMap(string[] data, float x, float y, int condition, Image targetimg = null)
    {
        x += default_x;
        y += default_y;

        int width = 300;
        int height = 300;
        Texture2D newTex = new Texture2D(width, height);

        for (int j = 0; j < 100; j++)
        {
            for (int i = 0; i < 100; i++)
            {
                for (int k = 0; k < 3; k++)
                {
                    switch (data[i + (j * 100) + (k * 100 * 100)])
                    {
                        case "00":
                            //GameObject temp = Instantiate(blueDot, canvasParent);
                            //RectTransform rect = temp.GetComponent<RectTransform>();
                            //rect.position = new Vector3(rect.position.x + (i * 4) + x, rect.position.y + (j * 4) + y, rect.position.z);
                            _setPixel(newTex, i, j, Color.blue);
                            break;
                        case "01":
                            //GameObject temp1 = Instantiate(greenDot, canvasParent);
                            //RectTransform rect1 = temp1.GetComponent<RectTransform>();
                            //rect1.position = new Vector3(rect1.position.x + (i * 4) + x, rect1.position.y + (j * 4) + y, rect1.position.z);
                            _setPixel(newTex, i, j, Color.green);
                            break;
                        case "09":
                            //GameObject temp2 = Instantiate(greenDot, canvasParent);
                            //RectTransform rect2 = temp2.GetComponent<RectTransform>();
                            //rect2.position = new Vector3(rect2.position.x + (i * 4) + x, rect2.position.y + (j * 4) + y, rect2.position.z);
                            _setPixel(newTex, i, j, Color.green);
                            break;
                        case "99":
                            float xratio = Screen.width / 2280.0f;
                            float yratio = Screen.height / 1080.0f;
                            if (AirPortPos != null)
                                AirPortPos.anchoredPosition = new Vector3(((i * 3)) * 1, ((j * 3)) * 1, 1);
                            break;
                    }

                }
            }
        }
        newTex.Apply();
        Sprite sprite = Sprite.Create(newTex, new Rect(0, 0, newTex.width, newTex.height), new Vector2(0.5f, 0.5f));
        if (condition == 1)
            canvasParent.gameObject.GetComponent<Image>().sprite = sprite;
        else
        {
            targetimg.sprite = sprite;
        }

    }

    void _setPixel(Texture2D t2d, int x, int y, Color color)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                t2d.SetPixel(i + x * 3, j + y * 3, color);
            }
        }
    }
}