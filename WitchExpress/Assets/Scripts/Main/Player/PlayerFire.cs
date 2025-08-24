using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video; // 비디오 재생을 위해 필요한 라이브러리 추가

public class PlayerFire : MonoBehaviour
{
    //총알 생산할 공장
    public GameObject bulletFactory;

    //(오브젝트풀) 탄창에 넣을 총알 개수
    public int poolSize;

    //오브젝트 풀 리스트 생성
    public List<GameObject> bulletObjectPool;

    //총구 위치
    public GameObject firePosition;

    // 공격 모션
    public Animator witchAttack;

    // 스킬에 필요한 새로운 변수들
    public VideoPlayer skillVideoPlayer; // 동영상 재생을 위한 VideoPlayer 컴포넌트
    public GameObject skillVideoUIObject;  // Raw Image를 담고 있는 UI 오브젝트 변수
    public GameObject skillEffectObject; // 플레이어의 자식인 SkillEffect 오브젝트를 참조할 변수 


    // 스킬 사용 가능 상태를 추적하는 변수
    private bool canUseSkill = false;
    // 스킬 실행 상태 추적하는 코루틴 변수
    private Coroutine skillCoroutine;

    // 동영상 재생 여부를 추적하는 변수
    private bool hasPlayedSkillVideo = false;

    // PauseManager 스크립트와 연결하기 위한 변수
    private PauseManager pauseManager;

    // Background 스크립트와 연결하기 위한 변수
    private Background backgroundManager;

    private void Start()
    {
        skillEffectObject.SetActive(false);
        // 오브젝트 풀 리스트로 관리
        //탄창의 크기를 총알을 담을 수 있는 크기로 만들어준다.
        bulletObjectPool = new List<GameObject>();

        //탄창에 넣을 총알 개수만큼 반복하여 
        for (int i = 0; i < poolSize; i++)
        {
            //총알 공장에서 총알을 생성한다.
            GameObject bullet = Instantiate(bulletFactory);

            //총알을 오브젝트 풀 리스트에 추가 한다.
            bulletObjectPool.Add(bullet);

            // 오브젝트 비활성화 시킨다.
            bullet.SetActive(false);
        }

        // GameManager의 OnSkillActivated 이벤트 구독
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnSkillActivated += OnSkillReady;
        }

        // 씬에서 PauseManager 스크립트를 찾아 연결
        pauseManager = FindObjectOfType<PauseManager>();
        // 씬에서 Background 스크립트를 찾아 연결
        backgroundManager = FindObjectOfType<Background>();

        // 동영상 Raw Image를 담고 있는 UI 오브젝트 비활성
        skillVideoUIObject.SetActive(false);

        // 동영상이 끝났을 때를 감지하는 이벤트에 연결
        if (skillVideoPlayer != null)
        {
            skillVideoPlayer.loopPointReached += OnSkillVideoFinished;
        }
    }

    void Update()
    {
        // 게임 상태가 ‘게임 중’ 상태일 때만 조작할 수 있게 한다.
        if (GameManager.Instance.gState != GameManager.GameState.Run)
        {
            return;
        }

        // 스킬 코루틴이 실행 중이 아닐 때만 일반 공격과 스킬 입력을 받음
       
            // 일반 공격 로직
            // 목표: 사용자가 발사 버튼을 누르면 총알을 발사하고 싶다.
            // - 만약 사용자가 ctrl 버튼을 누르면
            if (Input.GetButtonDown("Fire1"))
            {
                //탄창 안에 있는 총알들 중에서
                if (bulletObjectPool.Count > 0)
                {
                    //비활성화 된(발사되지 않은) 첫번째 총알을
                    GameObject bullet = bulletObjectPool[0];

                    //총알을 활성화시킨다(발사시킨다)
                    bullet.SetActive(true);

                    //총알을 총구위치로 가져다 놓기
                    bullet.transform.position = firePosition.transform.position;

                    // 공격 모션 작동
                    witchAttack.SetTrigger("attack");

                    //오브젝트풀에서 총알 제거
                    bulletObjectPool.RemoveAt(0);
                }
            }
            // 스킬 사용 로직을 별도 메서드로 관리
            CheckForSkillInput();
        
    }

    // 스킬 사용 가능 상태 이벤트 수신시,  호출되는 메서드
    // (1) 스킬 사용 가능 상태로 변경

    private void OnSkillReady()
    {
        canUseSkill = true; // 스킬 사용 가능 상태로 변경
        Debug.Log("PlayerFire: Skill is ready!");
    }

    //  (2) 스킬 사용 입력을 감지
    private void CheckForSkillInput()
    {
        // 스킬 사용이 가능하고 Space 키를 누르면
        if (canUseSkill && Input.GetKeyDown(KeyCode.Space))
        {
            // 스킬 동작을 처리하는 코루틴 시작
            skillCoroutine = StartCoroutine(SkillSequence());
        }
    }

    // 특수 스킬 발동 코루틴
    private IEnumerator SkillSequence()
    {
        canUseSkill = false;

        // 배경 색상을 어둡게 변경
        backgroundManager.DarkenBackground();

        Debug.Log("PlayerFire: Special Skill Activated!");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.DecreasePlayerMP(80);
        }

        // 동영상이 아직 재생되지 않았을 경우에만 동영상 재생
        if (!hasPlayedSkillVideo)
        {
            if (skillVideoPlayer != null)
            {
                skillVideoUIObject.SetActive(true);
                skillVideoPlayer.Play();
                Debug.Log("스킬 동영상 재생 시작!");

                // 동영상 재생 시작과 동시에 게임 일시정지!
                // PauseManager 스크립트의 PauseGame() 메서드를 호출
                if (pauseManager != null)
                {
                    pauseManager.PauseGame();
                }

                hasPlayedSkillVideo = true;
            }
        }
        
        // 동영상 종료 후 또는 이미 재생된 경우 특수 공격 실행
        if (skillEffectObject != null)
        {
            skillEffectObject.SetActive(true);
            Debug.Log("특수 공격 활성화!");
        }
        
        // 20초 동안 스킬이 지속되도록 기다림
        yield return new WaitForSeconds(20f);
        
        Debug.Log("스킬 지속 시간 종료!");
        backgroundManager.LightenBackground();

        // 코루틴 종료 시 특수 공격을 비활성화
        if (skillEffectObject != null)
        {
            skillEffectObject.SetActive(false);
            Debug.Log("특수 공격 비활성화!");
        }
        // 코루틴 종료를 나타냄
        skillCoroutine = null;
    }

    private void OnSkillVideoFinished(VideoPlayer vp)
    {
        Debug.Log("스킬 동영상 재생 종료!");

        if (skillVideoPlayer != null)
        {
            skillVideoUIObject.SetActive(false);
        }

        // 동영상 재생이 끝나면 게임을 다시 시작합니다.
        // PauseManager 스크립트의 ResumeGame() 메서드를 호출합니다.
        if (pauseManager != null)
        {
            pauseManager.ResumeGame();
        }
    }
}
