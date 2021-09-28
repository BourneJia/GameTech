using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Path
{
    //����·��
    public Vector3[] wayPoints;
    //��ǰ·������
    public int index = -1;
    //��ǰ��·��
    public Vector3 wayPoint;
    //�Ƿ�ѭ��
    bool isLoop = false;
    //�������
    public float deviation = 5;
    //�Ƿ����
    public bool isFinish = false;

    //�Ƿ�ﵽĿ�ĵ�
    public bool IsReach(Transform trans) 
    {
        Vector3 pos = trans.position;
        float distance = Vector3.Distance(wayPoint, pos);
        return distance < deviation;
    }

    //��һ��·��
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

    //���ݳ�����ʶ������·��
    public void InitByObj(GameObject obj, bool isLoop = false) 
    {
        int length = obj.transform.childCount;
        //û��������
        if (length == 0) 
        {
            wayPoints = null;
            index = -1;
            Debug.LogWarning("Path.InitByObjLength == 0");
            return;
        }
        //��������������·��
        wayPoints = new Vector3[length];
        for (int i=0; i<length; i++) 
        {
            Transform trans = obj.transform.GetChild(i);
            wayPoints[i] = trans.position;
        }
        //����һЩ����
        index = 0;
        wayPoint = wayPoints[index];
        this.isLoop = isLoop;
        isFinish = false;
    }

    //���ݵ���ͼ��ʼ��·��
    public void InitByNavMeshPath(Vector3 po3, Vector3 targetPos) 
    {
        //����
        wayPoints = null;
        index = -1;
        //����·��
        NavMeshPath navPath = new NavMeshPath();
        bool hasFoundPath = NavMesh.CalculatePath(po3, targetPos, NavMesh.AllAreas, navPath);
        if (!hasFoundPath)
            return;
        //ʹ��·��
        int length = navPath.corners.Length;
        wayPoints = new Vector3[length];
        for (int i = 0; i < length; i++)
            wayPoints[i] = navPath.corners[i];

        index = 0;
        wayPoint = wayPoints[index];
        isFinish = false;
    }
    //����·��
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
