using UnityEngine;
using System.Collections;
using UnityEngine.UI;//この宣言が必要
using System.Collections.Generic;



public class positionDisp : MonoBehaviour {
	Text myText;
//	Camera camera;


	// Use this for initialization
	void Start () {
//		camera = GetComponent<Camera>();
		myText = GetComponentInChildren <Text>();//UIのテキストの取得の仕方
		myText.text = DataManager.Instance.hoge.ToString("F3");
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetPos = DataManager.Instance.hoge;
		Vector3 targetPosInCamera = DataManager.Instance.hogeInCamera;
//		myText.text = targetPos.ToString("F3")+"\n"+targetPosInCamera.ToString("F3");
		myText.text="";
		foreach (KeyValuePair<string, Vector3> pair in DataManager.Instance.cameraFrameBlocks) {//http://qiita.com/kwst/items/2cfd01b7f28daf0f495e
			myText.text=myText.text+pair.Key+"\n";
			myText.text=myText.text+pair.Value.ToString("F2")+"\n";
		}
		myText.text += "\n"+"COMMANDS"+"\n"+DataManager.Instance.commandText;
//		Vector3 viewPos = camera.WorldToViewportPoint(targetPos);
//		if (viewPos.x > 0.5F)
//			print("target is on the right side!");
//		else
//			print("target is on the left side!");
	}
}