using UnityEngine;
using System.Collections;

public class CameraResolution : MonoBehaviour
{
    public bool maintainWidth = true;
    [Range(-1, 1)]
    public int adaptPosition;
    float defaultWidth;
    float defaultHeight;
    public Transform target;
    public float lerpSpeed = 2f;
    float v = 0;
    Vector3 velocity = Vector3.zero;
    public Camera camera;

    Vector3 CameraPos;



    // Use this for initialization
    void Start()
    {

        CameraPos = Camera.main.transform.position;

        defaultHeight = Camera.main.orthographicSize;
        defaultWidth = Camera.main.orthographicSize * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {

        if (maintainWidth)
        {

            Camera.main.orthographicSize = defaultWidth / Camera.main.aspect;


            //CameraPos.y was added in case camera in case camera's y is not in 0
            Vector3 targetVector = Camera.main.transform.position = new Vector3(CameraPos.x, CameraPos.y + adaptPosition * (defaultHeight - Camera.main.orthographicSize), CameraPos.z);
            Vector3 fallowVector = Camera.main.transform.position = new Vector3(Mathf.Lerp(transform.position.x, target.position.x, lerpSpeed), target.position.y, target.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetVector, ref velocity, lerpSpeed);
        }
        else
        {
            //CameraPos.x was added in case camera in case camera's x is not in 0
            Vector3 targetVector = Camera.main.transform.position = new Vector3(CameraPos.x + adaptPosition * (defaultWidth - Camera.main.orthographicSize * Camera.main.aspect), CameraPos.y, CameraPos.z);
            Vector3 fallowVector = Camera.main.transform.position = new Vector3(Mathf.Lerp(transform.position.x, target.position.x, lerpSpeed), target.position.y, target.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetVector, ref velocity, lerpSpeed);
        }

        /// do zabawy
    }
}