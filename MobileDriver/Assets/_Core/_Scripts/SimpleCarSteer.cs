using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SimpleCarSteer : Car
{
    public int maxLanes = 3;
    public int startLane = 2;
    public float lineSize = 2f;
    public float lineChangeSpeed = 2f;
    public Text speedText;
    [SerializeField]
    public float m_speed = 10.0f;
    [SerializeField]
    private float m_speedIncreaseBy = 0.5f;
    [SerializeField]
    private float m_speedIncreaseEach = 2.0f;
    [SerializeField]
    private float m_maxSpeed = 100.0f;
    public float upSpeed = 1.01f;
    public float downSpeed = 0.9f;
    public float karaHamowania;
    int currentLane;
    Score score;
    float xPosition;
    float speedValue;
    // Use this for initialization

    ParticleSystem[] systems;
    float[] initLifetimes;

    void Start()
    {
        score = GetComponent<Score>();
        currentLane = startLane;
        xPosition = (float)startLane * lineSize;
        StartCoroutine(Co_IncreaseSpeed());
        transform.position = new Vector3(xPosition, transform.position.y, 0f);

        systems = GetComponentsInChildren<ParticleSystem>();
        initLifetimes = new float[systems.Length];

        for(int i = 0; i < systems.Length; i++)
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
        if (currentLane > maxLanes)
        {

            currentLane = maxLanes;
        }

    }
    void SpeedUp()
    {
        m_speed += upSpeed*Time.deltaTime;
    }
    void SpeedDown()
    {
        m_speed -= downSpeed*Time.deltaTime;
        if(m_speed<karaHamowania)
        {
            m_speed = karaHamowania;
        }
        else
        score.score -= karaHamowania;

        if (score.score < 0)
            score.score = 0;
    }
    // Update is called once per frame
    void Update()
    {
       speedValue=   Mathf.Floor(m_speed*10)/10;
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

        xPosition = Mathf.Lerp(xPosition, (float)currentLane * lineSize, Time.deltaTime * lineChangeSpeed /*m_speed*/);
        float zChange = m_speed * Time.deltaTime;

        transform.position = new Vector3(xPosition, transform.position.y, transform.position.z);
        transform.Translate(Vector3.forward * zChange);

        for (int i = 0; i < systems.Length; i++)
        {
            systems[i].startSize = speedValue / m_maxSpeed;
            systems[i].startLifetime = initLifetimes[i] + speedValue / m_maxSpeed *0.3f;
        }

        score.IncreaseScore(m_speed * Time.deltaTime);
    }


    IEnumerator Co_IncreaseSpeed()
    {
        WaitForSeconds yield = new WaitForSeconds(m_speedIncreaseEach);
        while (true)
        {
            yield return yield;

            if (m_speed < m_maxSpeed)
            {
                m_speed += m_speedIncreaseBy;
            }

        }

    }
}

