using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    //�����Ƶ�̹��
    public TankChapter2 tank;
    //������̹��
    private GameObject target;
    //��Ұ��Χ
    private float sightDistance = 30;
    //��һ����Ѱʱ��
    private float lastSearchTargetTime = 0;
    //��Ѱ���
    private float searchTargetInterval = 3;

    //�ϴθ���·��ʱ��
    private float lastUpdateWaypointTime = float.MinValue;

    //����·��CD
    private float updateWaypointInterval = 10;

    //״̬ö��
    public enum Status
    { 
        Patrol,
        Attack,
    }
    private Status status = Status.Patrol;
    //����״̬
    public void ChangeStatus(Status status) 
    {
        if (status == Status.Patrol)
            return;//PatrolStart();
        else if (status == Status.Attack)
            return;// AttackStart();
    }

    //״̬����
    private void FixedUpdate()
    {
        if (tank.ctrlType != TankChapter2.CtrlType.computer)
            return;
        if (status == Status.Patrol)
            return;// PatrolUpdate();
        else if (status == Status.Attack)
            return;// AttackUpdate();
    }

    //Ѳ�߿�ʼ
    void PatrolStart() 
    {
    
    }

    //������ʼ
    void AttackStart() 
    {
        Vector3 targetPos = target.transform.position;
        path.InitByNavMeshPath(transform.position, targetPos);
    }

    //Ѳ����
    void PatrolUpdate() 
    {
        //���ֵ���
        if (target != null)
            ChangeStatus(Status.Attack);
        //ʱ����
        float interval = Time.time - lastUpdateWaypointTime;
        if (interval < updateWaypointInterval)
            return;
        lastUpdateWaypointTime = Time.time;
        //����Ѳ�ߵ�
        if (path.wayPoints == null || path.isFinish) 
        {
            GameObject obj = GameObject.Find("WaypointContainer");
            {
                int count = obj.transform.childCount;
                if (count == 0) return;
                int index = Random.Range(0, count);
                Vector3 targetPos = obj.transform.GetChild(index).position;
                path.InitByNavMeshPath(transform.position, targetPos);
            }
        }
    }

    //������
    void AttackUpdate() 
    {
        //Ŀ�궪ʧ
        if (target == null)
            ChangeStatus(Status.Patrol);
        //ʱ����
        float interval = Time.time - lastUpdateWaypointTime;
        if (interval < updateWaypointInterval)
            return;
        lastUpdateWaypointTime = Time.time;
        //����·��
        Vector3 targetPos = target.transform.position;
        path.InitByNavMeshPath(transform.position, targetPos);
    }

    //��ѰĿ��
    void TargetUpdate() 
    {
        //cdʱ��
        float interval = Time.time - lastSearchTargetTime;
        if (interval < searchTargetInterval)
            return;
        lastSearchTargetTime = Time.time;

        //����Ŀ���������ж��Ƿ�ʧĿ��
        if (target != null)
            HasTarget();
        else
            NoTarget();
    }

    private void Update()
    {
        TargetUpdate();
        //����
        if (path.IsReach(transform)) 
        {
            path.NextWaypoint();
        }
    }

    void HasTarget() 
    {
        TankChapter2 targetTank = target.GetComponent<TankChapter2>();
        Vector3 pos = transform.position;
        Vector3 targetPos = target.transform.position;

        if (targetTank.ctrlType == TankChapter2.CtrlType.none)
        {
            Debug.Log("Ŀ����������ʧ����");
            target = null;
        }
        else if (Vector3.Distance(pos, targetPos) > sightDistance) 
        {
            Debug.Log("�����Զ����ʧĿ��");
            target = null;
        }
    }

    void NoTarget() 
    {
        //��С����ֵ
        float minHp = float.MaxValue;
        //��������̹��
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Tank");
        for (int i = 0; i < targets.Length; i++)
        {
            //Tank���
            TankChapter2 tank = targets[i].GetComponent<TankChapter2>();
            if (tank == null)
                continue;
            //�Լ�
            if (targets[i] == gameObject)
                continue;
            //����
            if (tank.ctrlType == TankChapter2.CtrlType.none)
                continue;
            //�жϾ���
            Vector3 pos = transform.position;
            Vector3 targetPos = targets[i].transform.position;
            if (Vector3.Distance(pos, targetPos) > sightDistance)
                continue;
            //�ж�����ֵ
            if (minHp > tank.hp)
                target = tank.gameObject;
        }
        //����
        if (target != null)
            Debug.Log("��ȡĿ�� " + target.name);
    }
    //������
    public void OnAttecked(GameObject attackTank) 
    {
        target = attackTank;
    }

    //��ȡ�ڹܺ�������Ŀ��Ƕ�
    public Vector3 GetTurretTarget() 
    {
        //û��Ŀ�꣬����������̹��ǰ��
        if (target == null)
        {
            float y = transform.eulerAngles.y;
            Vector3 rot = new Vector3(0, y, 0);
            return rot;
        }
        //��Ŀ�꣬���׼Ŀ��
        else 
        {
            Vector3 pos = transform.position;
            Vector3 targetPos = target.transform.position;
            Vector3 vec = targetPos - pos;
            return Quaternion.LookRotation(vec).eulerAngles;
        }
    }

    //�Ƿ����ڵ�
    public bool IsShoot() 
    {
        if (target == null)
            return false;

        //Ŀ��Ƕ�
        float turretRoll = tank.turret.eulerAngles.y;
        float angle = turretRoll - GetTurretTarget().y;
        if (angle < 0) angle += 360;
        //30�����ڷ����ڵ�
        if (angle < 30 || angle > 330)
            return true;
        else
            return false;
    }

    //·��
    private Path path = new Path();

    //��ʼ��·��
    void InitWaypoint()
    {
        GameObject obj = GameObject.Find("WaypointContainer");
        if (obj && obj.transform.GetChild(0) != null) 
        {
            Vector3 targetPos = obj.transform.GetChild(0).position;
            path.InitByNavMeshPath(transform.position, targetPos);
        }
            //path.InitByObj(obj);
    }

    private void Start()
    {
        InitWaypoint();
    }

    private void OnDrawGizmos()
    {
        path.DrawWayPoints();
    }

    //��ȡת���
    public float GetSteering() 
    {
        if (tank == null)
            return 0;

        Vector3 itp = transform.InverseTransformPoint(path.wayPoint);
        if (itp.x > path.deviation / 5)
            return tank.maxSteeringAngle;
        else if (itp.x < -path.deviation / 5)
            return -tank.maxSteeringAngle;
        else
            return 0;
    }

    //��ȡ����
    public float GetMotor() 
    {
        if (tank == null)
            return 0;

        Vector3 itp = transform.InverseTransformPoint(path.wayPoint);
        float x = itp.x;
        float z = itp.z;
        float r = 6;

        if (z < 0 && Mathf.Abs(x) < -z && Mathf.Abs(x) < r)
            return -tank.maxMotorTorque;
        else
            return tank.maxMotorTorque;
    }

    //��ȡɲ��
    public float GetBrakeTorque() 
    {
        if (path.isFinish)
            return tank.maxMotorTorque;
        else
            return 0;
    }
}
