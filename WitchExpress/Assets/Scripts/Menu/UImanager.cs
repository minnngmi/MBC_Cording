using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    // 버튼 이미지 전체 오브젝트
    public GameObject btnImg;

    // 버튼들을 배열로 순서 관리
    public Button[] buttons;
    private int buttonIndex = 0;

    // 버튼 하이라이트 효과를 위한 스프라이트들
    public Sprite[] normalSprites;
    public Sprite[] highlightedSprites;
    public Sprite[] pressedSprites;


    void Start()
    {
        StartCoroutine(ButtonOn());
        btnImg.SetActive(false);
    }


    private void Update()
    {
        // 버튼 이미지가 활성화 되었을때만 키보드 탐색 허용
        if (!btnImg.activeInHierarchy)
            return;


        // 위 방향키 입력 감지
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // 이전 버튼의 스프라이트 해제
            ApplySprite(buttons[buttonIndex], normalSprites[buttonIndex]);

            buttonIndex--;
            if (buttonIndex < 0)
            {
                buttonIndex = buttons.Length - 1;
                Debug.Log("위");
            }
            // 현재 선택된 버튼 변경
            EventSystem.current.SetSelectedGameObject(buttons[buttonIndex].gameObject);
            ApplySprite(buttons[buttonIndex], highlightedSprites[buttonIndex]);
        }

        // 아래 방향키 입력 감지
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // 이전 버튼의 스프라이트 해제
            ApplySprite(buttons[buttonIndex], normalSprites[buttonIndex]);

            buttonIndex++;
            if (buttonIndex >= buttons.Length)
            {
                buttonIndex = 0;
            }
            // 현재 선택된 버튼 변경
            EventSystem.current.SetSelectedGameObject(buttons[buttonIndex].gameObject);
            ApplySprite(buttons[buttonIndex], highlightedSprites[buttonIndex]);

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


    // 버튼이 활성화 메서드
    IEnumerator ButtonOn()
    {
        yield return new WaitForSeconds(4.5f);
        btnImg.SetActive(true);

        // 첫번째 버튼이 선택됨
        if (buttons.Length > 0)
        {
            Debug.Log("첫번째 버튼 선택");
            EventSystem.current.SetSelectedGameObject(buttons[buttonIndex].gameObject);
            // 초기 하이라이트 상태를 적용합니다.
            ApplySprite(buttons[buttonIndex], highlightedSprites[buttonIndex]);

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


    // 스토리 씬으로 버튼 클릭 시 이동
    public void StartGame()
    {
        SceneManager.LoadScene("StoryMenu");
    }

    public void Ending()
    {
        SceneManager.LoadScene("Ending");
    }
}
