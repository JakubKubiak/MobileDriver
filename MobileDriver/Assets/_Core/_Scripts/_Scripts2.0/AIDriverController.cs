using UnityEngine;
using System.Collections;

public class AIDriverController : Driver
{
    public int maxStrips = 3;
    public int startStrip = 2;
    public float stripSize = 2f;
    public float stripChangeSpeed = 2f;
    public float maxSpeed = 1000f;
    public float accelerationSpeed = 10f;
    public float startSpeed = 10f;
    public float currentSpeed = 0f;
    //float xStripPosition;

    void Start()
    {
        currentSpeed =startSpeed;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }
    void Update()
    {

        currentSpeed = Mathf.Lerp(startSpeed, maxSpeed, accelerationSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + currentSpeed * Time.deltaTime);

    }
}

