using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Background : MonoBehaviour

{
    //스크롤속도
    public float scrollSpeed = 0.2f;

    // 배경의 렌더러 컴포넌트를 담을 변수
    // private MeshRenderer backgroundRenderer;
    // 렌더러의 material을 바로 접근하기 위한 변수
    public Material backgroundMaterial;


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
}