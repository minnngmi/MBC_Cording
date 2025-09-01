using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StoryManager : MonoBehaviour
{
    // 스토리 이미지들을 담을 배열
    public Sprite[] storyImages;

    // 스토리를 보여줄 이미지 컴포넌트
    public Image storyImgHolder;
    // 페이드 효과를 위한 새로운 이미지 컴포넌트 추가
    public Image nextImgHolder;

    // 현재 이미지 인덱스
    private int currentImgIndex = 0;

    void Start()
    {
        if (storyImages.Length > 0)
        {
            // 다음 이미지 홀더 비활성화
            nextImgHolder.gameObject.SetActive(false);
            // 첫 번째 이미지 
            storyImgHolder.sprite = storyImages[currentImgIndex];
        }
    }
    void Update()
    {
        // 마우스 왼쪽 버튼 클릭 또는 스페이스바 입력 감지
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

    // 다음 이미지 전환 메서드 (페이드 효과 추가)
    void NextImage()
    {
        // 페이드 효과 코루틴 실행
        StartCoroutine(FadeToNextImage());
    }


    // 이미지를 투명하게 만들었다가 다시 서서히 나타나게 하는 코루틴
    IEnumerator FadeToNextImage()
    {
        float fadeTime = 0.3f; // 페이드 효과 지속 시간

        // 다음 이미지가 존재하면
        if (currentImgIndex < storyImages.Length)
        {
            nextImgHolder.sprite = storyImages[currentImgIndex];

            float timer = 0f;
            Color color = nextImgHolder.color;
            color.a = 0f;
            nextImgHolder.color = color;

            nextImgHolder.gameObject.SetActive(true);

            // 다음 이미지를 서서히 나타나게 합니다 (페이드 인)
            while (timer < fadeTime)
            {
                timer += Time.deltaTime;
                color.a = Mathf.Lerp(0f, 1f, timer / fadeTime);
                nextImgHolder.color = color;
                yield return null;
            }

            // 페이드 인 완료 후 이미지 교체 및 nextImageHolder 비활성화
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
        // 더 이상 이미지가 없으면 메인 게임 씬으로 이동합니다.
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Loading");

    }

}
