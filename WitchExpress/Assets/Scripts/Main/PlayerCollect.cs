using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Candy"))
        {
            // �ڽĿ��� �θ� ������Ʈ(GameObject) ã��
            GameObject parentObj = other.transform.parent.gameObject;
            // �θ� ���� �� �ڽĵ� ���� ����
            Destroy(parentObj);
        }
    }

}
