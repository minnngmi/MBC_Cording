using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    //�Ѿ� ������ ����
    public GameObject bulletFactory;

    //(������ƮǮ) źâ�� ���� �Ѿ� ����
    public int poolSize; 
    
    //������Ʈ Ǯ ����Ʈ ����
    public List<GameObject> bulletObjectPool;
    
    //�ѱ� ��ġ
    public GameObject firePosition;

    // ���� ���
    public Animator witchAttack; 

    private void Start()
    {

        // ������Ʈ Ǯ ����Ʈ�� ����
        //źâ�� ũ�⸦ �Ѿ��� ���� �� �ִ� ũ��� ������ش�.
        bulletObjectPool = new List<GameObject>();

        //źâ�� ���� �Ѿ� ������ŭ �ݺ��Ͽ� 
        for (int i = 0; i < poolSize; i++)
        {
            //�Ѿ� ���忡�� �Ѿ��� �����Ѵ�.
            GameObject bullet = Instantiate(bulletFactory);

            //�Ѿ��� ������Ʈ Ǯ ����Ʈ�� �߰� �Ѵ�.
            bulletObjectPool.Add(bullet);

            // ������Ʈ ��Ȱ��ȭ ��Ų��.
            bullet.SetActive(false);
        }
    }

    void Update()
    {
        //��ǥ: ����ڰ� �߻� ��ư�� ������ �Ѿ��� �߻��ϰ� �ʹ�.
        // - ���� ����ڰ� ctrl ��ư�� ������
        if (Input.GetButtonDown("Fire1"))
        {
            //źâ �ȿ� �ִ� �Ѿ˵� �߿���
            if (bulletObjectPool.Count > 0)
            {
                //��Ȱ��ȭ ��(�߻���� ����) ù��° �Ѿ���
                GameObject bullet = bulletObjectPool[0];
                
                //�Ѿ��� Ȱ��ȭ��Ų��(�߻��Ų��)
                bullet.SetActive(true);

                //�Ѿ��� �ѱ���ġ�� ������ ����
                bullet.transform.position = firePosition.transform.position;

                // ���� ��� �۵�
                witchAttack.SetTrigger("attack");

                //������ƮǮ���� �Ѿ� ����
                bulletObjectPool.RemoveAt(0);
            }
        }
    }
}
