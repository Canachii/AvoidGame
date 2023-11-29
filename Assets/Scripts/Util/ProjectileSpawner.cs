using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    Transform target;

    private void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    public void CircleShot(GameObject bullet, int amount)
    {
        //360�� �ݺ�
        for (int i = 0; i < 360; i += amount)
        {
            //�Ѿ� ����
            GameObject temp = Instantiate(bullet);

            //�Ѿ� ���� ��ġ�� (0,0) ��ǥ�� �Ѵ�.
            temp.transform.position = transform.position;

            //Z�� ���� ���ؾ� ȸ���� �̷�����Ƿ�, Z�� i�� �����Ѵ�.
            temp.transform.rotation = Quaternion.Euler(0, 0, i);
        }
    }

    public void CircleTargetShot(GameObject bullet, int amount, float rotateTime)
    {
        //Target�������� �߻�� ������Ʈ ����
        var bl = new List<Transform>();

        for (int i = 0; i < 360; i += amount)
        {
            //�Ѿ� ����
            var temp = Instantiate(bullet);

            //�Ѿ� ���� ��ġ�� (0,0) ��ǥ�� �Ѵ�.
            temp.transform.position = transform.position;

            //?���Ŀ� Target���� ���ư� ������Ʈ ����
            bl.Add(temp.transform);

            //Z�� ���� ���ؾ� ȸ���� �̷�����Ƿ�, Z�� i�� �����Ѵ�.
            temp.transform.rotation = Quaternion.Euler(0, 0, i);
        }
        //�Ѿ��� Target �������� �̵���Ų��.
        StartCoroutine(BulletToTarget(bl, target, rotateTime));
    }

    IEnumerator BulletToTarget(List<Transform> bl, Transform target, float rotateTime)
    {
        //0.5�� �Ŀ� ����
        yield return new WaitForSeconds(rotateTime);

        for (int i = 0; i < bl.Count; i++)
        {
            //���� �Ѿ��� ��ġ���� �÷����� ��ġ�� ���Ͱ��� �y���Ͽ� ������ ����
            var target_dir = target.transform.position - bl[i].position;

            //x,y�� ���� �����Ͽ� Z���� ������ ������. -> ~�� ������ ����
            var angle = Mathf.Atan2(target_dir.y, target_dir.x) * Mathf.Rad2Deg;

            //Target �������� �̵�
            bl[i].rotation = Quaternion.Euler(0, 0, angle);
        }

        //������ ����
        bl.Clear();
    }

    public void SpinShot(GameObject bullet, float rot_Speed)
    {
        //ȸ��
        transform.Rotate(Vector3.forward * rot_Speed * 100 * Time.deltaTime);

        //�Ѿ� ����
        GameObject temp = Instantiate(bullet);

        //�Ѿ� ���� ��ġ�� ���� �Ա��� �Ѵ�.
        temp.transform.position = transform.position;

        //�Ѿ��� ������ ������Ʈ�� �������� �Ѵ�.
        //->�ش� ������Ʈ�� ������Ʈ�� 360�� ȸ���ϰ� �����Ƿ�, Rotation�� ������ ��.
        temp.transform.rotation = transform.rotation;
    }

    public void SpreadShot(GameObject bullet, float rot_Speed)
    {
        //�Ѿ� ����
        GameObject temp = Instantiate(bullet);

        //�Ѿ� ���� ��ġ�� ���� �Ա��� �Ѵ�.
        temp.transform.position = transform.position;

        //�Ѿ��� ������ ������Ʈ�� �������� �Ѵ�.
        //->�ش� ������Ʈ�� ������Ʈ�� 360�� ȸ���ϰ� �����Ƿ�, Rotation�� ������ ��.
        temp.transform.rotation = transform.rotation;

        //ȸ��
        temp.transform.Rotate(Vector3.forward * rot_Speed * 100 * Time.deltaTime);
    }

    public void NormalShot(GameObject bullet)
    {
        //�Ѿ� ����
        var temp = Instantiate(bullet);

        //�Ѿ� ���� ��ġ�� ���� �Ա��� �Ѵ�.
        temp.transform.position = transform.position;

        //�Ѿ��� ������ Center�� �������� �Ѵ�.
        //->������ Center������Ʈ�� Target�� �ٶ󺸰� �����Ƿ�, Rotation�� ������ ��.
        temp.transform.rotation = transform.rotation;
    }

    public void TargetShot(GameObject bullet)
    {
        var temp = Instantiate(bullet);

        var rot = target.position - transform.position;

        //x,y�� ���� �����Ͽ� Z���� ������ ������. -> ~�� ������ ����
        var angle = Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg;

        temp.transform.position = transform.position;

        //�ش� Ÿ�� �������� ȸ���Ѵ�.
        temp.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
