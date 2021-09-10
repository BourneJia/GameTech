using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankChi_Ha : MonoBehaviour
{
    /**
    //����
    public List<AxleInfoChi_Ha> axleInfos;
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

    //�ִ���
    public Transform tracksR;
    //�ִ���
    public Transform tracksL;

    private void Start()
    {
        //��ȡ����
        turret = transform.Find("Chi_Ha_Turret");
        //��ȡ�ڹ�
        gun = turret.Find("Chi_Ha_Gun");
        //��ȡ����
        tracksR = transform.Find("Chi_Ha_Track_R");
        tracksL = transform.Find("Chi_Ha_Track_L");
    }

    private void Update()
    {
        //��ҿ��Ʋ���
        PlayerCtrl();

        //��������
        foreach (AxleInfoChi_Ha axleInfo in axleInfos) 
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
            //ת�������Ĵ�
            if (axleInfos[1] != null && axleInfo == axleInfos[1])
            {
                TrackMoveR();
                TrackMoveL();
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
        //�ƶ�
        brakeTorque = 0;
        foreach (AxleInfoChi_Ha axleInfo in axleInfos)
        {
            if (axleInfo.leftWheel.rpm > 5 && motor < 0)  //ǰ��ʱ�����¡��¡���
                brakeTorque = maxBrakeTorque;
            else if (axleInfo.leftWheel.rpm < -5 && motor > 0)  //����ʱ�����¡��ϡ���
                brakeTorque = maxBrakeTorque;
            continue;
        }
        //�����ڹܽǶ�
        turretRotTarget = Camera.main.transform.eulerAngles.y;
        turretRollTarget = Camera.main.transform.eulerAngles.x;
    }

    //�Ĵ�����R
    public void TrackMoveR()
    {
        if (tracksR == null)
            return;

        float offset = 0;

        //Debug.Log("���� TrackMoveR");

        foreach (Transform track in tracksR)
        {
            Debug.Log("TrackMoveR ����");
            MeshRenderer mr = track.gameObject.GetComponent<MeshRenderer>();
            if (mr == null) continue;
            Material mtl = mr.material;
            mtl.mainTextureOffset = new Vector2(0, offset);
        }
    }

    //�Ĵ�����L
    public void TrackMoveL()
    {
        if (tracksL == null)
            return;

        float offset = 0;

        //Debug.Log("���� TrackMoveL");

        MeshRenderer mr = tracksL.gameObject.GetComponent<MeshRenderer>();
        if (mr == null) 
        {
            Debug.Log("���� TrackMoveL mr == null");
            return;
        }
        Debug.Log("���� TrackMoveL mr != null");
        Material mtl = mr.material;
        mtl.mainTextureOffset = new Vector2(0, offset);

        //foreach (Transform track in tracksL)
        //{
        //    Debug.Log("TrackMoveL ����");
        //    MeshRenderer mr = track.gameObject.GetComponent<MeshRenderer>();
        //    if (mr == null) continue;
        //    Material mtl = mr.material;
        //    mtl.mainTextureOffset = new Vector2(0, offset);
        //}
    }
    */
}
