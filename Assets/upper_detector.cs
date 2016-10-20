using UnityEngine;
using System.Collections;

public class upper_detector : MonoBehaviour {

	private Rigidbody rb;
	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody>();
		rb.useGravity = false;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
