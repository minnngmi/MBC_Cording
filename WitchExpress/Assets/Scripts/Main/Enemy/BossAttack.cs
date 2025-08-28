using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class BossAttack : MonoBehaviour
{
    [Header("가시덩굴 공격 설정")]
    // 가시 스폰 최소시간
    public float minTime;
    // 가시 스폰 최대시간
    public float maxTime;

    // 가시 스폰 위치
    public Vector3 spawnValues;
    // 가시를 몇 초마다 발사할지 설정합니다.
    public float fireInterval = 1.0f;


    [Header("가시덩쿨 오브젝트 풀 설정")]
    // 가시 덩굴 공장
    public GameObject[] thronsFactory;


    private void Start()
    {
        // 가시덩굴 생산하고 비활성화
        for (int i = 0; i < thronsFactory.Length; i++)
        {
            GameObject throns = Instantiate(thronsFactory[i]);
            throns.SetActive(false);
        }
    // 게임 흐름을 제어하는 메인 코루틴을 시작합니다.
    //StartCoroutine(GameFlowRoutine());
    }


    //모든 코루틴을 순서대로 실행하는 메인 코루틴
    IEnumerator GameFlowRoutine()
    {
        // GameManager의 상태가 'Run'이 될 때까지 기다립니다.
        while (GameManager.Instance.gState != GameState.Run)
        {
            yield return null;
        }

        Debug.Log("게임 시작! 스테이지 1 몬스터 스폰 시작.");
        // 첫번째 가시덩쿨 공격 코루틴이 끝날 때까지 기다립니다.
        //yield return StartCoroutine(ThronsAttackRoutine());


    }

    // 첫번째 가시덩쿨 공격 코루틴
   // IEnumerator ThronsAttackRoutine()


}
