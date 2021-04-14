using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 10;

    public bool isPlayerBullect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.up * moveSpeed * Time.deltaTime, Space.World);   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag) 
        {
            case "Tank":
                if (!isPlayerBullect) 
                {
                    collision.SendMessage("Die");
                }
                break;
            case "Heart":
                collision.SendMessage("Die");
                break;
            case "Enemy":
                break;
            case "Wall":
                Destroy(collision.gameObject);
                Destroy(gameObject);
                break;
            case "Barriar":
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }
}
