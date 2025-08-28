using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Enemy 프리팹에 적용된 스크립트
public class CandyManager : MonoBehaviour
{
    // 캔디 공장(캔디 프리팹들을 담는 배열) 생성
    public GameObject[] candyFactory;

 
    // 캔디 생성에 관련된 모든 기능을 담당하는 메서드
    public void SpawnRandomCandy()
    {
        // 게임 상태가 'Run'일 때만 메서드 내용을 실행
        if (GameManager.Instance.gState != GameManager.GameState.Run)
        {
            // 상태가 Run이 아니면 즉시 메서드를 종료합니다.
            return;
        }

        // 1. 몇 개를 만들지 확률로 결정 (40% 확률로 2개, 나머지는 1개)
        int rand = Random.Range(0, 10);
        int spawnCount = (rand < 4) ? 2 : 1;

        // 2. 캔디 종류가 1개일 경우, spawnCount를 1로 강제 수정
        int candyCount = candyFactory.Length;
        if (candyCount == 1)
        {
            spawnCount = 1;
        }

        // 3. 캔디 종류의 개수만큼 정렬된 인덱스 배열을 만듭니다.
        int[] allCandyIndices = new int[candyCount];
        for (int i = 0; i < candyCount; i++)
        {
            allCandyIndices[i] = i;
        }

        // 4. 배열을 무작위로 섞어줍니다.
        System.Random rng = new System.Random();
        int n = allCandyIndices.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int value = allCandyIndices[k];
            allCandyIndices[k] = allCandyIndices[n];
            allCandyIndices[n] = value;
        }

        // 5. 섞인 배열에서 정해진 개수만큼 순서대로 뽑아 캔디를 생성
        for (int i = 0; i < spawnCount; i++)
        {
            // 섞인 배열에서 i번째 인덱스를 가져와 캔디 종류로 사용합니다.
            int candyPoolIndex = allCandyIndices[i];

            // 캔디가 생성될 위치를 원래 위치에서 약간 옆으로 옮겨줍니다.
            // i 값에 따라 캔디가 왼쪽 또는 오른쪽으로 이동합니다.
            Vector3 offset = Vector3.right * (i - (spawnCount - 1) / 2.0f) * 0.5f;

            // 6. 실제 캔디를 생성하는 메서드 호출
            CreatCandy(candyPoolIndex, offset);
        }
    }


    // 특정 종류의 캔디를 지정 위치에 생성시키는 메서드
    public GameObject CreatCandy(int poolIndex, Vector3 offset)
    {
        // 해당 종류의 프리팹으로 생성하고 위치에 오프셋을 더합니다.
        GameObject candy = Instantiate(candyFactory[poolIndex], transform.position + offset, Quaternion.identity);

        // 생성한 캔디를 반환
        return candy;
    }
}
