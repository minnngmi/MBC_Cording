using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // ��ư �������� �ش� ������ �̵�
    public void GameStart()
    {
        SceneManager.LoadScene(1); // Story Menu ������ �̵�
    }
    public void GoEndingMenu()
    {
        SceneManager.LoadScene(3); // Eding Menu ������ �̵�
    }
   
}
