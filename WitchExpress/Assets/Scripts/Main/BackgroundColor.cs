
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
    //private Color targetColor;
    // 배경의 렌더러 컴포넌트를 담을 변수
    private MeshRenderer backgroundRenderer;

    private void Start()
    {
        // 배경 오브젝트의 Renderer 컴포넌트를 가져옵니다.
        //backgroundRenderer = GetComponent<MeshRenderer>();

        // 초기 목표 색상을 normalColor로 설정
        if (backgroundMaterial != null)
        {
            Debug.Log($"Main_BG Material 찾음: _Color");

            backgroundMaterial.color = normalColor;
            
        }
        else
        {
            Debug.LogError("Background Material을 Inspector에서 할당해주세요!");
        }
    }


    // 배경 색상을 어둡게 만드는 메서드
    public void DarkenBackground()
    {
        Debug.Log("배경 색상이 어두워집니다.");
        backgroundMaterial.color = darkColor;
    }

    // 배경 색상을 원래대로 (흰색) 되돌리는 메서드
    public void LightenBackground()
    {
        Debug.Log("배경 색상이 원래대로 돌아왔습니다.");
        backgroundMaterial.color = normalColor;
    }
}
