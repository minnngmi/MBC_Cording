using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;


public class EndingManager : MonoBehaviour
{


    private void Update()
    {
        // ESC 키 입력 감지
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopAllCoroutines();
            // 메인 메뉴씬으로 
            SceneManager.LoadScene("Menu");
        }
    }

}
