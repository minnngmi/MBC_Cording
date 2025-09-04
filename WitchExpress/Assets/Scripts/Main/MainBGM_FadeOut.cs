using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBGM_FadeOut : MonoBehaviour
{
    private AudioSource audioSource;
    private bool isFading = false;

    // ��ũ��Ʈ�� ���۵� �� AudioSource ������Ʈ�� �����ɴϴ�.
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // ���̵�ƿ� ���� �޼���
    // fadeTime: ������ �پ��� �� �ɸ��� �ð� (��)
    public void FadeOutBGM(float fadeTime)
    {

        Debug.Log("ȣ���");
        StartCoroutine(FadeOutCoroutine(fadeTime));

    }

    // ���̵�ƿ� ��� ���� �ڷ�ƾ
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
