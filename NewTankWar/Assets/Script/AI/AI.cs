using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    //所控制的坦克
    public TankChapter2 tank;
    //锁定的坦克
    private GameObject target;
    //视野范围
    private float sightDistance = 30;
    //上一次搜寻时间
    private float lastSearchTargetTime = 0;
    //搜寻间隔
    private float searchTargetInterval = 3;

    //上次更新路径时间
    private float lastUpdateWaypointTime = float.MinValue;

    //更新路径CD
    private float updateWaypointInterval = 10;

    //状态枚举
    public enum Status
    { 
        Patrol,
        Attack,
    }
    private Status status = Status.Patrol;
    //更改状态
    public void ChangeStatus(Status status) 
    {
        if (status == Status.Patrol)
            return;//PatrolStart();
        else if (status == Status.Attack)
            return;// AttackStart();
    }

    //状态处理
    private void FixedUpdate()
    {
        if (tank.ctrlType != TankChapter2.CtrlType.computer)
            return;
        if (status == Status.Patrol)
            return;// PatrolUpdate();
        else if (status == Status.Attack)
            return;// AttackUpdate();
    }

    //巡逻开始
    void PatrolStart() 
    {
    
    }

    //攻击开始
    void AttackStart() 
    {
        Vector3 targetPos = target.transform.position;
        path.InitByNavMeshPath(transform.position, targetPos);
    }

    //巡逻中
    void PatrolUpdate() 
    {
        //发现敌人
        if (target != null)
            ChangeStatus(Status.Attack);
        //时间间隔
        float interval = Time.time - lastUpdateWaypointTime;
        if (interval < updateWaypointInterval)
            return;
        lastUpdateWaypointTime = Time.time;
        //处理巡逻点
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

    //攻击中
    void AttackUpdate() 
    {
        //目标丢失
        if (target == null)
            ChangeStatus(Status.Patrol);
        //时间间隔
        float interval = Time.time - lastUpdateWaypointTime;
        if (interval < updateWaypointInterval)
            return;
        lastUpdateWaypointTime = Time.time;
        //更新路径
        Vector3 targetPos = target.transform.position;
        path.InitByNavMeshPath(transform.position, targetPos);
    }

    //搜寻目标
    void TargetUpdate() 
    {
        //cd时间
        float interval = Time.time - lastSearchTargetTime;
        if (interval < searchTargetInterval)
            return;
        lastSearchTargetTime = Time.time;

        //已有目标的情况，判断是否丢失目标
        if (target != null)
            HasTarget();
        else
            NoTarget();
    }

    private void Update()
    {
        TargetUpdate();
        //行走
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
            Debug.Log("目标死亡，丢失坐标");
            target = null;
        }
        else if (Vector3.Distance(pos, targetPos) > sightDistance) 
        {
            Debug.Log("距离过远，丢失目标");
            target = null;
        }
    }

    void NoTarget() 
    {
        //最小生命值
        float minHp = float.MaxValue;
        //遍历所有坦克
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Tank");
        for (int i = 0; i < targets.Length; i++)
        {
            //Tank组件
            TankChapter2 tank = targets[i].GetComponent<TankChapter2>();
            if (tank == null)
                continue;
            //自己
            if (targets[i] == gameObject)
                continue;
            //死亡
            if (tank.ctrlType == TankChapter2.CtrlType.none)
                continue;
            //判断距离
            Vector3 pos = transform.position;
            Vector3 targetPos = targets[i].transform.position;
            if (Vector3.Distance(pos, targetPos) > sightDistance)
                continue;
            //判断生命值
            if (minHp > tank.hp)
                target = tank.gameObject;
        }
        //调试
        if (target != null)
            Debug.Log("获取目标 " + target.name);
    }
    //被攻击
    public void OnAttecked(GameObject attackTank) 
    {
        target = attackTank;
    }

    //获取炮管和炮塔的目标角度
    public Vector3 GetTurretTarget() 
    {
        //没有目标，朝着则炮塔坦克前方
        if (target == null)
        {
            float y = transform.eulerAngles.y;
            Vector3 rot = new Vector3(0, y, 0);
            return rot;
        }
        //有目标，则对准目标
        else 
        {
            Vector3 pos = transform.position;
            Vector3 targetPos = target.transform.position;
            Vector3 vec = targetPos - pos;
            return Quaternion.LookRotation(vec).eulerAngles;
        }
    }

    //是否发射炮弹
    public bool IsShoot() 
    {
        if (target == null)
            return false;

        //目标角度
        float turretRoll = tank.turret.eulerAngles.y;
        float angle = turretRoll - GetTurretTarget().y;
        if (angle < 0) angle += 360;
        //30度以内发射炮弹
        if (angle < 30 || angle > 330)
            return true;
        else
            return false;
    }

    //路径
    private Path path = new Path();

    //初始化路点
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

    //获取转向角
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

    //获取马力
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

    //获取刹车
    public float GetBrakeTorque() 
    {
        if (path.isFinish)
            return tank.maxMotorTorque;
        else
            return 0;
    }
}
