using UnityEngine;
using System.Collections;

public class mm : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void LoadGame()
    {
        Application.LoadLevel(1);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
