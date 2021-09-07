using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    //����
    public Transform turret;
    //������ת�ٶ�
    private float turretRotSpeed = 0.5f;
    //����Ŀ��Ƕ�
    private float turretRotTarget = 0;

    //�ڹ�
    public Transform gun;
    //�ڹܵ���ת��Χ
    private float maxRoll = 10f;
    private float minRoll = -4f;
    //�ڹ�Ŀ��Ƕ�
    //private float gunRotTarget = 0;

    private void Start()
    {
        //��ȡ����
        turret = transform.Find("Chi_Ha_Turret");
        //��ȡ�ڹ�
        gun = transform.Find("Chi_Ha_Gun");
    }

    private void Update()
    {
        //��ת
        float steer = 20;
        float x = Input.GetAxis("Horizontal");
        transform.Rotate(0, x * steer * Time.deltaTime, 0);

        //ǰ������
        float speed = 3f;
        float y = Input.GetAxis("Vertical");
        Vector3 s = y * transform.forward * speed * Time.deltaTime;
        transform.transform.position += s;

        //������ת
        //TurrentRotation();

        //�ڹ���ת
        turretRotTarget = Camera.main.transform.eulerAngles.y;
        GunRoll();
    }

    //������ת
    public void TurrentRotation() 
    {
        if (Camera.main == null)
            return;
        if (turret == null)
            return;

        //��һ���Ƕ�
        float angle = turret.eulerAngles.y - turretRotTarget;
        if (angle < 0) angle += 360;
        if (angle > turretRotSpeed && angle < 180)
            turret.Rotate(0f, -turretRotSpeed, 0f);
        else if (angle > 180 && angle < 360 - turretRotSpeed)
            turret.Rotate(0f, turretRotSpeed, 0f);
    }

    //�ڹ���ת
    public void GunRoll() 
    {
        if (Camera.main == null)
            return;
        if (gun == null)
            return;

        //��ȡ�Ƕ�
        Vector3 worldEuler = gun.eulerAngles;
        Vector3 localEuler = gun.localEulerAngles;
        //��������ϵ�Ƕȼ���
        worldEuler.x = turretRotTarget;
        gun.eulerAngles = worldEuler;
        //��������ϵ�Ƕ�����
        Vector3 euler = gun.localEulerAngles;
        if (euler.x > 180)
            euler.x -= 360;

        if (euler.x > maxRoll)
            euler.x = maxRoll;
        if (euler.x < minRoll)
            euler.x = minRoll;
        gun.localEulerAngles = new Vector3(euler.x, localEuler.y, localEuler.z);
    }
}
