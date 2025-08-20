using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;  // Action ����� ���� �߰�


public class GameManager : MonoBehaviour
{
    // 1. �̱��� �ν��Ͻ�: static ������ �ڱ� �ڽ��� ���� �׸��� ����ϴ�.
    public static GameManager Instance;

    // 2. ������ ������ �������� �����մϴ�.
    public int enemyKills;
    public int candyCount;
    public int playerHP = 100; // �ʱ� HP ����

    // HP�� ����� �� ȣ���� �̺�Ʈ
    // UIManager�� �� �̺�Ʈ�� �����Ͽ� HP �����̴��� ������Ʈ�մϴ�.
    public Action<int> OnHPChanged;


    // 3. Awake �޼���: �� ��ũ��Ʈ�� �ε�� �� ���� ���� ����˴ϴ�.
    private void Awake()
    {
        // �ν��Ͻ��� �̹� �����ϴ��� Ȯ���մϴ�.
        if (Instance == null)
        {
            // �ν��Ͻ��� ������, �� GameManager ������Ʈ�� �ν��Ͻ��� �����մϴ�.
            Instance = this;

            // ���� �ٲ� �ı����� �ʰ� �մϴ�. (�ʿ信 ���� ����)
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // �ν��Ͻ��� �̹� �ִٸ�, ���� ������ ������Ʈ�� �ı��Ͽ� �ߺ��� �����ϴ�.
            Destroy(gameObject);
        }
    }

    // 4. �����͸� �����ϴ� �޼������ ����ϴ�.
    // �� óġ Ƚ���� ������Ű�� �޼���
    public void IncreaseEnemyKills()
    {
        enemyKills++;
        Debug.Log("�� óġ ��: " + enemyKills);

        // �߰� �۾� (��: Ư�� Ƚ�� �޼� �� ������ ����)
        if (enemyKills % 10 == 0)
        {
            Debug.Log("�����մϴ�! Ư�� �������� ��Ÿ���ϴ�!");
            // TODO: Ư�� ������ ���� �ڵ带 ���⿡ �ۼ�
        }
    }

    // ���� ī��Ʈ�� ������Ű�� �޼���
    public void IncreaseCandyCount()
    {
        candyCount++;
        Debug.Log("���� ȹ�� ��: " + candyCount);
    }

    // �÷��̾� HP�� ���ҽ�Ű�� �޼���
    public void DecreasePlayerHP(int damage)
    {
        playerHP -= damage;

        if (playerHP < 0)
        {
            playerHP = 0;
        }

        Debug.Log("�÷��̾� HP: " + playerHP);


        // HP�� ����Ǿ����� �˸��� �̺�Ʈ ȣ��
        // UIManager�� �� �̺�Ʈ�� �����ϰ� �ִٸ� UIManager�� �ڵ尡 ����˴ϴ�.
        OnHPChanged?.Invoke(playerHP);


        if (playerHP <= 0)
        {
            playerHP = 0;
            Debug.Log("���� ����!");
            // TODO: ���� ���� ó�� (ȭ�� ��ȯ ��)
        }
    }
}
