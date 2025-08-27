using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("시작 오프닝 타임")]
    public float openingTime;
    public float bossOpeningTime;

    [Header("이동 속도")]
    public float speed;

    [Header("배경 맵 사이즈")]
    // 배경 맵 사이즈
    public float xMin;
    public float xMax;
    public float zMin;
    public float zMax;

    // 기울기 정도 (각도)
    private float tiltAmountSide = 15f;   // 좌우 기울임
    private float tiltAmountForward = 30f; // 앞뒤 기울임

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 게임 시작 시 오프닝 시퀀스를 담당하는 코루틴을 시작
        StartCoroutine(OpeningSequence());
    }

    private void Update()
    {
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

    // 오프닝 시퀀스를 처리하는 코루틴
    private IEnumerator OpeningSequence()
    {
        // 캐릭터의 시작 위치와 목표 위치를 설정합니다.
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(0, 0, -1);
        float elapsedTime = 0f;

        // Rigidbody의 속도를 0으로 설정하여 혹시 모를 움직임을 막습니다.
        rb.velocity = Vector3.zero;

        // 캐릭터의 시작 위치를 설정
        rb.position = startPos;

        // 오프닝 시간동안 캐릭터를 서서히 이동시킵니다.
        while (elapsedTime < openingTime)
        {
            // Lerp 함수를 사용하여 현재 위치를 목표 위치로 부드럽게 보간합니다.
            rb.position = Vector3.Lerp(startPos, endPos, elapsedTime / openingTime);
            elapsedTime += Time.deltaTime;

            // 다음 프레임까지 기다립니다.
            yield return null;
        }

        // 오프닝 시간이 끝난 후 목표 위치에 정확하게 위치하도록 합니다.
        rb.position = endPos;

        // 오프닝이 끝났으므로 GameManager의 상태를 '게임 중'으로 변경합니다.
        // 이 코드가 없으면 플레이어가 움직이지 않습니다.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.gState = GameManager.GameState.Run;
        }
    }

    // 보스 등장 시퀀스를 처리하는 코루틴
    public IEnumerator BossOpening()
    {
        // 캐릭터의 시작 위치와 목표 위치를 설정합니다.
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(0, 0, -2);
        float elapsedTime = 0f;

        // Rigidbody의 속도를 0으로 설정하여 혹시 모를 움직임을 막습니다.
        rb.velocity = Vector3.zero;
        // 캐릭터의 시작 위치를 설정
        rb.position = startPos;

        // 오프닝 시간동안 캐릭터를 서서히 이동시킵니다.
        while (elapsedTime < bossOpeningTime)
        {
            // Lerp 함수를 사용하여 현재 위치를 목표 위치로 부드럽게 보간합니다.
            rb.position = Vector3.Lerp(startPos, endPos, elapsedTime / bossOpeningTime);
            elapsedTime += Time.deltaTime;

            // 다음 프레임까지 기다립니다.
            yield return null;
        }

        // 오프닝 시간이 끝난 후 목표 위치에 정확하게 위치하도록 합니다.
        rb.position = endPos;

        // 오프닝이 끝났으므로 GameManager의 상태를 '게임 중'으로 변경합니다.
        // 이 코드가 없으면 플레이어가 움직이지 않습니다.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.gState = GameManager.GameState.Run;
        }
    }

}
