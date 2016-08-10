using UnityEngine;
using System.Collections;

public class hurdle_set : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector3 hurdlePos = new Vector3 (-3.0f, 1.0f, -7.5f);//これをあらかじめ定められたところからランダムに選択できるといいね。
		transform.position = hurdlePos;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
