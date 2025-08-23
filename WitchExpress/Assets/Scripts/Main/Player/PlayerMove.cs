using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float openingTime;
    public float speed;
    
    // ��� �� ������
    public float xMin, xMax, zMin, zMax;

    // ���� ���� (����)
    public float tiltAmountSide = 30f;   // �¿� �����
    public float tiltAmountForward = 1f; // �յ� �����


    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // ���� ���� �� ������ �������� ����ϴ� �ڷ�ƾ�� ����
        StartCoroutine(OpeningSequence());
    }

    private void Update()
    {

        // ���� ���°� ������ �ߡ� ������ ���� ������ �� �ְ� �Ѵ�.
        if (GameManager.Instance.gState != GameManager.GameState.Run)
        {
            return;
        }

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

        // ������ �ð�(10��) ���� ĳ���͸� ������ �̵���ŵ�ϴ�.
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


}
