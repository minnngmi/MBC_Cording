using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBullet : MonoBehaviour
{

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // 1. �浹�� ������Ʈ�� Ŭ���� ������Ʈ ��������
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            // 2. ���� ȿ�� �޼��� ȣ�� (Enemy�� ó��)
            if (enemy != null)
            {
                enemy.ExplosionEnemy(transform.position); // �� �ȿ��� ��Ȱ��ȭ�� �Ͼ�ϴ�.
            }

            // GameManager�� ���� �� óġ ī��Ʈ�� ����
            if (GameManager.Instance != null)
            {
                GameManager.Instance.IncreaseEnemyKills();
            }

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
    }
}
