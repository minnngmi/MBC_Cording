using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;  // Action 사용을 위해 

public class GameManager : MonoBehaviour
{
    // 싱글톤 변수: static 변수로 자기 자신을 담을 그릇 생성
    public static GameManager Instance;


    // 관리할 데이터 변수들 선언
    [Header("초기 아이템 설정")]
    private int enemyKills;               // 적을 처치한 횟수
    public int candyCount;             // 획득한 사탕 갯수
    public int HPpotion;                 // 초기 포션 갯수

    public GameObject PotionEffect;    // 포션 사용 가능 이펙트
    
    private int PosionCandy;                 // 포션 사용시 사라지는 캔디량
    private int potionUsedCount = 0;   // 포션 사용 횟수를 추적하는 변수

    [Header("초기 캐릭터 상태 설정")]
    public int playerHP = 100;       // 초기 HP 값
    public int playerMP = 0;          // 초기 MP 값
    public int maxHP = 100;         
    public int maxMP = 100;

    // 이벤트 모음
    // UIManager가 이 이벤트를 구독하여 HP/MP 슬라이더를 업데이트
    public Action<int> OnHPChanged;      // (1) HP 변경 이벤트
    public Action<int> OnMPChanged;     // (2) MP 변경 이벤트
    public Action OnSkillActivated;             // (3) 스킬 사용 이벤트
    public Action<int> OnPotionUsed;       // (4) 포션 사용시 소모된 캔디량 이벤트

    // 특수 스킬 활성화 상태를 추적하는 변수
    public bool isSkillActive = false; 

    // 게임 상태 상수
    public enum GameState
    {
        Ready,
        Run,
        GameOver,
        Ending
    }

    // 현재의 게임 상태 변수
    public GameState gState;

    // Awake 메서드 : 로드될 때 가장 먼저 실행
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
        gState = GameState.Ready;

        // 포션 사용 가능 이펙트 off
        PotionEffect.SetActive(false);

        // 포션에 필요한 캔디 초기값 설정
        PosionCandy = 10;
    }

    private void Update()
    {
        CheckPotionEffect();
    }

    // 메서드 모음
    // 1. 적 처치 카운트 메서드
    public void IncreaseEnemyKills()
    {
        enemyKills++;
        //Debug.Log("적 처치 수: " + enemyKills);

         // (1) MP 5 증가
        IncreasePlayerMP(5);

        if (enemyKills % 10 == 0)
        {
            // (2) 포션 1 증가
            IncreasePosion();
        }
    }

    // 2) 사탕 수를 증가시키는 메서드
    public void IncreaseCandyCount()
    {
        candyCount++;
        //Debug.Log("사탕 획득 수: " + candyCount);
    }

    // 2) 사탕 수를 감소시키는 메서드
    public void DecreaseCandyCount(int amount)
    {
        candyCount = candyCount - amount;

        // 사탕이 0 미만으로 내려가지 않도록 제한
        if (candyCount < 0)
        {
            candyCount = 0;
        }
    }

    // 3) 플레이어 HP를 감소시키는 메서드
    public void DecreasePlayerHP(int damage)
    {
        playerHP -= damage;

        if (playerHP < 0)
        {
            playerHP = 0;
        }

        // HP가 변경되었음을 알리는 이벤트 호출(UIManager가 구독 중)
        OnHPChanged?.Invoke(playerHP);

        if (playerHP <= 0)
        {
            playerHP = 0;
            Debug.Log("게임 오버!");

            // 오프닝이 끝났으므로 GameManager의 상태를 '게임오버'로 변경합니다.
            GameManager.Instance.gState = GameManager.GameState.GameOver;

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

    // 5) (스킬 사용시) MP를 감소시키는 메서드
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

    // (6) 포션 갯수를 증가 시키는 메서드
    public void IncreasePosion()
    {
        HPpotion++;
    }
  
    // (7) 포션 갯수를 감소 시키는 메서드(포션 사용)
    public void DereasePosion()
    {
        if (candyCount < PosionCandy)
        {
            return;
        }
        else
        {
            // 포션 1개 사용하여 HP max 회복
            HPpotion--;
            playerHP = maxHP;

            // 소모될 캔디 양을 변수에 저장
            int consumedCandyAmount = PosionCandy; 

            // 캔디 소모
            DecreaseCandyCount(PosionCandy);

            // 포션 갯수가  0 미만으로 내려가지 않도록 제한
            if (HPpotion < 0)
            {
                HPpotion = 0;
            }

            // HP가 변경되었음을 알리는 이벤트 호출(UIManager가 구독 중)
            OnHPChanged?.Invoke(playerHP);
            // 소모된 캔디량을 알리는 이벤트 호출
            OnPotionUsed?.Invoke(consumedCandyAmount);

            // 포션 사용 횟수 증가 및 PosionCandy 값 업데이트
            potionUsedCount++;

            if (potionUsedCount < 3)
            {
                PosionCandy = 10;
            }
            else if (potionUsedCount < 8) // 3번 사용 이후부터 8번까지 (총 5번)
            {
                PosionCandy = 15;

            }
            else
            {
                PosionCandy = 20; // 8번 사용 이후부터
            }
        }
    }

    // (7) 포션 이펙트 상태를 확인하고 변경하는 메서드
    private void CheckPotionEffect()
    {
        // 포션이 1개 이상 있고, 캔디 수가 충분할 때만 이펙트 활성화
        if (candyCount >= PosionCandy && HPpotion > 0)
        {
            PotionEffect.SetActive(true);
        }
        else
        {
            PotionEffect.SetActive(false);
        }
    }
}
