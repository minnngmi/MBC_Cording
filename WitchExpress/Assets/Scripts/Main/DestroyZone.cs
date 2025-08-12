using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    //영역 안에 다른 물체가 감지될 경우 
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Bullet"))
        {
            // 부딪힌 물체 비활성화 
            other.gameObject.SetActive(false);
            // PlayerFire 스크립트 객체 얻어오기
            PlayerFire playerFire =
                GameObject.Find("Player").GetComponent<PlayerFire>();

            // 총알 오브젝트 풀 리스트에 총알 추가
            playerFire.bulletObjectPool.Add(other.gameObject);
        }
        else if (other.CompareTag("Enemy"))
        {
            // 부딪힌 물체 비활성화 
            other.gameObject.SetActive(false);
            // EnemyManager 스크립트 객체 얻어오기
            EnemyManager em =
                GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
            // 부딪힌 적의 고유넘버 확인
            int enemyIdx = other.GetComponent<Enemy>().enemyIdx;
            // 해당 적 오브젝트 풀 리스트에 추가
            em.enemyObjectPool[enemyIdx].Add(other.gameObject);
        }

        else
        {
            other.gameObject.SetActive(false);
        }
    }
}
