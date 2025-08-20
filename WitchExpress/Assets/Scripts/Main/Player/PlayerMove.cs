using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static float opening = 10; 
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
    }

    private void Update()
    {
        // ���� ���۽� ���� ������ Ÿ��
        if (opening > 0)
        {
            rb.velocity = new Vector3(0, 0, 3f);
            opening -= Time.fixedDeltaTime;
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
}
