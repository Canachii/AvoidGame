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
        //360번 반복
        for (int i = 0; i < 360; i += amount)
        {
            //총알 생성
            GameObject temp = Instantiate(bullet);

            //총알 생성 위치를 (0,0) 좌표로 한다.
            temp.transform.position = transform.position;

            //Z에 값이 변해야 회전이 이루어지므로, Z에 i를 대입한다.
            temp.transform.rotation = Quaternion.Euler(0, 0, i);
        }
    }

    public void CircleTargetShot(GameObject bullet, int amount, float rotateTime)
    {
        //Target방향으로 발사될 오브젝트 수록
        var bl = new List<Transform>();

        for (int i = 0; i < 360; i += amount)
        {
            //총알 생성
            var temp = Instantiate(bullet);

            //총알 생성 위치를 (0,0) 좌표로 한다.
            temp.transform.position = transform.position;

            //?초후에 Target에게 날아갈 오브젝트 수록
            bl.Add(temp.transform);

            //Z에 값이 변해야 회전이 이루어지므로, Z에 i를 대입한다.
            temp.transform.rotation = Quaternion.Euler(0, 0, i);
        }
        //총알을 Target 방향으로 이동시킨다.
        StartCoroutine(BulletToTarget(bl, target, rotateTime));
    }

    IEnumerator BulletToTarget(List<Transform> bl, Transform target, float rotateTime)
    {
        //0.5초 후에 시작
        yield return new WaitForSeconds(rotateTime);

        for (int i = 0; i < bl.Count; i++)
        {
            //현재 총알의 위치에서 플레이의 위치의 벡터값을 뻴셈하여 방향을 구함
            var target_dir = target.transform.position - bl[i].position;

            //x,y의 값을 조합하여 Z방향 값으로 변형함. -> ~도 단위로 변형
            var angle = Mathf.Atan2(target_dir.y, target_dir.x) * Mathf.Rad2Deg;

            //Target 방향으로 이동
            bl[i].rotation = Quaternion.Euler(0, 0, angle);
        }

        //데이터 해제
        bl.Clear();
    }

    public void SpinShot(GameObject bullet, float rot_Speed)
    {
        //회전
        transform.Rotate(Vector3.forward * rot_Speed * 100 * Time.deltaTime);

        //총알 생성
        GameObject temp = Instantiate(bullet);

        //총알 생성 위치를 머즐 입구로 한다.
        temp.transform.position = transform.position;

        //총알의 방향을 오브젝트의 방향으로 한다.
        //->해당 오브젝트가 오브젝트가 360도 회전하고 있으므로, Rotation이 방향이 됨.
        temp.transform.rotation = transform.rotation;
    }

    public void SpreadShot(GameObject bullet, float rot_Speed)
    {
        //총알 생성
        GameObject temp = Instantiate(bullet);

        //총알 생성 위치를 머즐 입구로 한다.
        temp.transform.position = transform.position;

        //총알의 방향을 오브젝트의 방향으로 한다.
        //->해당 오브젝트가 오브젝트가 360도 회전하고 있으므로, Rotation이 방향이 됨.
        temp.transform.rotation = transform.rotation;

        //회전
        temp.transform.Rotate(Vector3.forward * rot_Speed * 100 * Time.deltaTime);
    }

    public void NormalShot(GameObject bullet)
    {
        //총알 생성
        var temp = Instantiate(bullet);

        //총알 생성 위치를 머즐 입구로 한다.
        temp.transform.position = transform.position;

        //총알의 방향을 Center의 방향으로 한다.
        //->참조된 Center오브젝트가 Target을 바라보고 있으므로, Rotation이 방향이 됨.
        temp.transform.rotation = transform.rotation;
    }

    public void TargetShot(GameObject bullet)
    {
        var temp = Instantiate(bullet);

        var rot = target.position - transform.position;

        //x,y의 값을 조합하여 Z방향 값으로 변형함. -> ~도 단위로 변형
        var angle = Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg;

        temp.transform.position = transform.position;

        //해당 타겟 방향으로 회전한다.
        temp.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
