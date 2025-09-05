using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI ����� ���� �߰�


public class BossHP : MonoBehaviour
{
    [Header("���� ���� ����")]
    public int bossHP = 10000;
    public int bossMaxHP = 10000;
    // ������ ����Ʈ ������ ���� ����
    //public GameObject damageEffect;

    [Header("���� UI ����")]
    public Slider bossHpSlider;
    // ���� �����̴��� Fill ����
    public Image BossHpFill;
    // HP ���¿� ���� ������ Fill ���� ���� �̸� ����
    public Color bossNormalColor; // Ǯ��
    public Color bossDeadlyColor; // ���鸮 ����
    // HP UI text ����
    public Text bossHpTxt;

    [Header("������ ȿ��")]
    public float blinkDuration = 0.5f; //  ������ ȿ���� �� �ð�
    public float blinkInterval = 0.1f; //  �����̴� ����

    //  ������ ������Ʈ ����
    private Renderer bossRenderer;
    //  �ڷ�ƾ ���� ����
    private IEnumerator flashCoroutine;
    // �ִϸ����� ������Ʈ ����
    private Animator bossAnimator;

    [Header("���� �ִϸ��̼� ����")]
    public float deathAnimationDuration = 1.5f; // ��� �ִϸ��̼� ��� �ð�
    // ������ �׾����� Ȯ���ϴ� ����
    private bool isDead = false;

    private Rigidbody rb;

    private void Start()
    {
        bossHpTxt.text = $"{bossHP} / {bossMaxHP}";
        rb = GetComponent<Rigidbody>();

        bossAnimator = GetComponent<Animator>();
        if (bossAnimator == null)
        {
            Debug.LogError("Animator ������Ʈ�� ã�� �� �����ϴ�. ���� ������Ʈ�� Animator ������Ʈ�� �߰��ߴ��� Ȯ���ϼ���.");
        }

        // "Pumpkin" ������Ʈ Renderer ������Ʈ
        Transform pumpkinChild = transform.Find("Pumpkin");
        if (pumpkinChild != null)
        {
            bossRenderer = pumpkinChild.GetComponent<Renderer>();
        }
        else
        {
            Debug.LogError("Renderer�� ã�� �� �����ϴ�. �ڽ� ������Ʈ 'Pumpkin'�� �����ϴ��� Ȯ���ϼ���.");
        }

        // �����̴��� �ִ밪�� ���簪 �ʱ�ȭ
        bossHpSlider.maxValue = bossMaxHP;
        bossHpSlider.value = bossHP;
        if (BossHpFill != null)
        {
            BossHpFill.color = bossNormalColor; // �ʱ� ������ NormalColor�� ����
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // ������ ������� ���� �������� �԰� �մϴ�.
        if (isDead)
        {
            return;
        }

        // ���� ���°� ������ �ߡ� ������ ���� ������ �� �ְ� �Ѵ�.
        if (GameManager.Instance.gState != GameManager.GameState.Run)
        {
            Debug.Log("���� ���� �ƴմϴ�");
            return;
        }

        if (other.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            int playerDamage = bullet.damage;
            Debug.Log("������ Ȯ�� : " + playerDamage);
            BossTakeDamage(playerDamage);
        }
    }
    public void BossTakeDamage(int amount)
    {
        // ������ �̹� �׾����� �������� ���� �ʽ��ϴ�.
        if (isDead) return;

        bossHP -= amount;
        //  �����̴��� �ؽ�Ʈ ��� ������Ʈ
        if (bossHpSlider != null)
        {
            bossHpSlider.value = bossHP;
        }
        bossHpTxt.text = $"{bossHP} / {bossMaxHP}";

        // HP�� Ư�� �� ������ �� ���� ����
        if (BossHpFill != null && (float)bossHP / bossMaxHP <= 0.3f)
        {
            BossHpFill.color = bossDeadlyColor;
        }

        //  ���� �ڷ�ƾ�� �ִٸ� ����
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        //  �����̴� ȿ�� �ڷ�ƾ ����
        flashCoroutine = BlinkEffect();
        StartCoroutine(flashCoroutine);

        // �ǰ� �ִϸ��̼� ���
        bossAnimator.SetTrigger("Damaged");

        // ������ �׾����� Ȯ��
        if (bossHP <= 0)
        {
            bossHP = 0;
           bossHpTxt.text = "0 / " + bossMaxHP;

            // �� ���� ����ǵ��� �÷��� ����
            isDead = true;

            // ��� �ִϸ��̼� ���
            bossAnimator.SetTrigger("IsDead");
            

            // ��� �� ĵ�� ��� �ڷ�ƾ ����
             StartCoroutine(CandyParty());
        }
    }

    // ������Ʈ�� �����̰� �ϴ� �ڷ�ƾ
    private IEnumerator BlinkEffect()
    {
        // �������� ������ �ڷ�ƾ ����
        if (bossRenderer == null) yield break;

        float timer = 0f;
        while (timer < blinkDuration)
        {
            // �������� �״� ���� �ݺ�
            bossRenderer.enabled = !bossRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        // �������� ���� �Ŀ��� ������ �ݵ�� Ȱ��ȭ 
        bossRenderer.enabled = true;

        // �ڷ�ƾ�� �������Ƿ� ������ null�� �ʱ�ȭ
        flashCoroutine = null;
    }

    private IEnumerator CandyParty()
    {
        yield return new WaitForSeconds(deathAnimationDuration);
        bossHpSlider.gameObject.SetActive(false);
        Debug.Log("���� ������Ʈ�� �ı��˴ϴ�.");
        Destroy(gameObject);

        // PlayerMove ��ũ��Ʈ�� �����ͼ� GoEnding() �ڷ�ƾ ����
        PlayerMove playerMove = GameObject.Find("Player").GetComponent<PlayerMove>();
        if (playerMove != null)
        {
            yield return playerMove.StartCoroutine(playerMove.GoEnding());
        }

        // ���� ������Ʈ �ı� �� "Ending" �� �ε�
        SceneManager.LoadScene("Ending");
    }
}
