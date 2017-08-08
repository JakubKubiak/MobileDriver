using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSpinner : MonoBehaviour
{
    public float r = 0.5f;
    public float angularSpeed;
    DriverController driver;

    void Start()
    {
        driver = FindObjectOfType<DriverController>();
        MeshRenderer rend = GetComponent<MeshRenderer>();
        r = rend.bounds.extents.x;
    }

    void Update()
    {
        angularSpeed = driver.p_speed * 60 / r;
        transform.Rotate(new Vector3(angularSpeed * Time.deltaTime, 0, 0));
    }
}
