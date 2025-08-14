using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    

    void Update()
    {
        // 1. ������ ���Ѵ�.
        Vector3 dir = Vector3.forward;

        // 2. �̵��ϰ� �ʹ�. ���� P = P0 + vt
        transform.position += dir * speed * Time.deltaTime;
    }

    // �÷��̾��� �Ѿ˿� �¾�����
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // 1. �浹�� ������Ʈ�� Ŭ���� ������Ʈ ��������(*�߿�)
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            // 2. ���� ȿ�� �޼��� ȣ��
            enemy.ExplosionEnemy(transform.position);


            // 3. Enemy �ȿ��� CandyManager ��������
            //    -> Enemy �����տ� CandyManager�� �ٿ�����, �״�� ���� ��
            CandyManager candyManager =
                other.gameObject.GetComponent<CandyManager>();

            // 4. ĵ�� ���� ����
            // CandyManager ���� candyFactory �迭 ���� Ȯ��
            int candyCount = candyManager.candyFactory.Length;

            // 5) �� ���� ������ Ȯ���� ���� (��: 40% Ȯ���� 2��, �������� 1��)
            int rand = Random.Range(0, 10);   // 0~9
            int spawnCount = (rand < 4) ? 2 : 1;

            // 7) ������ ������ŭ ����
            for (int i = 0; i < spawnCount; i++)
            {
                // 0 ~ (����-1) ���̿��� ������ ���� �̱�
                int candyPoolIndex = Random.Range(0, candyCount);

                // CandyManager�� ��ġ�� �ε����� �����ؼ� ĵ�� ����
                GameObject newCandy = candyManager.CreatCandy(candyPoolIndex);
            }


            // ���� ��� ��Ȱ��ȭ
            other.gameObject.SetActive(false);

            // EnemyManager ��ü ������
            EnemyManager em =
                GameObject.Find("EnemyManager").GetComponent<EnemyManager>();

            // �ε��� ���� �����ѹ� Ȯ��
            int enemyIdx = enemy.enemyIdx;
            // �ش� �� ������Ʈ Ǯ ����Ʈ�� �߰�
            em.enemyObjectPool[enemyIdx].Add(other.gameObject);
        }
         // �ڽ�(�Ѿ�)�� ��Ȱ��ȭ
        gameObject.SetActive(false);
        // PlayerFire ��ü ������
        PlayerFire player =
            GameObject.Find("Player").GetComponent<PlayerFire>();
        // ������Ʈ Ǯ ����Ʈ�� �Ѿ� �߰�
        player.bulletObjectPool.Add(gameObject);
    }
}
