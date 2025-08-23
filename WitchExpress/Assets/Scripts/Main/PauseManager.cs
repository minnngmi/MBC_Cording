using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    // ������ ���� �Ͻ����� �������� Ȯ���ϴ� ����
    private bool isPaused = false;

    // ESC Ű�� �Ͻ�����/�簳�ϴ� ���
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // �Ͻ����� ���¸� ����ϴ� �޼���
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

    // ������ �Ͻ�������Ű�� �޼���
    public void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        Debug.Log("���� �Ͻ�����");
    }

    // ������ �簳�ϴ� �޼���
    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        Debug.Log("���� �簳");
    }
}
