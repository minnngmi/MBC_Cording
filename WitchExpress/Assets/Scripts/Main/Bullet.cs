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
            // �浹�� ������Ʈ�� Ŭ���� ������Ʈ ��������(*�߿�)
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            // ���� ȿ�� �޼��� ȣ��
            enemy.ExplosionEnemy(transform.position);


            // �� CandyManager ��ü ������
            CandyManager candyManager =
                GameObject.Find("CandyManager").GetComponent<CandyManager>();

            // CandyManager ���� candyFactory �迭 ���� Ȯ��
            int candyCount = candyManager.candyFactory.Length;
            // 0 ~ (����-1) ���̿��� ������ ���� �̱�
            int candyPoolIndex = Random.Range(0, candyCount);

            // ĵ�� ���� ��ġ = Enemy�� ���� ��ġ
            Vector3 candySpawnPos = other.transform.position;

            // CandyManager�� ��ġ�� �ε����� �����ؼ� ĵ�� ����
            GameObject newCandy = candyManager.CreatCandy(candyPoolIndex, candySpawnPos);


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
