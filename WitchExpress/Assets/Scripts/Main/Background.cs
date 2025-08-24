using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    //스크롤속도
    public float scrollSpeed = 0.2f;

    // 배경의 렌더러 컴포넌트를 담을 변수
    private MeshRenderer backgroundRenderer;

    // 렌더러의 material을 바로 접근하기 위한 변수
    private Material backgroundMaterial;

    // 메테리얼의 기본 색상 (흰색)과 어두운 색상 (회색)을 저장할 변수
    private Color originalColor = Color.white;
    private Color darkenedColor = new Color(0.427f, 0.427f, 0.427f, 1.0f); // 6D6D6D

    private void Start()
    {

        // 배경 오브젝트의 Renderer 컴포넌트를 가져옵니다.
        backgroundRenderer = GetComponent<MeshRenderer>();

        // Renderer의 material을 한 번만 가져와서 변수에 저장합니다.
        if (backgroundRenderer != null)
        {
            backgroundMaterial = backgroundRenderer.material;
        }
    }


    // 1. 살아 있는 동안 계속 하고 싶다.
    private void Update()
    {
        // 배경 스크롤 기능은 그대로 둡니다.
        if (backgroundMaterial != null)
        {
            Vector2 direction = Vector2.up;
            backgroundMaterial.mainTextureOffset += direction * scrollSpeed * Time.deltaTime;
        }
    }

    // 배경 색상을 어둡게 만드는 메서드
    public void DarkenBackground()
    {
        Debug.Log("배경 색상이 어두워집니다.");
        if (backgroundRenderer != null)
        {
            //backgroundMaterial.SetColor("Main_BG", darkenedColor);
            backgroundMaterial.color = darkenedColor;
        }
    }

    // 배경 색상을 원래대로 (흰색) 되돌리는 메서드
    public void LightenBackground()
    {
        if (backgroundRenderer != null)
        {
            // 메테리얼의 색상을 원래의 흰색으로 변경
            backgroundMaterial.color = originalColor;

            Debug.Log("배경 색상이 원래대로 돌아왔습니다.");
        }
    }
}
