using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankChi_Ha : MonoBehaviour
{
    /**
    //轮轴
    public List<AxleInfoChi_Ha> axleInfos;
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

    //轮带右
    public Transform tracksR;
    //轮带左
    public Transform tracksL;

    private void Start()
    {
        //获取炮塔
        turret = transform.Find("Chi_Ha_Turret");
        //获取炮管
        gun = turret.Find("Chi_Ha_Gun");
        //获取轮子
        tracksR = transform.Find("Chi_Ha_Track_R");
        tracksL = transform.Find("Chi_Ha_Track_L");
    }

    private void Update()
    {
        //玩家控制操作
        PlayerCtrl();

        //遍历车轴
        foreach (AxleInfoChi_Ha axleInfo in axleInfos) 
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
            //转动轮子履带
            if (axleInfos[1] != null && axleInfo == axleInfos[1])
            {
                TrackMoveR();
                TrackMoveL();
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
        //制动
        brakeTorque = 0;
        foreach (AxleInfoChi_Ha axleInfo in axleInfos)
        {
            if (axleInfo.leftWheel.rpm > 5 && motor < 0)  //前进时，按下“下”键
                brakeTorque = maxBrakeTorque;
            else if (axleInfo.leftWheel.rpm < -5 && motor > 0)  //后退时，按下“上”键
                brakeTorque = maxBrakeTorque;
            continue;
        }
        //炮塔炮管角度
        turretRotTarget = Camera.main.transform.eulerAngles.y;
        turretRollTarget = Camera.main.transform.eulerAngles.x;
    }

    //履带滚动R
    public void TrackMoveR()
    {
        if (tracksR == null)
            return;

        float offset = 0;

        //Debug.Log("进入 TrackMoveR");

        foreach (Transform track in tracksR)
        {
            Debug.Log("TrackMoveR 遍历");
            MeshRenderer mr = track.gameObject.GetComponent<MeshRenderer>();
            if (mr == null) continue;
            Material mtl = mr.material;
            mtl.mainTextureOffset = new Vector2(0, offset);
        }
    }

    //履带滚动L
    public void TrackMoveL()
    {
        if (tracksL == null)
            return;

        float offset = 0;

        //Debug.Log("进入 TrackMoveL");

        MeshRenderer mr = tracksL.gameObject.GetComponent<MeshRenderer>();
        if (mr == null) 
        {
            Debug.Log("进入 TrackMoveL mr == null");
            return;
        }
        Debug.Log("进入 TrackMoveL mr != null");
        Material mtl = mr.material;
        mtl.mainTextureOffset = new Vector2(0, offset);

        //foreach (Transform track in tracksL)
        //{
        //    Debug.Log("TrackMoveL 遍历");
        //    MeshRenderer mr = track.gameObject.GetComponent<MeshRenderer>();
        //    if (mr == null) continue;
        //    Material mtl = mr.material;
        //    mtl.mainTextureOffset = new Vector2(0, offset);
        //}
    }
    */
}
