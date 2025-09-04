using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

public class PlayerMove : MonoBehaviour
{
    [Header("시작 오프닝 타임")]
    public float openingTime;
    public float bossOpeningTime;

    // 오프닝 텍스트
    public GameObject opTxt;

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
        opTxt.SetActive(false);
        rb = GetComponent<Rigidbody>();

        // 게임 시작 시 오프닝 시퀀스를 담당하는 코루틴을 시작
        StartCoroutine(OpeningSequence());
        StartCoroutine(OpeningText());
    }

    private void Update()
    {
        // 게임 상태가 ‘게임 중’ 상태일 때만 조작할 수 있게 한다.
        if (GameManager.Instance.gState != GameManager.GameState.Run)
        {
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
        GameManager.Instance.gState = GameManager.GameState.Run;

        

    }
    private IEnumerator OpeningText()
    {
        yield return new WaitForSeconds(2f);
        opTxt.SetActive(true);
    }


    // 보스 등장 시퀀스를 처리하는 코루틴
    public IEnumerator BossOpening()
    {
        Debug.Log($"보스 오프닝 진행중!");
        GameManager.Instance.gState = GameManager.GameState.Ready;


        PlayerFire playerFire = GameObject.Find("Player").GetComponent<PlayerFire>();
        GameObject skillEffect = playerFire.skillEffectObject;
        skillEffect.SetActive(false);


        // 캐릭터의 시작 위치와 목표 위치를 설정합니다.
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(0, 0, -2);

        // 캐릭터의 시작 회전값과 목표 회전값을 설정
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.identity; // 회전값이 없는 상태 (0,0,0)

        float elapsedTime = 0f;

        // Rigidbody의 속도를 0으로 설정하여 혹시 모를 움직임을 막습니다.
        rb.velocity = Vector3.zero;
        // 캐릭터의 시작 위치를 설정
        rb.position = startPos;

        // 오프닝 시간동안 캐릭터를 서서히 이동시키고 회전시킵니다.
        while (elapsedTime < bossOpeningTime)
        {
            float t = elapsedTime / bossOpeningTime;

            // Lerp 함수를 사용하여 현재 위치를 목표 위치로 부드럽게 보간합니다
            rb.position = Vector3.Lerp(startPos, endPos, t);
            // Lerp 함수를 사용하여 현재 회전을 목표 회전으로 부드럽게 보간합니다
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);

            elapsedTime += Time.deltaTime;

            // 다음 프레임까지 기다립니다.
            yield return null;
        }

        // 오프닝 시간이 끝난 후 목표 위치에 정확하게 위치하도록 합니다.
        rb.position = endPos;
        transform.rotation = endRot;
    }
}
