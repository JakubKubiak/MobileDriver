using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class Score : MonoBehaviour {
    public Text scoreText;
    public   float score;

 //   SimpleCarSteer carSteer;
	// Use this for initialization
	void Start () {
	//	carSteer=GetComponent< SimpleCarSteer > ();
	}
	
	// Update is called once per frame
	void Update () {
        
       // score = Mathf.FloorToInt(transform.position.z)*growRate;
       // scoreText.text =score.ToString();
	}

    public void IncreaseScore(float value)
    {
        score += value;
       
       scoreText.text = "Score " +  score.ToString();
    }
}
