using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DriverController : Driver
{
    public int maxStrips = 3;
    public int startStrips = 2;
    public float stripsSize = 2f;
    public float stripChangeSpeed = 2f;
    public Text speedText;
    [SerializeField] public float p_speed = 10.0f;
    [SerializeField] private float p_speedIncreaseBy = 0.5f;
    [SerializeField] private float p_speedIncreaseEach = 2.0f;
    [SerializeField] private float p_maxSpeed = 100.0f;
    public float upSpeed = 1.01f;
    public float downSpeed = 0.9f;
    public float breakpenalty;
    int currentLane;
    Score score;
    float xStripPosition;
    float speedValue;
    // Use this for initialization

    ParticleSystem[] systems;
    float[] initLifetimes;

    void Start()
    {
        score = GetComponent<Score>();
        currentLane = startStrips;
        xStripPosition = (float)startStrips * stripsSize;
        StartCoroutine(Co_IncreaseSpeed());
        transform.position = new Vector3(xStripPosition, transform.position.y, 0f);

        systems = GetComponentsInChildren<ParticleSystem>();
        initLifetimes = new float[systems.Length];

        for (int i = 0; i < systems.Length; i++)
        {
            initLifetimes[i] = systems[i].startLifetime;
        }
    }

    void ChangeLane(int change)
    {
        currentLane -= change;
        if (currentLane < 1)
        {

            currentLane = 1;
        }
        if (currentLane > maxStrips)
        {

            currentLane = maxStrips;
        }

    }
    void SpeedUp()
    {
        p_speed += upSpeed * Time.deltaTime;
    }
    void SpeedDown()
    {
        p_speed -= downSpeed * Time.deltaTime;
        if (p_speed < breakpenalty)
        {
            p_speed = breakpenalty;
        }
        else
            score.score -= breakpenalty;

        if (score.score < 0)
            score.score = 0;
    }
    // Update is called once per frame
    void Update()
    {
        speedValue = Mathf.Floor(p_speed * 10) / 10;
        speedText.text = "Speed " + speedValue.ToString();
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeLane(1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeLane(-1);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            SpeedUp();
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            SpeedDown();
        }

        xStripPosition = Mathf.Lerp(xStripPosition, (float)currentLane * stripsSize, Time.deltaTime * stripChangeSpeed /*m_speed*/);
        float zChange = p_speed * Time.deltaTime;

        transform.position = new Vector3(xStripPosition, transform.position.y, transform.position.z);
        transform.Translate(Vector3.forward * zChange);

        for (int i = 0; i < systems.Length; i++)
        {
            systems[i].startSize = speedValue / p_maxSpeed;
            systems[i].startLifetime = initLifetimes[i] + speedValue / p_maxSpeed * 0.3f;
        }

        score.IncreaseScore(p_speed * Time.deltaTime);
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

