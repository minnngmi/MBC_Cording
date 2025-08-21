using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningAttack : MonoBehaviour
{
    // �� �Ѿ��� �÷��̾�� �� �������� �����մϴ�.
    public int damage;

    // OnEnable �޼��� ����
    // �̵��� LightningMove ��ũ��Ʈ�� ����ϹǷ� ���⼭�� �ʿ����� �ʽ��ϴ�.

    // OnTriggerEnter: �Ѿ��� �ٸ� �ݶ��̴� ������ �������� �� ȣ��˴ϴ�.
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
        // �浹 �� �Ѿ��� ��Ȱ��ȭ�Ͽ� ������Ʈ Ǯ�� ���������ϴ�.
        gameObject.SetActive(false);
        }
    }

}
