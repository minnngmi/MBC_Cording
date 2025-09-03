using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnManager : MonoBehaviour
{
    // 버튼들을 배열로 순서 관리
    public Button[] buttons;
    private int buttonIndex = 0;

    // 버튼 하이라이트 효과를 위한 스프라이트들
    public Sprite normalSprites;
    public Sprite highlightedSprites;
    public Sprite pressedSprites;

    // 효과음 재생을 위한 AudioSource
    private AudioSource audioSource;
    // 재생할 효과음 클립
    public AudioClip buttonClickSound;

    // 2번째 엔딩 사진 이미지
    public GameObject btn02BG;

    private void Start()
    {
        btn02BG.SetActive(false);

        // 첫번째 버튼이 선택됨
        if (buttons.Length > 0)
        {
            EventSystem.current.SetSelectedGameObject(buttons[buttonIndex].gameObject);
            // 초기 하이라이트 상태를 적용합니다.
            ApplySprite(buttons[0], highlightedSprites);
        }
        // 효과음 재생을 위한 AudioSource 컴포넌트를 가져오거나 추가합니다.
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // 왼쪽 방향키 입력 감지
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // 효과음 재생
            PlaySound();

            // 이전 버튼의 스프라이트 해제
            ApplySprite(buttons[buttonIndex], normalSprites);


            buttonIndex--;
            if (buttonIndex < 0)
            {
                buttonIndex = buttons.Length - 1;
            }
            // 현재 선택된 버튼 변경
            EventSystem.current.SetSelectedGameObject(buttons[buttonIndex].gameObject);
            ApplySprite(buttons[buttonIndex], highlightedSprites);

        }

        // 오른쪽 방향키 입력 감지
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // 효과음 재생
            PlaySound();

            // 이전 버튼의 스프라이트 해제
            ApplySprite(buttons[buttonIndex], normalSprites);

            buttonIndex++;
            if (buttonIndex >= buttons.Length)
            {
                buttonIndex = 0;
            }
            // 현재 선택된 버튼 변경
            EventSystem.current.SetSelectedGameObject(buttons[buttonIndex].gameObject);
            ApplySprite(buttons[buttonIndex], highlightedSprites);
        }

        // 아래 방향키 입력 감지
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // 효과음 재생
            PlaySound();

            // 이전 버튼의 스프라이트 해제
            ApplySprite(buttons[buttonIndex], normalSprites);

            buttonIndex = buttonIndex + 2;
            if (buttonIndex >= buttons.Length)
            {
                buttonIndex = 0;
            }
            // 현재 선택된 버튼 변경
            EventSystem.current.SetSelectedGameObject(buttons[buttonIndex].gameObject);
            ApplySprite(buttons[buttonIndex], highlightedSprites);
        }

        // 위 방향키 입력 감지
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // 효과음 재생
            PlaySound();

            // 이전 버튼의 스프라이트 해제
            ApplySprite(buttons[buttonIndex], normalSprites);

            buttonIndex = buttonIndex - 2;

            if (buttonIndex < 0)
            {
                buttonIndex = buttons.Length - 1;
            }

            // 현재 선택된 버튼 변경
            EventSystem.current.SetSelectedGameObject(buttons[buttonIndex].gameObject);
            ApplySprite(buttons[buttonIndex], highlightedSprites);
        }

        // 스페이스바 입력 감지
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 현재 선택된 버튼의 클릭 이벤트를 호출
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                Button selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
                if (selectedButton != null)
                {
                    selectedButton.onClick.Invoke();
                }
            }
        }
    }


    // 버튼의 스프라이트 변경 메서드
    private void ApplySprite(Button button, Sprite sprite)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = sprite;
        }
    }

    // 효과음 재생 메서드
    private void PlaySound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }

    // Button02 작동 메서드
    public void Btn02()
    {
        btn02BG.SetActive(!btn02BG.activeSelf);
    }

}
