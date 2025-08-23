using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningMove : MonoBehaviour
{
    // 이 스크립트는 번개를 앞으로 이동시킵니다.

    // 번개가 이동하는 속도
    public float moveSpeed;

    private void Update()
    {
        // 게임 상태가 ‘게임 중’ 상태일 때만 조작할 수 있게 한다.
        if (GameManager.Instance.gState != GameManager.GameState.Run)
        {
            return;
        }

        // transform.forward는 오브젝트가 바라보는 앞쪽 방향을 의미합니다.
        // moveSpeed 속도로 앞쪽 방향으로 계속 이동시킵니다.
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
