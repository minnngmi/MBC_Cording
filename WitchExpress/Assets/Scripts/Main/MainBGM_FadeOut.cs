using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBGM_FadeOut : MonoBehaviour
{
    private AudioSource audioSource;
    private bool isFading = false;

    [Header("���� ����")]
    // ���� ���� (0.0���� ����)
    public float startVolume = 0.0f;
    // ��ǥ ���� ����
    public float targetVolume = 0.6f;
    // ������ ������ Ŀ���� �ð� (����: ��)
    public float fadeDuration = 5.0f;

    // ��ũ��Ʈ�� ���۵� �� AudioSource ������Ʈ�� �����ɴϴ�.
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // ������ ����� �� ������ 0.0���� �����ϵ��� �����մϴ�.
        audioSource.volume = startVolume;
        // ���̵� �� �ڷ�ƾ ����
        StartCoroutine(FadeInMusic());
    }

    // ���̵�ƿ� ���� �޼���
    // fadeTime: ������ �پ��� �� �ɸ��� �ð� (��)
    public void FadeOutBGM(float fadeTime)
    {

        Debug.Log("ȣ���");
        StartCoroutine(FadeOutCoroutine(fadeTime));

    }


    // ���̵��� �ڷ�ƾ
    IEnumerator FadeInMusic()
    {
        float timer = 0f; // ��� �ð� ������ ����

        // ������ �ð�(fadeDuration) ���� �ݺ��մϴ�.
        while (timer < fadeDuration)
        {
            // ��� �ð��� ���մϴ�.
            timer += Time.deltaTime;

            // ���� ������ ����մϴ�. Mathf.Lerp �Լ��� ����Ͽ� �ڿ������� ������ �����մϴ�.
            // Lerp�� ���� ���� �� �� ���̸� '����'�� ���� �������� �̵����� �ݴϴ�.
            // timer / fadeDuration �� 0.0 ~ 1.0 ������ ������ �˴ϴ�.
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, timer / fadeDuration);

            // ���� �����ӱ��� ��ٸ��ϴ�.
            yield return null;
        }

        // �ݺ����� ���� ��, ��Ȯ�� ��ǥ ������ �����ϵ��� ���� �����մϴ�.
        audioSource.volume = targetVolume;
    }


    // ���̵�ƿ�  �ڷ�ƾ
    private IEnumerator FadeOutCoroutine(float fadeTime)
    {
        isFading = true;  // �ڷ�ƾ�� ���۵Ǿ����� ǥ��
        Debug.Log("�ڷ�ƾ ����");
        float startVolume = audioSource.volume;
        float timer = 0f;

        // Ÿ�̸Ӱ� ������ fadeTime�� ������ ������ �ݺ�
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            // Mathf.Lerp�� ����� ���� ������ 0f���� �ε巴�� ����
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeTime);
            yield return null; // �� ������(���� ȭ���� �� �� �����̴� �ð�) ��ٸ��ϴ�.
        }

        // ���̵�ƿ��� �Ϸ�Ǹ� BGM�� ������ ����
        audioSource.Stop();

        // ������ �ٽ� ������� ������ ���� ����� �غ�
        audioSource.volume = startVolume;

        isFading = false; // �ڷ�ƾ ���� ǥ��
        audioSource.Stop();
    }

}
