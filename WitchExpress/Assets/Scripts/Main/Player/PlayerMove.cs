using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("���� ������ Ÿ��")]
    public float openingTime;
    public float bossOpeningTime;

    [Header("�̵� �ӵ�")]
    public float speed;

    [Header("��� �� ������")]
    // ��� �� ������
    public float xMin;
    public float xMax;
    public float zMin;
    public float zMax;

    // ���� ���� (����)
    private float tiltAmountSide = 15f;   // �¿� �����
    private float tiltAmountForward = 30f; // �յ� �����

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // ���� ���� �� ������ �������� ����ϴ� �ڷ�ƾ�� ����
        StartCoroutine(OpeningSequence());
    }

    private void Update()
    {
        // �̵�Ű
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        rb.velocity = movement * speed;

        // �¿� ���� ����
        float tiltZ = -moveHorizontal * tiltAmountSide;
        // �յ� ���� ����
        float tiltX = moveVertical * tiltAmountForward;

        // Quaternion.Euler( X����, Yȸ��, Z���� )
        Quaternion targetRotation = Quaternion.Euler(tiltX, 0, tiltZ);

        // �ε巴�� ȸ�� ���� (0.1f�� �ӵ����, ���� Ŭ���� ������ ����)
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

        // �÷��̾� �̵����� ����
        rb.position = new Vector3(
           Mathf.Clamp(rb.position.x, xMin, xMax),
           0,
           Mathf.Clamp(rb.position.z, zMin, zMax));

    }

    // ������ �������� ó���ϴ� �ڷ�ƾ
    private IEnumerator OpeningSequence()
    {
        // ĳ������ ���� ��ġ�� ��ǥ ��ġ�� �����մϴ�.
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(0, 0, -1);
        float elapsedTime = 0f;

        // Rigidbody�� �ӵ��� 0���� �����Ͽ� Ȥ�� �� �������� �����ϴ�.
        rb.velocity = Vector3.zero;

        // ĳ������ ���� ��ġ�� ����
        rb.position = startPos;

        // ������ �ð����� ĳ���͸� ������ �̵���ŵ�ϴ�.
        while (elapsedTime < openingTime)
        {
            // Lerp �Լ��� ����Ͽ� ���� ��ġ�� ��ǥ ��ġ�� �ε巴�� �����մϴ�.
            rb.position = Vector3.Lerp(startPos, endPos, elapsedTime / openingTime);
            elapsedTime += Time.deltaTime;

            // ���� �����ӱ��� ��ٸ��ϴ�.
            yield return null;
        }

        // ������ �ð��� ���� �� ��ǥ ��ġ�� ��Ȯ�ϰ� ��ġ�ϵ��� �մϴ�.
        rb.position = endPos;

        // �������� �������Ƿ� GameManager�� ���¸� '���� ��'���� �����մϴ�.
        // �� �ڵ尡 ������ �÷��̾ �������� �ʽ��ϴ�.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.gState = GameManager.GameState.Run;
        }
    }

    // ���� ���� �������� ó���ϴ� �ڷ�ƾ
    public IEnumerator BossOpening()
    {
        // ĳ������ ���� ��ġ�� ��ǥ ��ġ�� �����մϴ�.
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(0, 0, -2);
        float elapsedTime = 0f;

        // Rigidbody�� �ӵ��� 0���� �����Ͽ� Ȥ�� �� �������� �����ϴ�.
        rb.velocity = Vector3.zero;
        // ĳ������ ���� ��ġ�� ����
        rb.position = startPos;

        // ������ �ð����� ĳ���͸� ������ �̵���ŵ�ϴ�.
        while (elapsedTime < bossOpeningTime)
        {
            // Lerp �Լ��� ����Ͽ� ���� ��ġ�� ��ǥ ��ġ�� �ε巴�� �����մϴ�.
            rb.position = Vector3.Lerp(startPos, endPos, elapsedTime / bossOpeningTime);
            elapsedTime += Time.deltaTime;

            // ���� �����ӱ��� ��ٸ��ϴ�.
            yield return null;
        }

        // ������ �ð��� ���� �� ��ǥ ��ġ�� ��Ȯ�ϰ� ��ġ�ϵ��� �մϴ�.
        rb.position = endPos;

        // �������� �������Ƿ� GameManager�� ���¸� '���� ��'���� �����մϴ�.
        // �� �ڵ尡 ������ �÷��̾ �������� �ʽ��ϴ�.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.gState = GameManager.GameState.Run;
        }
    }

}
