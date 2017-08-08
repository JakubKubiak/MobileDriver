using UnityEngine;
using System.Collections;

public class SimpleCarSteer1 : Car
{

    public int maxLanes = 3;
	public int startLane = 2;
	public float lineSize = 2f;
	public float lineChangeSpeed = 2f;

    [SerializeField]
    private float m_speed = 4.0f;
    [SerializeField]
    private float m_speedIncreaseBy = 0.2f;
    [SerializeField]
    private float m_speedIncreaseEach = 3.0f;
    [SerializeField]
    private float m_maxSpeed = 8.0f;

	int currentLane;

	float xPosition;

	// Use this for initialization
	void Start () {
		currentLane = startLane;
		xPosition = (float)startLane*lineSize;
        StartCoroutine( Co_IncreaseSpeed() );
		transform.position = new Vector3(xPosition, transform.position.y, 0f);
	
	}

	void ChangeLane(int change)
	{
		currentLane -= change;
		if(currentLane < 1)
		{

			currentLane = 1;
		}
		if(currentLane > maxLanes)
		{

			currentLane = maxLanes;
		}

	}

	// Update is called once per frame
	void Update () {


		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
				ChangeLane(1);
		}
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
				ChangeLane(-1);
		}
        xPosition = Mathf.Lerp(xPosition, (float)currentLane * lineSize, Time.deltaTime * lineChangeSpeed);
        float zChange = m_speed * Time.deltaTime;

        transform.Translate( xPosition, 0, zChange );

    }

    IEnumerator Co_IncreaseSpeed()
    {
        WaitForSeconds yield = new WaitForSeconds( m_speedIncreaseEach );
        while ( true )
        {
            yield return yield;

            if ( m_speed < m_maxSpeed )
            {
                m_speed += m_speedIncreaseBy;
            }

       }

    }
}
