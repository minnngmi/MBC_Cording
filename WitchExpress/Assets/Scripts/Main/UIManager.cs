using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI 사용을 위해 추가

public class UIManager : MonoBehaviour
{
    // 슬라이더 연결
    public Slider hpSlider; // HP 슬라이더 
    public Slider mpSlider; // MP 슬라이더 

    // MP 슬라이더의 Fill 연결
    public Image mpFill;
    // MP 상태에 따라 변경할 Fill 색상 변수 미리 지정
    private Color normalColor = Color.gray; // MP가 꽉 차지 않았을 때
    public Color fullMPColor; // 꽉 찼을 때

    // 스킬 버튼과 이펙트 게임오브젝트 연결
    public GameObject skillBtn;
    public GameObject skillBtnEffect;

    //사탕, 포션 UI text 연결
    public Text candyTxt;
    public Text posionTxt;


    private void Start()
    {
        // GameManager 인스턴스가 존재하는지 확인
        if (GameManager.Instance != null)
        {
            // OnHPChanged 이벤트 구독
            // HP 변경될 때마다 UpdateHPSlider 메서드 호출
            GameManager.Instance.OnHPChanged += UpdateHPSlider;

            // OnMPChanged 이벤트 구독
            // MP 변경될 때마다 UpdateMPSlider 메서드 호출
            GameManager.Instance.OnMPChanged += UpdateMPSlider;


            // 초기 HP 슬라이더 동기화
            hpSlider.maxValue = GameManager.Instance.maxHP; // HP의 최대값
            hpSlider.value = GameManager.Instance.playerHP;

            // 초기 MP 슬라이더 동기화
            mpSlider.maxValue = GameManager.Instance.maxMP;  // MP의 최대값
            mpSlider.value = GameManager.Instance.playerMP;

            // 스킬 버튼과 이펙트 비활성화
            skillBtn.SetActive(false);
            skillBtnEffect.SetActive(false);

            // 캔디, 포션 카운트 초기화 0
            candyTxt.text = "0";
            posionTxt.text = "0";
        }
    }

    private void Update()
    {
        int candyCount = GameManager.Instance.candyCount;
        int HPpotion = GameManager.Instance.HPpotion;
        
        candyTxt.text = candyCount.ToString();
        posionTxt.text = HPpotion.ToString();
    }

    // GameManager에서 이벤트 발생 시 호출될 메서드

    // 1. HP 슬라이더 업데이트(동기화) 메서드 
    private void UpdateHPSlider(int currentHP)
    {
        // HP 슬라이더 값 업데이트
        hpSlider.value = currentHP;

        // HP가 0이 되면 게임 오버 화면을 표시하는 등의 추가 작업
        if (currentHP <= 0)
        {
            // TODO: 게임 오버 UI 표시
            Debug.Log("UI Manager: 게임 오버 UI를 표시합니다.");
        }
    }

    //  2. MP 슬라이더 업데이트 메서드
    private void UpdateMPSlider(int currentMP)
    {
        // MP 슬라이더 값 업데이트
        mpSlider.value = currentMP;

        // MP가 100이 되면
        if (currentMP == 100)
        {
            // 슬라이더 색상 변화
            mpFill.color = fullMPColor;
            // 스킬 버튼과 버튼 이펙트 활성
            skillBtn.SetActive(true);
            skillBtnEffect.SetActive(true);
        }

        else // MP가 100이 아닐 때
        {
            mpFill.color = normalColor;
            skillBtn.SetActive(false);
            skillBtnEffect.SetActive(false);
        }
    }

    // 게임 종료 시 이벤트 구독 해제 (선택 사항이지만 좋은 습관)
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnHPChanged -= UpdateHPSlider;
            GameManager.Instance.OnMPChanged -= UpdateMPSlider; 
        }
    }
}
