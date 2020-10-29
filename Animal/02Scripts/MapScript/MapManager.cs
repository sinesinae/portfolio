using System.Collections;
using UnityEngine;

/// <summary>
/// 맵을 생성하는데 필요한 자원들을 관리하는 클래스
/// </summary>
public class MapManager : MonoBehaviour
{
    public GameObject lakePrefab;   // 물타일 프리팹
    public GameObject landPrefab;   // 육지타일 프리팹
    public GameObject treePrefab;   // 나무 프리팹
    public GameObject[] flowerPrefabs;  // 꽃, 풀, 버섯, 나뭇가지 프리팹
    public GameObject portPrefabs;  // 항구 프리팹(시작위치)

    public int size;  // 맵 한변의 크기

    public string[] MapDataStr; // 맵 데이터 배열
    
    public delegate void MapCheck();    // 맵 체크 델리게이트 선언
    public event MapCheck GroundEndCheck;   // 맵체크 이벤트 선언

    public CreateMapData CMD;

    // 맵 데이터를 기반으로 실제 맵을 인스턴스화 하는 함수
    public void CreateMap()
    {
        for (int k = 0; k < 3; k++)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    switch (MapDataStr[i + (j * size) + (k * size * size)])
                    {
                        case "00":  // 바다
                            CreatePrefab(lakePrefab, new Vector3(i, k-0.5f, j));
                            break;
                        case "01":  // 땅
                            CreatePrefab(landPrefab, new Vector3(i, k, j));
                            break;
                        case "09":  // 땅이지만 다른것을 설치할수 없는 땅
                            CreatePrefab(landPrefab, new Vector3(i, k, j));
                            break;
                        case "11":  // 나무
                            GameObject tree = CreatePrefab(treePrefab, new Vector3(i, k, j));
                            tree.AddComponent<Wood>();
                            break;
                            
                        case "22":  // 꽃
                            CreatePrefab(flowerPrefabs[0], new Vector3(i, k, j), 10000022, 1);
                            break;
                        case "23":  // 버섯
                            CreatePrefab(flowerPrefabs[1], new Vector3(i, k, j), 10000023, 1);
                            break;
                        case "21":  // 잡초
                            CreatePrefab(flowerPrefabs[2], new Vector3(i, k, j), 10000021, 1);
                            break;
                        case "24":  // 나뭇가지
                            CreatePrefab(flowerPrefabs[3], new Vector3(i, k, j), 10000024, 1);
                            break;
                        case "99":  // 출입 항구
                            CreatePrefab(portPrefabs, new Vector3(i + 5.69f, k + 0.11f, j + 4.44f));
                            break;
                        case "98":  // 텐트 
                            GameObject tentPrefabs = Resources.Load("Prefabs/buildings/tent", typeof(GameObject)) as GameObject; // 텐트 프리팹(퀘스트 1_2)  
                            CreatePrefab(tentPrefabs, new Vector3(i, k, j));
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }


    //프리팹을 맵에 생성 시키는 함수
    public GameObject CreatePrefab(GameObject prefab, Vector3 position)
    {
        float x = position.x;
        float y = position.y;
        float z = position.z;

        GameObject temp = Instantiate(prefab, transform);
        temp.transform.position = new Vector3(x, y, z);
        return temp;
    }
    // 프리팹 생성 함수
    public void CreatePrefab(GameObject prefab, Vector3 position, int itemID, int itemCount = 1)
    {
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.y);
        int z = Mathf.RoundToInt(position.z); 

        GameObject temp = Instantiate(prefab, transform);
        temp.transform.position = new Vector3(x, y, z);
        ItemPickup iteminfo = temp.AddComponent<ItemPickup>();
        iteminfo.itemID = itemID;
        iteminfo._count = itemCount;
        iteminfo.x = x;
        iteminfo.y = y;
        iteminfo.z = z;
    }

    private void Start()
    {
        StartCoroutine(LockGround()); // 맵 끝에 콜라이더를 세우는 코루틴실행
    }

    private void Awake()
    {
        GetComponent<CreateMapData>().LodingMapData(); // 맵파일에서 맵데이터를 로딩함
        size = GetComponent<CreateMapData>().size; // 맵 사이즈를 읽어옴
    }


    // 메인 맵 로딩시 발생하는 렉 때문에 오류가 발생할수 있으므로
    // 몇 프레임의 시간이 지난후 이벤트 호출
    IEnumerator LockGround()
    {
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        GroundEndCheck();
    }
}
