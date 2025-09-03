using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnManager : MonoBehaviour
{
    // ��ư���� �迭�� ���� ����
    public Button[] buttons;
    private int buttonIndex = 0;

    // ��ư ���̶���Ʈ ȿ���� ���� ��������Ʈ��
    public Sprite normalSprites;
    public Sprite highlightedSprites;
    public Sprite pressedSprites;

    // ȿ���� ����� ���� AudioSource
    private AudioSource audioSource;
    // ����� ȿ���� Ŭ��
    public AudioClip buttonClickSound;

    // 2��° ���� ���� �̹���
    public GameObject btn02BG;

    private void Start()
    {
        btn02BG.SetActive(false);

        // ù��° ��ư�� ���õ�
        if (buttons.Length > 0)
        {
            EventSystem.current.SetSelectedGameObject(buttons[buttonIndex].gameObject);
            // �ʱ� ���̶���Ʈ ���¸� �����մϴ�.
            ApplySprite(buttons[0], highlightedSprites);
        }
        // ȿ���� ����� ���� AudioSource ������Ʈ�� �������ų� �߰��մϴ�.
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // ���� ����Ű �Է� ����
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // ȿ���� ���
            PlaySound();

            // ���� ��ư�� ��������Ʈ ����
            ApplySprite(buttons[buttonIndex], normalSprites);


            buttonIndex--;
            if (buttonIndex < 0)
            {
                buttonIndex = buttons.Length - 1;
            }
            // ���� ���õ� ��ư ����
            EventSystem.current.SetSelectedGameObject(buttons[buttonIndex].gameObject);
            ApplySprite(buttons[buttonIndex], highlightedSprites);

        }

        // ������ ����Ű �Է� ����
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // ȿ���� ���
            PlaySound();

            // ���� ��ư�� ��������Ʈ ����
            ApplySprite(buttons[buttonIndex], normalSprites);

            buttonIndex++;
            if (buttonIndex >= buttons.Length)
            {
                buttonIndex = 0;
            }
            // ���� ���õ� ��ư ����
            EventSystem.current.SetSelectedGameObject(buttons[buttonIndex].gameObject);
            ApplySprite(buttons[buttonIndex], highlightedSprites);
        }

        // �Ʒ� ����Ű �Է� ����
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // ȿ���� ���
            PlaySound();

            // ���� ��ư�� ��������Ʈ ����
            ApplySprite(buttons[buttonIndex], normalSprites);

            buttonIndex = buttonIndex + 2;
            if (buttonIndex >= buttons.Length)
            {
                buttonIndex = 0;
            }
            // ���� ���õ� ��ư ����
            EventSystem.current.SetSelectedGameObject(buttons[buttonIndex].gameObject);
            ApplySprite(buttons[buttonIndex], highlightedSprites);
        }

        // �� ����Ű �Է� ����
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // ȿ���� ���
            PlaySound();

            // ���� ��ư�� ��������Ʈ ����
            ApplySprite(buttons[buttonIndex], normalSprites);

            buttonIndex = buttonIndex - 2;

            if (buttonIndex < 0)
            {
                buttonIndex = buttons.Length - 1;
            }

            // ���� ���õ� ��ư ����
            EventSystem.current.SetSelectedGameObject(buttons[buttonIndex].gameObject);
            ApplySprite(buttons[buttonIndex], highlightedSprites);
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


    // ��ư�� ��������Ʈ ���� �޼���
    private void ApplySprite(Button button, Sprite sprite)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = sprite;
        }
    }

    // ȿ���� ��� �޼���
    private void PlaySound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }

    // Button02 �۵� �޼���
    public void Btn02()
    {
        btn02BG.SetActive(!btn02BG.activeSelf);
    }

}
