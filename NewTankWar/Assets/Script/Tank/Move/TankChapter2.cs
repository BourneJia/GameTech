using UnityEngine;
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

    //炮弹预设
    public GameObject bullet;
    //上一次开炮的时间
    public float lastShootTime = 0;
    //开炮的时间间隔
    private float shootInterval = 0.5f;

    //操控类型
    public enum CtrlType
    { 
        none,
        player,
        computer
    }
    public CtrlType ctrlType = CtrlType.player;

    //最大生命值
    private float maxHp = 100;
    //当前生命值
    public float hp = 100;

    //焚烧特效
    public GameObject destoryEffect;

    //中心准心
    public Texture2D centerSight;
    //坦克准心
    public Texture2D tankSight;

    //生命指示条素材
    public Texture2D hpBarBg;
    public Texture2D hpBar;

    //击杀提示图标
    public Texture2D killUI;
    //击杀图标开始显示时间
    private float killUIStartTime = float.MinValue;

    //发射炮弹音源
    public AudioSource shootAudioSource;
    //发射音效
    public AudioClip shootClip;

    //显示击杀图标
    public void StartDrawKill() 
    {
        killUIStartTime = Time.time;
    }

    public void Shoot() 
    {
        //发射间隔
        if (Time.time - lastShootTime < shootInterval)
            return;
        //子弹
        if (bullet == null)
        {
            Debug.Log("bullet == null");
            return;
        }
        //发射
        Vector3 pos = gun.position + gun.forward * 5;
        GameObject bulletObj = (GameObject)Instantiate(bullet, pos, gun.rotation);
        Bullet bulletCmp = bulletObj.GetComponent<Bullet>();
        if (bulletCmp != null) 
            bulletCmp.attackTank = this.gameObject;
        
        lastShootTime = Time.time;

        shootAudioSource.PlayOneShot(shootClip);
    }

    private void Start()
    {
        //马达音源
        motorAudioSource = gameObject.AddComponent<AudioSource>();
        Debug.Log(motorAudioSource);
        motorAudioSource.spatialBlend = 1;
        Debug.Log(motorAudioSource);

        //发射音源
        shootAudioSource = gameObject.AddComponent<AudioSource>();
        Debug.Log(shootAudioSource);
        shootAudioSource.spatialBlend = 1;
        Debug.Log(shootAudioSource);
        //获取炮塔
        turret = transform.Find("turret");
        //获取炮管
        gun = turret.Find("gun");
        //获取轮子
        wheels = transform.Find("wheels");
        //获取履带
        tracks = transform.Find("tracks");
        ////马达音源
        //motorAudioSource = gameObject.AddComponent<AudioSource>();
        //Debug.Log(motorAudioSource);
        //motorAudioSource.spatialBlend = 1;
        //Debug.Log(motorAudioSource);

        ////发射音源
        //shootAudioSource = gameObject.AddComponent<AudioSource>();
        //Debug.Log(shootAudioSource);
        //shootAudioSource.spatialBlend = 1;
        //Debug.Log(shootAudioSource);
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

    //绘图
    private void OnGUI()
    {
        if (ctrlType != CtrlType.player)
            return;
        DrawSight();
        DrawHp();
        DrawKillUI();
    }

    //玩家控制
    public void PlayerCtrl() 
    {
        if (ctrlType != CtrlType.player)
            return;

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
        //turretRotTarget = Camera.main.transform.eulerAngles.y;
        //turretRollTarget = Camera.main.transform.eulerAngles.x;
        //TargetSignPos();

        //发射炮弹
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }

        //炮塔炮管角度
        TargetSignPos();
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

    public void BeAttacked(float att, GameObject attackTank) 
    {
        //坦克已经被摧毁
        if (hp < 0)
            return;
        //击中处理
        if (hp > 0)
        {
            hp -= att;
        }
        //被摧毁
        if (hp <= 0) 
        {
            GameObject destoryObj = (GameObject)Instantiate(destoryEffect);
            destoryObj.transform.SetParent(transform, false);
            destoryObj.transform.localPosition = Vector3.zero;
            ctrlType = CtrlType.none;
            if (attackTank != null)
            {
                TankChapter2 tankCmp = attackTank.GetComponent<TankChapter2>();
                if (tankCmp != null && tankCmp.ctrlType == CtrlType.player)
                {
                    tankCmp.StartDrawKill();
                }
            }
        }
    }

    //计算目标角度
    public void TargetSignPos() 
    {
        //碰撞信息和碰撞点
        Vector3 hitPoint = Vector3.zero;
        RaycastHit raycastHit;
        //屏幕中心位置
        Vector3 centerVec = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(centerVec);
        //射线检测，获取hitPoint
        if (Physics.Raycast(ray, out raycastHit, 400.0f))
        {
            hitPoint = raycastHit.point;
        }
        else 
        {
            hitPoint = ray.GetPoint(400);
        }
        //计算目标角度
        Vector3 dir = hitPoint - turret.position;
        Quaternion angle = Quaternion.LookRotation(dir);
        turretRotTarget = angle.eulerAngles.y;
        turretRollTarget = angle.eulerAngles.x;
        //测试
        //Transform targetCube = GameObject.Find("Chi_Ha").transform;
        //targetCube.position = hitPoint;
    }

    //计算爆炸位置
    public Vector3 CalExplodePoint() 
    {
        //碰撞信息和碰撞点
        Vector3 hitPoint = Vector3.zero;
        RaycastHit hit;
        //沿着炮管方向的射线
        Vector3 pos = gun.position + gun.forward * 5;
        Ray ray = new Ray(pos, gun.forward);
        //射线检测
        if (Physics.Raycast(ray, out hit, 400.0f))
        {
            hitPoint = hit.point;
        }
        else 
        {
            hitPoint = ray.GetPoint(400);
        }

        return hitPoint;
    }

    //绘制准心
    public void DrawSight() 
    {
        //计算实际射击位置
        Vector3 explodePoint = CalExplodePoint();
        //获取“坦克准心”的屏幕坐标
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(explodePoint);
        //绘制坦克准心
        Rect tankRect = new Rect(screenPoint.x - tankSight.width / 2, Screen.height - screenPoint.y - tankSight.height / 2, tankSight.width, tankSight.height);
        GUI.DrawTexture(tankRect, tankSight);
        //绘制中心准心
        Rect centerRect = new Rect(Screen.width / 2 - centerSight.width / 2, Screen.height / 2 - centerSight.height / 2, centerSight.width, centerSight.height);
        GUI.DrawTexture(centerRect, centerSight);
    }

    //绘制生命条
    public void DrawHp() 
    {
        //底框
        Rect bgRect = new Rect(30, Screen.height - hpBarBg.height - 15, hpBarBg.width, hpBarBg.height);
        GUI.DrawTexture(bgRect, hpBarBg);
        //指示条
        float width = hp * 102 / maxHp;
        Rect hpRect = new Rect(bgRect.x + 29, bgRect.y + 9, width, hpBar.height);
        GUI.DrawTexture(hpRect, hpBar);
        //文字
        string text = Mathf.Ceil(hp).ToString() + "/" + Mathf.Ceil(maxHp).ToString();
        Rect textRect = new Rect(bgRect.x + 80, bgRect.y - 10, 50, 50);
        GUI.Label(textRect, text);
    }

    //绘制击杀图标
    private void DrawKillUI() 
    {
        if (Time.time - killUIStartTime < 1f) 
        {
            Rect rect = new Rect(Screen.width / 2 - killUI.width / 2, 30, killUI.width, killUI.height);
            GUI.DrawTexture(rect, killUI);
        }
    }
}