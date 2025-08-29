using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 사용을 위해 추가


public class BossHP : MonoBehaviour
{
    [Header("보스 상태 정보")]
    public int bossHP = 10000;
    public int bossMaxHP = 10000;
    // 데미지 이펙트 프리팹 연결 변수
    public GameObject damageEffect;

    [Header("보스 UI 정보")]
    public Slider bossHpSlider;
    // 보스 슬라이더의 Fill 연결
    public Image BossHpFill;
    // HP 상태에 따라 변경할 Fill 색상 변수 미리 지정
    public Color bossNormalColor; // 풀피
    public Color bossDeadlyColor; // 데들리 상태
    // HP UI text 연결
    public Text bossHpTxt;

    private void Start()
    {
        bossHpTxt.text = $"{bossHP} / {bossMaxHP}";
    }

    public void BossTakeDamage(int amount)
    {
        Debug.Log(" 30의 데미지를 받았습니다.");
        bossHP -= amount;
        bossHpTxt.text = $"{bossHP} / {bossMaxHP}";
        GameObject effectInstance = Instantiate(damageEffect, transform.position, Quaternion.identity);

        // HP가 0 미만으로 내려가지 않도록 제한
        if (bossHP < 0)
        {
            bossHP = 0;
        }
    }
}
