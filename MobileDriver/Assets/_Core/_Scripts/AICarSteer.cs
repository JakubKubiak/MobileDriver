using UnityEngine;
using System.Collections;

public class AICarSteer : Car {
	public int maxLanes = 3;
	public int startLane = 2;
	public float lineSize = 2f;
	public float lineChangeSpeed = 2f;
	public float maxSpeed = 1000f;
	public float accelerationSpeed = 10f;
	public float startSpeed = 10f;

	public float currentSpeed = 0f;

	float xPosition;

	// Use this for initialization
	void Start () 
	{

		currentSpeed = startSpeed;
		transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

	}
		

	// Update is called once per frame
	void Update () 
	{

		currentSpeed = Mathf.Lerp(startSpeed, maxSpeed, accelerationSpeed*Time.deltaTime);
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z+currentSpeed*Time.deltaTime);

	}
}

