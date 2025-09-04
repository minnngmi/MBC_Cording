using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBGM_FadeOut : MonoBehaviour
{
    private AudioSource audioSource;
    private bool isFading = false;

    // 스크립트가 시작될 때 AudioSource 컴포넌트를 가져옵니다.
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // 페이드아웃 실행 메서드
    // fadeTime: 볼륨이 줄어드는 데 걸리는 시간 (초)
    public void FadeOutBGM(float fadeTime)
    {

        Debug.Log("호출됨");
        StartCoroutine(FadeOutCoroutine(fadeTime));

    }

    // 페이드아웃 기능 수행 코루틴
    private IEnumerator FadeOutCoroutine(float fadeTime)
    {
        isFading = true;  // 코루틴이 시작되었음을 표시
        Debug.Log("코루틴 시작");
        float startVolume = audioSource.volume;
        float timer = 0f;

        // 타이머가 지정된 fadeTime에 도달할 때까지 반복
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            // Mathf.Lerp를 사용해 현재 볼륨을 0f까지 부드럽게 감소
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeTime);
            yield return null; // 한 프레임(게임 화면이 한 번 깜빡이는 시간) 기다립니다.
        }

        // 페이드아웃이 완료되면 BGM을 완전히 정지
        audioSource.Stop();

        // 볼륨을 다시 원래대로 복원해 다음 재생을 준비
        audioSource.volume = startVolume;

        isFading = false; // 코루틴 종료 표시
        audioSource.Stop();
    }

}
