using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // 내비메쉬 관련 코드
public class MulgogiManager : MonoBehaviour
{
    public Mulgogi[] Mulgogis;//생성될 게임오브젝트
    public Transform[] spawnPoints;//물고기생성지점
    Lake[] lakes;//강배열
    Vector3 pos;//강의 위치


    public float Distance = 3f; // 생성지점으로 부터 물고기가 배치될 최대 반경

    public float SpawnMax = 300f; // 최대 시간 간격
    public float SpawnMin = 120f; // 최소 시간 간격

    private float Spawntime; // 생성 간격
    private float lastSpawnTime; // 마지막 생성 시점




    private void Start()
    {
        // 생성 간격과 마지막 생성 시점 초기화
        Spawntime = Random.Range(SpawnMin, SpawnMax);
        lastSpawnTime = 0;
        lakes = FindObjectsOfType<Lake>();

    }
    //강이면서 그라운드 스크립트를 가지고 있는친구랑 거리가 2이내면 물고기 스폰


    // 주기적으로 아이템 생성 처리 실행
    private void Update()
    {
        if (Time.time >= lastSpawnTime + Spawntime)
        {
            // 마지막 생성 시간 갱신
            lastSpawnTime = Time.time;
            // 생성 주기를 랜덤으로 변경
            Spawntime = Random.Range(SpawnMin, SpawnMax);
            // 아이템 생성 실행
            isspawn();
        }
    }

    // 실제 아이템 생성 처리

    public void isspawn()
    {
        RaycastHit hit;
        Ray ray = new Ray();
        int idx = Random.Range(0, lakes.Length);
        pos = lakes[idx].transform.position + new Vector3(0, 1, 0);
        ray.origin = pos;
        ray.direction = Vector3.forward;//z축방향으로 검사하겠다

        //동서남북검사


        if (Physics.Raycast(ray, out hit, Distance))
        {//Distance 2만큼만 쏘겠다
            Ground ground = hit.transform.GetComponent<Ground>();//그라운드를 가져오겠다.
            if (ground != null)
            {
                Spawn();
                return;
            }
        }
        ray.direction = Vector3.right;
        if (Physics.Raycast(ray, out hit, Distance))
        {//Distance 2만큼만 쏘겠다
            Ground ground = hit.transform.GetComponent<Ground>();//그라운드를 가져오겠다.

            if (ground != null)
            {
                Spawn();
                return;
            }
        }
        ray.direction = -Vector3.forward;
        if (Physics.Raycast(ray, out hit, Distance))
        {//Distance 2만큼만 쏘겠다
            Ground ground = hit.transform.GetComponent<Ground>();//그라운드를 가져오겠다.

            if (ground != null)
            {
                Spawn();
                return;
            }
        }
        ray.direction = -Vector3.right;
        if (Physics.Raycast(ray, out hit, Distance))
        {//Distance 2만큼만 쏘겠다
            Ground ground = hit.transform.GetComponent<Ground>();//그라운드를 가져오겠다.

            if (ground != null)
            {
                Spawn();
                return;
            }
        }
        StartCoroutine(isisspawn());
    }

    IEnumerator isisspawn()
    {
        yield return null;
        isspawn();
    }

    private void Spawn()
    {
        Item item;
        Mulgogi itemToCreate = MulgogiCtrl(out item);//Mulgogis[Random.Range(0, Mulgogis.Length)];

        // 네트워크의 모든 클라이언트에서 해당 아이템 생성

        Mulgogi createdMulgogi =
           Instantiate(itemToCreate, pos,
                Quaternion.identity);
        Mulgogi mulgogi = createdMulgogi.gameObject.AddComponent<Mulgogi>();
        mulgogi.thisMulgogi = item;

        // 생성한 아이템을 0.1초 뒤에 파괴
        //낚시에 실패하거나 성공했을때의 함수실행
        StartCoroutine(DestroyAfter(createdMulgogi, 0.1f));


    }

    // 포톤의 PhotonNetwork.Destroy()를 지연 실행하는 코루틴 
    IEnumerator DestroyAfter(Mulgogi target, float delay)
    {
        // delay 만큼 대기
        yield return new WaitForSeconds(delay);

        // target이 파괴되지 않았으면 파괴 실행
        if (target != null)
        {
            Destroy(target);
            lastSpawnTime = Time.time;//Time.time 현재까지 플레이한 게임시간
        }
    }

    // 네브 메시 위의 랜덤한 위치를 반환하는 메서드
    // center를 중심으로 distance 반경 안에서 랜덤한 위치를 찾는다.

    public Mulgogi MulgogiCtrl(out Item item)
    {
        int a = Random.Range(0, 10000);
        ItemDatabase iDB = FindObjectOfType<ItemDatabase>();
        if (a < 10)
        {
            item = iDB.itemList[40];
            return Mulgogis[3];


            //Mulgogis 프리팹배열 중에서 향어가 속할 프리팹을고른다
            //해당 프리팹을 소환한다
            //해당 프리팹에 있는 mulgogi스크립트에서 item으로 지정된 thismulgogi를 향어로 정해준다
            //itemdatabase에 있는 향어를 정해주면됨 thismulgogi=itemList[40]
        }
        else if (a < 100)
        {

            item = iDB.itemList[Random.Range(38, 40)];
            return Mulgogis[1];

        }
        else if (a < 1000)
        {
            item = iDB.itemList[Random.Range(36, 38)];
            return Mulgogis[5];
        }
        else if (a < 2500)
        {
            item = iDB.itemList[Random.Range(34, 36)];
            return Mulgogis[2];
        }
        else if (a < 6000)
        {
            item = iDB.itemList[Random.Range(31, 34)];
            return Mulgogis[0];

        }
        else
        {
            item = iDB.itemList[Random.Range(29, 31)];

            return Mulgogis[4];

        }
    }
}
