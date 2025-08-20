using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public void TakeDamage(int damage)
    {
        // GameManager의 DecreasePlayerHP 메서드를 호출하여 데미지를 전달
        // GameManager.Instance는 싱글톤 패턴 덕분에 어디서든 접근 가능
        if (GameManager.Instance != null)
        {
            GameManager.Instance.DecreasePlayerHP(damage);
        }
    }
}