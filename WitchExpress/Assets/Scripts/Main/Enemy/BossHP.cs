using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI 사용을 위해 추가


public class BossHP : MonoBehaviour
{
    [Header("보스 상태 정보")]
    public int bossHP = 10000;
    public int bossMaxHP = 10000;
    // 데미지 이펙트 프리팹 연결 변수
    //public GameObject damageEffect;

    [Header("보스 UI 정보")]
    public Slider bossHpSlider;
    // 보스 슬라이더의 Fill 연결
    public Image BossHpFill;
    // HP 상태에 따라 변경할 Fill 색상 변수 미리 지정
    public Color bossNormalColor; // 풀피
    public Color bossDeadlyColor; // 데들리 상태
    // HP UI text 연결
    public Text bossHpTxt;

    [Header("깜빡임 효과")]
    public float blinkDuration = 0.5f; //  깜빡임 효과의 총 시간
    public float blinkInterval = 0.1f; //  깜빡이는 간격

    //  렌더러 컴포넌트 저장
    private Renderer bossRenderer;
    //  코루틴 참조 저장
    private IEnumerator flashCoroutine;
    // 애니메이터 컴포넌트 저장
    private Animator bossAnimator;

    [Header("보스 애니메이션 제어")]
    public float deathAnimationDuration = 1.5f; // 사망 애니메이션 재생 시간
    // 보스가 죽었는지 확인하는 변수
    private bool isDead = false;

    private Rigidbody rb;

    private void Start()
    {
        bossHpTxt.text = $"{bossHP} / {bossMaxHP}";
        rb = GetComponent<Rigidbody>();

        bossAnimator = GetComponent<Animator>();
        if (bossAnimator == null)
        {
            Debug.LogError("Animator 컴포넌트를 찾을 수 없습니다. 보스 오브젝트에 Animator 컴포넌트를 추가했는지 확인하세요.");
        }

        // "Pumpkin" 오브젝트 Renderer 컴포넌트
        Transform pumpkinChild = transform.Find("Pumpkin");
        if (pumpkinChild != null)
        {
            bossRenderer = pumpkinChild.GetComponent<Renderer>();
        }
        else
        {
            Debug.LogError("Renderer를 찾을 수 없습니다. 자식 오브젝트 'Pumpkin'이 존재하는지 확인하세요.");
        }

        // 슬라이더의 최대값과 현재값 초기화
        bossHpSlider.maxValue = bossMaxHP;
        bossHpSlider.value = bossHP;
        if (BossHpFill != null)
        {
            BossHpFill.color = bossNormalColor; // 초기 색상을 NormalColor로 설정
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // 보스가 살아있을 때만 데미지를 입게 합니다.
        if (isDead)
        {
            return;
        }

        // 게임 상태가 ‘게임 중’ 상태일 때만 조작할 수 있게 한다.
        if (GameManager.Instance.gState != GameManager.GameState.Run)
        {
            Debug.Log("게임 중이 아닙니다");
            return;
        }

        if (other.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            int playerDamage = bullet.damage;
            Debug.Log("데미지 확인 : " + playerDamage);
            BossTakeDamage(playerDamage);
        }
    }
    public void BossTakeDamage(int amount)
    {
        // 보스가 이미 죽었으면 데미지를 입지 않습니다.
        if (isDead) return;

        bossHP -= amount;
        //  슬라이더와 텍스트 즉시 업데이트
        if (bossHpSlider != null)
        {
            bossHpSlider.value = bossHP;
        }
        bossHpTxt.text = $"{bossHP} / {bossMaxHP}";

        // HP가 특정 값 이하일 때 색상 변경
        if (BossHpFill != null && (float)bossHP / bossMaxHP <= 0.3f)
        {
            BossHpFill.color = bossDeadlyColor;
        }

        //  기존 코루틴이 있다면 정지
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        //  깜빡이는 효과 코루틴 시작
        flashCoroutine = BlinkEffect();
        StartCoroutine(flashCoroutine);

        // 피격 애니메이션 재생
        bossAnimator.SetTrigger("Damaged");

        // 보스가 죽었는지 확인
        if (bossHP <= 0)
        {
            bossHP = 0;
           bossHpTxt.text = "0 / " + bossMaxHP;

            // 한 번만 실행되도록 플래그 설정
            isDead = true;

            // 사망 애니메이션 재생
            bossAnimator.SetTrigger("IsDead");
            

            // 사망 후 캔디 드랍 코루틴 시작
             StartCoroutine(CandyParty());
        }
    }

    // 오브젝트를 깜빡이게 하는 코루틴
    private IEnumerator BlinkEffect()
    {
        // 렌더러가 없으면 코루틴 종료
        if (bossRenderer == null) yield break;

        float timer = 0f;
        while (timer < blinkDuration)
        {
            // 렌더러를 켰다 껐다 반복
            bossRenderer.enabled = !bossRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        // 깜빡임이 끝난 후에는 렌더러 반드시 활성화 
        bossRenderer.enabled = true;

        // 코루틴이 끝났으므로 참조를 null로 초기화
        flashCoroutine = null;
    }

    private IEnumerator CandyParty()
    {
        yield return new WaitForSeconds(deathAnimationDuration);
        bossHpSlider.gameObject.SetActive(false);
        Debug.Log("보스 오브젝트가 파괴됩니다.");
        Destroy(gameObject);

        // PlayerMove 스크립트를 가져와서 GoEnding() 코루틴 시작
        PlayerMove playerMove = GameObject.Find("Player").GetComponent<PlayerMove>();
        if (playerMove != null)
        {
            yield return playerMove.StartCoroutine(playerMove.GoEnding());
        }

        // 보스 오브젝트 파괴 후 "Ending" 씬 로드
        SceneManager.LoadScene("Ending");
    }
}
