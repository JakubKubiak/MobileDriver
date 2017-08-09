using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DriverCresh : MonoBehaviour
{
    public GameObject endText;

    private void OnCollisionEnter(Collision collision)
    {
        Driver driver = collision.collider.GetComponent<Driver>();
        if (driver != null && IsPlayer())
        {
            CrashCars(driver);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Driver driver = other.GetComponent<Driver>();

        if (driver != null && IsPlayer())
        {
            CrashCars(driver);
        }
    }
    private bool IsPlayer()
    {
        return gameObject.tag == "Player";
    }
    private void CrashCars(Driver driver)
    {
        driver.enabled = false;
        GetComponent<Driver>().enabled = false;
        if (endText != null)
            endText.SetActive(true);
    }
}
