using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyManager : MonoBehaviour
{
    // ĵ�� ����(ĵ�� �����յ��� ��� �迭) ����
    public GameObject[] candyFactory;
    // �� �������� �̸� ������ ����
    public int poolSize;
    // ������ ������Ʈ Ǯ(����Ʈ) �迭
    public List<GameObject>[] candyObjectPool;

    private void Start()
    {
        // ĵ�� ���� ����ŭ Ǯ �迭 �����
        candyObjectPool = new List<GameObject>[candyFactory.Length];

        // �������� Ǯ�� �ʱ�ȭ
        for (int i = 0; i < candyFactory.Length; i++)
        {
            // i��° ������ Ǯ ����Ʈ ����
            candyObjectPool[i] = new List<GameObject>();

            for (int j = 0; j < poolSize; j++)
            {
                //���ʹ� ���忡�� ���ʹ̸� �����Ѵ�.
                GameObject candy = Instantiate(candyFactory[i]);
                // ���ʹ̸� ������Ʈ Ǯ ����Ʈ�� �߰� �Ѵ�.
                candyObjectPool[i].Add(candy);
                // ������Ʈ�� ��Ȱ��ȭ ��Ų��.
                candy.SetActive(false);
            }
        }
    }

    // Ư�� ������ ĵ�� ���� ��ġ�� ������Ű�� �޼���
    // poolIndex : CandySpawner�� pools ����Ʈ���� �� ��° Ǯ�� ���� (0���� ����)
    // pos          : ĵ�� ������ ��ġ
    // ��ȯ��     : ������(�Ǵ� ������) ĵ�� GameObject
    public GameObject CreatCandy(int poolIndex, Vector3 pos)
    {
        /*
        // �߸��� �ε����̸� �ƹ��͵� ���� �ʰ� null ��ȯ
        if (poolIndex < 0 || poolIndex >= candyFactory.Length)
        {
            return null;
        }
        */
        // �ش� �ε����� Ǯ�� �����´�
        List<GameObject> pool = candyObjectPool[poolIndex];

        // Ǯ���� ��Ȱ��ȭ��(��� ����) ������Ʈ�� ã�´�
        GameObject candy = null;
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeSelf)
            {
                candy = pool[i];
                break; // �ϳ� ã���� �ٷ� ����
            }
        }

        // �� ã������ ���� �ϳ� ���� Ǯ�� �߰�
        if (candy == null)
        {
            // �ش� ������ ���������� ����
            candy = Instantiate(candyFactory[poolIndex], transform);

            // Ǯ�� ���
            pool.Add(candy);

            // ���� ���Ŀ� ���� ���·� ����α�(�Ʒ����� �ٷ� ��)
            candy.SetActive(false);
        }

        // ��ġ(�ʿ��ϸ� ȸ����) ����
        candy.transform.position = pos;

        // Ȱ��ȭ�ؼ� ���̰� �����
        candy.SetActive(true);

        // ������ ĵ�� ��ȯ
        return candy;
    }
}
