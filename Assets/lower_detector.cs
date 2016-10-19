using UnityEngine;
using System.Collections;

public class lower_detector : MonoBehaviour {
	private Rigidbody rb;
	public float decisionTime=0.7f;
	private float timeElapsed;
	private string guess;
	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody>();
		rb.useGravity = false;
	}

	// Update is called once per frame
	void Update () {

	}

	//オブジェクトが触れている間
	void OnTriggerStay(Collider other) {
		if(other.gameObject.name!="upper"){//upper以外と重なってたら判定せず
			timeElapsed = 0.0f;
		}
		if (guess != other.gameObject.transform.parent.name) {
			timeElapsed = 0.0f;
			guess = other.gameObject.transform.parent.name;
		}
		timeElapsed += Time.deltaTime;
		if(timeElapsed >= decisionTime) {
			DataManager.Instance.Connections.Remove (gameObject.transform.parent.name);
			DataManager.Instance.Connections.Add (gameObject.transform.parent.name,other.gameObject.transform.parent.name);
			Debug.Log ("connected"+gameObject.transform.parent.name+"&"+other.gameObject.transform.parent.name);
			// Do anything
			timeElapsed = 0.0f;
		}
	}

	void OnTriggerExit(Collider other) {
		Debug.Log(other.gameObject.transform.parent.name);
		DataManager.Instance.Connections.Remove (gameObject.transform.parent.name);
		DataManager.Instance.Connections.Add (gameObject.transform.parent.name,"none");
		Debug.Log ("disconnected"+gameObject.transform.parent.name+"&"+other.gameObject.transform.parent.name);
	}

}
