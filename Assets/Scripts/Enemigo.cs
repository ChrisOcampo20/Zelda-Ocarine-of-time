using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public float runningSpeed = 1.5f;

    public int enemyDamage = 10;

    Rigidbody2D rigidBody;

    public bool facingRight = false;


    //private Vector3 startPosition;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        //startPosition = this.transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        //this.transform.position = startPosition;
    }

    private void FixedUpdate()
    {
        float currentRunningSpeed = runningSpeed;

        if (facingRight)
        {
            //Mirando hacia la derecha
            currentRunningSpeed = runningSpeed;
            this.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            //Mirando hacia la izquierda
            currentRunningSpeed = -runningSpeed;
            this.transform.eulerAngles = Vector3.zero;

        }

        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            rigidBody.velocity = new Vector2(currentRunningSpeed, rigidBody.velocity.y);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "RupiaGreen" || collision.collider.tag == "RupiaBlue" || collision.collider.tag == "RupiaRed")
        {
            return;
        }
        //Si llegamos aquí, no hemos chocado ni con monedas, ni con players
        //Lo más normal es que aquí haya otro enemigo o bien escenario
        //Vamos a hacer que el enemigo rote
        if (facingRight == false)
        {
            facingRight = true;
        }
        else
        {
            facingRight = false;
        }

        if (collision.collider.name == "Link")
        {
            collision.gameObject.GetComponent<Link>().CollectHealth(-enemyDamage);

        }

    }

   



}



