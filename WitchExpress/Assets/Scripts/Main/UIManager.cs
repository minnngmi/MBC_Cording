using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI ����� ���� �߰�

public class UIManager : MonoBehaviour
{
    // �ν����� â���� �����̴��� ������ ����
    public Slider hpSlider;

    private void Start()
    {
        // GameManager �ν��Ͻ��� �����ϴ��� Ȯ��
        if (GameManager.Instance != null)
        {
            // GameManager�� OnHPChanged �̺�Ʈ�� �����մϴ�.
            // HP�� ����� ������ UpdateHPSlider �޼��尡 ȣ��˴ϴ�.
            GameManager.Instance.OnHPChanged += UpdateHPSlider;

            // ���� ���� �� �ʱ� HP�� �����̴��� �����մϴ�.
            hpSlider.maxValue = GameManager.Instance.playerHP;
            hpSlider.value = GameManager.Instance.playerHP;
        }
    }

    // GameManager���� �̺�Ʈ�� �߻��ϸ� ȣ��� �޼���
    private void UpdateHPSlider(int currentHP)
    {
        // HP �����̴��� ���� ������Ʈ�մϴ�.
        hpSlider.value = currentHP;

        // HP�� 0�� �Ǹ� ���� ���� ȭ���� ǥ���ϴ� ���� �߰� �۾�
        if (currentHP <= 0)
        {
            // TODO: ���� ���� UI ǥ��
            Debug.Log("UI Manager: ���� ���� UI�� ǥ���մϴ�.");
        }
    }

    // ���� ���� �� �̺�Ʈ ���� ���� (���� ���������� ���� ����)
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnHPChanged -= UpdateHPSlider;
        }
    }
}
