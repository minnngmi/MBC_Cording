using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBGM_FadeOut : MonoBehaviour
{
    private AudioSource audioSource;
    private bool isFading = false;

    [Header("볼륨 설정")]
    // 시작 볼륨 (0.0으로 설정)
    public float startVolume = 0.0f;
    // 목표 볼륨 설정
    public float targetVolume = 0.6f;
    // 볼륨이 서서히 커지는 시간 (단위: 초)
    public float fadeDuration = 5.0f;

    // 스크립트가 시작될 때 AudioSource 컴포넌트를 가져옵니다.
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // 음악이 재생될 때 볼륨이 0.0에서 시작하도록 설정합니다.
        audioSource.volume = startVolume;
        // 페이드 인 코루틴 시작
        StartCoroutine(FadeInMusic());
    }

    // 페이드아웃 실행 메서드
    // fadeTime: 볼륨이 줄어드는 데 걸리는 시간 (초)
    public void FadeOutBGM(float fadeTime)
    {

        Debug.Log("호출됨");
        StartCoroutine(FadeOutCoroutine(fadeTime));

    }


    // 페이드인 코루틴
    IEnumerator FadeInMusic()
    {
        float timer = 0f; // 경과 시간 측정용 변수

        // 설정한 시간(fadeDuration) 동안 반복합니다.
        while (timer < fadeDuration)
        {
            // 경과 시간을 더합니다.
            timer += Time.deltaTime;

            // 현재 볼륨을 계산합니다. Mathf.Lerp 함수를 사용하여 자연스러운 보간을 적용합니다.
            // Lerp는 시작 값과 끝 값 사이를 '비율'에 맞춰 선형으로 이동시켜 줍니다.
            // timer / fadeDuration 은 0.0 ~ 1.0 사이의 비율이 됩니다.
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, timer / fadeDuration);

            // 다음 프레임까지 기다립니다.
            yield return null;
        }

        // 반복문이 끝난 후, 정확한 목표 볼륨에 도달하도록 최종 설정합니다.
        audioSource.volume = targetVolume;
    }


    // 페이드아웃  코루틴
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
