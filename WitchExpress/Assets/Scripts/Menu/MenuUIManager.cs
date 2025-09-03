using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIManager : MonoSingleton<MenuUIManager>
{
    public GameObject MenuBG;
    

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
    public Animator TitleAnim;

    // ESC 키로 닫을 수 있도록 갤러리(ending) 씬이 활성화된 상태인지 확인하는 변수
    private bool isSceneLoaded = false;
    private bool sceneAlreadyLoad = false;
 


    protected override void Awake()
    {
        base.Awake();
        Debug.Log("SceneStart");

        MenuBG.TryGetComponent(out TitleAnim);
        TitleAnim.SetBool("MenuBGOnOff", true);
    }

    private void SetAnimBool()
    {
        sceneAlreadyLoad = true;
    }

    void Start()
    {
        if (!sceneAlreadyLoad) StartCoroutine(ButtonOn());
        MenuBG.SetActive(true);
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
                }
            }
        }

        // 씬이 로드된 상태에서 ESC 키를 누르면
        if (isSceneLoaded && Input.GetKeyDown(KeyCode.Escape))
        {
            // 씬을 언로드(닫기)합니다.
            SceneManager.UnloadSceneAsync("Ending");
            isSceneLoaded = false;
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

    // 엔딩 앨범 씬으로 버튼 클릭 시 이동
    public void Ending()
    {
        if (!isSceneLoaded)
        {
            //  기존 씬을 파괴하지 않고 "Ending" 씬을 추가 로드
            SceneManager.LoadSceneAsync("Ending", LoadSceneMode.Additive);
            SetAnimBool();
            isSceneLoaded = true;
        }

        //SceneManager.LoadScene("Ending");
    }

    // 옵션 창이 버튼 클릭 시 on,off됨
    public void Option()
    {
        OptionAnim.SetTrigger("OptionOnOFF");
    }
}
