using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIManager : MonoSingleton<MenuUIManager>
{
    public GameObject MenuBG;
    

    // ��ư �̹��� ��ü ������Ʈ
    public GameObject btnImg;

    // ��ư���� �迭�� ���� ����
    public Button[] buttons;
    private int buttonIndex = 0;

    // ��ư ���̶���Ʈ ȿ���� ���� ��������Ʈ��
    public Sprite[] normalSprites;
    public Sprite[] highlightedSprites;
    public Sprite[] pressedSprites;

    // ȿ���� ����� ���� AudioSource
    private AudioSource audioSource;
    // ����� ȿ���� Ŭ��
    public AudioClip buttonClickSound;

    // �ɼ�â �ִϸ����� ����
    public Animator OptionAnim;
    public Animator TitleAnim;

    // ESC Ű�� ���� �� �ֵ��� ������(ending) ���� Ȱ��ȭ�� �������� Ȯ���ϴ� ����
    private bool isSceneLoaded = false;
    private bool sceneAlreadyLoad = false;
 


    protected override void Awake()
    {
        base.Awake();
        Debug.Log("SceneStart");

        MenuBG.TryGetComponent(out TitleAnim);
        TitleAnim.SetBool("MenuBGOnOff", true);
    }

    private void SetAnimBool()
    {
        sceneAlreadyLoad = true;
    }

    void Start()
    {
        if (!sceneAlreadyLoad) StartCoroutine(ButtonOn());
        MenuBG.SetActive(true);
        btnImg.SetActive(false); 


        // ȿ���� ����� ���� AudioSource ������Ʈ�� �������ų� �߰��մϴ�.
        audioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {
        // ��ư �̹����� Ȱ��ȭ �Ǿ������� Ű���� Ž�� ���
        if (!btnImg.activeInHierarchy)
            return;

        // �� ����Ű �Է� ����
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // ȿ���� ���
            PlaySound();

            // ���� ��ư�� ��������Ʈ ����
            ApplySprite(buttons[buttonIndex], normalSprites[buttonIndex]);

            buttonIndex--;
            if (buttonIndex < 0)
            {
                buttonIndex = buttons.Length - 1;
            }
            // ���� ���õ� ��ư ����
            EventSystem.current.SetSelectedGameObject(buttons[buttonIndex].gameObject);
            ApplySprite(buttons[buttonIndex], highlightedSprites[buttonIndex]);

        }

        // �Ʒ� ����Ű �Է� ����
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // ȿ���� ���
            PlaySound();

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

        // ���� �ε�� ���¿��� ESC Ű�� ������
        if (isSceneLoaded && Input.GetKeyDown(KeyCode.Escape))
        {
            // ���� ��ε�(�ݱ�)�մϴ�.
            SceneManager.UnloadSceneAsync("Ending");
            isSceneLoaded = false;
        }
    }

    // ��ư Ȱ��ȭ �޼���
    IEnumerator ButtonOn()
    {
           yield return new WaitForSeconds(4.5f);
            
            btnImg.SetActive(true);
        

        // ù��° ��ư�� ���õ�
        if (buttons.Length > 0)
        {
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

    // ȿ���� ��� �޼���
    private void PlaySound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }


    // ���丮 ������ ��ư Ŭ�� �� �̵�
    public void StartGame()
    {
        SceneManager.LoadScene("StoryMenu");
    }

    // ���� �ٹ� ������ ��ư Ŭ�� �� �̵�
    public void Ending()
    {
        if (!isSceneLoaded)
        {
            //  ���� ���� �ı����� �ʰ� "Ending" ���� �߰� �ε�
            SceneManager.LoadSceneAsync("Ending", LoadSceneMode.Additive);
            SetAnimBool();
            isSceneLoaded = true;
        }

        //SceneManager.LoadScene("Ending");
    }

    // �ɼ� â�� ��ư Ŭ�� �� on,off��
    public void Option()
    {
        OptionAnim.SetTrigger("OptionOnOFF");
    }
}
