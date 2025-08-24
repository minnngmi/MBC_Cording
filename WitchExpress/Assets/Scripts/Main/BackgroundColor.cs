
using UnityEngine;

public class BackgroundColor : MonoBehaviour
{
    [Header("��� Material ����")]
    // Material�� public ������ ���� �����Ͽ� �ν��Ͻ� ������ �ذ�
    public Material backgroundMaterial;

    // ���׸����� �⺻ ���� (���)�� ��ο� ���� (ȸ��)�� ������ ����
    public Color normalColor = Color.white;
    public Color darkColor = new Color(0.427f, 0.427f, 0.427f, 1.0f); // 6D6D6D
    public float fadeSpeed = 2f;

    // ���� ���� ��ǥ�� �����ϴ� ����
    private Color targetColor;


    private void Start()
    {
        // �ʱ� ��ǥ ������ normalColor�� ����
        if (backgroundMaterial != null)
        {
            backgroundMaterial.color = normalColor;
            targetColor = backgroundMaterial.color;
        }
        else
        {
            Debug.LogError("Background Material�� Inspector���� �Ҵ����ּ���!");
        }
    }

    private void Update()
    {
        //������ �ε巴�� ����
        if (backgroundMaterial != null)
        {
            // ���� ������ ��ǥ �������� �ε巴�� ����(Lerp)
            Color currentColor = backgroundMaterial.color;
            Color newColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * fadeSpeed);
            backgroundMaterial.color = newColor; // �Ǵ� SetColor("_Color", newColor);
        }
    }

    // ��� ������ ��Ӱ� ����� �޼���

    public void offBackground()
    {
        Debug.Log("������ ��� ������ ��ο����ϴ�.");
        backgroundMaterial.color = darkColor;
    }
    
    public void DarkenBackground()
    {
        Debug.Log("��� ������ ��ο����ϴ�.");
        targetColor = darkColor;
    }

    // ��� ������ ������� (���) �ǵ����� �޼���
    public void LightenBackground()
    {
        Debug.Log("��� ������ ������� ���ƿԽ��ϴ�.");
        targetColor = normalColor;
    }
}
