using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripNumberChange : MonoBehaviour {
    public T_DriverController driver;
    public int newStripsNumber = 10;

    void OnTriggerEnter(Collider other)
    {
        driver.maxStrips = newStripsNumber;
    }

}