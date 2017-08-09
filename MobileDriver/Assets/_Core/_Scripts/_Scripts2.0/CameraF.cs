using UnityEngine;
using System.Collections;

public class CameraF : MonoBehaviour {

    // TODO jutro ustawickamere do rozdielczości przetestować na iphonach skalowanie najlepiej 2 urządzenia
	public Transform target;
	public float lerpSpeed = 2f;
    float v = 0;
    Vector3 velocity = Vector3.zero;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
       Vector3 targetVector= new Vector3(Mathf.Lerp(transform.position.x, target.position.x,  lerpSpeed ), target.position.y, target.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetVector, ref velocity, lerpSpeed);
	
	}
}
