using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBooMove : MonoBehaviour
{
    // 공격에 사용할 게임 오브젝트들을 연결할 변수들
    // Inspector 창에서 드래그하여 연결

    // Inspector 창에 자식 오브젝트로 있는 번개 3개를 드래그해서 연결해주세요.
    public GameObject[] lightningBolts;

    public GameObject lightningEffect; // 번개 발사 시 나타날 이펙트 프리팹
    public Transform firePoint; // 번개가 발사될 위치 (보통 몬스터의 입이나 손)

    // 번개 공격이 시작될 때까지 기다릴 시간 (2초)
    public float attackDelay;
    public float fireDelay;

    void Start()
    {
        // 몬스터가 시작될 때 이펙트 오브젝트를 미리 비활성화합니다.
        // 이렇게 해야 공격 전에는 이펙트가 보이지 않습니다.
        if (lightningEffect != null)
        {
            lightningEffect.SetActive(false);
        }

        //추가: 게임 시작 시 모든 번개 오브젝트를 비활성화합니다.
        foreach (GameObject bolt in lightningBolts)
        {
            if (bolt != null)
            {
                bolt.SetActive(false);
            }
        }

        // 'AttackCoroutine'이라는 함수를 바로 시작하도록 합니다.
        // 이 코루틴 안에서 시간을 두고 특정 작업을 순서대로 실행합니다.
        StartCoroutine(AttackCoroutine());
    }

   // 코루틴 함수입니다. 이펙트 생성 후 번개를 발사하는 기능을 순서대로 처리합니다.
    IEnumerator AttackCoroutine()
    {
        // 1. 몬스터가 나타난 후 2초를 기다립니다.
        // 기존 코드의 attackDelay는 이 단계에서 활용됩니다.
        attackDelay = Random.Range(attackDelay - 0.5f, attackDelay + 0.5f);
        yield return new WaitForSeconds(attackDelay);

        // 2. 번개 이펙트 오브젝트를 활성화합니다.
        // 이제 이펙트가 보이고, 이펙트가 가진 파티클 시스템이 재생됩니다.
        if (lightningEffect != null)
        {
            lightningEffect.SetActive(true);
        }

        // 3. 번개 발사 전에 fireDelay(2초)만큼 다시 기다립니다.
        yield return new WaitForSeconds(fireDelay);

        // 4. 번개를 발사하는 함수를 호출합니다.
        FireLightningBolts();
    }

    // ⭐ 수정: Instantiate 대신 미리 만들어진 번개를 활성화하는 방식으로 변경
    void FireLightningBolts()
    {
        // 번개를 발사할 각도들을 배열로 만듭니다.
        float[] angles = { -15f, 0f, 15f };

        // 3개의 번개 오브젝트가 연결되어 있는지 확인합니다.
        if (lightningBolts.Length >= 3)
        {
            for (int i = 0; i < 3; i++)
            {
                // 번개 오브젝트를 가져옵니다.
                GameObject bolt = lightningBolts[i];
                if (bolt != null)
                {
                    // 1. 번개의 위치와 회전을 설정합니다.
                    bolt.transform.position = firePoint.position;
                    bolt.transform.rotation = Quaternion.AngleAxis(angles[i], Vector3.up) * transform.rotation;

                    // 2. 번개를 활성화(보이게) 합니다.
                    bolt.SetActive(true);
                }
            }
        }
    }


    /*
    // 3개의 번개를 발사하는 함수입니다.
    void FireLightningBolts()
    {
        // 번개를 발사할 각도들을 배열로 만듭니다. (중앙, 왼쪽 15도, 오른쪽 15도)
        float[] angles = { -15f, 0f, 15f };

        // 3개의 번개를 만들기 위해 반복문을 사용합니다.
        for (int i = 0; i < angles.Length; i++)
        {
            // 몬스터가 바라보는 방향을 기준으로 번개 회전 각도를 계산합니다.
            // transform.forward는 몬스터가 바라보는 앞 방향을 의미합니다.
            // Quaternion.AngleAxis는 특정 축(위쪽 방향)을 기준으로 각도를 회전시켜 줍니다.
            Quaternion lightningRotation = Quaternion.AngleAxis(angles[i], Vector3.up) * transform.rotation;

            // firePoint 위치에 번개 프리팹을 생성합니다.
            // 생성된 번개는 계산된 회전값(lightningRotation)을 따르게 됩니다.
            Instantiate(lightningPrefab, firePoint.position, lightningRotation);
        }
    }
    */
}
