using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBullet : MonoBehaviour
{

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // 1. 충돌한 오브젝트의 클래스 컴포넌트 가져오기
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            // 2. 폭발 효과 메서드 호출 (Enemy가 처리)
            if (enemy != null)
            {
                enemy.ExplosionEnemy(transform.position); // 이 안에서 비활성화가 일어납니다.
            }

            // GameManager를 통해 적 처치 카운트를 증가
            if (GameManager.Instance != null)
            {
                GameManager.Instance.IncreaseEnemyKills();
            }

            // 3. Enemy의 CandyManager를 가져와서 캔디 생성 메서드 호출
            CandyManager candyManager = other.gameObject.GetComponent<CandyManager>();
            if (candyManager != null)
            {
                candyManager.SpawnRandomCandy();
            }

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
    }
}
