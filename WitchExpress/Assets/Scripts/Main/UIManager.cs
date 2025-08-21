using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI 사용을 위해 추가

public class UIManager : MonoBehaviour
{
    // 인스펙터 창에서 슬라이더를 연결
    public Slider hpSlider; // HP 슬라이더 연결
    public Slider mpSlider; // MP 슬라이더 연결


    // MP 슬라이더의 채워지는 부분(Fill)을 연결
    public Image mpFill;
    // MP 상태에 따라 변경할 Fill 색상 변수 미리 지정
    private Color normalColor = Color.gray; // MP가 꽉 차지 않았을 때
    public Color fullMPColor; // 꽉 찼을 때

    // 스킬 버튼과 이펙트 게임오브젝트를 연결할 변수
    public GameObject skillBtn;
    public GameObject skillBtnEffect;



    private void Start()
    {
        // GameManager 인스턴스가 존재하는지 확인
        if (GameManager.Instance != null)
        {
            // GameManager의 OnHPChanged 이벤트 구독
            // HP가 변경될 때마다 UpdateHPSlider 메서드가 호출
            GameManager.Instance.OnHPChanged += UpdateHPSlider;

            // GameManager의 OnMPChanged 이벤트 구독
            // MP가 변경될 때마다 UpdateMPSlider 메서드가 호출
            GameManager.Instance.OnMPChanged += UpdateMPSlider;


            // 게임 시작 시 초기 HP 슬라이더 설정
            hpSlider.maxValue = GameManager.Instance.maxHP; // HP의 최대값
            hpSlider.value = GameManager.Instance.playerHP;

            // 게임 시작 시 초기 MP 슬라이더 설정
            mpSlider.maxValue = GameManager.Instance.maxMP;  // MP의 최대값
            mpSlider.value = GameManager.Instance.playerMP;


            // 게임 시작 시(MP 0) 스킬 버튼과 이펙트를 비활성화
            if (skillBtn != null)
            {
                skillBtn.SetActive(false);
            }
            if (skillBtnEffect != null)
            {
                skillBtnEffect.SetActive(false);
            }

        }
    }

    // GameManager에서 이벤트가 발생하면 호출될 메서드

    // 1. HP 슬라이더를 업데이트 하는 메서드 
    private void UpdateHPSlider(int currentHP)
    {
        // HP 슬라이더 값을 업데이트
        hpSlider.value = currentHP;

        // HP가 0이 되면 게임 오버 화면을 표시하는 등의 추가 작업
        if (currentHP <= 0)
        {
            // TODO: 게임 오버 UI 표시
            Debug.Log("UI Manager: 게임 오버 UI를 표시합니다.");
        }
    }

    //  2. MP 슬라이더를 업데이트하는 메서드
    private void UpdateMPSlider(int currentMP)
    {
        // MP 슬라이더 값을 업데이트
        mpSlider.value = currentMP;

        // MP가 100이 되면
        if (currentMP == 100)
        {
            // 색상 변화
            if (mpFill != null)
            {
                mpFill.color = fullMPColor;
            }

            // 스킬 버튼과 이펙트를 활성화
            if (skillBtn != null)
            {
                skillBtn.SetActive(true);
            }
            if (skillBtnEffect != null)
            {
                skillBtnEffect.SetActive(true);
            }
        }

        else // MP가 100이 아닐 때
        {
            if (mpFill != null)
            {
                mpFill.color = normalColor;
            }

            //  스킬 버튼과 이펙트를 비활성화
            if (skillBtn != null)
            {
                skillBtn.SetActive(false);
            }
            if (skillBtnEffect != null)
            {
                skillBtnEffect.SetActive(false);
            }
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
