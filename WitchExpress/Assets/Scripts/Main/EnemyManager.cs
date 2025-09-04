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

    [Header("�������� ���� �ð� ����")]
    // �������� �� ���� ���� �ð� (30)
    public float stageTime;
    // 2ź �������� �� ����������� ���ð� 
    public float stageDelayTime = 5f;
    // ���� �������� ���� ��� �ð�
    public float bossDelayTime;
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

    [Header("���� ����")]
    public GameObject boss;
    public Slider bossHpSlider;
    private PlayerMove playerMove;
    public AudioSource bossLaughing;
    public AudioSource mainBGM02;   // ������ ����
    public MainBGM_FadeOut bgmFader; // ���� fade ȿ��



    private void Start()
    {
        opTxt.SetActive(false);
        mainBGM01.Play();

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
                // ������Ʈ ��Ȱ��ȭ
                enemy.SetActive(false);
            }
        }
        // ���� �� ��Ȱ��ȭ 
        boss.SetActive(false);
        bossHpSlider.gameObject.SetActive(false);


        // ���� �帧�� �����ϴ� ���� �ڷ�ƾ�� �����մϴ�.
        StartCoroutine(GameFlowRoutine());
    }

    //��� �ڷ�ƾ�� ������� �����ϴ� ���� �ڷ�ƾ
    IEnumerator GameFlowRoutine()
    {
        // GameManager�� ���°� 'Run'�� �� ������ ��ٸ��ϴ�.
        while (GameManager.Instance.gState != GameState.Run)
        {
            yield return null;
        }

        Debug.Log("���� ����! �������� 1 ���� ���� ����.");
        // ù ��° ���� ���� �ڷ�ƾ�� ���� ������ ��ٸ��ϴ�.
        yield return StartCoroutine(SpawnEnemiesRoutine());

        Debug.Log("�������� 2 ���� ���� ����.");
        opTxt.SetActive(true);
        // �� ��° ���� ���� �ڷ�ƾ�� ���� ������ ��ٸ��ϴ�.
        yield return StartCoroutine(SpawnBooRoutine());

        Debug.Log("���� �������� ����.");
        // ���� ���� �ڷ�ƾ�� ���� ������ ��ٸ��ϴ�.
        yield return StartCoroutine(SpawnBossRoutine());


        Debug.Log("��� �������� �Ϸ�! ���� ����.");
        // ��� ���������� ������ ���� ���¸� Ending���� �����մϴ�.
        GameManager.Instance.gState = GameState.Ending;
    }

    //���� ���� �ڷ�ƾ (�������� 1)
    IEnumerator SpawnEnemiesRoutine()
    {
        yield return new WaitForSeconds(stageDelayTime);
        // �ð� Ÿ�̸�
        float patternChangeTimer = 0f;
        // ���� ������ ���� �ε��� (enemyFactory�� ����)
        int currentEnemyIndex = 0;

        while (patternChangeTimer < stageTime)
        {
            // ������ �ð���ŭ ��ٸ� �� ���� �����մϴ�.
            float spawnInterval = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(spawnInterval);
            // ��� �ð��� �����մϴ�.
            patternChangeTimer += spawnInterval;
            // ���� Ǯ���� ���� �����ϴ� �Լ��� ȣ��
            SpawnEnemyFromPool(currentEnemyIndex);
        }
    }

    // ���� ���� �ڷ�ƾ (�������� 2)
    IEnumerator SpawnBooRoutine()
    {
        yield return new WaitForSeconds(stageDelayTime);
        // �ð� Ÿ�̸�
        float patternChangeTimer = 0f;
        // ���� �ε����� �ش��ϴ� ���� ��� ����
        while (patternChangeTimer < stageTime+5)
        {
            // ������ �ð���ŭ ��ٸ� �� ���� �����մϴ�
            float spawnInterval = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(spawnInterval);

            // ��� �ð��� �����մϴ�.
            patternChangeTimer += spawnInterval;
            int enemyNum = 0;

            // enemyFactory�� �ּ� 2���� ���Ͱ� �ִ��� Ȯ��
            if (enemyFactory.Length > 1)
            {
                // ���� ������ �������� ����
                // 0~10 ������ ���� ���ڸ� �̽��ϴ�.
                float randomChance = Random.Range(0f, 10f);
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
    }

    // ���� ���� �ڷ�ƾ
    IEnumerator SpawnBossRoutine()
    {
        playerMove = FindObjectOfType<PlayerMove>();
        // ���� ���� ��� �ð�
        yield return new WaitForSeconds(stageDelayTime-10);

        // �÷��̾� ĳ���͸� �̵���Ű�� �ڷ�ƾ�� ���� ������ ��ٸ��ϴ�.
        yield return StartCoroutine(playerMove.BossOpening());
        // BGM01 ���� ���̵� �ƿ� �޼��� ȣ��
        bgmFader.FadeOutBGM(5f);

        Debug.Log($"���� ����!");
        boss.SetActive(true);
        // (���� �ִϸ��̼� �۵���)
        yield return new WaitForSeconds(bossDelayTime);

        GameManager.Instance.gState = GameManager.GameState.Run;

        // ������ ���� ���
        mainBGM02.Play();

        // ���� HP UI Ȱ��ȭ
        bossHpSlider.gameObject.SetActive(true);

        // �ð� Ÿ�̸�
        float patternChangeTimer = 0f;

        while (patternChangeTimer < bossStageTime)
        {

            
            // ���⿡ ������ ���� ������ �߰�
            // ���� ���, yield return new WaitForSeconds(1.0f);
            yield return null;
            patternChangeTimer += Time.deltaTime;
        }
        Debug.Log($"��� ���� ������ ����Ǿ����ϴ�.");
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
