using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // ���� ���� �ð�
    float createEnemyTime;
    // ����ð�
    float currentTime;
    // �ּҽð�
    public float minTime;
    // �ִ�ð�
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
        createEnemyTime = 3; 

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
    }

    void Update()
    {
        // 1. �ð��� �帣�ٰ�
        currentTime += Time.deltaTime;

        // 2. ���� ����ð��� �����ð��� �Ǹ�
        if (currentTime > createEnemyTime)
        {       
            int enemyNum = Random.Range(0, enemyFactory.Length);
            List<GameObject> enemyNumPool = enemyObjectPool[enemyNum];

            //���ʹ�Ǯ �ȿ� �ִ� ���ʹ̵� �߿���
            if (enemyNumPool.Count > 0)
            {
                //��Ȱ��ȭ ��(�߻���� ����) ù��° ����
                GameObject enemy = enemyNumPool[0];

                 // -3~3���� x�� ������ �����ϰ� ���ʹ� ��ġ
                Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnValues.x, spawnValues.x),
                spawnValues.y,
                spawnValues.z);
                enemy.transform.position = spawnPosition;

                 //������Ʈ Ǯ ����Ʈ���� �� ����
                 enemyNumPool.RemoveAt(0);

                // Ȱ��ȭ ��Ű��
                enemy.SetActive(true);
            }

            // ����ð��� 0���� �ʱ�ȭ
            currentTime = 0;
            // ���� ������ �� �� �����ð��� �ٽ� �����ϰ� �ʹ�.
            createEnemyTime = UnityEngine.Random.Range(minTime, maxTime);
        }
    }
}
