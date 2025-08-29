using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UImanager : MonoBehaviour
{
    public GameObject btnImg;
        
    void Start()
    {
        StartCoroutine(ButtonOn());
        btnImg.SetActive(false);
    }

    IEnumerator ButtonOn()
    {
        yield return new WaitForSeconds(4);
        btnImg.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("StoryMenu");
    }
}
