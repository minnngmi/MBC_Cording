
using UnityEngine;

public class BackgroundColor : MonoBehaviour
{
    [Header("배경 Material 설정")]
    // Material을 public 변수로 직접 연결하여 인스턴스 문제를 해결
    public Material backgroundMaterial;

    // 메테리얼의 기본 색상 (흰색)과 어두운 색상 (회색)을 저장할 변수
    public Color normalColor = Color.white;
    public Color darkColor = new Color(0.427f, 0.427f, 0.427f, 1.0f); // 6D6D6D
    public float fadeSpeed = 2f;

    // 현재 색상 목표를 추적하는 변수
    private Color targetColor;


    private void Start()
    {
        // 초기 목표 색상을 normalColor로 설정
        if (backgroundMaterial != null)
        {
            backgroundMaterial.color = normalColor;
            targetColor = backgroundMaterial.color;
        }
        else
        {
            Debug.LogError("Background Material을 Inspector에서 할당해주세요!");
        }
    }

    private void Update()
    {
        //색상을 부드럽게 변경
        if (backgroundMaterial != null)
        {
            // 현재 색상을 목표 색상으로 부드럽게 변경(Lerp)
            Color currentColor = backgroundMaterial.color;
            Color newColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * fadeSpeed);
            backgroundMaterial.color = newColor; // 또는 SetColor("_Color", newColor);
        }
    }

    // 배경 색상을 어둡게 만드는 메서드

    public void offBackground()
    {
        Debug.Log("영상의 배경 색상이 어두워집니다.");
        backgroundMaterial.color = darkColor;
    }
    
    public void DarkenBackground()
    {
        Debug.Log("배경 색상이 어두워집니다.");
        targetColor = darkColor;
    }

    // 배경 색상을 원래대로 (흰색) 되돌리는 메서드
    public void LightenBackground()
    {
        Debug.Log("배경 색상이 원래대로 돌아왔습니다.");
        targetColor = normalColor;
    }
}
