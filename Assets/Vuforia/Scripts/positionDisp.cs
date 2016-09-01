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
		myText.text += DataManager.Instance.stability;
		myText.text += "\n"+"New Detected"+"\n"+DataManager.Instance.NewDetected;
		myText.text += "\n"+"New Lost"+"\n"+DataManager.Instance.NewLost;
		myText.text +="\n"+"Now Detecting" + "\n";
		Dictionary<string, int> status =DataManager.Instance.blockStatus;
		foreach (KeyValuePair<string, Vector3> pair in DataManager.Instance.cameraFrameBlocks) {//http://qiita.com/kwst/items/2cfd01b7f28daf0f495e
//			Debug.Log("Key"+pair.Key);
//			Debug.Log("status"+pair.Key+" "+status[pair.Key]);
			if(status[pair.Key]==1){//blockのstatusが1のとき// やっぱひょうじしない
//				myText.text=myText.text+pair.Key+"\n";
//				myText.text=myText.text+pair.Value.ToString("F2")+"\n";
			}
		}
//		foreach (KeyValuePair<string, int> pair in status ) {//http://qiita.com/kwst/items/2cfd01b7f28daf0f495e
//			myText.text=myText.text+pair.Key+" "+pair.Value+"\n";
//		}


//		myText.text += "\n"+"COMMANDS"+"\n"+DataManager.Instance.commandText;

//
		string[] array;
//		array = DataManager.Instance.blockCodes.ToArray();//blockCodesの表示
//		myText.text += "\n"+"COMMANDS LIST"+"\n";
//		for (int i = 0; i < array.Length; i++) {
//			myText.text += array[i] + "\n";
//		}
//

		array = DataManager.Instance.CodeSnapDisp.ToArray();//fixedBlockCodesの表示
		myText.text += "\n"+"SNAP"+"\n";
		for (int i = 0; i < array.Length; i++) {
			myText.text += array[i] + "\n";
		}

		//		Vector3 viewPos = camera.WorldToViewportPoint(targetPos);
//		if (viewPos.x > 0.5F)
//			print("target is on the right side!");
//		else
//			print("target is on the left side!");

	}
}