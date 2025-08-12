using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // 몬스터 생성 시간
    float createEnemyTime;
    // 현재시간
    float currentTime;
    // 최소시간
    public float minTime;
    // 최대시간
    public float maxTime;

    // 적 공장
    public GameObject[] enemyFactory;

    // 오브젝트 풀 크기
    public int poolSize; 

    //오브젝트 풀 리스트 배열
    public List<GameObject>[] enemyObjectPool;

    // 적 스폰 위치
    public Vector3 spawnValues;

    private void Start()
    {
        // 최초 적 생성 시간
        createEnemyTime = 3; 

        //오브젝트풀 리스트를 에너미를 담을 수 있는 크기의 배열로 만들어준다.
        enemyObjectPool = new List<GameObject>[enemyFactory.Length];

        //오브젝트 풀에 넣을 에너미 개수 만큼 반복하여
        for (int i = 0; i < enemyFactory.Length; i++)
        {
            // 각각의 에너미 오브젝트 리스트를 생성
            enemyObjectPool[i] = new List<GameObject>();

            for (int j = 0; j < poolSize; j++)
            {
                //에너미 공장에서 에너미를 생성한다.
                GameObject enemy = Instantiate(enemyFactory[i]);
                // 에너미를 오브젝트 풀 리스트에 추가 한다.
                enemyObjectPool[i].Add(enemy);
                // 오브젝트를 비활성화 시킨다.
                enemy.SetActive(false);
            }
        }
    }

    void Update()
    {
        // 1. 시간이 흐르다가
        currentTime += Time.deltaTime;

        // 2. 만약 현재시간이 일정시간이 되면
        if (currentTime > createEnemyTime)
        {       
            int enemyNum = Random.Range(0, enemyFactory.Length);
            List<GameObject> enemyNumPool = enemyObjectPool[enemyNum];

            //에너미풀 안에 있는 에너미들 중에서
            if (enemyNumPool.Count > 0)
            {
                //비활성화 된(발사되지 않은) 첫번째 적을
                GameObject enemy = enemyNumPool[0];

                 // -3~3사이 x축 값으로 랜덤하게 에너미 배치
                Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnValues.x, spawnValues.x),
                spawnValues.y,
                spawnValues.z);
                enemy.transform.position = spawnPosition;

                 //오브젝트 풀 리스트에서 적 제거
                 enemyNumPool.RemoveAt(0);

                // 활성화 시키고
                enemy.SetActive(true);
            }

            // 현재시간을 0으로 초기화
            currentTime = 0;
            // 적을 생성한 후 적 생성시간을 다시 설정하고 싶다.
            createEnemyTime = UnityEngine.Random.Range(minTime, maxTime);
        }
    }
}
