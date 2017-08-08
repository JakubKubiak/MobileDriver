using UnityEngine;
using System.Collections;

public class DriverCamera : MonoBehaviour
{

    public Transform target;
    public float l_Speed = 2f;
    float v = 0;
    Vector3 velocity = Vector3.zero;
    void Start()
    {

    }

    void Update()
    {
        Vector3 targetVector = new Vector3(Mathf.Lerp(transform.position.x, target.position.x, l_Speed), target.position.y, target.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetVector, ref velocity, l_Speed);

    }
}
