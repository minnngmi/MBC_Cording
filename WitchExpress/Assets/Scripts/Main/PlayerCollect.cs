using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Candy"))
        {
            // 자식에서 부모 오브젝트(GameObject) 찾기
            GameObject parentObj = other.transform.parent.gameObject;
            // 부모 삭제 → 자식도 같이 삭제
            Destroy(parentObj);

            // GameManager를 사용하여 사탕 카운트를 증가시킵니다.
            GameManager.Instance.IncreaseCandyCount();
        }
    }
}
