using System.Collections;
using System.Collections.Generic;
using UnityEngine;

             //PoruszanieSięGracza
public class PlayerMovement : MonoBehaviour
{
            //Fizyczne Atrybuty Gracza
    [Header("Psyhical Attributes of the Player")]
    [SerializeField]
    float speed = 70.0f;//Szybkość
    [SerializeField]
    float jumpHeight = 7;//Wysokość Skoku
    [SerializeField]
    Rigidbody psyhicalProperties = null;//Fizyczne Właściwości
    IEnumerator time = null;
    bool activation = false;
    float stopwatch = 0f;

    void Start()
    {
        transform.position = new Vector3(0, 2, -499.64f);
    }

    
    void Update()
    {
        Solutions();
        MovingThePlayer();
        Stabilization();
    }

    void Solutions()
    {
        //transform.rotation = new Quaternion(0, 0, 0, 0);
        psyhicalProperties.velocity = new Vector3(0, psyhicalProperties.velocity.y, 0);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void MovingThePlayer()
    {
        if((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && transform.position.x > -3)
        {
            transform.position += Vector3.left * 3;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 25));
            activation = true;
            stopwatch = 0;
        }
        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && transform.position.x < 3)
        {
            transform.position += Vector3.right * 3;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -25));
            activation = true;
            stopwatch = 0;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            psyhicalProperties.AddForce(Vector3.up * jumpHeight);
        }
    }

    void Stabilization()
    {
        if(stopwatch >= 0.2f && activation == true)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            activation = false;
        }
        stopwatch += Time.deltaTime;
    }

    IEnumerator TimeOfReturn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
}
