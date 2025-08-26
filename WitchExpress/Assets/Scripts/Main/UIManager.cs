using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI ����� ���� �߰�

public class UIManager : MonoBehaviour
{
    // �����̴� ����
    public Slider hpSlider; // HP �����̴� 
    public Slider mpSlider; // MP �����̴� 

    // MP �����̴��� Fill ����
    public Image mpFill;
    // MP ���¿� ���� ������ Fill ���� ���� �̸� ����
    private Color normalColor = Color.gray; // MP�� �� ���� �ʾ��� ��
    public Color fullMPColor; // �� á�� ��

    // ��ų ��ư�� ����Ʈ ���ӿ�����Ʈ ����
    public GameObject skillBtn;
    public GameObject skillBtnEffect;

    //����, ���� UI text ����
    public Text candyTxt;
    public Text posionTxt;


    private void Start()
    {
        // GameManager �ν��Ͻ��� �����ϴ��� Ȯ��
        if (GameManager.Instance != null)
        {
            // OnHPChanged �̺�Ʈ ����
            // HP ����� ������ UpdateHPSlider �޼��� ȣ��
            GameManager.Instance.OnHPChanged += UpdateHPSlider;

            // OnMPChanged �̺�Ʈ ����
            // MP ����� ������ UpdateMPSlider �޼��� ȣ��
            GameManager.Instance.OnMPChanged += UpdateMPSlider;


            // �ʱ� HP �����̴� ����ȭ
            hpSlider.maxValue = GameManager.Instance.maxHP; // HP�� �ִ밪
            hpSlider.value = GameManager.Instance.playerHP;

            // �ʱ� MP �����̴� ����ȭ
            mpSlider.maxValue = GameManager.Instance.maxMP;  // MP�� �ִ밪
            mpSlider.value = GameManager.Instance.playerMP;

            // ��ų ��ư�� ����Ʈ ��Ȱ��ȭ
            skillBtn.SetActive(false);
            skillBtnEffect.SetActive(false);

            // ĵ��, ���� ī��Ʈ �ʱ�ȭ 0
            candyTxt.text = "0";
            posionTxt.text = "0";
        }
    }

    private void Update()
    {
        int candyCount = GameManager.Instance.candyCount;
        int HPpotion = GameManager.Instance.HPpotion;
        
        candyTxt.text = candyCount.ToString();
        posionTxt.text = HPpotion.ToString();
    }

    // GameManager���� �̺�Ʈ �߻� �� ȣ��� �޼���

    // 1. HP �����̴� ������Ʈ(����ȭ) �޼��� 
    private void UpdateHPSlider(int currentHP)
    {
        // HP �����̴� �� ������Ʈ
        hpSlider.value = currentHP;

        // HP�� 0�� �Ǹ� ���� ���� ȭ���� ǥ���ϴ� ���� �߰� �۾�
        if (currentHP <= 0)
        {
            // TODO: ���� ���� UI ǥ��
            Debug.Log("UI Manager: ���� ���� UI�� ǥ���մϴ�.");
        }
    }

    //  2. MP �����̴� ������Ʈ �޼���
    private void UpdateMPSlider(int currentMP)
    {
        // MP �����̴� �� ������Ʈ
        mpSlider.value = currentMP;

        // MP�� 100�� �Ǹ�
        if (currentMP == 100)
        {
            // �����̴� ���� ��ȭ
            mpFill.color = fullMPColor;
            // ��ų ��ư�� ��ư ����Ʈ Ȱ��
            skillBtn.SetActive(true);
            skillBtnEffect.SetActive(true);
        }

        else // MP�� 100�� �ƴ� ��
        {
            mpFill.color = normalColor;
            skillBtn.SetActive(false);
            skillBtnEffect.SetActive(false);
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
