using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowChi_Ha : MonoBehaviour
{
    //����
    public float distance = 8;
    //����Ƕ�
    public float rot = 0;
    //����Ƕ�
    private float roll = 30f * Mathf.PI * 2 / 360;
    //������ת�ٶ�
    public float rotSpeed = 0.2f;
    //����Ƕȷ�Χ
    private float maxRoll = 70f * Mathf.PI * 2 / 360;
    private float minRoll = -10f * Mathf.PI * 2 / 360;
    //������ת�ٶ�
    private float rollSpeed = 0.2f;
    //���뷶Χ
    public float maxDistance = 22f;
    public float minDistance = 5f;
    //����仯�ٶ�
    public float zoomSpeed = 0.2f;
    //Ŀ������
    private GameObject target;

    private void Start()
    {
        //�ҵ�̹��
        target = GameObject.Find("Chi_Ha");
        //SetTarget(GameObject.Find("Tank"));
    }

    private void LateUpdate()
    {
        //һЩ�ж�
        if (target == null)
            return;
        if (Camera.main == null)
            return;
        //������ת
        Rotate();
        //������ת
        Roll();
        //��������
        Zoom();
        //Ŀ�꺯��
        Vector3 targetPos = target.transform.position;
        //�����Ǻ����������λ��
        Vector3 cameraPos;
        float d = distance * Mathf.Cos(roll);
        float height = distance * Mathf.Sin(roll);
        cameraPos.x = targetPos.x + d * Mathf.Cos(rot);
        cameraPos.z = targetPos.z + d * Mathf.Sin(rot);
        cameraPos.y = targetPos.y + height;
        Camera.main.transform.position = cameraPos;
        //��׼Ŀ��
        Camera.main.transform.LookAt(target.transform);
    }

    //����Ŀ��
    public void SetTarget(GameObject target) 
    {
        if (target.transform.Find("Chi_Ha_Turret") != null)
            this.target = target.transform.Find("Chi_Ha_Turret").gameObject;
        else
            this.target = target;
    }

    //������ת
    void Rotate() 
    {
        float w = Input.GetAxis("Mouse X") * rotSpeed;
        rot -= w;
    }

    //������ת
    void Roll() 
    {
        float w = Input.GetAxis("Mouse Y") * rollSpeed * 0.5f;

        roll -= w;
        if (roll > maxRoll)
            roll = maxRoll;
        if (roll < minRoll)
            roll = minRoll;
    }

    //��������
    void Zoom() 
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (distance > minDistance)
                distance -= zoomSpeed;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (distance < maxDistance)
                distance += zoomSpeed;
        }
    }
}
