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
    }

    void Update()
    {
        //목표: 사용자가 발사 버튼을 누르면 총알을 발사하고 싶다.
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
    }
}
