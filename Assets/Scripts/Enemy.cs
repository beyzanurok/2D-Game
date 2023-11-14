using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int pointValue;
    private Rigidbody2D body;

    [SerializeField] //değişkenleri tanımlarız
    private float speed;
    private Vector2 movementDirection;

    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //movementDirection *= -1f; //düşmanın hareket ettiği yönün tersine çevirir
        Move(movementDirection); //fonksiyon ile birlikte hareket eder
    }

    //hıza dayalı düşman hareketi
    public void Move(Vector2 direction)
    {
        movementDirection = direction;
        body.velocity = new Vector2(movementDirection.x * speed, body.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) //etikete bakar (layer)
        {
            movementDirection *= -1f;
        }
    }
}
