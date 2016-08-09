using UnityEngine;
using System.Collections;
using UnityEngine.UI;//この宣言が必要



public class KabayakiDisp : MonoBehaviour {
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
		myText.text = "蒲焼さんまでの距離"+targetPosInCamera.z.ToString("f1");

//		Vector3 viewPos = camera.WorldToViewportPoint(targetPos);
//		if (viewPos.x > 0.5F)
//			print("target is on the right side!");
//		else
//			print("target is on the left side!");
	}
}