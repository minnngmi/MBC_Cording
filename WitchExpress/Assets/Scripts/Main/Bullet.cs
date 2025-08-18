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
            // 1. 충돌한 오브젝트의 클래스 컴포넌트 가져오기
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            // 2. 폭발 효과 메서드 호출 (Enemy가 처리)
            enemy.ExplosionEnemy(transform.position);

            // 3. Enemy의 CandyManager를 가져와서 캔디 생성 메서드 호출
            CandyManager candyManager = other.gameObject.GetComponent<CandyManager>();

            // **캔디 생성에 관련된 모든 로직을 CandyManager에 맡깁니다.**
            candyManager.SpawnRandomCandy();

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
