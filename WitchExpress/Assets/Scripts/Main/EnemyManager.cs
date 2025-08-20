using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // 게임 시작 후 첫 몬스터가 나오기까지의 시간 (인스펙터에서 조절 가능)
    public float opening; // 3f
    public float SecondStage = 3f;

    // 적 스폰 최소시간
    public float minTime;
    // 적 스폰 최대시간
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
        //createEnemyTime = 3; 

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

        // 코루틴 시작
        StartCoroutine(SpawnEnemiesRoutine());
    }

    // 코루틴: 1분마다 몬스터 스폰 패턴을 변경하며 적을 스폰합니다.
    IEnumerator SpawnEnemiesRoutine()
    {
        // 게임 시작 후 지정된 시간(opening)만큼 기다립니다.
        yield return new WaitForSeconds(opening);


        // 무한 반복 루프: 게임 내내 패턴을 반복합니다.
        while (true)
        {
            // --- 40초 동안 첫 번째 몬스터만 스폰하는 구간 ---
            // 시간 타이머
            float patternChangeTimer = 0f;

            // 현재 스폰할 적의 인덱스 (enemyFactory의 순서)
            int currentEnemyIndex = 0;

            while (patternChangeTimer < 10f)     // 40
            {
                // 랜덤한 시간만큼 기다린 후 적을 스폰합니다.
                float spawnInterval = UnityEngine.Random.Range(minTime, maxTime);
                yield return new WaitForSeconds(spawnInterval);

                // 경과 시간을 누적합니다.
                patternChangeTimer += spawnInterval;

                // 적을 풀에서 꺼내 스폰하는 함수를 호출
                SpawnEnemyFromPool(currentEnemyIndex);
            }

            Debug.Log($"{currentEnemyIndex + 1}번째 몬스터 패턴으로 변경되었습니다.");


            // --- 3초 대기 후, 40초 동안 첫 번째, 두 번째 몬스터가 섞여서 스폰되는 구간 ---
            yield return new WaitForSeconds(SecondStage);
            patternChangeTimer = 0f;

            // 40초 동안 현재 인덱스에 해당하는 적을 계속 스폰
            while (patternChangeTimer < 20f)
            {
                // 랜덤한 시간만큼 기다린 후 적을 스폰합니다.
                float spawnInterval = UnityEngine.Random.Range(minTime, maxTime);
                yield return new WaitForSeconds(spawnInterval);

                // 경과 시간을 누적합니다.
                patternChangeTimer += spawnInterval;

                // 몬스터 종류를 랜덤으로 결정
                int enemyNum = 0;
                // enemyFactory에 최소 2개의 몬스터가 있는지 확인
                if (enemyFactory.Length > 1)
                {
                    // 0~10 사이의 랜덤 숫자를 뽑습니다.
                    float randomChance = UnityEngine.Random.Range(0f, 10f);

                    if (randomChance < 2f)
                    {
                        // 20% 확률로 2번째 몬스터 스폰 (인덱스 1)
                        enemyNum = 1;
                    }
                    else
                    {
                        // 70% 확률로 1번째 몬스터 스폰 (인덱스 0)
                        enemyNum = 0;
                    }
                }

                    // 적을 풀에서 꺼내 스폰하는 함수를 호출
                    SpawnEnemyFromPool(enemyNum);
            }
            Debug.Log($"모든 몬스터 패턴이 종료되었습니다.");

        }
    }

    // 적을 풀에서 꺼내 스폰(생성)하는 함수 
    void SpawnEnemyFromPool(int enemyNum)
    {
        //에너미풀 안에 있는 에너미들 중에서
        if (enemyObjectPool[enemyNum].Count > 0)
        {
            //비활성화 된(발사되지 않은) 첫번째 적을
            GameObject enemy = enemyObjectPool[enemyNum][0];

            // -3~3사이 x축 값으로 랜덤하게 에너미 배치
            Vector3 spawnPosition = new Vector3(
            Random.Range(-spawnValues.x, spawnValues.x),
            spawnValues.y,
            spawnValues.z);
            enemy.transform.position = spawnPosition;

            //오브젝트 풀 리스트에서 적 제거
            enemyObjectPool[enemyNum].RemoveAt(0);

            // 활성화 시키고
            enemy.SetActive(true);
        }
    }
}
