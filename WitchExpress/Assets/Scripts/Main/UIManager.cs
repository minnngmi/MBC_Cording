using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI ����� ���� �߰�

public class UIManager : MonoBehaviour
{
    // �ν����� â���� �����̴��� ����
    public Slider hpSlider; // HP �����̴� ����
    public Slider mpSlider; // MP �����̴� ����


    // MP �����̴��� ä������ �κ�(Fill)�� ����
    public Image mpFill;
    // MP ���¿� ���� ������ Fill ���� ���� �̸� ����
    private Color normalColor = Color.gray; // MP�� �� ���� �ʾ��� ��
    public Color fullMPColor; // �� á�� ��

    // ��ų ��ư�� ����Ʈ ���ӿ�����Ʈ�� ������ ����
    public GameObject skillBtn;
    public GameObject skillBtnEffect;



    private void Start()
    {
        // GameManager �ν��Ͻ��� �����ϴ��� Ȯ��
        if (GameManager.Instance != null)
        {
            // GameManager�� OnHPChanged �̺�Ʈ ����
            // HP�� ����� ������ UpdateHPSlider �޼��尡 ȣ��
            GameManager.Instance.OnHPChanged += UpdateHPSlider;

            // GameManager�� OnMPChanged �̺�Ʈ ����
            // MP�� ����� ������ UpdateMPSlider �޼��尡 ȣ��
            GameManager.Instance.OnMPChanged += UpdateMPSlider;


            // ���� ���� �� �ʱ� HP �����̴� ����
            hpSlider.maxValue = GameManager.Instance.maxHP; // HP�� �ִ밪
            hpSlider.value = GameManager.Instance.playerHP;

            // ���� ���� �� �ʱ� MP �����̴� ����
            mpSlider.maxValue = GameManager.Instance.maxMP;  // MP�� �ִ밪
            mpSlider.value = GameManager.Instance.playerMP;


            // ���� ���� ��(MP 0) ��ų ��ư�� ����Ʈ�� ��Ȱ��ȭ
            if (skillBtn != null)
            {
                skillBtn.SetActive(false);
            }
            if (skillBtnEffect != null)
            {
                skillBtnEffect.SetActive(false);
            }

        }
    }

    // GameManager���� �̺�Ʈ�� �߻��ϸ� ȣ��� �޼���

    // 1. HP �����̴��� ������Ʈ �ϴ� �޼��� 
    private void UpdateHPSlider(int currentHP)
    {
        // HP �����̴� ���� ������Ʈ
        hpSlider.value = currentHP;

        // HP�� 0�� �Ǹ� ���� ���� ȭ���� ǥ���ϴ� ���� �߰� �۾�
        if (currentHP <= 0)
        {
            // TODO: ���� ���� UI ǥ��
            Debug.Log("UI Manager: ���� ���� UI�� ǥ���մϴ�.");
        }
    }

    //  2. MP �����̴��� ������Ʈ�ϴ� �޼���
    private void UpdateMPSlider(int currentMP)
    {
        // MP �����̴� ���� ������Ʈ
        mpSlider.value = currentMP;

        // MP�� 100�� �Ǹ�
        if (currentMP == 100)
        {
            // ���� ��ȭ
            if (mpFill != null)
            {
                mpFill.color = fullMPColor;
            }

            // ��ų ��ư�� ����Ʈ�� Ȱ��ȭ
            if (skillBtn != null)
            {
                skillBtn.SetActive(true);
            }
            if (skillBtnEffect != null)
            {
                skillBtnEffect.SetActive(true);
            }
        }

        else // MP�� 100�� �ƴ� ��
        {
            if (mpFill != null)
            {
                mpFill.color = normalColor;
            }

            //  ��ų ��ư�� ����Ʈ�� ��Ȱ��ȭ
            if (skillBtn != null)
            {
                skillBtn.SetActive(false);
            }
            if (skillBtnEffect != null)
            {
                skillBtnEffect.SetActive(false);
            }
        }
    }

    // ���� ���� �� �̺�Ʈ ���� ���� (���� ���������� ���� ����)
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnHPChanged -= UpdateHPSlider;
            GameManager.Instance.OnMPChanged -= UpdateMPSlider; 
        }
    }
}
