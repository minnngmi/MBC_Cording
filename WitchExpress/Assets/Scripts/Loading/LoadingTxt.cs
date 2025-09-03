using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingTxt : MonoBehaviour
{
    //  Text 컴포넌트
    public Text loadingText;

    private void OnEnable()
    {
        // 오브젝트가 활성화될 때 애니메이션 코루틴을 시작합니다.
        StartCoroutine(AnimateLoadingText());
    }

    private void OnDisable()
    {
        // 오브젝트가 비활성화될 때 코루틴을 멈춥니다.
        StopAllCoroutines();
    }

    private IEnumerator AnimateLoadingText()
    {
        string[] dots = { "", ".", "..", "...", "...." };
        int dotIndex = 0;

        float elapsedTime = 0f;
        float minWaitTime = 0.03f; // 최소 대기 시간 (10초)
        loadingText.color = Color.gray;

        while (elapsedTime < minWaitTime)
        {
            elapsedTime += Time.deltaTime;

            if (loadingText != null)
            {
                loadingText.text = "Now Loading " + dots[dotIndex];
                dotIndex = (dotIndex + 1) % dots.Length;
            }
            yield return new WaitForSeconds(0.5f); // 0.5초마다 점을 변경합니다.
        }
       
        SceneManager.LoadScene("Main");
    }
}
