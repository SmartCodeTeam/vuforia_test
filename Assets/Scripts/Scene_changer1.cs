using UnityEngine;
using System.Collections;

public class Scene_changer1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate () {
		if (Input.GetButton("Jump")) {
			Application.LoadLevel("Show_Commands");
		}
	}

	void OnGUI () { 
		GUI.Label(new Rect(20, 40, 80, 20), "miniGame");
	}
}
