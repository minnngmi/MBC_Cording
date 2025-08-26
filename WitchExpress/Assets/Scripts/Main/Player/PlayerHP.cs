using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    // ������ ����Ʈ ������ ���� ����
    public GameObject damageEffect;


    private void Update()
    {
        // ���� ���, HP ȸ�� 
        if (Input.GetKeyDown(KeyCode.LeftAlt))
            GameManager.Instance.DereasePosion();
    }

    public void TakeDamage(int damage)
    {
        // GameManager�� DecreasePlayerHP �޼��带 ȣ���Ͽ� �������� ����
        // GameManager.Instance�� �̱��� ���� ���п� ��𼭵� ���� ����
        if (GameManager.Instance != null)
        {
            GameManager.Instance.DecreasePlayerHP(damage);

        }

        // �������� ���� �� ī�޶� ��鸮�� ����Ʈ ����
        // ����Ʈ ������ ������ ������� ������ Ȯ��
        if (damageEffect != null)
        {
            // ����Ʈ�� �����ϰ� �÷��̾��� ��ġ�� ��ġ�մϴ�.
            GameObject effectInstance = Instantiate(damageEffect, transform.position, Quaternion.identity);

            // ������ ����Ʈ�� �ڵ����� ��������� �մϴ�.
            // ��ƼŬ �ý����� ������ �ð��� �������� ������Ʈ�� �ı�
            ParticleSystem ps = effectInstance.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(effectInstance, ps.main.duration);
            }
            else
            {
                // ��ƼŬ �ý����� ���� ����Ʈ�� ��츦 ����� 2�� �ڿ� �ı�
                Destroy(effectInstance, 2f);
            }
        }
    }
}