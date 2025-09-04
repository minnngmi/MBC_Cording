using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBGMManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static MenuBGMManager instance;

    // 현재 씬의 이름을 저장할 변수
    private string currentSceneName;

    private void Awake()
    {
        // 만약 인스턴스가 이미 존재하고, 자기 자신이 아니라면 파괴
        // 씬 전환 후 다시 돌아와도 음악이 중복 재생되지 않음
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 인스턴스가 없다면 자기 자신을 인스턴스로 지정합니다.
        instance = this;

        // 씬이 변경되어도 파괴되지 않도록 설정합니다.
        DontDestroyOnLoad(gameObject);

        // 초기 씬 이름 설정
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        // 현재 씬의 이름이 이전 씬 이름과 다를 경우 씬이 전환되었다고 판단
        if (currentSceneName != SceneManager.GetActiveScene().name)
        {
            currentSceneName = SceneManager.GetActiveScene().name;

            // 만약 로드된 씬 이름이 "StoryMenu"일 경우
            if (currentSceneName == "StoryMenu")
            {
                // DontDestroyOnLoad로 설정된 이 게임 오브젝트 파괴
                Destroy(gameObject);
            }
        }
    }
}

