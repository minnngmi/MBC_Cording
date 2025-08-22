using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;  // Action ����� ���� 



public class GameManager : MonoBehaviour
{
    // 1. �̱��� �ν��Ͻ�: static ������ �ڱ� �ڽ��� ���� �׸��� ����ϴ�.
    public static GameManager Instance;

    // 2. ������ ������ �������� �����մϴ�.
    public int enemyKills;
    public int candyCount;
    public int playerHP = 100; // �ʱ� HP ����
    public int maxHP = 100;
    public int playerMP = 0; // MP ���� �߰�
    public int maxMP = 100;

    // �̺�Ʈ ����
    // UIManager�� �� �̺�Ʈ�� �����Ͽ� HP/MP �����̴��� ������Ʈ
    public Action<int> OnHPChanged; //  1) HP ���� �̺�Ʈ
    public Action<int> OnMPChanged; //  2) MP ���� �̺�Ʈ
    public Action OnSkillActivated; // 3) ��ų ��� �̺�Ʈ

    // ���� ���� ���
    public enum GameState
    {
        Ready,
        Run,
        GameOver
    }

    // ������ ���� ���� ����
    public GameState gState;


    // 3. Awake �޼���: �ε�� �� ���� ���� ����
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
        gState = GameState.Run;
    }

    // 4. �����͸� �����ϴ� �޼���� ����

    // 1) �� óġ Ƚ���� ������Ű�� �޼���
    public void IncreaseEnemyKills()
    {
        enemyKills++;
        IncreasePlayerMP(5);  // MP 5 �� ����
        Debug.Log("�� óġ ��: " + enemyKills);

        // �߰� �۾� (��: Ư�� Ƚ�� �޼� �� ������ ����)
        if (enemyKills % 10 == 0)
        {
            Debug.Log("�����մϴ�! Ư�� �������� ��Ÿ���ϴ�!");
            // TODO: Ư�� ������ ���� �ڵ带 ���⿡ �ۼ�
        }
    }

    // 2) ���� ī��Ʈ�� ������Ű�� �޼���
    public void IncreaseCandyCount()
    {
        candyCount++;
        Debug.Log("���� ȹ�� ��: " + candyCount);
    }

    // 3) �÷��̾� HP�� ���ҽ�Ű�� �޼���
    public void DecreasePlayerHP(int damage)
    {
        playerHP -= damage;

        if (playerHP < 0)
        {
            playerHP = 0;
        }

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
}
