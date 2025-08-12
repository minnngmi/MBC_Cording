using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // 버튼 눌렀을시 해당 씬으로 이동
    public void GameStart()
    {
        SceneManager.LoadScene(1); // Story Menu 씬으로 이동
    }
    public void GoEndingMenu()
    {
        SceneManager.LoadScene(3); // Eding Menu 씬으로 이동
    }
   
}
