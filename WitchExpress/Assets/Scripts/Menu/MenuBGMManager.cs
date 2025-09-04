using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBGMManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static MenuBGMManager instance;

    // ���� ���� �̸��� ������ ����
    private string currentSceneName;

    private void Awake()
    {
        // ���� �ν��Ͻ��� �̹� �����ϰ�, �ڱ� �ڽ��� �ƴ϶�� �ı�
        // �� ��ȯ �� �ٽ� ���ƿ͵� ������ �ߺ� ������� ����
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // �ν��Ͻ��� ���ٸ� �ڱ� �ڽ��� �ν��Ͻ��� �����մϴ�.
        instance = this;

        // ���� ����Ǿ �ı����� �ʵ��� �����մϴ�.
        DontDestroyOnLoad(gameObject);

        // �ʱ� �� �̸� ����
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        // ���� ���� �̸��� ���� �� �̸��� �ٸ� ��� ���� ��ȯ�Ǿ��ٰ� �Ǵ�
        if (currentSceneName != SceneManager.GetActiveScene().name)
        {
            currentSceneName = SceneManager.GetActiveScene().name;

            // ���� �ε�� �� �̸��� "StoryMenu"�� ���
            if (currentSceneName == "StoryMenu")
            {
                // DontDestroyOnLoad�� ������ �� ���� ������Ʈ �ı�
                Destroy(gameObject);
            }
        }
    }
}

