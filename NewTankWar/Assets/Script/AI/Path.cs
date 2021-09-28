using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Path
{
    //所有路点
    public Vector3[] wayPoints;
    //当前路点索引
    public int index = -1;
    //当前的路点
    public Vector3 wayPoint;
    //是否循环
    bool isLoop = false;
    //到达误差
    public float deviation = 5;
    //是否完成
    public bool isFinish = false;

    //是否达到目的地
    public bool IsReach(Transform trans) 
    {
        Vector3 pos = trans.position;
        float distance = Vector3.Distance(wayPoint, pos);
        return distance < deviation;
    }

    //下一个路点
    public void NextWaypoint() 
    {
        if (index < 0)
            return;
        if (index < wayPoints.Length - 1)
        {
            index++;
        }
        else
        {
            if (isLoop)
                index = 0;
            else
                isFinish = true;
        }
        wayPoint = wayPoints[index];
    }

    //根据场景标识物生成路点
    public void InitByObj(GameObject obj, bool isLoop = false) 
    {
        int length = obj.transform.childCount;
        //没有子物体
        if (length == 0) 
        {
            wayPoints = null;
            index = -1;
            Debug.LogWarning("Path.InitByObjLength == 0");
            return;
        }
        //遍历子物体生成路点
        wayPoints = new Vector3[length];
        for (int i=0; i<length; i++) 
        {
            Transform trans = obj.transform.GetChild(i);
            wayPoints[i] = trans.position;
        }
        //设置一些参数
        index = 0;
        wayPoint = wayPoints[index];
        this.isLoop = isLoop;
        isFinish = false;
    }

    //根据导航图初始化路径
    public void InitByNavMeshPath(Vector3 po3, Vector3 targetPos) 
    {
        //重置
        wayPoints = null;
        index = -1;
        //计算路径
        NavMeshPath navPath = new NavMeshPath();
        bool hasFoundPath = NavMesh.CalculatePath(po3, targetPos, NavMesh.AllAreas, navPath);
        if (!hasFoundPath)
            return;
        //使用路径
        int length = navPath.corners.Length;
        wayPoints = new Vector3[length];
        for (int i = 0; i < length; i++)
            wayPoints[i] = navPath.corners[i];

        index = 0;
        wayPoint = wayPoints[index];
        isFinish = false;
    }
    //调试路径
    public void DrawWayPoints() 
    {
        if (wayPoints == null)
            return;
        int length = wayPoints.Length;
        for (int i = 0; i < length; i++) 
        {
            if (i == index)
                Gizmos.DrawSphere(wayPoints[i], 1);
            else
                Gizmos.DrawCube(wayPoints[i], Vector3.one);
        }
    }
}
