using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingTxt : MonoBehaviour
{
    //  Text ������Ʈ
    public Text loadingText;
    public float minWaitTime;

    private void OnEnable()
    {
        // ������Ʈ�� Ȱ��ȭ�� �� �ִϸ��̼� �ڷ�ƾ�� �����մϴ�.
        StartCoroutine(AnimateLoadingText());
    }

    private void OnDisable()
    {
        // ������Ʈ�� ��Ȱ��ȭ�� �� �ڷ�ƾ�� ����ϴ�.
        StopAllCoroutines();
    }

    private IEnumerator AnimateLoadingText()
    {
        string[] dots = { "", ".", "..", "...", "...." };
        int dotIndex = 0;

        float elapsedTime = 0f;
        loadingText.color = Color.gray;

        while (elapsedTime < minWaitTime)
        {
            elapsedTime += Time.deltaTime;

            if (loadingText != null)
            {
                loadingText.text = "Now Loading " + dots[dotIndex];
                dotIndex = (dotIndex + 1) % dots.Length;
            }
            yield return new WaitForSeconds(0.5f); // 0.5�ʸ��� ���� �����մϴ�.
        }
       
        SceneManager.LoadScene("Main");
    }
}
