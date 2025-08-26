using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;  // Action ����� ���� 



public class GameManager : MonoBehaviour
{
    // �̱��� ����: static ������ �ڱ� �ڽ��� ���� �׸��� ����ϴ�.
    public static GameManager Instance;

    // ������ ������ �������� �����մϴ�.
    public int enemyKills;               // ���� óġ�� Ƚ��
    public int candyCount;             // ȹ���� ���� ����
    public int HPpotion;                 // �ʱ� ���� ����

    public int playerHP = 100;       // �ʱ� HP ��
    public int maxHP = 100;         
    public int playerMP = 0;          // �ʱ� MP ��
    public int maxMP = 100;

    // �̺�Ʈ ����
    // UIManager�� �� �̺�Ʈ�� �����Ͽ� HP/MP �����̴��� ������Ʈ
    public Action<int> OnHPChanged;      //  1) HP ���� �̺�Ʈ
    public Action<int> OnMPChanged;     //  2) MP ���� �̺�Ʈ
    public Action OnSkillActivated;             // 3) ��ų ��� �̺�Ʈ

    // Ư�� ��ų Ȱ��ȭ ���¸� �����ϴ� ����
    public bool isSkillActive = false; 

    // ���� ���� ���
    public enum GameState
    {
        Ready,
        Run,
        GameOver
    }

    // ������ ���� ���� ����
    public GameState gState;

    // Awake �޼��� : �ε�� �� ���� ���� ����
    private void Awake()
    {
        // �ν��Ͻ��� �̹� �����ϴ��� Ȯ��
        if (Instance == null)
        {
            // �ν��Ͻ��� ������, �� GameManager ������Ʈ�� �ν��Ͻ��� ����
            Instance = this;
            // ���� �ٲ� �ı����� �ʰ�(�ʿ信 ���� ����)
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // �ν��Ͻ��� �̹� �ִٸ�, ���� ������ ������Ʈ�� �ı��Ͽ� �ߺ�����
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // �ʱ� ���� ����
        gState = GameState.Ready;
    }

    
    // �޼��� ����
    // 1. �� óġ ī��Ʈ �޼���
    public void IncreaseEnemyKills()
    {
        enemyKills++;
        Debug.Log("�� óġ ��: " + enemyKills);

         // (1) MP 5 ����
        IncreasePlayerMP(5);

        if (enemyKills % 10 == 0)
        {
            // (2) ���� 1 ����
            IncreasePosion();
        }
    }

    // 2) ���� ���� ������Ű�� �޼���
    public void IncreaseCandyCount()
    {
        candyCount++;
        Debug.Log("���� ȹ�� ��: " + candyCount);
    }

    // 2) ���� ���� ���ҽ�Ű�� �޼���
    public void DecreaseCandyCount(int amount)
    {
        candyCount = candyCount - amount;

        // ������ 0 �̸����� �������� �ʵ��� ����
        if (candyCount < 0)
        {
            candyCount = 0;
        }
    }

    // 3) �÷��̾� HP�� ���ҽ�Ű�� �޼���
    public void DecreasePlayerHP(int damage)
    {
        playerHP -= damage;

        if (playerHP < 0)
        {
            playerHP = 0;
        }

        // HP�� ����Ǿ����� �˸��� �̺�Ʈ ȣ��(UIManager�� ���� ��)
        OnHPChanged?.Invoke(playerHP);

        if (playerHP <= 0)
        {
            playerHP = 0;
            Debug.Log("���� ����!");
            // TODO: ���� ���� ó�� (ȭ�� ��ȯ ��)
        }
    }

    // 4) MP�� ������Ű�� �޼���
    public void IncreasePlayerMP(int amount)
    {
        playerMP += amount;

        // MP�� �ִ�ġ�� ���� �ʵ��� ����
        if (playerMP > maxMP)
        {
            playerMP = maxMP;

            //MP�� ���� ���� OnSkillActivated �̺�Ʈ�� ȣ���Ͽ� PlayerFire�� ���
            OnSkillActivated?.Invoke();
        }

        // MP�� ����Ǿ����� �˸��� �̺�Ʈ ȣ��
        OnMPChanged?.Invoke(playerMP);
    }

    // 5) MP�� ���ҽ�Ű�� �޼���
    public void DecreasePlayerMP(int amount)
    {
        playerMP -= amount;

        // MP�� 0 �̸����� �������� �ʵ��� ����
        if (playerMP < 0)
        {
            playerMP = 0;
        }

        // MP�� ����Ǿ����� �˸��� �̺�Ʈ ȣ��
        OnMPChanged?.Invoke(playerMP);
    }

    // (6) ���� ������ ���� ��Ű�� �޼���
    public void IncreasePosion()
    {
        HPpotion++;
    }

    // (7) ���� ������ ���� ��Ű�� �޼���(���� ���)
    public void DereasePosion()
    {
        if (candyCount < 15)
        {
            Debug.Log("ĵ�� ���� �����մϴ�.");
            return;
        }
        else
        {
            HPpotion--;
            playerHP = maxHP;
            DecreaseCandyCount(15);

            // ���� ������  0 �̸����� �������� �ʵ��� ����
            if (HPpotion < 0)
            {
                HPpotion = 0;
            }

            // HP�� ����Ǿ����� �˸��� �̺�Ʈ ȣ��(UIManager�� ���� ��)
            OnHPChanged?.Invoke(playerHP);
        }
    }
}
