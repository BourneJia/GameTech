using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    //轮轴
    public List<AxleInfo> axleInfos;
    //马力/最大马力
    private float motor = 0;
    public float maxMotorTorque;
    //制动/最大制动
    private float brakeTorque = 0;
    public float maxBrakeTorque = 100;
    //转向角/最大转向角
    private float steering = 0;
    public float maxSteeringAngle;

    //炮塔
    public Transform turret;
    //炮塔旋转速度
    private float turretRotSpeed = 0.5f;
    //炮塔目标角度
    private float turretRotTarget = 0;

    //炮管
    public Transform gun;
    //炮管的旋转范围
    private float maxRoll = 10f;
    private float minRoll = -4f;
    //炮管目标角度
    private float turretRollTarget = 0;

    private void Start()
    {
        //获取炮塔
        turret = transform.Find("Chi_Ha_Turret");
        //获取炮管
        gun = transform.Find("Chi_Ha_Gun");
    }

    private void Update()
    {
        //玩家控制操作
        PlayerCtrl();

        //遍历车轴
        foreach (AxleInfo axleInfo in axleInfos) 
        {
            //转向
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            //马力
            if (axleInfo.motor) 
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            //制动
            if (true) 
            {
                axleInfo.leftWheel.brakeTorque = brakeTorque;
                axleInfo.rightWheel.brakeTorque = brakeTorque;
            }
        }

        //炮塔旋转
        TurrentRotation();

        //炮管旋转
        //turretRollTarget = Camera.main.transform.eulerAngles.y;
        TurretRoll();
    }

    //炮塔旋转
    public void TurrentRotation() 
    {
        if (Camera.main == null)
            return;
        if (turret == null)
            return;

        //归一化角度
        float angle = turret.eulerAngles.y - turretRotTarget;
        if (angle < 0) angle += 360;
        if (angle > turretRotSpeed && angle < 180)
            turret.Rotate(0f, -turretRotSpeed, 0f);
        else if (angle > 180 && angle < 360 - turretRotSpeed)
            turret.Rotate(0f, turretRotSpeed, 0f);
    }

    //炮管旋转
    public void TurretRoll() 
    {
        if (Camera.main == null)
            return;
        if (gun == null)
            return;

        //获取角度
        Vector3 worldEuler = gun.eulerAngles;
        Vector3 localEuler = gun.localEulerAngles;
        //世界坐标系角度计算
        worldEuler.x = turretRollTarget;
        gun.eulerAngles = worldEuler;
        //本地坐标系角度限制
        Vector3 euler = gun.localEulerAngles;
        if (euler.x > 180)
            euler.x -= 360;

        if (euler.x > maxRoll)
            euler.x = maxRoll;
        if (euler.x < minRoll)
            euler.x = minRoll;
        gun.localEulerAngles = new Vector3(euler.x, localEuler.y, localEuler.z);
    }

    //玩家控制
    public void PlayerCtrl() 
    {
        //马力和转向角
        motor = maxMotorTorque * Input.GetAxis("Vertical");
        steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        //炮塔炮管角度
        turretRotTarget = Camera.main.transform.eulerAngles.y;
        turretRollTarget = Camera.main.transform.eulerAngles.x;
    }
}
