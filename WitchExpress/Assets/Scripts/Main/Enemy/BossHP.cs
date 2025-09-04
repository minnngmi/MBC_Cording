using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI ����� ���� �߰�


public class BossHP : MonoBehaviour
{
    [Header("���� ���� ����")]
    public int bossHP = 10000;
    public int bossMaxHP = 10000;
    // ������ ����Ʈ ������ ���� ����
    public GameObject damageEffect;

    [Header("���� UI ����")]
    public Slider bossHpSlider;
    // ���� �����̴��� Fill ����
    public Image BossHpFill;
    // HP ���¿� ���� ������ Fill ���� ���� �̸� ����
    public Color bossNormalColor; // Ǯ��
    public Color bossDeadlyColor; // ���鸮 ����
    // HP UI text ����
    public Text bossHpTxt;

    private Rigidbody rb;

    private void Start()
    {
        bossHpTxt.text = $"{bossHP} / {bossMaxHP}";
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���� ���°� ������ �ߡ� ������ ���� ������ �� �ְ� �Ѵ�.
        if (GameManager.Instance.gState != GameManager.GameState.Run)
        {
            Debug.Log("���� ���� �ƴմϴ�");
            return;
        }

        if (other.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            int playerDamage = bullet.damage;
            Debug.Log("������ Ȯ�� : " + playerDamage);
            BossTakeDamage(playerDamage);
        }
    }

    public void BossTakeDamage(int amount)
    {
        bossHP -= amount;
        bossHpTxt.text = $"{bossHP} / {bossMaxHP}";
        GameObject effectInstance = Instantiate(damageEffect, transform.position, Quaternion.identity);

        // HP�� 0 �̸����� �������� �ʵ��� ����
        if (bossHP < 0)
        {
            bossHP = 0;
        }
    }
}
