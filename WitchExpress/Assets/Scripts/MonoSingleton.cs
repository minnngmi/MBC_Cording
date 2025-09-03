using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> :  MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    // ✨ public static 프로퍼티로 외부에서 인스턴스에 접근할 수 있도록 합니다.
    public static T Instance
    {
        get
        {
            // 만약 인스턴스가 아직 할당되지 않았다면
            if (instance == null)
            {
                // 씬에서 해당 타입의 오브젝트를 찾아 할당합니다.
                instance = FindObjectOfType<T>();

                // 만약 씬에 오브젝트가 없다면, 오류 메시지를 출력합니다.
                if (instance == null)
                {
                    Debug.LogError("싱글톤 인스턴스를 찾을 수 없습니다. " + typeof(T).Name + " 타입의 오브젝트가 씬에 있는지 확인하세요.");
                }
            }
            return instance;
        }
    }

    // ✨ Awake()는 인스턴스 초기화에 사용됩니다.
    protected virtual void Awake()
    {
        // 인스턴스가 이미 존재하고, 자기 자신이 아니라면
        if (instance != null && instance != this)
        {
            // 자기 자신을 파괴하여 중복 생성을 막습니다.
            Destroy(gameObject);
            return;
        }

        // 자기 자신을 인스턴스로 할당합니다.
        instance = this as T;

        // 씬 전환 시에도 파괴되지 않도록 합니다.
        DontDestroyOnLoad(gameObject);
    }
}
