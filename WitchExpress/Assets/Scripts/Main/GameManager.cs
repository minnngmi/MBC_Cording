using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;  // Action 사용을 위해 추가


public class GameManager : MonoBehaviour
{
    // 1. 싱글톤 인스턴스: static 변수로 자기 자신을 담을 그릇을 만듭니다.
    public static GameManager Instance;

    // 2. 관리할 데이터 변수들을 선언합니다.
    public int enemyKills;
    public int candyCount;
    public int playerHP = 100; // 초기 HP 설정

    // HP가 변경될 때 호출할 이벤트
    // UIManager가 이 이벤트를 구독하여 HP 슬라이더를 업데이트합니다.
    public Action<int> OnHPChanged;


    // 3. Awake 메서드: 이 스크립트가 로드될 때 가장 먼저 실행됩니다.
    private void Awake()
    {
        // 인스턴스가 이미 존재하는지 확인합니다.
        if (Instance == null)
        {
            // 인스턴스가 없으면, 이 GameManager 오브젝트를 인스턴스로 지정합니다.
            Instance = this;

            // 씬이 바뀌어도 파괴되지 않게 합니다. (필요에 따라 선택)
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 인스턴스가 이미 있다면, 새로 생성된 오브젝트를 파괴하여 중복을 막습니다.
            Destroy(gameObject);
        }
    }

    // 4. 데이터를 변경하는 메서드들을 만듭니다.
    // 적 처치 횟수를 증가시키는 메서드
    public void IncreaseEnemyKills()
    {
        enemyKills++;
        Debug.Log("적 처치 수: " + enemyKills);

        // 추가 작업 (예: 특정 횟수 달성 시 아이템 생성)
        if (enemyKills % 10 == 0)
        {
            Debug.Log("축하합니다! 특별 아이템이 나타납니다!");
            // TODO: 특별 아이템 생성 코드를 여기에 작성
        }
    }

    // 사탕 카운트를 증가시키는 메서드
    public void IncreaseCandyCount()
    {
        candyCount++;
        Debug.Log("사탕 획득 수: " + candyCount);
    }

    // 플레이어 HP를 감소시키는 메서드
    public void DecreasePlayerHP(int damage)
    {
        playerHP -= damage;

        if (playerHP < 0)
        {
            playerHP = 0;
        }

        Debug.Log("플레이어 HP: " + playerHP);


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
}
