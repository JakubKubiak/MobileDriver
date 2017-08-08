using UnityEngine;
using System.Collections;

public class T_DriverController : Driver
{

    public int maxStrips = 3;
    public int startStrips = 2;
    public float stripsSize = 2f;
    public float stripChangeSpeed = 2f;

    [SerializeField] private float p_speed = 4.0f;
    [SerializeField] private float p_speedIncreaseBy = 0.2f;
    [SerializeField] private float p_speedIncreaseEach = 3.0f;
    [SerializeField] private float p_maxSpeed = 8.0f;

    int currentStrip;
    float xStripPosition;

    void Start()
    {
        currentStrip = startStrips;
        xStripPosition = (float)startStrips * stripsSize;
        StartCoroutine(Co_IncreaseSpeed());
        transform.position = new Vector3(xStripPosition, transform.position.y, 0f);
    }

    void ChangeLane(int change)
    {
        currentStrip -= change;
        if (currentStrip < 1)
        {
            currentStrip = 1;
        }

        if (currentStrip > maxStrips)
        {
            currentStrip = maxStrips;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeLane(1);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeLane(-1);
        }

        xStripPosition = Mathf.Lerp(xStripPosition, (float)currentStrip * stripsSize, Time.deltaTime * stripChangeSpeed);
        float zChange = p_speed * Time.deltaTime;
        transform.Translate(xStripPosition, 0, zChange);
    }
    IEnumerator Co_IncreaseSpeed()
    {
        WaitForSeconds yield = new WaitForSeconds(p_speedIncreaseEach);
        while (true)
        {
            yield return yield;

            if (p_speed < p_maxSpeed)
            {
                p_speed += p_speedIncreaseBy;
            }
        }
    }
}
