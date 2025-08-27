using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("스테이지 진행 시간 설정")]
    // 스테이지 당 몬스터 등장 시간 (30)
    public float stageTime;
    // 2탄 스테이지 몹 나오기까지의 대기시간 
    public float stageDelayTime = 5f;
    // 보스 스테이지 시작 대기 시간
    public float bossDelayTime = 5f;
    public float bossStageTime;

    [Header("적 스폰 설정")]
    // 적 스폰 최소시간
    public float minTime;
    // 적 스폰 최대시간
    public float maxTime;
    // 적 스폰 위치
    public Vector3 spawnValues;

    [Header("오브젝트 풀 설정")]
    // 적 공장
    public GameObject[] enemyFactory;
    // 오브젝트 풀 크기
    public int poolSize; 
    //오브젝트 풀 리스트 배열
    public List<GameObject>[] enemyObjectPool;

    public GameObject boss;
    private PlayerMove playerMove;


    private void Update()
    {
        // 게임 상태가 ‘게임 중’ 상태일 때만 조작할 수 있게 한다.
        if (GameManager.Instance.gState != GameManager.GameState.Run)
        {
            return;
        }
    }

    private void Start()
    {
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
                // 오브젝트 비활성화 시킨다.
                enemy.SetActive(false);
                
                // 보스 몹 비활성화 
                boss.SetActive(false);

            }
        }

        playerMove = FindObjectOfType<PlayerMove>();

        // 코루틴 시작
        StartCoroutine(SpawnEnemiesRoutine());
    }


    // 코루틴: 1분마다 몬스터 스폰 패턴을 변경하며 적을 스폰합니다.
    IEnumerator SpawnEnemiesRoutine()
    {

        // 무한 반복 루프: 게임 내내 패턴을 반복합니다.
        while (true)
        {
            // 게임 상태가 'Run'이 아닐 경우, 다음 프레임을 기다립니다.
            // 게임 상태가 'Run'이 될 때까지 이 루프를 계속 반복합니다.
            while (GameManager.Instance.gState != GameManager.GameState.Run)
            {
                yield return null;
            }

            Debug.Log($"몬스터가 등장합니다.");

            // --- 첫 번째 몬스터만 스폰되는 구간 ---
            // 시간 타이머
            float patternChangeTimer = 0f;

            // 현재 스폰할 적의 인덱스 (enemyFactory의 순서)
            int currentEnemyIndex = 0;

            while (patternChangeTimer < stageTime)     
            {
                // 랜덤한 시간만큼 기다린 후 적을 스폰합니다.
                float spawnInterval = UnityEngine.Random.Range(minTime, maxTime);
                yield return new WaitForSeconds(spawnInterval);

                // 경과 시간을 누적합니다.
                patternChangeTimer += spawnInterval;

                // 적을 풀에서 꺼내 스폰하는 함수를 호출
                SpawnEnemyFromPool(currentEnemyIndex);
            }

            Debug.Log($"스테이지 2가 시작됩니다.");

            //  --- 대기 후, 두 번째 몬스터가 섞여 스폰되는 구간 ---
            yield return new WaitForSeconds(stageDelayTime);
            patternChangeTimer = 0f;

            // 현재 인덱스에 해당하는 적을 계속 스폰
            while (patternChangeTimer < stageTime)
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

            //  ---대기 후, 보스 몬스터 등장 두둥 ---
            Debug.Log($"보스 등장!");
            boss.SetActive(true);
            playerMove.BossOpening();

            yield return new WaitForSeconds(stageDelayTime);
            patternChangeTimer = 0f;

            //while (patternChangeTimer < bossStageTime)
            //{

            //}
            
            Debug.Log($"모든 몬스터 패턴이 종료되었습니다.");

            GameManager.Instance.gState = GameManager.GameState.Ready;

            if (GameManager.Instance.gState != GameManager.GameState.Run)
            {
                // break를 사용해 최상위 while(true) 루프로 돌아갑니다.
                break;
            }
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
