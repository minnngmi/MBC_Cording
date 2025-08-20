using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static float opening = 10; 
    public float speed;
    
    // 배경 맵 사이즈
    public float xMin, xMax, zMin, zMax;

    // 기울기 정도 (각도)
    public float tiltAmountSide = 30f;   // 좌우 기울임
    public float tiltAmountForward = 1f; // 앞뒤 기울임


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

        // 좌우 기울기 적용
        float tiltZ = -moveHorizontal * tiltAmountSide;
        // 앞뒤 기울기 적용
        float tiltX = moveVertical * tiltAmountForward;

        // Quaternion.Euler( X기울기, Y회전, Z기울기 )
        Quaternion targetRotation = Quaternion.Euler(tiltX, 0, tiltZ);

        // 부드럽게 회전 보간 (0.1f는 속도계수, 값이 클수록 빠르게 복귀)
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

        // 플레이어 이동범위 제한
        rb.position = new Vector3(
           Mathf.Clamp(rb.position.x, xMin, xMax),
           0,
           Mathf.Clamp(rb.position.z, zMin, zMax));

    }
}
