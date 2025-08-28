using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class BossAttack : MonoBehaviour
{
    [Header("���õ��� ���� ����")]
    // ���� ���� �ּҽð�
    public float minTime;
    // ���� ���� �ִ�ð�
    public float maxTime;

    // ���� ���� ��ġ
    public Vector3 spawnValues;
    // ���ø� �� �ʸ��� �߻����� �����մϴ�.
    public float fireInterval = 1.0f;


    [Header("���õ��� ������Ʈ Ǯ ����")]
    // ���� ���� ����
    public GameObject[] thronsFactory;


    private void Start()
    {
        // ���õ��� �����ϰ� ��Ȱ��ȭ
        for (int i = 0; i < thronsFactory.Length; i++)
        {
            GameObject throns = Instantiate(thronsFactory[i]);
            throns.SetActive(false);
        }
    // ���� �帧�� �����ϴ� ���� �ڷ�ƾ�� �����մϴ�.
    //StartCoroutine(GameFlowRoutine());
    }


    //��� �ڷ�ƾ�� ������� �����ϴ� ���� �ڷ�ƾ
    IEnumerator GameFlowRoutine()
    {
        // GameManager�� ���°� 'Run'�� �� ������ ��ٸ��ϴ�.
        while (GameManager.Instance.gState != GameState.Run)
        {
            yield return null;
        }

        Debug.Log("���� ����! �������� 1 ���� ���� ����.");
        // ù��° ���õ��� ���� �ڷ�ƾ�� ���� ������ ��ٸ��ϴ�.
        //yield return StartCoroutine(ThronsAttackRoutine());


    }

    // ù��° ���õ��� ���� �ڷ�ƾ
   // IEnumerator ThronsAttackRoutine()


}
