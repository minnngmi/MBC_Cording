
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
    //private Color targetColor;
    // ����� ������ ������Ʈ�� ���� ����
    private MeshRenderer backgroundRenderer;

    private void Start()
    {
        // ��� ������Ʈ�� Renderer ������Ʈ�� �����ɴϴ�.
        //backgroundRenderer = GetComponent<MeshRenderer>();

        // �ʱ� ��ǥ ������ normalColor�� ����
        if (backgroundMaterial != null)
        {
            Debug.Log($"Main_BG Material ã��: _Color");

            backgroundMaterial.color = normalColor;
            
        }
        else
        {
            Debug.LogError("Background Material�� Inspector���� �Ҵ����ּ���!");
        }
    }


    // ��� ������ ��Ӱ� ����� �޼���
    public void DarkenBackground()
    {
        Debug.Log("��� ������ ��ο����ϴ�.");
        backgroundMaterial.color = darkColor;
    }

    // ��� ������ ������� (���) �ǵ����� �޼���
    public void LightenBackground()
    {
        Debug.Log("��� ������ ������� ���ƿԽ��ϴ�.");
        backgroundMaterial.color = normalColor;
    }
}
