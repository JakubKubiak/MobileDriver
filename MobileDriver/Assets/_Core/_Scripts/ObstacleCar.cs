using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCar : Car {
    public float maxSpeed= 30;
    public float minSpeed = 5;
    float speed = 0;
    float breakMod = 0.8f;
	// Use this for initialization
	void Start () {
        SetSpeed();

    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if ( !IsCar( collision ) )
        {
            return;
        }
        if ( collision.collider.gameObject.transform.position.z > gameObject.transform.position.z )
        {
            speed *= breakMod;
            StartCoroutine( "ResetSpeed" );
        }            
    }

    private void SetSpeed()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        StopCoroutine( "ResetSpeed" );
    }

    private bool IsCar( Collision collision )
    {
        Car car = collision.collider.GetComponent<Car>();
        return car != null;
    }

    IEnumerator ResetSpeed()
    {
       float waitTime  = Random.Range( 1, 2 );
        yield return new WaitForSeconds( waitTime );
        SetSpeed();
        yield return null;
    }


}
