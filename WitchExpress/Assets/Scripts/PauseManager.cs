using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    // 게임이 현재 일시정지 상태인지 확인하는 변수
    private bool isPaused = false;

    // ESC 키로 일시정지/재개하는 기능
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // 일시정지 상태를 토글하는 메서드
    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    // 게임을 일시정지시키는 메서드
    public void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        Debug.Log("게임 일시정지");
    }

    // 게임을 재개하는 메서드
    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        Debug.Log("게임 재개");
    }
}
