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
        // ESC Ű �Է� ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopAllCoroutines();
            // ���� �޴������� 
            SceneManager.LoadScene("Menu");
        }
    }

}
