using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video; // ���� ����� ���� �ʿ��� ���̺귯�� �߰�

public class PlayerFire : MonoBehaviour
{
    //�Ѿ� ������ ����
    public GameObject bulletFactory;

    //(������ƮǮ) źâ�� ���� �Ѿ� ����
    public int poolSize;

    //������Ʈ Ǯ ����Ʈ ����
    public List<GameObject> bulletObjectPool;

    //�ѱ� ��ġ
    public GameObject firePosition;

    // ���� ���
    public Animator witchAttack;

    // ��ų�� �ʿ��� ���ο� ������
    public VideoPlayer skillVideoPlayer; // ������ ����� ���� VideoPlayer ������Ʈ
    public GameObject skillVideoUIObject;  // Raw Image�� ��� �ִ� UI ������Ʈ ����
    public GameObject skillEffectObject; // �÷��̾��� �ڽ��� SkillEffect ������Ʈ�� ������ ���� 


    // ��ų ��� ���� ���¸� �����ϴ� ����
    private bool canUseSkill = false;
    // ��ų ���� ���� �����ϴ� �ڷ�ƾ ����
    private Coroutine skillCoroutine;

    // ������ ��� ���θ� �����ϴ� ����
    private bool hasPlayedSkillVideo = false;

    // PauseManager ��ũ��Ʈ�� �����ϱ� ���� ����
    private PauseManager pauseManager;

    // Background ��ũ��Ʈ�� �����ϱ� ���� ����
    private Background backgroundManager;

    private void Start()
    {
        skillEffectObject.SetActive(false);
        // ������Ʈ Ǯ ����Ʈ�� ����
        //źâ�� ũ�⸦ �Ѿ��� ���� �� �ִ� ũ��� ������ش�.
        bulletObjectPool = new List<GameObject>();

        //źâ�� ���� �Ѿ� ������ŭ �ݺ��Ͽ� 
        for (int i = 0; i < poolSize; i++)
        {
            //�Ѿ� ���忡�� �Ѿ��� �����Ѵ�.
            GameObject bullet = Instantiate(bulletFactory);

            //�Ѿ��� ������Ʈ Ǯ ����Ʈ�� �߰� �Ѵ�.
            bulletObjectPool.Add(bullet);

            // ������Ʈ ��Ȱ��ȭ ��Ų��.
            bullet.SetActive(false);
        }

        // GameManager�� OnSkillActivated �̺�Ʈ ����
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnSkillActivated += OnSkillReady;
        }

        // ������ PauseManager ��ũ��Ʈ�� ã�� ����
        pauseManager = FindObjectOfType<PauseManager>();
        // ������ Background ��ũ��Ʈ�� ã�� ����
        backgroundManager = FindObjectOfType<Background>();

        // ������ Raw Image�� ��� �ִ� UI ������Ʈ ��Ȱ��
        skillVideoUIObject.SetActive(false);

        // �������� ������ ���� �����ϴ� �̺�Ʈ�� ����
        if (skillVideoPlayer != null)
        {
            skillVideoPlayer.loopPointReached += OnSkillVideoFinished;
        }
    }

    void Update()
    {
        // ���� ���°� ������ �ߡ� ������ ���� ������ �� �ְ� �Ѵ�.
        if (GameManager.Instance.gState != GameManager.GameState.Run)
        {
            return;
        }

        // ��ų �ڷ�ƾ�� ���� ���� �ƴ� ���� �Ϲ� ���ݰ� ��ų �Է��� ����
       
            // �Ϲ� ���� ����
            // ��ǥ: ����ڰ� �߻� ��ư�� ������ �Ѿ��� �߻��ϰ� �ʹ�.
            // - ���� ����ڰ� ctrl ��ư�� ������
            if (Input.GetButtonDown("Fire1"))
            {
                //źâ �ȿ� �ִ� �Ѿ˵� �߿���
                if (bulletObjectPool.Count > 0)
                {
                    //��Ȱ��ȭ ��(�߻���� ����) ù��° �Ѿ���
                    GameObject bullet = bulletObjectPool[0];

                    //�Ѿ��� Ȱ��ȭ��Ų��(�߻��Ų��)
                    bullet.SetActive(true);

                    //�Ѿ��� �ѱ���ġ�� ������ ����
                    bullet.transform.position = firePosition.transform.position;

                    // ���� ��� �۵�
                    witchAttack.SetTrigger("attack");

                    //������ƮǮ���� �Ѿ� ����
                    bulletObjectPool.RemoveAt(0);
                }
            }
            // ��ų ��� ������ ���� �޼���� ����
            CheckForSkillInput();
        
    }

    // ��ų ��� ���� ���� �̺�Ʈ ���Ž�,  ȣ��Ǵ� �޼���
    // (1) ��ų ��� ���� ���·� ����

    private void OnSkillReady()
    {
        canUseSkill = true; // ��ų ��� ���� ���·� ����
        Debug.Log("PlayerFire: Skill is ready!");
    }

    //  (2) ��ų ��� �Է��� ����
    private void CheckForSkillInput()
    {
        // ��ų ����� �����ϰ� Space Ű�� ������
        if (canUseSkill && Input.GetKeyDown(KeyCode.Space))
        {
            // ��ų ������ ó���ϴ� �ڷ�ƾ ����
            skillCoroutine = StartCoroutine(SkillSequence());
        }
    }

    // Ư�� ��ų �ߵ� �ڷ�ƾ
    private IEnumerator SkillSequence()
    {
        canUseSkill = false;

        // ��� ������ ��Ӱ� ����
        backgroundManager.DarkenBackground();

        Debug.Log("PlayerFire: Special Skill Activated!");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.DecreasePlayerMP(80);
        }

        // �������� ���� ������� �ʾ��� ��쿡�� ������ ���
        if (!hasPlayedSkillVideo)
        {
            if (skillVideoPlayer != null)
            {
                skillVideoUIObject.SetActive(true);
                skillVideoPlayer.Play();
                Debug.Log("��ų ������ ��� ����!");

                // ������ ��� ���۰� ���ÿ� ���� �Ͻ�����!
                // PauseManager ��ũ��Ʈ�� PauseGame() �޼��带 ȣ��
                if (pauseManager != null)
                {
                    pauseManager.PauseGame();
                }

                hasPlayedSkillVideo = true;
            }
        }
        
        // ������ ���� �� �Ǵ� �̹� ����� ��� Ư�� ���� ����
        if (skillEffectObject != null)
        {
            skillEffectObject.SetActive(true);
            Debug.Log("Ư�� ���� Ȱ��ȭ!");
        }
        
        // 20�� ���� ��ų�� ���ӵǵ��� ��ٸ�
        yield return new WaitForSeconds(20f);
        
        Debug.Log("��ų ���� �ð� ����!");
        backgroundManager.LightenBackground();

        // �ڷ�ƾ ���� �� Ư�� ������ ��Ȱ��ȭ
        if (skillEffectObject != null)
        {
            skillEffectObject.SetActive(false);
            Debug.Log("Ư�� ���� ��Ȱ��ȭ!");
        }
        // �ڷ�ƾ ���Ḧ ��Ÿ��
        skillCoroutine = null;
    }

    private void OnSkillVideoFinished(VideoPlayer vp)
    {
        Debug.Log("��ų ������ ��� ����!");

        if (skillVideoPlayer != null)
        {
            skillVideoUIObject.SetActive(false);
        }

        // ������ ����� ������ ������ �ٽ� �����մϴ�.
        // PauseManager ��ũ��Ʈ�� ResumeGame() �޼��带 ȣ���մϴ�.
        if (pauseManager != null)
        {
            pauseManager.ResumeGame();
        }
    }
}
