using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    // 데미지 이펙트 프리팹 연결 변수
    public GameObject damageEffect;

    public void TakeDamage(int damage)
    {
        // GameManager의 DecreasePlayerHP 메서드를 호출하여 데미지를 전달
        // GameManager.Instance는 싱글톤 패턴 덕분에 어디서든 접근 가능
        if (GameManager.Instance != null)
        {
            GameManager.Instance.DecreasePlayerHP(damage);

        }

        // 데미지를 입을 때 이펙트 생성
        // 이펙트 프리팹 변수가 비어있지 않은지 확인
        if (damageEffect != null)
        {
            // 이펙트를 생성하고 플레이어의 위치에 배치합니다.
            GameObject effectInstance = Instantiate(damageEffect, transform.position, Quaternion.identity);

            // 생성된 이펙트가 자동으로 사라지도록 합니다.
            // 파티클 시스템이 끝나는 시간을 기준으로 오브젝트를 파괴
            ParticleSystem ps = effectInstance.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(effectInstance, ps.main.duration);
            }
            else
            {
                // 파티클 시스템이 없는 이펙트일 경우를 대비해 2초 뒤에 파괴
                Destroy(effectInstance, 2f);
            }
        }
    }
}