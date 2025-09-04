using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StoryManager : MonoBehaviour
{

    [Header("�̹��� ����")]
    // ���丮 �̹������� ���� �迭
    public Sprite[] storyImages;

    // ���丮�� ������ ���� �̹��� ������Ʈ
    public Image storyImgHolder;

    // ���̵� ȿ���� ����  ���� �� �̹���  ������Ʈ �߰�
    public Image nextImgHolder;

    // ���� �̹��� �ε���
    private int currentImgIndex = 0;

    [Header("���� ����")]
    public AudioSource storyBGM01;
    public AudioSource storyBGM02;


    void Start()
    {
        // ù��° ���� ��� 
        storyBGM01.Play();

        if (storyImages.Length > 0)
        {
            //  ���� �� �̹��� ��Ȱ��ȭ
            nextImgHolder.gameObject.SetActive(false);
            // ù ��° �̹��� 
            storyImgHolder.sprite = storyImages[currentImgIndex];
        }
    }
    void Update()
    {
        // �����̽��� �Է� ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentImgIndex++;

            // ���� �������
            NextImage();
        }

        // ����Ű �Է� ����
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            StopAllCoroutines();
            // ���� ��� ����
            StartCoroutine(Loading());
        }

        // ESC Ű �Է� ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopAllCoroutines();
            // ���� �޴������� 
            SceneManager.LoadScene("Menu");
        }
    }

    // ���� �̹��� ��ȯ �޼���
    void NextImage()
    {
        // �ε��� �ѹ� 12�� �� ���� ü����
        if (currentImgIndex == 13)
        {
            storyBGM01.Stop();
            storyBGM02.Play();
        }

        // ���̵� ȿ�� �ڷ�ƾ ����
        StartCoroutine(FadeToNextImage());
    }


    // �̹����� �����ϰ� ������ٰ� �ٽ� ������ ��Ÿ���� �ϴ� �ڷ�ƾ
    IEnumerator FadeToNextImage()
    {
        float fadeTime = 0.3f; // ���̵� ȿ�� ���� �ð�

        // ���� �̹����� �����ϸ�
        if (currentImgIndex < storyImages.Length)
        {

            nextImgHolder.sprite = storyImages[currentImgIndex];

            float timer = 0f;
            Color color = nextImgHolder.color;
            color.a = 0f;
            nextImgHolder.color = color;

            nextImgHolder.gameObject.SetActive(true);

            // ���� �̹����� ������ ��Ÿ���� �մϴ� (���̵� ��)
            while (timer < fadeTime)
            {
                timer += Time.deltaTime;
                color.a = Mathf.Lerp(0f, 1f, timer / fadeTime);
                nextImgHolder.color = color;
                yield return null;
            }

            // ���̵� �� �Ϸ� �� �̹��� ��ü �� nextImageHolder ��Ȱ��ȭ
            storyImgHolder.sprite = storyImages[currentImgIndex];
            nextImgHolder.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(Loading());
        }
    }

    IEnumerator Loading()
    {
        // �� �̻� �̹����� ������ ���� ���� ������ �̵��մϴ�.
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Loading");
    }

}
