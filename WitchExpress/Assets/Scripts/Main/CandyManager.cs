using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Enemy �����տ� ����� ��ũ��Ʈ
public class CandyManager : MonoBehaviour
{
    // ĵ�� ����(ĵ�� �����յ��� ��� �迭) ����
    public GameObject[] candyFactory;

    // Ư�� ������ ĵ�� ���� ��ġ�� ������Ű�� �޼���
    // poolIndex : Bullet ��ũ��Ʈ���� �����ϰ� �޾ƿ��� ����
    // ��ȯ��     : ������(�Ǵ� ������) ĵ�� GameObject
    public GameObject CreatCandy(int poolIndex)
    {
        // �ش� ������ ���������� ����
        GameObject candy = Instantiate(candyFactory[poolIndex], transform.position, Quaternion.identity);

        // ������ ĵ�� ��ȯ
        return candy;
    }
}
