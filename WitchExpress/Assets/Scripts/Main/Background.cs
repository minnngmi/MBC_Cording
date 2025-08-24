using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    //��ũ�Ѽӵ�
    public float scrollSpeed = 0.2f;

    // ����� ������ ������Ʈ�� ���� ����
    private MeshRenderer backgroundRenderer;

    // �������� material�� �ٷ� �����ϱ� ���� ����
    private Material backgroundMaterial;

    // ���׸����� �⺻ ���� (���)�� ��ο� ���� (ȸ��)�� ������ ����
    private Color originalColor = Color.white;
    private Color darkenedColor = new Color(0.427f, 0.427f, 0.427f, 1.0f); // 6D6D6D

    private void Start()
    {

        // ��� ������Ʈ�� Renderer ������Ʈ�� �����ɴϴ�.
        backgroundRenderer = GetComponent<MeshRenderer>();

        // Renderer�� material�� �� ���� �����ͼ� ������ �����մϴ�.
        if (backgroundRenderer != null)
        {
            backgroundMaterial = backgroundRenderer.material;
        }
    }


    // 1. ��� �ִ� ���� ��� �ϰ� �ʹ�.
    private void Update()
    {
        // ��� ��ũ�� ����� �״�� �Ӵϴ�.
        if (backgroundMaterial != null)
        {
            Vector2 direction = Vector2.up;
            backgroundMaterial.mainTextureOffset += direction * scrollSpeed * Time.deltaTime;
        }
    }

    // ��� ������ ��Ӱ� ����� �޼���
    public void DarkenBackground()
    {
        Debug.Log("��� ������ ��ο����ϴ�.");
        if (backgroundRenderer != null)
        {
            //backgroundMaterial.SetColor("Main_BG", darkenedColor);
            backgroundMaterial.color = darkenedColor;
        }
    }

    // ��� ������ ������� (���) �ǵ����� �޼���
    public void LightenBackground()
    {
        if (backgroundRenderer != null)
        {
            // ���׸����� ������ ������ ������� ����
            backgroundMaterial.color = originalColor;

            Debug.Log("��� ������ ������� ���ƿԽ��ϴ�.");
        }
    }
}
