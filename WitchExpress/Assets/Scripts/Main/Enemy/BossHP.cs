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

    private void Start()
    {
        bossHpTxt.text = $"{bossHP} / {bossMaxHP}";
    }

    public void BossTakeDamage(int amount)
    {
        Debug.Log(" 30�� �������� �޾ҽ��ϴ�.");
        bossHP -= amount;
        bossHpTxt.text = $"{bossHP} / {bossMaxHP}";
        GameObject effectInstance = Instantiate(damageEffect, transform.position, Quaternion.identity);

        // HP�� 0 �̸����� �������� �ʵ��� ����
        if (bossHP < 0)
        {
            bossHP = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� "Player" �±׸� ������ �ִ��� Ȯ���մϴ�.
        if (other.CompareTag("Bullet"))
        {
            // ������Ʈ�� ã�ƿɴϴ�
            Bullet bullet = other.GetComponent<Bullet>();
            BossTakeDamage(bullet.damage);

            //�Ѿ� ��Ȱ��ȭ
            bullet.gameObject.SetActive(false);
            // PlayerFire ��ü ������
            PlayerFire player =
                GameObject.Find("Player").GetComponent<PlayerFire>();
            // ������Ʈ Ǯ ����Ʈ�� �Ѿ� �߰�
            player.bulletObjectPool.Add(gameObject);
        }
    }

}
