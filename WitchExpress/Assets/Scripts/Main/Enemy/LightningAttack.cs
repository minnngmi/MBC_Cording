using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningAttack : MonoBehaviour
{
    // 이 총알이 플레이어에게 줄 데미지를 설정합니다.
    public int damage;

    // OnEnable 메서드 제거
    // 이동은 LightningMove 스크립트가 담당하므로 여기서는 필요하지 않습니다.

    // OnTriggerEnter: 총알이 다른 콜라이더 영역에 진입했을 때 호출됩니다.
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
        // 충돌 후 총알을 비활성화하여 오브젝트 풀로 돌려보냅니다.
        gameObject.SetActive(false);
        }
    }

}
