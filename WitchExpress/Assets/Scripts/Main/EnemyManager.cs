using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // ���� ���� �� ù ���Ͱ� ����������� �ð� (�ν����Ϳ��� ���� ����)
    public float opening; // 3f
    public float SecondStage = 3f;

    // �� ���� �ּҽð�
    public float minTime;
    // �� ���� �ִ�ð�
    public float maxTime;

    // �� ����
    public GameObject[] enemyFactory;

    // ������Ʈ Ǯ ũ��
    public int poolSize; 

    //������Ʈ Ǯ ����Ʈ �迭
    public List<GameObject>[] enemyObjectPool;

    // �� ���� ��ġ
    public Vector3 spawnValues;



    private void Start()
    {
        // ���� �� ���� �ð�
        //createEnemyTime = 3; 

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
                // ������Ʈ�� ��Ȱ��ȭ ��Ų��.
                enemy.SetActive(false);
            }
        }

        // �ڷ�ƾ ����
        StartCoroutine(SpawnEnemiesRoutine());
    }

    // �ڷ�ƾ: 1�и��� ���� ���� ������ �����ϸ� ���� �����մϴ�.
    IEnumerator SpawnEnemiesRoutine()
    {
        // ���� ���� �� ������ �ð�(opening)��ŭ ��ٸ��ϴ�.
        yield return new WaitForSeconds(opening);


        // ���� �ݺ� ����: ���� ���� ������ �ݺ��մϴ�.
        while (true)
        {
            // --- 40�� ���� ù ��° ���͸� �����ϴ� ���� ---
            // �ð� Ÿ�̸�
            float patternChangeTimer = 0f;

            // ���� ������ ���� �ε��� (enemyFactory�� ����)
            int currentEnemyIndex = 0;

            while (patternChangeTimer < 10f)     // 40
            {
                // ������ �ð���ŭ ��ٸ� �� ���� �����մϴ�.
                float spawnInterval = UnityEngine.Random.Range(minTime, maxTime);
                yield return new WaitForSeconds(spawnInterval);

                // ��� �ð��� �����մϴ�.
                patternChangeTimer += spawnInterval;

                // ���� Ǯ���� ���� �����ϴ� �Լ��� ȣ��
                SpawnEnemyFromPool(currentEnemyIndex);
            }

            Debug.Log($"{currentEnemyIndex + 1}��° ���� �������� ����Ǿ����ϴ�.");


            // --- 3�� ��� ��, 40�� ���� ù ��°, �� ��° ���Ͱ� ������ �����Ǵ� ���� ---
            yield return new WaitForSeconds(SecondStage);
            patternChangeTimer = 0f;

            // 40�� ���� ���� �ε����� �ش��ϴ� ���� ��� ����
            while (patternChangeTimer < 20f)
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
            Debug.Log($"��� ���� ������ ����Ǿ����ϴ�.");

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
