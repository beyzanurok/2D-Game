using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Game game;
    private void Awake()
    {
        game = FindObjectOfType<Game>();
    }

    private void OnCollisionEnter2D(Collision2D collision)//düşman nesnesine çarpma durumları
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Hurt();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy": //düşman
                StartCoroutine(HurtEnemy(collision.gameObject.GetComponent<Enemy>()));
                break;
            case "Gem":
                game.AddLife();
                Destroy(collision.gameObject);
                break;
            case "Coin": //altın alırsa puan ekle
                game.AddPoints(100);
                Destroy(collision.gameObject);
                break;
            default:
                break;
        }
    }

    IEnumerator HurtEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
        yield return new WaitForEndOfFrame();
        game.AddPoints(enemy.pointValue);
    }

    void Hurt() //oyuncu
    {
        game.LoseLife();
        Destroy(this.gameObject);
    }
}
