using UnityEngine;
using System.Collections;
using UnityEngine.UI;//この宣言が必要
using System.Collections.Generic;



public class ComDisp1 : MonoBehaviour {
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


		Dictionary<string, string> dic;
//		array = DataManager.Instance.EstimatedComs.ToArray();//blockCodesの表示
		dic = DataManager.Instance.Connections;//blockCodesの表示
		myText.text += "\n"+"CONNECTION LIST"+"\n";
		foreach (KeyValuePair<string, string> pair in dic) {
			Debug.Log (pair.Key + " : " + pair.Value);
			myText.text += pair.Key + " : " + pair.Value + "\n";
		}
	}
}