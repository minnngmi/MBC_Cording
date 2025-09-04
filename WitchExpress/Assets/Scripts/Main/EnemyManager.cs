using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class EnemyManager : MonoBehaviour
{
    public GameObject opTxt;
    public AudioSource mainBGM01;

    [Header("스테이지 진행 시간 설정")]
    // 스테이지 당 몬스터 등장 시간 (30)
    public float stageTime;
    // 2탄 스테이지 몹 나오기까지의 대기시간 
    public float stageDelayTime = 5f;
    // 보스 스테이지 시작 대기 시간
    public float bossDelayTime;
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

    [Header("보스 설정")]
    public GameObject boss;
    public Slider bossHpSlider;
    private PlayerMove playerMove;
    public AudioSource bossLaughing;
    public AudioSource mainBGM02;   // 보스전 음악
    public MainBGM_FadeOut bgmFader; // 음악 fade 효과



    private void Start()
    {
        opTxt.SetActive(false);
        mainBGM01.Play();

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
                // 오브젝트 비활성화
                enemy.SetActive(false);
            }
        }
        // 보스 몹 비활성화 
        boss.SetActive(false);
        bossHpSlider.gameObject.SetActive(false);


        // 게임 흐름을 제어하는 메인 코루틴을 시작합니다.
        StartCoroutine(GameFlowRoutine());
    }

    //모든 코루틴을 순서대로 실행하는 메인 코루틴
    IEnumerator GameFlowRoutine()
    {
        // GameManager의 상태가 'Run'이 될 때까지 기다립니다.
        while (GameManager.Instance.gState != GameState.Run)
        {
            yield return null;
        }

        Debug.Log("게임 시작! 스테이지 1 몬스터 스폰 시작.");
        // 첫 번째 몬스터 스폰 코루틴이 끝날 때까지 기다립니다.
        yield return StartCoroutine(SpawnEnemiesRoutine());

        Debug.Log("스테이지 2 몬스터 스폰 시작.");
        opTxt.SetActive(true);
        // 두 번째 몬스터 스폰 코루틴이 끝날 때까지 기다립니다.
        yield return StartCoroutine(SpawnBooRoutine());

        Debug.Log("보스 스테이지 시작.");
        // 보스 스폰 코루틴이 끝날 때까지 기다립니다.
        yield return StartCoroutine(SpawnBossRoutine());


        Debug.Log("모든 스테이지 완료! 게임 종료.");
        // 모든 스테이지가 끝나면 게임 상태를 Ending으로 변경합니다.
        GameManager.Instance.gState = GameState.Ending;
    }

    //몬스터 등장 코루틴 (스테이지 1)
    IEnumerator SpawnEnemiesRoutine()
    {
        yield return new WaitForSeconds(stageDelayTime);
        // 시간 타이머
        float patternChangeTimer = 0f;
        // 현재 스폰할 적의 인덱스 (enemyFactory의 순서)
        int currentEnemyIndex = 0;

        while (patternChangeTimer < stageTime)
        {
            // 랜덤한 시간만큼 기다린 후 적을 스폰합니다.
            float spawnInterval = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(spawnInterval);
            // 경과 시간을 누적합니다.
            patternChangeTimer += spawnInterval;
            // 적을 풀에서 꺼내 스폰하는 함수를 호출
            SpawnEnemyFromPool(currentEnemyIndex);
        }
    }

    // 몬스터 등장 코루틴 (스테이지 2)
    IEnumerator SpawnBooRoutine()
    {
        yield return new WaitForSeconds(stageDelayTime);
        // 시간 타이머
        float patternChangeTimer = 0f;
        // 현재 인덱스에 해당하는 적을 계속 스폰
        while (patternChangeTimer < stageTime+5)
        {
            // 랜덤한 시간만큼 기다린 후 적을 스폰합니다
            float spawnInterval = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(spawnInterval);

            // 경과 시간을 누적합니다.
            patternChangeTimer += spawnInterval;
            int enemyNum = 0;

            // enemyFactory에 최소 2개의 몬스터가 있는지 확인
            if (enemyFactory.Length > 1)
            {
                // 몬스터 종류를 랜덤으로 결정
                // 0~10 사이의 랜덤 숫자를 뽑습니다.
                float randomChance = Random.Range(0f, 10f);
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
    }

    // 보스 등장 코루틴
    IEnumerator SpawnBossRoutine()
    {
        playerMove = FindObjectOfType<PlayerMove>();
        // 보스 등장 대기 시간
        yield return new WaitForSeconds(stageDelayTime-10);

        // 플레이어 캐릭터를 이동시키는 코루틴이 끝날 때까지 기다립니다.
        yield return StartCoroutine(playerMove.BossOpening());
        // BGM01 볼륨 페이드 아웃 메서드 호출
        bgmFader.FadeOutBGM(5f);

        Debug.Log($"보스 등장!");
        boss.SetActive(true);
        // (보스 애니메이션 작동중)
        yield return new WaitForSeconds(bossDelayTime);

        GameManager.Instance.gState = GameManager.GameState.Run;

        // 보스전 음악 재생
        mainBGM02.Play();

        // 보스 HP UI 활성화
        bossHpSlider.gameObject.SetActive(true);

        // 시간 타이머
        float patternChangeTimer = 0f;

        while (patternChangeTimer < bossStageTime)
        {

            
            // 여기에 보스의 공격 로직을 추가
            // 예를 들어, yield return new WaitForSeconds(1.0f);
            yield return null;
            patternChangeTimer += Time.deltaTime;
        }
        Debug.Log($"모든 몬스터 패턴이 종료되었습니다.");
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
