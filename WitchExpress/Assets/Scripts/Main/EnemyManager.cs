using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("�������� ���� �ð� ����")]
    // �������� �� ���� ���� �ð� (30)
    public float stageTime;
    // 2ź �������� �� ����������� ���ð� 
    public float stageDelayTime = 5f;
    // ���� �������� ���� ��� �ð�
    public float bossDelayTime = 5f;
    public float bossStageTime;

    [Header("�� ���� ����")]
    // �� ���� �ּҽð�
    public float minTime;
    // �� ���� �ִ�ð�
    public float maxTime;
    // �� ���� ��ġ
    public Vector3 spawnValues;

    [Header("������Ʈ Ǯ ����")]
    // �� ����
    public GameObject[] enemyFactory;
    // ������Ʈ Ǯ ũ��
    public int poolSize; 
    //������Ʈ Ǯ ����Ʈ �迭
    public List<GameObject>[] enemyObjectPool;

    public GameObject boss;
    private PlayerMove playerMove;


    private void Update()
    {
        // ���� ���°� ������ �ߡ� ������ ���� ������ �� �ְ� �Ѵ�.
        if (GameManager.Instance.gState != GameManager.GameState.Run)
        {
            return;
        }
    }

    private void Start()
    {
        //������ƮǮ ����Ʈ�� ���ʹ̸� ���� �� �ִ� ũ���� �迭�� ������ش�.
        enemyObjectPool = new List<GameObject>[enemyFactory.Length];

        //������Ʈ Ǯ�� ���� ���ʹ� ���� ��ŭ �ݺ��Ͽ�
        for (int i = 0; i < enemyFactory.Length; i++)
        {
            // ������ ���ʹ� ������Ʈ ����Ʈ�� ����
            enemyObjectPool[i] = new List<GameObject>();

            for (int j = 0; j < poolSize; j++)
            {
                //���ʹ� ���忡�� ���ʹ̸� �����Ѵ�.
                GameObject enemy = Instantiate(enemyFactory[i]);
                // ���ʹ̸� ������Ʈ Ǯ ����Ʈ�� �߰� �Ѵ�.
                enemyObjectPool[i].Add(enemy);
                // ������Ʈ ��Ȱ��ȭ ��Ų��.
                enemy.SetActive(false);
                
                // ���� �� ��Ȱ��ȭ 
                boss.SetActive(false);

            }
        }

        playerMove = FindObjectOfType<PlayerMove>();

        // �ڷ�ƾ ����
        StartCoroutine(SpawnEnemiesRoutine());
    }


    // �ڷ�ƾ: 1�и��� ���� ���� ������ �����ϸ� ���� �����մϴ�.
    IEnumerator SpawnEnemiesRoutine()
    {

        // ���� �ݺ� ����: ���� ���� ������ �ݺ��մϴ�.
        while (true)
        {
            // ���� ���°� 'Run'�� �ƴ� ���, ���� �������� ��ٸ��ϴ�.
            // ���� ���°� 'Run'�� �� ������ �� ������ ��� �ݺ��մϴ�.
            while (GameManager.Instance.gState != GameManager.GameState.Run)
            {
                yield return null;
            }

            Debug.Log($"���Ͱ� �����մϴ�.");

            // --- ù ��° ���͸� �����Ǵ� ���� ---
            // �ð� Ÿ�̸�
            float patternChangeTimer = 0f;

            // ���� ������ ���� �ε��� (enemyFactory�� ����)
            int currentEnemyIndex = 0;

            while (patternChangeTimer < stageTime)     
            {
                // ������ �ð���ŭ ��ٸ� �� ���� �����մϴ�.
                float spawnInterval = UnityEngine.Random.Range(minTime, maxTime);
                yield return new WaitForSeconds(spawnInterval);

                // ��� �ð��� �����մϴ�.
                patternChangeTimer += spawnInterval;

                // ���� Ǯ���� ���� �����ϴ� �Լ��� ȣ��
                SpawnEnemyFromPool(currentEnemyIndex);
            }

            Debug.Log($"�������� 2�� ���۵˴ϴ�.");

            //  --- ��� ��, �� ��° ���Ͱ� ���� �����Ǵ� ���� ---
            yield return new WaitForSeconds(stageDelayTime);
            patternChangeTimer = 0f;

            // ���� �ε����� �ش��ϴ� ���� ��� ����
            while (patternChangeTimer < stageTime)
            {
                // ������ �ð���ŭ ��ٸ� �� ���� �����մϴ�.
                float spawnInterval = UnityEngine.Random.Range(minTime, maxTime);
                yield return new WaitForSeconds(spawnInterval);

                // ��� �ð��� �����մϴ�.
                patternChangeTimer += spawnInterval;

                // ���� ������ �������� ����
                int enemyNum = 0;
                // enemyFactory�� �ּ� 2���� ���Ͱ� �ִ��� Ȯ��
                if (enemyFactory.Length > 1)
                {
                    // 0~10 ������ ���� ���ڸ� �̽��ϴ�.
                    float randomChance = UnityEngine.Random.Range(0f, 10f);

                    if (randomChance < 2f)
                    {
                        // 20% Ȯ���� 2��° ���� ���� (�ε��� 1)
                        enemyNum = 1;
                    }
                    else
                    {
                        // 70% Ȯ���� 1��° ���� ���� (�ε��� 0)
                        enemyNum = 0;
                    }
                }
                    // ���� Ǯ���� ���� �����ϴ� �Լ��� ȣ��
                    SpawnEnemyFromPool(enemyNum);
            }

            //  ---��� ��, ���� ���� ���� �ε� ---
            Debug.Log($"���� ����!");
            boss.SetActive(true);
            playerMove.BossOpening();

            yield return new WaitForSeconds(stageDelayTime);
            patternChangeTimer = 0f;

            //while (patternChangeTimer < bossStageTime)
            //{

            //}
            
            Debug.Log($"��� ���� ������ ����Ǿ����ϴ�.");

            GameManager.Instance.gState = GameManager.GameState.Ready;

            if (GameManager.Instance.gState != GameManager.GameState.Run)
            {
                // break�� ����� �ֻ��� while(true) ������ ���ư��ϴ�.
                break;
            }
        }
    }

    // ���� Ǯ���� ���� ����(����)�ϴ� �Լ� 
    void SpawnEnemyFromPool(int enemyNum)
    {
        //���ʹ�Ǯ �ȿ� �ִ� ���ʹ̵� �߿���
        if (enemyObjectPool[enemyNum].Count > 0)
        {
            //��Ȱ��ȭ ��(�߻���� ����) ù��° ����
            GameObject enemy = enemyObjectPool[enemyNum][0];

            // -3~3���� x�� ������ �����ϰ� ���ʹ� ��ġ
            Vector3 spawnPosition = new Vector3(
            Random.Range(-spawnValues.x, spawnValues.x),
            spawnValues.y,
            spawnValues.z);
            enemy.transform.position = spawnPosition;

            //������Ʈ Ǯ ����Ʈ���� �� ����
            enemyObjectPool[enemyNum].RemoveAt(0);

            // Ȱ��ȭ ��Ű��
            enemy.SetActive(true);
        }
    }
}
