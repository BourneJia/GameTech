using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100f;
    public GameObject explode;
    public float maxLiftTime = 2f;
    public float instantiaeTime = 0f;
    //攻击方
    public GameObject attackTank;
    //爆炸音效
    public AudioClip explodeClip;

    // Start is called before the first frame update
    void Start()
    {
        instantiaeTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //前进
        transform.position += transform.forward * speed * Time.deltaTime;
        //摧毁
        if (Time.time - instantiaeTime > maxLiftTime)
            Destroy(gameObject);
    }

    //碰撞
    private void OnCollisionEnter(Collision collision)
    {
        //爆炸效果
        //Instantiate(explode, transform.position, transform.rotation);
        GameObject explodeObj = (GameObject)Instantiate(explode, transform.position, transform.rotation);
        //爆炸音效
        AudioSource audioSource = explodeObj.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1;
        audioSource.PlayOneShot(explodeClip);
        //摧毁自身
        Destroy(gameObject);
        //击中坦克
        TankChapter2 tankChapter2 = collision.gameObject.GetComponent<TankChapter2>();
        if (tankChapter2 != null) 
        {
            float att = GetAtt();
            tankChapter2.BeAttacked(att, attackTank);
        }
    }

    //计算攻击力
    private float GetAtt()
    {
        float att = 1000 - (Time.time - instantiaeTime)*40;
        if (att < 1)
            att = 1;
        return att;
    }
}
