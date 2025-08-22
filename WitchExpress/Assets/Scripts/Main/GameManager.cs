using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;  // Action 사용을 위해 



public class GameManager : MonoBehaviour
{
    // 1. 싱글톤 인스턴스: static 변수로 자기 자신을 담을 그릇을 만듭니다.
    public static GameManager Instance;

    // 2. 관리할 데이터 변수들을 선언합니다.
    public int enemyKills;
    public int candyCount;
    public int playerHP = 100; // 초기 HP 설정
    public int maxHP = 100;
    public int playerMP = 0; // MP 변수 추가
    public int maxMP = 100;

    // 이벤트 모음
    // UIManager가 이 이벤트를 구독하여 HP/MP 슬라이더를 업데이트
    public Action<int> OnHPChanged; //  1) HP 변경 이벤트
    public Action<int> OnMPChanged; //  2) MP 변경 이벤트
    public Action OnSkillActivated; // 3) 스킬 사용 이벤트

    // 게임 상태 상수
    public enum GameState
    {
        Ready,
        Run,
        GameOver
    }

    // 현재의 게임 상태 변수
    public GameState gState;


    // 3. Awake 메서드: 로드될 때 가장 먼저 실행
    private void Awake()
    {
        // 인스턴스가 이미 존재하는지 확인
        if (Instance == null)
        {
            // 인스턴스가 없으면, 이 GameManager 오브젝트를 인스턴스로 지정
            Instance = this;

            // 씬이 바뀌어도 파괴되지 않게(필요에 따라 선택)
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 인스턴스가 이미 있다면, 새로 생성된 오브젝트를 파괴하여 중복막기
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 초기 게임 상태
        gState = GameState.Run;
    }

    // 4. 데이터를 변경하는 메서드들 모음

    // 1) 적 처치 횟수를 증가시키는 메서드
    public void IncreaseEnemyKills()
    {
        enemyKills++;
        IncreasePlayerMP(5);  // MP 5 식 증가
        Debug.Log("적 처치 수: " + enemyKills);

        // 추가 작업 (예: 특정 횟수 달성 시 아이템 생성)
        if (enemyKills % 10 == 0)
        {
            Debug.Log("축하합니다! 특별 아이템이 나타납니다!");
            // TODO: 특별 아이템 생성 코드를 여기에 작성
        }
    }

    // 2) 사탕 카운트를 증가시키는 메서드
    public void IncreaseCandyCount()
    {
        candyCount++;
        Debug.Log("사탕 획득 수: " + candyCount);
    }

    // 3) 플레이어 HP를 감소시키는 메서드
    public void DecreasePlayerHP(int damage)
    {
        playerHP -= damage;

        if (playerHP < 0)
        {
            playerHP = 0;
        }

        // HP가 변경되었음을 알리는 이벤트 호출
        // UIManager가 이 이벤트를 구독하고 있다면 UIManager의 코드가 실행됩니다.
        OnHPChanged?.Invoke(playerHP);

        if (playerHP <= 0)
        {
            playerHP = 0;
            Debug.Log("게임 오버!");
            // TODO: 게임 오버 처리 (화면 전환 등)
        }
    }

    // 4) MP를 증가시키는 메서드
    public void IncreasePlayerMP(int amount)
    {
        playerMP += amount;

        // MP가 최대치를 넘지 않도록 제한
        if (playerMP > maxMP)
        {
            playerMP = maxMP;

            //MP가 가득 차면 OnSkillActivated 이벤트를 호출하여 PlayerFire로 배달
            OnSkillActivated?.Invoke();
        }

        // MP가 변경되었음을 알리는 이벤트 호출
        OnMPChanged?.Invoke(playerMP);
    }

    // 5) MP를 감소시키는 메서드
    public void DecreasePlayerMP(int amount)
    {
        playerMP -= amount;

        // MP가 0 미만으로 내려가지 않도록 제한
        if (playerMP < 0)
        {
            playerMP = 0;
        }

        // MP가 변경되었음을 알리는 이벤트 호출
        OnMPChanged?.Invoke(playerMP);
    }
}
