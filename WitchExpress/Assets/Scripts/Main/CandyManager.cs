using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Enemy 프리팹에 적용된 스크립트
public class CandyManager : MonoBehaviour
{
    // 캔디 공장(캔디 프리팹들을 담는 배열) 생성
    public GameObject[] candyFactory;

    // 특정 종류의 캔디를 지정 위치에 생성시키는 메서드
    // poolIndex : Bullet 스크립트에서 랜덤하게 받아오는 숫자
    // 반환값     : 생성된(또는 재사용한) 캔디 GameObject
    public GameObject CreatCandy(int poolIndex)
    {
        // 해당 종류의 프리팹으로 생성
        GameObject candy = Instantiate(candyFactory[poolIndex], transform.position, Quaternion.identity);

        // 생성한 캔디를 반환
        return candy;
    }
}
