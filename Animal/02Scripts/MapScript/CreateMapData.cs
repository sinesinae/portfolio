using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 맵 데이터 직렬화를 위한 클래스
/// </summary>
[Serializable]
public class MapDataJson
{
    public string[] data;
}

/// <summary>
/// 맵 데이터를 생성, 저장, 불러오기 위한 클래스
/// </summary>
public class CreateMapData : MonoBehaviour
{

    public int size = 100;  // 맵의 한변의 크기
    public string[] MapDataStr; // 맵 정보를 담는 배열
    public MinimapCtrl minimap; // 미니맵 컨트롤러 연결
    public MapManager mapmanager;   // 맵 매니저 연결
    public bool saveDone;   // 맵 저장이 완료 됐는지 확인하는 bool값
    Vector2 startPoint = new Vector2(85 ,15);   // 시작 위치 값
    List<int> constLand_x = new List<int>();    // 반드시 육지로 생성될 x좌표
    List<int> constLand_z = new List<int>();    // 반드시 육지로 생성될 z좌표
    public int grasscount 
    { 
        get { return PlayerPrefs.GetInt("GRASSCOUNT", 0); } 
        set { PlayerPrefs.SetInt("GRASSCOUNT", value); } 
    }
   

    // 새로운 맵 데이터를 생성하여 맵 데이터를 반환하는 함수
    public CreateMapData CreateNewMapData(string name)
    {
        GameObject temp = new GameObject(name);
        CreateMapData CM = temp.AddComponent<CreateMapData>();
        CM.minimap = FindObjectOfType<MinimapCtrl>();
        CM.mapmanager = FindObjectOfType<MapManager>();
        CM.CreateTerrain();
        CM.Fill_Map();
        CM.TreeGrow();
        return CM;
    }

    // 맵 데이터를 불러오는 함수
    public void LodingMapData()
    {
        if (File.Exists(Application.persistentDataPath + "/datajson.json"))
        {
            string loadjson = File.ReadAllText(Application.persistentDataPath + "/datajson.json");
            MapDataJson mapdata = new MapDataJson();
            mapdata = JsonUtility.FromJson<MapDataJson>(loadjson);
            MapDataStr = mapdata.data;
        }

        
        minimap.ShowMiniMap(MapDataStr,0,0,1);
        CallCreateMap();
        //SaveStrData(MapdataToString());
    }

    // 확률적으로 땅이 될 지점에 땅 데이터("01")를 기록하는 함수
    void CreateTerrain()
    {
        MapDataStr = new string[size* size* 3];
        for (int q = -6; q < 5; q++)
        {
            constLand_x.Add((int)startPoint.x + q);
            constLand_z.Add((int)startPoint.y + q);
        }

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                MapDataStr[i + (j * size) + 0] = "00";

                if (j > 10 && j < size - 10 && i > 10 && i < size - 10)
                {
                    

                    float temp = Random.Range(0, 100);

                    if (constLand_x.Contains(i) && constLand_z.Contains(j))
                    {
                        MapDataStr[i + (j * size) + 0] = "09";

                    }
                    else if (temp < 38)
                    {
                        MapDataStr[i + (j * size) + 0] = "01";
                    }
                }
            }
        }
    }
    
    // 육지를 매꿔주는 함수
    // 조건을 10번 반복하여 충분히 메꿔지게 함
    void Fill_Map()
    {
        for (int i = 0; i < 10; i++)
        {
            Check_Map();
        }
        Delete_Map();
    }

    // 맵 한칸한칸에 대해 육지가 될수 있는 조건인지 체크함
    void Check_Map()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Check_Eight(i, j);
            }
        }
    }

    // 바다의 8방향을 체크하여
    // 5칸 이상이 육지인 곳이라면 육지로 변경함
    void Check_Eight(int i, int j)
    {
        if (MapDataStr[i + (j * size) + 0] != "00")
            return;

        int count = 0;
        if (i < size - 1 && j < size - 1)
            if (MapDataStr[(i+1) + ((j+1) * size) + 0] != "00")
                count++;

        if (i < size - 1)
            if (MapDataStr[(i+1) + (j * size) + 0] != "00")
                count++;

        if (i < size - 1 && j > 0)
            if (MapDataStr[(i+1) + ((j-1) * size) + 0] != "00")
                count++;

        if (j < size - 1)
            if (MapDataStr[i + ((j+1) * size) + 0] != "00")
                count++;

        if (j > 0)
            if (MapDataStr[i + ((j-1) * size) + 0] != "00")
                count++;

        if (i > 0 && j < size - 1)
            if (MapDataStr[(i-1) + ((j+1) * size) + 0] != "00")
                count++;

        if (i > 0)
            if (MapDataStr[(i-1) + (j * size) + 0] != "00")
                count++;

        if (i > 0 && j > 0)
            if (MapDataStr[(i-1) + ((j-1) * size) + 0] != "00")
                count++;

        if (count > 4)
        {
            MapDataStr[i + (j * size) + 0] = "01";
        }
    }

    // 홀로 동떨어진 육지 타일을 제거하는 함수
    // 열번 반복하여 충분히 삭제 시킨다
    void Delete_Map()
    {
        for (int i = 0; i < 10; i++)
        {
            Check_Delete();
        }
    }

    // 육지 한칸 한칸에 조건 검사를 실행함
    void Check_Delete()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Check_Four(i, j);
            }
        }
    }

    // 육지의 주변 네방향을 체크하여
    // 3칸 이상이 바다이면 육지를 바다로 변경함
    void Check_Four(int i, int j)
    {
        if (MapDataStr[i + (j * size) + 0] == null)
            return;

        int count = 0;

        if (i < size - 1)
            if (MapDataStr[(i+1) + (j * size) + 0] == "00")
                count++;

        if (j < size - 1)
            if (MapDataStr[i + ((j+1) * size) + 0] == "00")
                count++;

        if (i > 0)
            if (MapDataStr[(i-1) + (j * size) + 0] == "00")
                count++;

        if (j > 0)
            if (MapDataStr[i + ((j-1) * size) + 0] == "00")
                count++;

        if (count > 2)
        {
            MapDataStr[i + (j * size) + 0] = "00";
        }
    }

    // 나무와 꽃을 랜덤한 위치에 배치시키는 함수
    // 스타트 포인트에 항구의 위치도 기록한다
    void TreeGrow()
    {
        for (int i = 1; i < size; i++)
        {
            for (int j = 1; j < size; j++)
            {
                if (constLand_x.Contains(i) && constLand_z.Contains(j))
                    continue;
                if (MapDataStr[i + (j * size) + 0] != "00")
                {
                    int rand = Random.Range(0, 100);
                    if (rand < 2)
                    {
                        //나무값
                        MapDataStr[i + (j * size) + 10000] = "11";
                    }
                    else if (rand > 2 && rand < 7)
                    {
                        //꽃,풀
                        int idx = Random.Range(0, 4);
                        switch (idx)
                        {
                            case 0:
                                MapDataStr[i + (j * size) + 10000] = "21";
                                break;
                            case 1:
                                MapDataStr[i + (j * size) + 10000] = "22";
                                break;
                            case 2:
                                MapDataStr[i + (j * size) + 10000] = "23";
                                break;
                            case 3:
                                MapDataStr[i + (j * size) + 10000] = "24";
                                break;
                        }
                        grasscount++;
                    }
                    MapDataStr[(int)startPoint.x+ ((int)startPoint.y*size)+ (1*size*size)] = "99";
                }
            }
        }
    }

    // 게임 중간중간 풀들을 증가시켜주는 함수
    public void TreeGrowPerDay()
    {
        
        if (grasscount > 1000)
            return;

        for (int q = -6; q < 5; q++)
        {
            constLand_x.Add((int)startPoint.x + q);
            constLand_z.Add((int)startPoint.y + q);
        }
        
        for (int i = 1; i < size; i++)
        {
            for (int j = 1; j < size; j++)
            {
                if (constLand_x.Contains(i) && constLand_z.Contains(j))
                    continue;
                if (MapDataStr[i + (j * size) + 0] != "00")
                {
                    int rand = Random.Range(0, 100);
                   
                    if (rand > 2 && rand < 5)
                    {
                        //꽃,풀
                        int idx = Random.Range(0, 4);
                        switch (idx)
                        {
                            case 0:
                                MapDataStr[i + (j * size) + 10000] = "21";
                                break;
                            case 1:
                                MapDataStr[i + (j * size) + 10000] = "22";
                                break;
                            case 2:
                                MapDataStr[i + (j * size) + 10000] = "23";
                                break;
                            case 3:
                                MapDataStr[i + (j * size) + 10000] = "24";
                                break;
                        }
                        grasscount++;
                    }
                }
            }
        }
        SaveDataJson();
    }

    // 맵 데이터를 json형식으로 저장하는 함수
    public void SaveDataJson()
    {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();
        MapDataJson bbbb = new MapDataJson();

        bbbb.data = MapDataStr;


        string json = JsonUtility.ToJson(bbbb);
        File.WriteAllText(Application.persistentDataPath + "/datajson.json", json);
        
        saveDone = true;
        //sw.Stop();
        //UnityEngine.Debug.Log("저장시간 " + sw.ElapsedMilliseconds.ToString());
    }
    

    // 맵 데이터를 기반으로 실제 맵을 인스턴스화 하는 함수
    void CallCreateMap()
    {
        mapmanager.MapDataStr = new string[size * size * 3];
        mapmanager.MapDataStr = MapDataStr;
        mapmanager.size = size;
        mapmanager.CreateMap();
    }
}