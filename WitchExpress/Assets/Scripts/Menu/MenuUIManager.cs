using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
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

    // 효과음 재생을 위한 AudioSource
    private AudioSource audioSource;

    // 재생할 효과음 클립
    public AudioClip buttonClickSound;

    // 옵션창 애니메이터 변수
    public Animator OptionAnim;

    // ESC 키로 닫을 수 있도록 갤러리(ending) 씬이 활성화된 상태인지 확인하는 변수
    private bool isSceneLoaded = false;
 

    void Start()
    {
        StartCoroutine(ButtonOn());

        btnImg.SetActive(false); 

        // 효과음 재생을 위한 AudioSource 컴포넌트를 가져오거나 추가합니다.
        audioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {
        // 버튼 이미지가 활성화 되었을때만 키보드 탐색 허용
        if (!btnImg.activeInHierarchy)
            return;

        // 위 방향키 입력 감지
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // 효과음 재생
            PlaySound();

            // 이전 버튼의 스프라이트 해제
            ApplySprite(buttons[buttonIndex], normalSprites[buttonIndex]);

            buttonIndex--;
            if (buttonIndex < 0)
            {
                buttonIndex = buttons.Length - 1;
            }
            // 현재 선택된 버튼 변경
            EventSystem.current.SetSelectedGameObject(buttons[buttonIndex].gameObject);
            ApplySprite(buttons[buttonIndex], highlightedSprites[buttonIndex]);

        }

        // 아래 방향키 입력 감지
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // 효과음 재생
            PlaySound();

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

                    // 효과음 재생
                    PlaySound();
                }
            }
        }

        // ESC 키 입력 감지
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 현재 "OptionOnOFF" 파라미터의 값 가져오기
            bool isOptionOn = OptionAnim.GetBool("OptionOn");

            // 만약 현재 값이 true이면, false로 변경
            if (isOptionOn)
            {
                OptionAnim.SetBool("OptionOn", false);
            }

            // 만약 씬이 로드된 상태라면, 씬 언로드
            if (isSceneLoaded)
            {
                // 씬을 언로드(닫기)합니다.
                SceneManager.UnloadSceneAsync("Gallery");
                isSceneLoaded = false;
            }
        }
    }

    // 버튼 활성화 메서드
    IEnumerator ButtonOn()
    {
        yield return new WaitForSeconds(4.5f);
        btnImg.SetActive(true);
        
        // 첫번째 버튼이 선택됨
        if (buttons.Length > 0)
        {
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

    // 효과음 재생 메서드
    private void PlaySound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }

    // 스토리 씬으로 버튼 클릭 시 이동
    public void StartGame()
    {
        SceneManager.LoadScene("StoryMenu");
    }

    // 엔딩 Gallery 씬으로 버튼 클릭 시 이동
    public void Ending()
    {
        if (!isSceneLoaded)
        {
            //  기존 씬을 파괴하지 않고 "Ending" 씬을 추가 로드
            SceneManager.LoadSceneAsync("Gallery", LoadSceneMode.Additive);
            isSceneLoaded = true;
        }

        //SceneManager.LoadScene("Ending");
    }

    // 버튼 클릭 시 옵션창 나타나기
    public void Option()
    {
        OptionAnim.SetBool("OptionOn", true);
    }
}
