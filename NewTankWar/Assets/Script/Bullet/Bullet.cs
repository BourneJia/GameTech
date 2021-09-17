using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100f;
    public GameObject explode;
    public float maxLiftTime = 2f;
    public float instantiaeTime = 0f;
    //������
    public GameObject attackTank;
    //��ը��Ч
    public AudioClip explodeClip;

    // Start is called before the first frame update
    void Start()
    {
        instantiaeTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //ǰ��
        transform.position += transform.forward * speed * Time.deltaTime;
        //�ݻ�
        if (Time.time - instantiaeTime > maxLiftTime)
            Destroy(gameObject);
    }

    //��ײ
    private void OnCollisionEnter(Collision collision)
    {
        //��ըЧ��
        //Instantiate(explode, transform.position, transform.rotation);
        GameObject explodeObj = (GameObject)Instantiate(explode, transform.position, transform.rotation);
        //��ը��Ч
        AudioSource audioSource = explodeObj.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1;
        audioSource.PlayOneShot(explodeClip);
        //�ݻ�����
        Destroy(gameObject);
        //����̹��
        TankChapter2 tankChapter2 = collision.gameObject.GetComponent<TankChapter2>();
        if (tankChapter2 != null) 
        {
            float att = GetAtt();
            tankChapter2.BeAttacked(att, attackTank);
        }
    }

    //���㹥����
    private float GetAtt()
    {
        float att = 1000 - (Time.time - instantiaeTime)*40;
        if (att < 1)
            att = 1;
        return att;
    }
}
