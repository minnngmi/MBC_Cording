using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    // 고유번호
    public int enemyIdx;

    // 플레이어에게 줄 데미지
    public int damage;

    // 처치시 획득 점수
    public int scoreValue;

    //필요속성 : 이동속도
    public float speed;

    // 방향을 전역변수로 만들어서 Start와 Update에서 사용
    Vector3 dir;

    //폭발(효과) 공장 주소
    public GameObject explosionFactory;


    private void OnEnable()
    {
        // 적 이동
        // 0부터 9(10-1) 까지 값중에 하나를 랜덤으로 가져와서
        int randValue = Random.Range(0, 10);

        // 만약 3보다 작으면 플레이어방향
        if (randValue < 4)
        {
            // 플레이어를 태그로 찾아서 target으로 설정
            GameObject target = GameObject.FindWithTag("Player");
            // 바라보는 방향을 플레이어 쪽으로 회전
            if (target != null)
            {
                transform.LookAt(target.transform);
            }
        }
    }

    void Update()
    {
        //transform.position += dir * speed * Time.deltaTime;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }


    // 폭발 이펙트를 작동시키는 메서드
    public void ExplosionEnemy(Vector3 position)
    {
        //2.폭발 효과 공장에서 폭발 효과를 하나 만들어야 한다.
        GameObject explosion = Instantiate(explosionFactory);

        //3.폭발 효과를 발생(위치) 시키고 싶다.
        explosion.transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 "Player" 태그를 가지고 있는지 확인합니다.
        if (other.CompareTag("Player"))
        {
            // Player 오브젝트에서 PlayerHP 컴포넌트를 찾아옵니다.
            PlayerHP playerHP = other.GetComponent<PlayerHP>();

            // PlayerHP 컴포넌트가 있다면 데미지를 전달합니다.
            if (playerHP != null)
            {
                playerHP.TakeDamage(damage); // PlayerHP 스크립트의 메서드
            }


        }
    }
}


