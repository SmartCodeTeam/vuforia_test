using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetPosInCameraFlame : MonoBehaviour {

	Camera camera;

	// Use this for initialization
	void Start () {
		camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetPos = DataManager.Instance.hoge;
		Vector3 viewPos = camera.WorldToViewportPoint(targetPos);
		DataManager.Instance.hogeInCamera = viewPos;
//		if (viewPos.x > 0.5F)
//			print("target is on the right side!");
//		else
//			print("target is on the left side!");

		foreach (KeyValuePair<string, Vector3> pair in DataManager.Instance.rawBlocks) {//http://qiita.com/kwst/items/2cfd01b7f28daf0f495e
//			Debug.Log (pair.Key + " : " + pair.Value);
			Vector3 fixedPos= camera.WorldToViewportPoint(pair.Value);
			DataManager.Instance.cameraFrameBlocks.Remove (pair.Key);
			DataManager.Instance.cameraFrameBlocks.Add (pair.Key,fixedPos);
		}
	}
}
