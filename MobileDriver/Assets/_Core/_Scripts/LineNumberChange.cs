using UnityEngine;
using System.Collections;

public class LineNumberChange : MonoBehaviour {

	public SimpleCarSteer car;

	public int newLineNumber = 10;

	// Use this for initialization
	void OnTriggerEnter (Collider other) {

		car.maxLanes = newLineNumber;
	
	}

}
