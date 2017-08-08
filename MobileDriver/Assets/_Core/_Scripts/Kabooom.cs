using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kabooom : MonoBehaviour {
    public GameObject endText;

   
    private void OnCollisionEnter(Collision collision)
    {
       
        Car car = collision.collider.GetComponent<Car>();
        if (car != null && IsPlayer())
        {
            CrashCars( car );
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Car car = other.GetComponent<Car>();


        if ( car != null && IsPlayer() )
        {
            CrashCars( car );
        }

    }

    private bool IsPlayer()
    {
        return gameObject.tag == "Player";
    }

    private void CrashCars( Car car )
    {
        car.enabled = false;
        GetComponent<Car>().enabled = false;
        if ( endText != null )
            endText.SetActive( true );
    }

}
