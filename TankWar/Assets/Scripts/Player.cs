using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //属性值
    public float moveSpeed = 3;
    private Vector3 bullectAulerAngles;
    private float timeVal;
    private float defendTimeVal=3;
    private bool isDefended = true;
    
    //引用
    private SpriteRenderer sr;
    public Sprite[] tankSprite;
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public GameObject defendEffectPrefab;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update 
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        //是否处于无敌状态
        if (isDefended) 
        {
            defendEffectPrefab.SetActive(true);
            defendTimeVal -= Time.deltaTime;
            if (defendTimeVal<=0) 
            {
                isDefended = false;
                defendEffectPrefab.SetActive(false);
            }
        }

        //判定攻击CD
        if (timeVal >= 0.4f)
        {
            Attack();
        }
        else 
        {
            timeVal += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        Move();
        Attack();
    }

    private void Attack() 
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            //子弹产生的角度：当前坦克的角度+子弹应该旋转的角度
            Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.eulerAngles + bullectAulerAngles));
            timeVal = 0;
        }
    }

    private void Move()
    {
        float v = Input.GetAxisRaw("Vertical");
        transform.Translate(Vector3.up * v * moveSpeed * Time.fixedDeltaTime, Space.World);

        if (v < 0)
        {
            sr.sprite = tankSprite[2];
            bullectAulerAngles = new Vector3(0,0,-180);
        }
        else if (v > 0)
        {
            sr.sprite = tankSprite[0];
            bullectAulerAngles = new Vector3(0, 0, 180);
        }

        if (v != 0)
        {
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector3.right * h * moveSpeed * Time.fixedDeltaTime, Space.World);
        if (h < 0)
        {
            sr.sprite = tankSprite[3];
            bullectAulerAngles = new Vector3(0, 0, 90);
        }
        else if (h > 0)
        {
            sr.sprite = tankSprite[1];
            bullectAulerAngles = new Vector3(0, 0, -90);
        }
    }

    private void Die() 
    {
        if (isDefended) 
        {
            return;
        }
        //产生爆炸特效
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        //死亡
        Destroy(gameObject);
    }
}
