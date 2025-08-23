using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningMove : MonoBehaviour
{
    // �� ��ũ��Ʈ�� ������ ������ �̵���ŵ�ϴ�.

    // ������ �̵��ϴ� �ӵ�
    public float moveSpeed;

    private void Update()
    {
        // ���� ���°� ������ �ߡ� ������ ���� ������ �� �ְ� �Ѵ�.
        if (GameManager.Instance.gState != GameManager.GameState.Run)
        {
            return;
        }

        // transform.forward�� ������Ʈ�� �ٶ󺸴� ���� ������ �ǹ��մϴ�.
        // moveSpeed �ӵ��� ���� �������� ��� �̵���ŵ�ϴ�.
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
