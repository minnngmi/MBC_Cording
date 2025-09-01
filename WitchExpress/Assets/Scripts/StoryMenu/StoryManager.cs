using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StoryManager : MonoBehaviour
{
    // ���丮 �̹������� ���� �迭
    public Sprite[] storyImages;

    // ���丮�� ������ �̹��� ������Ʈ
    public Image storyImgHolder;
    // ���̵� ȿ���� ���� ���ο� �̹��� ������Ʈ �߰�
    public Image nextImgHolder;

    // ���� �̹��� �ε���
    private int currentImgIndex = 0;

    void Start()
    {
        if (storyImages.Length > 0)
        {
            // ���� �̹��� Ȧ�� ��Ȱ��ȭ
            nextImgHolder.gameObject.SetActive(false);
            // ù ��° �̹��� 
            storyImgHolder.sprite = storyImages[currentImgIndex];
        }
    }
    void Update()
    {
        // ���콺 ���� ��ư Ŭ�� �Ǵ� �����̽��� �Է� ����
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            currentImgIndex++;
            NextImage();
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            StopAllCoroutines();
            StartCoroutine(Loading());
        }
    }

    // ���� �̹��� ��ȯ �޼��� (���̵� ȿ�� �߰�)
    void NextImage()
    {
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
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Loading");

    }

}
