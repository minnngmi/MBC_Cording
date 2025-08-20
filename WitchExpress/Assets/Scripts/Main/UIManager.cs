using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI 사용을 위해 추가

public class UIManager : MonoBehaviour
{
    // 인스펙터 창에서 슬라이더를 연결할 변수
    public Slider hpSlider;

    private void Start()
    {
        // GameManager 인스턴스가 존재하는지 확인
        if (GameManager.Instance != null)
        {
            // GameManager의 OnHPChanged 이벤트를 구독합니다.
            // HP가 변경될 때마다 UpdateHPSlider 메서드가 호출됩니다.
            GameManager.Instance.OnHPChanged += UpdateHPSlider;

            // 게임 시작 시 초기 HP로 슬라이더를 설정합니다.
            hpSlider.maxValue = GameManager.Instance.playerHP;
            hpSlider.value = GameManager.Instance.playerHP;
        }
    }

    // GameManager에서 이벤트가 발생하면 호출될 메서드
    private void UpdateHPSlider(int currentHP)
    {
        // HP 슬라이더의 값을 업데이트합니다.
        hpSlider.value = currentHP;

        // HP가 0이 되면 게임 오버 화면을 표시하는 등의 추가 작업
        if (currentHP <= 0)
        {
            // TODO: 게임 오버 UI 표시
            Debug.Log("UI Manager: 게임 오버 UI를 표시합니다.");
        }
    }

    // 게임 종료 시 이벤트 구독 해제 (선택 사항이지만 좋은 습관)
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnHPChanged -= UpdateHPSlider;
        }
    }
}
