using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // ����� ȿ���� Ŭ��
    public AudioClip buttonClickSound;

    // ȿ���� ����� ���� AudioSource
    private AudioSource audioSource;

    private void Awake()
    {
        // �� ��ũ��Ʈ�� ������ ������Ʈ���� AudioSource ������Ʈ�� �������ų� �߰��մϴ�.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // �ܺο��� ȣ���Ͽ� ȿ������ ����ϴ� �޼���
    public void PlayButtonClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}
