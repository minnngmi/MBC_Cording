using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    // ������ȣ
    public int enemyIdx;

    // �÷��̾�� �� ������
    public int damage;

    // óġ�� ȹ�� ����
    public int scoreValue;

    //�ʿ�Ӽ� : �̵��ӵ�
    public float speed;

    // ������ ���������� ���� Start�� Update���� ���
    Vector3 dir;

    //����(ȿ��) ���� �ּ�
    public GameObject explosionFactory;


    private void OnEnable()
    {
        // �� �̵�
        // 0���� 9(10-1) ���� ���߿� �ϳ��� �������� �����ͼ�
        int randValue = Random.Range(0, 10);

        // ���� 3���� ������ �÷��̾����
        if (randValue < 4)
        {
            // �÷��̾ �±׷� ã�Ƽ� target���� ����
            GameObject target = GameObject.FindWithTag("Player");
            // �ٶ󺸴� ������ �÷��̾� ������ ȸ��
            if (target != null)
            {
                transform.LookAt(target.transform);
            }
        }
    }

    void Update()
    {
        //transform.position += dir * speed * Time.deltaTime;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }


    // ���� ����Ʈ�� �۵���Ű�� �޼���
    public void ExplosionEnemy(Vector3 position)
    {
        //2.���� ȿ�� ���忡�� ���� ȿ���� �ϳ� ������ �Ѵ�.
        GameObject explosion = Instantiate(explosionFactory);

        //3.���� ȿ���� �߻�(��ġ) ��Ű�� �ʹ�.
        explosion.transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� "Player" �±׸� ������ �ִ��� Ȯ���մϴ�.
        if (other.CompareTag("Player"))
        {
            // Player ������Ʈ���� PlayerHP ������Ʈ�� ã�ƿɴϴ�.
            PlayerHP playerHP = other.GetComponent<PlayerHP>();

            // PlayerHP ������Ʈ�� �ִٸ� �������� �����մϴ�.
            if (playerHP != null)
            {
                playerHP.TakeDamage(damage); // PlayerHP ��ũ��Ʈ�� �޼���
            }


        }
    }
}


