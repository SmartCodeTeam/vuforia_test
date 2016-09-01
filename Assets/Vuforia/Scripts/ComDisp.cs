using UnityEngine;
using System.Collections;
using UnityEngine.UI;//この宣言が必要
using System.Collections.Generic;



public class ComDisp : MonoBehaviour {
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


		string[] array;
//		array = DataManager.Instance.EstimatedComs.ToArray();//blockCodesの表示
		array = DataManager.Instance.CodeStack.ToArray();//blockCodesの表示
		myText.text += "\n"+"COMMANDS LIST"+"\n";
		for (int i = 0; i < array.Length; i++) {
			myText.text += array[i] + "\n";
		}
	}
}