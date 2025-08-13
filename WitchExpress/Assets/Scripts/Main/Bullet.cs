using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    

    void Update()
    {
        // 1. 방향을 구한다.
        Vector3 dir = Vector3.forward;

        // 2. 이동하고 싶다. 공식 P = P0 + vt
        transform.position += dir * speed * Time.deltaTime;
    }

    // 플레이어의 총알에 맞았을시
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // 충돌한 오브젝트의 클래스 컴포넌트 가져오기(*중요)
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            // 폭발 효과 메서드 호출
            enemy.ExplosionEnemy(transform.position);


            // ★ CandyManager 객체 얻어오기
            CandyManager candyManager =
                GameObject.Find("CandyManager").GetComponent<CandyManager>();

            // CandyManager 안의 candyFactory 배열 길이 확인
            int candyCount = candyManager.candyFactory.Length;
            // 0 ~ (길이-1) 사이에서 랜덤한 정수 뽑기
            int candyPoolIndex = Random.Range(0, candyCount);

            // 캔디 생성 위치 = Enemy의 현재 위치
            Vector3 candySpawnPos = other.transform.position;

            // CandyManager에 위치와 인덱스를 전달해서 캔디 생성
            GameObject newCandy = candyManager.CreatCandy(candyPoolIndex, candySpawnPos);


            // 맞은 상대 비활성화
            other.gameObject.SetActive(false);

            // EnemyManager 객체 얻어오기
            EnemyManager em =
                GameObject.Find("EnemyManager").GetComponent<EnemyManager>();

            // 부딪힌 적의 고유넘버 확인
            int enemyIdx = enemy.enemyIdx;
            // 해당 적 오브젝트 풀 리스트에 추가
            em.enemyObjectPool[enemyIdx].Add(other.gameObject);
        }
         // 자신(총알)도 비활성화
        gameObject.SetActive(false);
        // PlayerFire 객체 얻어오기
        PlayerFire player =
            GameObject.Find("Player").GetComponent<PlayerFire>();
        // 오브젝트 풀 리스트에 총알 추가
        player.bulletObjectPool.Add(gameObject);
    }
}
