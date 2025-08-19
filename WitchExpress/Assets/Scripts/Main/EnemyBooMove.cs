using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBooMove : MonoBehaviour
{
    // 공격에 사용할 게임 오브젝트들을 연결할 변수들
    // Inspector 창에서 드래그하여 연결

    // ⭐ 수정: 발사할 번개 프리팹 (풀 생성에 사용)
    public GameObject lightningPrefab;

    // ⭐ 추가: 오브젝트 풀의 크기를 인스펙터에서 설정할 수 있게 합니다.
    public int poolSize = 9;

    // ⭐ 수정: 오브젝트 풀을 관리할 리스트
    private List<GameObject> lightningPool;

    // ⭐ 추가: 번개를 몇 초마다 발사할지 설정합니다.
    public float fireInterval = 1.0f;

    public GameObject lightningEffect; // 번개 발사 시 나타날 이펙트 프리팹
    public Transform firePoint; // 번개가 발사될 위치 (보통 몬스터의 입이나 손)

    // 번개 공격이 시작될 때까지 기다릴 시간 (2초)
    public float attackDelay;
    public float fireDelay;

    private void Awake()
    {
        // ⭐ Awake() 함수는 Start()보다 먼저 호출됩니다.
        // 여기에서 오브젝트 풀을 미리 생성해 두는 것이 좋습니다.
        lightningPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bolt = Instantiate(lightningPrefab);
            bolt.SetActive(false);
            lightningPool.Add(bolt);
        }
    }

    private void OnEnable()
    {
        // ⭐ 수정: 몬스터가 활성화될 때 이펙트를 즉시 비활성화합니다.
        if (lightningEffect != null)
        {
            lightningEffect.SetActive(false);
        }

        // ⭐ 오브젝트가 활성화될 때마다 호출됩니다.
        // 몬스터가 풀링으로 다시 활성화될 때, 번개 발사 코루틴을 다시 시작합니다.
        StartCoroutine(AttackCoroutine());
    }

    private void OnDisable()
    {
        // ⭐ 오브젝트가 비활성화될 때 호출됩니다.
        // 몬스터가 사라질 때 번개 발사 코루틴을 중지하여 불필요한 작업이 발생하지 않게 합니다.
        StopAllCoroutines();
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

        // ⭐ 수정: 1초마다 반복해서 번개를 발사합니다.
        // 몬스터가 비활성화되기 전까지 이 코루틴은 계속 실행됩니다.
        while (true)
        {
            FireLightningBolts();
            yield return new WaitForSeconds(fireInterval);
        }
    }

    // ⭐ 수정: Instantiate 대신 미리 만들어진 번개를 활성화하는 방식으로 변경
    void FireLightningBolts()
    {
        // 번개를 발사할 각도들을 배열로 만듭니다.
        float[] angles = { -30f, 0f, 30f };
        int boltsToFire = 3;

        for (int i = 0; i < boltsToFire; i++)
        {
            GameObject bolt = GetPooledObject();
            if (bolt != null)
            {
                // 번개 오브젝트의 위치와 회전을 초기화합니다.
                // 이렇게 해야 몬스터가 어디에 있든 정확한 위치와 방향으로 발사됩니다.
                bolt.transform.position = firePoint.position;
                bolt.transform.rotation = Quaternion.AngleAxis(angles[i], Vector3.up) * transform.rotation;

                bolt.SetActive(true);
            }
        }
    }

    // ⭐ 추가: 오브젝트 풀에서 비활성화된 오브젝트를 찾아 반환합니다.
    private GameObject GetPooledObject()
    {
        for (int i = 0; i < lightningPool.Count; i++)
        {
            // 비활성화된 오브젝트를 찾습니다.
            if (!lightningPool[i].activeInHierarchy)
            {
                return lightningPool[i];
            }
        }
        // 풀에 사용 가능한 오브젝트가 없으면 null을 반환합니다.
        // 이 부분은 필요에 따라 풀 크기를 늘리도록 수정할 수도 있습니다.
        return null;
    }    
}
