using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    // ��ư �̹��� ��ü ������Ʈ
    public GameObject btnImg;

    // ��ư���� �迭�� ���� ����
    public Button[] buttons;
    private int buttonIndex = 0;

    // ��ư ���̶���Ʈ ȿ���� ���� ��������Ʈ��
    public Sprite[] normalSprites;
    public Sprite[] highlightedSprites;
    public Sprite[] pressedSprites;


    void Start()
    {
        StartCoroutine(ButtonOn());
        btnImg.SetActive(false);
    }


    private void Update()
    {
        // ��ư �̹����� Ȱ��ȭ �Ǿ������� Ű���� Ž�� ���
        if (!btnImg.activeInHierarchy)
            return;


        // �� ����Ű �Է� ����
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // ���� ��ư�� ��������Ʈ ����
            ApplySprite(buttons[buttonIndex], normalSprites[buttonIndex]);

            buttonIndex--;
            if (buttonIndex < 0)
            {
                buttonIndex = buttons.Length - 1;
                Debug.Log("��");
            }
            // ���� ���õ� ��ư ����
            EventSystem.current.SetSelectedGameObject(buttons[buttonIndex].gameObject);
            ApplySprite(buttons[buttonIndex], highlightedSprites[buttonIndex]);
        }

        // �Ʒ� ����Ű �Է� ����
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // ���� ��ư�� ��������Ʈ ����
            ApplySprite(buttons[buttonIndex], normalSprites[buttonIndex]);

            buttonIndex++;
            if (buttonIndex >= buttons.Length)
            {
                buttonIndex = 0;
            }
            // ���� ���õ� ��ư ����
            EventSystem.current.SetSelectedGameObject(buttons[buttonIndex].gameObject);
            ApplySprite(buttons[buttonIndex], highlightedSprites[buttonIndex]);

        }

        // �����̽��� �Է� ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ���� ���õ� ��ư�� Ŭ�� �̺�Ʈ�� ȣ��
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                Button selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
                if (selectedButton != null)
                {
                    selectedButton.onClick.Invoke();
                }
            }
        }
    }


    // ��ư�� Ȱ��ȭ �޼���
    IEnumerator ButtonOn()
    {
        yield return new WaitForSeconds(4.5f);
        btnImg.SetActive(true);

        // ù��° ��ư�� ���õ�
        if (buttons.Length > 0)
        {
            Debug.Log("ù��° ��ư ����");
            EventSystem.current.SetSelectedGameObject(buttons[buttonIndex].gameObject);
            // �ʱ� ���̶���Ʈ ���¸� �����մϴ�.
            ApplySprite(buttons[buttonIndex], highlightedSprites[buttonIndex]);

        }
    }

    // ��ư�� ��������Ʈ ���� �޼���
    private void ApplySprite(Button button, Sprite sprite)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = sprite;
        }
    }


    // ���丮 ������ ��ư Ŭ�� �� �̵�
    public void StartGame()
    {
        SceneManager.LoadScene("StoryMenu");
    }

    public void Ending()
    {
        SceneManager.LoadScene("Ending");
    }
}
