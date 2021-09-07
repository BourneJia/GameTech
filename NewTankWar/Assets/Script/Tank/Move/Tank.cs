using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
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
    //private float gunRotTarget = 0;

    private void Start()
    {
        //获取炮塔
        turret = transform.Find("Chi_Ha_Turret");
        //获取炮管
        gun = transform.Find("Chi_Ha_Gun");
    }

    private void Update()
    {
        //旋转
        float steer = 20;
        float x = Input.GetAxis("Horizontal");
        transform.Rotate(0, x * steer * Time.deltaTime, 0);

        //前进后退
        float speed = 3f;
        float y = Input.GetAxis("Vertical");
        Vector3 s = y * transform.forward * speed * Time.deltaTime;
        transform.transform.position += s;

        //炮塔旋转
        //TurrentRotation();

        //炮管旋转
        turretRotTarget = Camera.main.transform.eulerAngles.y;
        GunRoll();
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
    public void GunRoll() 
    {
        if (Camera.main == null)
            return;
        if (gun == null)
            return;

        //获取角度
        Vector3 worldEuler = gun.eulerAngles;
        Vector3 localEuler = gun.localEulerAngles;
        //世界坐标系角度计算
        worldEuler.x = turretRotTarget;
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
}
