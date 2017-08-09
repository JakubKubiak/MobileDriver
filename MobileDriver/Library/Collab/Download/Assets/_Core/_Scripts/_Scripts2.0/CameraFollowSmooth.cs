using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollowSmooth : MonoBehaviour {

	public Transform target;
    public Camera camera; 
	public float lerpSpeed = 2f;
    float v = 0;
    Vector3 velocity = Vector3.zero;
    Vector3 CameraPos;
    float targetaspect = 4.0f / 3.0f;
    // Use this for initialization
    void Start () {
    

        float targetaspect = 4.0f / 3.0f;

            // determine the game window's current aspect ratio
            float windowaspect = (float)Screen.width / (float)Screen.height;

            // current viewport height should be scaled by this amount
            float scaleheight = windowaspect / targetaspect;

            // obtain camera component so we can modify its viewport
           

            // if scaled height is less than current height, add letterbox
            if (scaleheight < 1.0f)
            {
                Rect rect = camera.rect;

                rect.width = 1.0f;
                rect.height = scaleheight;
                rect.x = 0;
                rect.y = (1.0f - scaleheight) / 2.0f;

                camera.rect = rect;
            }
            else // add pillarbox
            {
                float scalewidth = 1.0f / scaleheight;

                Rect rect = camera.rect;

                rect.width = scalewidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scalewidth) / 2.0f;
                rect.y = 0;

                camera.rect = rect;
            }
        }
	
	// Update is called once per frame
	void Update () {
       Vector3 targetVector= new Vector3(Mathf.Lerp(transform.position.x, target.position.x,  lerpSpeed ), target.position.y, target.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetVector, ref velocity, lerpSpeed);
	
	}
}
