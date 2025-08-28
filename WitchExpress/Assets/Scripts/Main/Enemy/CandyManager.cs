using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Enemy �����տ� ����� ��ũ��Ʈ
public class CandyManager : MonoBehaviour
{
    // ĵ�� ����(ĵ�� �����յ��� ��� �迭) ����
    public GameObject[] candyFactory;

 
    // ĵ�� ������ ���õ� ��� ����� ����ϴ� �޼���
    public void SpawnRandomCandy()
    {
        // ���� ���°� 'Run'�� ���� �޼��� ������ ����
        if (GameManager.Instance.gState != GameManager.GameState.Run)
        {
            // ���°� Run�� �ƴϸ� ��� �޼��带 �����մϴ�.
            return;
        }

        // 1. �� ���� ������ Ȯ���� ���� (40% Ȯ���� 2��, �������� 1��)
        int rand = Random.Range(0, 10);
        int spawnCount = (rand < 4) ? 2 : 1;

        // 2. ĵ�� ������ 1���� ���, spawnCount�� 1�� ���� ����
        int candyCount = candyFactory.Length;
        if (candyCount == 1)
        {
            spawnCount = 1;
        }

        // 3. ĵ�� ������ ������ŭ ���ĵ� �ε��� �迭�� ����ϴ�.
        int[] allCandyIndices = new int[candyCount];
        for (int i = 0; i < candyCount; i++)
        {
            allCandyIndices[i] = i;
        }

        // 4. �迭�� �������� �����ݴϴ�.
        System.Random rng = new System.Random();
        int n = allCandyIndices.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int value = allCandyIndices[k];
            allCandyIndices[k] = allCandyIndices[n];
            allCandyIndices[n] = value;
        }

        // 5. ���� �迭���� ������ ������ŭ ������� �̾� ĵ�� ����
        for (int i = 0; i < spawnCount; i++)
        {
            // ���� �迭���� i��° �ε����� ������ ĵ�� ������ ����մϴ�.
            int candyPoolIndex = allCandyIndices[i];

            // ĵ�� ������ ��ġ�� ���� ��ġ���� �ణ ������ �Ű��ݴϴ�.
            // i ���� ���� ĵ�� ���� �Ǵ� ���������� �̵��մϴ�.
            Vector3 offset = Vector3.right * (i - (spawnCount - 1) / 2.0f) * 0.5f;

            // 6. ���� ĵ�� �����ϴ� �޼��� ȣ��
            CreatCandy(candyPoolIndex, offset);
        }
    }


    // Ư�� ������ ĵ�� ���� ��ġ�� ������Ű�� �޼���
    public GameObject CreatCandy(int poolIndex, Vector3 offset)
    {
        // �ش� ������ ���������� �����ϰ� ��ġ�� �������� ���մϴ�.
        GameObject candy = Instantiate(candyFactory[poolIndex], transform.position + offset, Quaternion.identity);

        // ������ ĵ�� ��ȯ
        return candy;
    }
}
