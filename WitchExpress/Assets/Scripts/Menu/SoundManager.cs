using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // 재생할 효과음 클립
    public AudioClip buttonClickSound;

    // 효과음 재생을 위한 AudioSource
    private AudioSource audioSource;

    private void Awake()
    {
        // 이 스크립트가 부착된 오브젝트에서 AudioSource 컴포넌트를 가져오거나 추가합니다.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // 외부에서 호출하여 효과음을 재생하는 메서드
    public void PlayButtonClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}
