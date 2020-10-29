using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 텐트 설치시 텐트 설치 위치를 표시해주는 클래스
/// </summary>
public class BuildingGridCanvasCtrl : MonoBehaviour
{
    public static BuildingGridCanvasCtrl instance { get; set; }

    public Color permit = new Color(0.372549f, 1, 0, 0.5f);
    public Color notpermit = new Color(1, 0, 0, 0.5f);
    GameObject grid;
    MapManager mm;
    int size;
    public List<GameObject> child = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        grid = Resources.Load("Prefabs/buildings/BuildGridPixel", typeof(GameObject)) as GameObject;
        mm = FindObjectOfType<MapManager>();
        size = mm.size;
    }

    public void DrawGrid(Vector3 playerPos, int x_pixels, int z_pixels)
    {
        Vector3 playerPosInt = new Vector3(Mathf.RoundToInt(playerPos.x), Mathf.RoundToInt(playerPos.y), Mathf.RoundToInt(playerPos.z));
        Vector3 origin = playerPosInt + new Vector3(-x_pixels / 2, 0, 1);
        // 현재 플레이어가 위치한곳위쪽으로 그리드 기준을 잡음
        GetComponent<RectTransform>().position = origin;

        for (int z = 0; z < z_pixels; z++)
        {
            for (int x = 0; x < x_pixels; x++)
            {
                GameObject gridpix = Instantiate(grid, transform);
                child.Add(gridpix);
                Vector3 setPos = new Vector3((int)origin.x + x, (int)origin.y, (int)origin.z + z);
                // 입력받은 x, z값만큼 그리드를 생성해 위치시킴
                gridpix.GetComponent<RectTransform>().position = setPos;


                if (IsPermissionBuildingOnThisGrid(setPos))
                {   // 해당 그리드가 위치한곳이 건물을 지을수 있는곳인지 확인해 색을 다르게 표시함
                    gridpix.GetComponent<Image>().color = permit;
                }
                else
                {
                    gridpix.GetComponent<Image>().color = notpermit;
                }
            }
        }
    }

    bool IsPermissionBuildingOnThisGrid(Vector3 CheckPos)
    {   
        int x = (int)CheckPos.x;
        int y = (int)CheckPos.y;
        int z = (int)CheckPos.z;
        //해당 x,z좌표에 위치한 것을 확인하고
        //해당 좌표의 y축 아래칸과 위칸을 확인함
        string thisPos = mm.MapDataStr[x + (z * size) + (y * size * size)];
        string belowPos = mm.MapDataStr[x + (z * size) + ((y - 1) * size * size)];
        string upperPos = mm.MapDataStr[x + (z * size) + ((y + 1) * size * size)];



        if (belowPos != "01") // 해당위치의 아래쪽이 땅이 아니면 설치불가
            return false;
        else
        {
            if (thisPos == "" && upperPos == "") // 해당위치와 y축 위쪽칸이 비어있으면 설치가능
                return true;
        }

        return false; // 그 이외에는 설치불가
    }
}
