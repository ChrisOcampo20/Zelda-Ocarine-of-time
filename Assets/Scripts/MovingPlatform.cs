using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed;         //Velocidad de la plataforma
    public int startingPoints;  // Iniciar (Posicion de la plataforma)
    public Transform[] points;  //  Puntos de transformacion (La paltaforma necesita una posicion para moverse)

    private int i;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[startingPoints].position; // Ajuste de la posicion de la plataforma
                                                              // la posicion de uno de los puntos usando el "startingPoint"
    }

    // Update is called once per frame
    void Update()
    {

        // Verificar la distancia de la plataforma y el points
        if (Vector2.Distance(transform.position, points[i].position) < 0.02f )
        {
            i++; //Aumentar el indice
                if(i == points.Length) // Verificar si la plataforma esta en el ultimo punto despues de aumentar el indice.
            {
                i = 0; // Resetear el indice
            }
        }

        // Movimiento de la plataforma a la posicion del punto cuando el indice es "i"
        transform.position = Vector2.LerpUnclamped(transform.position, points[i].position, speed * Time.deltaTime);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }




}
