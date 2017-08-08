using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSpin : MonoBehaviour {
    public float r = 0.5f;
  public  float angularSpeed;
    SimpleCarSteer car;
	// Use this for initialization
	void Start () {
        car = FindObjectOfType<SimpleCarSteer>();
        MeshRenderer rend = GetComponent<MeshRenderer>();
        r = rend.bounds.extents.x;
	}
	
	// Update is called once per frame
	void Update () {
        angularSpeed = car.m_speed*60  / r;
        transform.Rotate(new Vector3(angularSpeed*Time.deltaTime, 0, 0));
	}
}
