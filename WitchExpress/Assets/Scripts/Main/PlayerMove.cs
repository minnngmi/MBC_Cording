using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static float opening = 10;
    public float speed;
    // 배경 맵 사이즈
    public float xMin, xMax, zMin, zMax;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        // 게임 시작시 무적 오프닝 타임
        if (opening > 0)
        {
            rb.velocity = new Vector3(0, 0, 3f);
            opening -= Time.fixedDeltaTime;
            return;
        }

        // 이동키
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        rb.velocity = movement * speed;

        // 플레이어 이동범위 제한
        rb.position = new Vector3(
           Mathf.Clamp(rb.position.x, xMin, xMax),
           0,
           Mathf.Clamp(rb.position.z, zMin, zMax));


    }
}
