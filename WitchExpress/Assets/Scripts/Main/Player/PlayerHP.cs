using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public void TakeDamage(int damage)
    {
        // GameManager�� DecreasePlayerHP �޼��带 ȣ���Ͽ� �������� ����
        // GameManager.Instance�� �̱��� ���� ���п� ��𼭵� ���� ����
        if (GameManager.Instance != null)
        {
            GameManager.Instance.DecreasePlayerHP(damage);
        }
    }
}