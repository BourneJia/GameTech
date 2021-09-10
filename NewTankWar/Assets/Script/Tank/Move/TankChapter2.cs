﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TankChapter2 : MonoBehaviour
{
    //轮轴
    public List<AxleInfoTankChapter2> axleInfos;
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
    //炮管目标角度
    private float turretRollTarget = 0;

    //炮管
    public Transform gun;
    //炮管的旋转范围
    private float maxRoll = 10f;
    private float minRoll = -4f;

    //轮子
    private Transform wheels;
    //履带
    private Transform tracks;

    //马达音源
    public AudioSource motorAudioSource;
    //马达音效
    public AudioClip motorClip;

    private void Start()
    {
        //获取炮塔
        turret = transform.Find("turret");
        //获取炮管
        gun = turret.Find("gun");
        //获取轮子
        wheels = transform.Find("wheels");
        //获取履带
        tracks = transform.Find("tracks");
        //马达音源
        motorAudioSource = gameObject.AddComponent<AudioSource>();
        motorAudioSource.spatialBlend = 1;
    }

    private void Update()
    {
        PlayerCtrl();

        //遍历车轴
        foreach (AxleInfoTankChapter2 axleInfoTankChapter in axleInfos) 
        {
            //转向
            if (axleInfoTankChapter.steering) 
            {
                axleInfoTankChapter.leftWheel.steerAngle = steering;
                axleInfoTankChapter.rightWheel.steerAngle = steering;
            }
            //马力
            if (axleInfoTankChapter.motor) 
            {
                axleInfoTankChapter.leftWheel.motorTorque = motor;
                axleInfoTankChapter.rightWheel.motorTorque = motor;
            }
            //制动
            if (true) 
            {
                axleInfoTankChapter.leftWheel.brakeTorque = brakeTorque;
                axleInfoTankChapter.rightWheel.brakeTorque = brakeTorque;
            }
            //转动轮子履带
            if (axleInfos[1] != null && axleInfoTankChapter == axleInfos[1]) 
            {
                WheelsRotation(axleInfos[1].leftWheel);
                TrackMove();
            }
        }

        //炮塔炮管旋转
        TurretRotation();
        TurretRool();
        //马达音效
        MotorSound();
    }

    //玩家控制
    public void PlayerCtrl() 
    {
        //马力和转向角
        motor = maxMotorTorque * Input.GetAxis("Vertical");
        steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        //制动
        brakeTorque = 0;
        foreach (AxleInfoTankChapter2 axleInfo in axleInfos) 
        {
            if (axleInfo.leftWheel.rpm > 5 && motor < 0) 
                brakeTorque = maxBrakeTorque;
            else if (axleInfo.rightWheel.rpm < -5 && motor > 0)
                brakeTorque = maxBrakeTorque;
            continue;
        }

        //炮塔炮管角度
        turretRotTarget = Camera.main.transform.eulerAngles.y;
        turretRollTarget = Camera.main.transform.eulerAngles.x;
    }

    //炮塔旋转
    public void TurretRotation() 
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
    public void TurretRool() 
    {
        if (Camera.main == null)
            return;
        if (turret == null)
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

    //轮子旋转
    public void WheelsRotation(WheelCollider collider) 
    {
        if (wheels == null)
            return;
        //获取旋转信息
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        //旋转每个轮子
        foreach (Transform wheel in wheels)
        {
            wheel.rotation = rotation;
        }
    }

    public void TrackMove() 
    {
        if (tracks == null)
            return;

        float offset = 0;
        if (wheels.GetChild(0) != null)
            offset = wheels.GetChild(0).localEulerAngles.x / 90f;

        foreach (Transform track in tracks) 
        {
            MeshRenderer mr = track.gameObject.GetComponent<MeshRenderer>();
            if (mr == null) continue;
            Material mtl = mr.material;
            mtl.mainTextureOffset = new Vector2(0, offset);
        }
    }

    //马达音效
    void MotorSound() 
    {
        if (motor != 0 && !motorAudioSource.isPlaying)
        {
            motorAudioSource.loop = true;
            motorAudioSource.clip = motorClip;
            motorAudioSource.Play();
        }
        else if (motor == 0)
        {
            motorAudioSource.Pause();
        }
    }
}