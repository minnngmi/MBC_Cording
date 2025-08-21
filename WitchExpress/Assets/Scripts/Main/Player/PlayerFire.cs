using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    //총알 생산할 공장
    public GameObject bulletFactory;

    //(오브젝트풀) 탄창에 넣을 총알 개수
    public int poolSize;

    //오브젝트 풀 리스트 생성
    public List<GameObject> bulletObjectPool;

    //총구 위치
    public GameObject firePosition;

    // 공격 모션
    public Animator witchAttack;


    // 스킬 동영상 재생을 위한 변수 추가 (예시)
    public GameObject skillVideo;
    // public GameObject skillEffect;

    //  스킬 사용 가능 상태를 추적하는 변수
    private bool canUseSkill = false;


    private void Start()
    {

        // 오브젝트 풀 리스트로 관리
        //탄창의 크기를 총알을 담을 수 있는 크기로 만들어준다.
        bulletObjectPool = new List<GameObject>();

        //탄창에 넣을 총알 개수만큼 반복하여 
        for (int i = 0; i < poolSize; i++)
        {
            //총알 공장에서 총알을 생성한다.
            GameObject bullet = Instantiate(bulletFactory);

            //총알을 오브젝트 풀 리스트에 추가 한다.
            bulletObjectPool.Add(bullet);

            // 오브젝트 비활성화 시킨다.
            bullet.SetActive(false);
        }

        // GameManager의 OnSkillActivated 이벤트 구독
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnSkillActivated += OnSkillReady;
        }
    }

    void Update()
    {
        // 일반 공격 로직
        // 목표: 사용자가 발사 버튼을 누르면 총알을 발사하고 싶다.
        // - 만약 사용자가 ctrl 버튼을 누르면
        if (Input.GetButtonDown("Fire1"))
        {
            //탄창 안에 있는 총알들 중에서
            if (bulletObjectPool.Count > 0)
            {
                //비활성화 된(발사되지 않은) 첫번째 총알을
                GameObject bullet = bulletObjectPool[0];

                //총알을 활성화시킨다(발사시킨다)
                bullet.SetActive(true);

                //총알을 총구위치로 가져다 놓기
                bullet.transform.position = firePosition.transform.position;

                // 공격 모션 작동
                witchAttack.SetTrigger("attack");

                //오브젝트풀에서 총알 제거
                bulletObjectPool.RemoveAt(0);
            }
        }

        // 스킬 사용 로직을 별도 메서드로 관리
        CheckForSkillInput();
    }


    // 스킬 사용 가능 상태 이벤트 수신시,  호출되는 메서드
    // (1) 스킬 사용 가능 상태로 변경

    private void OnSkillReady()
    {
        canUseSkill = true; // 스킬 사용 가능 상태로 변경
        Debug.Log("PlayerFire: Skill is ready!");
    }

    //  (2) 스킬 사용 입력을 감지
    private void CheckForSkillInput()
    {
        // 스킬 사용이 가능하고 Space 키를 누르면
        if (canUseSkill && Input.GetKeyDown(KeyCode.Space))
        {
            UseSkill();
        }
    }

    // (3) 스킬 사용
    private void UseSkill()
    {
        // 스킬 사용 로직을 여기에 구현
        Debug.Log("PlayerFire: Special Skill Activated!");

        // GameManager를 통해 MP를 80 감소시킵니다.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.DecreasePlayerMP(80);
        }

        // 스킬 사용 후 상태를 비활성화로 변경
        canUseSkill = false;

        // 여기에 특별 공격 로직(예: 특수 총알 발사, 이펙트)이나 동영상 재생 로직을 추가
        // 예시: skillVideo.SetActive(true);
        // 예시: skillEffect.Play();

    }
}
