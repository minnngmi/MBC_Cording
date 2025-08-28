using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public int damage;

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
        // ���� ���°� ������ �ߡ� ������ ���� ������ �� �ְ� �Ѵ�.
        if (GameManager.Instance.gState != GameManager.GameState.Run)
        {
            return;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            // 1. �浹�� ������Ʈ�� Ŭ���� ������Ʈ ��������
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            // 2. ���� ȿ�� �޼��� ȣ�� (Enemy�� ó��)
            enemy.ExplosionEnemy(transform.position); // �� �ȿ��� ��Ȱ��ȭ�� �Ͼ�ϴ�.

            // GameManager�� ���� �� óġ ī��Ʈ�� ����
            GameManager.Instance.IncreaseEnemyKills();

            // 3. Enemy�� CandyManager�� �����ͼ� ĵ�� ���� �޼��� ȣ��
            CandyManager candyManager = other.gameObject.GetComponent<CandyManager>();
            if (candyManager != null)
            {
                candyManager.SpawnRandomCandy();
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

        if (other.gameObject.CompareTag("Boss"))
        {
            BossHP bossHp =
            GameObject.Find("EnemyBoss").GetComponent<BossHP>();
            bossHp.BossTakeDamage(damage);
            Debug.Log(" ������ �޾ҽ��ϴ�.");
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
