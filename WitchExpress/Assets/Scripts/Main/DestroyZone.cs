using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    //���� �ȿ� �ٸ� ��ü�� ������ ��� 
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Bullet"))
        {
            // �ε��� ��ü ��Ȱ��ȭ 
            other.gameObject.SetActive(false);
            // PlayerFire ��ũ��Ʈ ��ü ������
            PlayerFire playerFire =
                GameObject.Find("Player").GetComponent<PlayerFire>();

            // �Ѿ� ������Ʈ Ǯ ����Ʈ�� �Ѿ� �߰�
            playerFire.bulletObjectPool.Add(other.gameObject);
        }
        else if (other.CompareTag("Enemy"))
        {
            // �ε��� ��ü ��Ȱ��ȭ 
            other.gameObject.SetActive(false);
            // EnemyManager ��ũ��Ʈ ��ü ������
            EnemyManager em =
                GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
            // �ε��� ���� �����ѹ� Ȯ��
            int enemyIdx = other.GetComponent<Enemy>().enemyIdx;
            // �ش� �� ������Ʈ Ǯ ����Ʈ�� �߰�
            em.enemyObjectPool[enemyIdx].Add(other.gameObject);
        }

        else if (other.CompareTag("Candy"))
        {
            // �ڽĿ��� �θ� ������Ʈ(GameObject) ã��
            GameObject parentObj = other.transform.parent.gameObject;
            // �θ� ���� �� �ڽĵ� ���� ����
            Destroy(parentObj);
        }

        else if (other.CompareTag("Player"))
        {
            return;
        }

        else
        {
            other.gameObject.SetActive(false);
        }
    }
}
