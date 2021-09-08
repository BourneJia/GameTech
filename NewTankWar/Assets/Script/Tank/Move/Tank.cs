using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    //����
    public List<AxleInfo> axleInfos;
    //����/�������
    private float motor = 0;
    public float maxMotorTorque;
    //�ƶ�/����ƶ�
    private float brakeTorque = 0;
    public float maxBrakeTorque = 100;
    //ת���/���ת���
    private float steering = 0;
    public float maxSteeringAngle;

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
    private float turretRollTarget = 0;

    private void Start()
    {
        //��ȡ����
        turret = transform.Find("Chi_Ha_Turret");
        //��ȡ�ڹ�
        gun = transform.Find("Chi_Ha_Gun");
    }

    private void Update()
    {
        //��ҿ��Ʋ���
        PlayerCtrl();

        //��������
        foreach (AxleInfo axleInfo in axleInfos) 
        {
            //ת��
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            //����
            if (axleInfo.motor) 
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            //�ƶ�
            if (true) 
            {
                axleInfo.leftWheel.brakeTorque = brakeTorque;
                axleInfo.rightWheel.brakeTorque = brakeTorque;
            }
        }

        //������ת
        TurrentRotation();

        //�ڹ���ת
        //turretRollTarget = Camera.main.transform.eulerAngles.y;
        TurretRoll();
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
    public void TurretRoll() 
    {
        if (Camera.main == null)
            return;
        if (gun == null)
            return;

        //��ȡ�Ƕ�
        Vector3 worldEuler = gun.eulerAngles;
        Vector3 localEuler = gun.localEulerAngles;
        //��������ϵ�Ƕȼ���
        worldEuler.x = turretRollTarget;
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

    //��ҿ���
    public void PlayerCtrl() 
    {
        //������ת���
        motor = maxMotorTorque * Input.GetAxis("Vertical");
        steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        //�����ڹܽǶ�
        turretRotTarget = Camera.main.transform.eulerAngles.y;
        turretRollTarget = Camera.main.transform.eulerAngles.x;
    }
}
