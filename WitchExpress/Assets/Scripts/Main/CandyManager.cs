using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyManager : MonoBehaviour
{
    // 캔디 공장(캔디 프리팹들을 담는 배열) 생성
    public GameObject[] candyFactory;
    // 각 종류별로 미리 만들어둘 개수
    public int poolSize;
    // 종류별 오브젝트 풀(리스트) 배열
    public List<GameObject>[] candyObjectPool;

    private void Start()
    {
        // 캔디 종류 수만큼 풀 배열 만들기
        candyObjectPool = new List<GameObject>[candyFactory.Length];

        // 종류별로 풀을 초기화
        for (int i = 0; i < candyFactory.Length; i++)
        {
            // i번째 종류의 풀 리스트 생성
            candyObjectPool[i] = new List<GameObject>();

            for (int j = 0; j < poolSize; j++)
            {
                //에너미 공장에서 에너미를 생성한다.
                GameObject candy = Instantiate(candyFactory[i]);
                // 에너미를 오브젝트 풀 리스트에 추가 한다.
                candyObjectPool[i].Add(candy);
                // 오브젝트를 비활성화 시킨다.
                candy.SetActive(false);
            }
        }
    }

    // 특정 종류의 캔디를 지정 위치에 생성시키는 메서드
    // poolIndex : CandySpawner의 pools 리스트에서 몇 번째 풀을 쓸지 (0부터 시작)
    // pos          : 캔디를 생성할 위치
    // 반환값     : 생성된(또는 재사용한) 캔디 GameObject
    public GameObject CreatCandy(int poolIndex, Vector3 pos)
    {
        /*
        // 잘못된 인덱스이면 아무것도 하지 않고 null 반환
        if (poolIndex < 0 || poolIndex >= candyFactory.Length)
        {
            return null;
        }
        */
        // 해당 인덱스의 풀을 가져온다
        List<GameObject> pool = candyObjectPool[poolIndex];

        // 풀에서 비활성화된(사용 가능) 오브젝트를 찾는다
        GameObject candy = null;
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeSelf)
            {
                candy = pool[i];
                break; // 하나 찾으면 바로 종료
            }
        }

        // 못 찾았으면 새로 하나 만들어서 풀에 추가
        if (candy == null)
        {
            // 해당 종류의 프리팹으로 생성
            candy = Instantiate(candyFactory[poolIndex], transform);

            // 풀에 등록
            pool.Add(candy);

            // 생성 직후엔 꺼둔 상태로 맞춰두기(아래에서 바로 켬)
            candy.SetActive(false);
        }

        // 위치(필요하면 회전도) 설정
        candy.transform.position = pos;

        // 활성화해서 보이게 만들기
        candy.SetActive(true);

        // 생성한 캔디를 반환
        return candy;
    }
}
