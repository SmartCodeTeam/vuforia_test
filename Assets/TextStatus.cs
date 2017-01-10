using UnityEngine;
using System.Collections;
using UnityEngine.UI;//この宣言が必要


public class TextStatus : MonoBehaviour {

	private string blockName;

	// Use this for initialization
	void Start () {
		blockName=gameObject.transform.parent.name;
	}

	// Update is called once per frame
	void Update () {
		gameObject.GetComponent<TextMesh> ().text = "";
		int index=DataManager.Instance.CodeStack.IndexOf (blockName);
		if (index<0) {
			gameObject.GetComponent<TextMesh> ().text = "***";
		} else {
			gameObject.GetComponent<TextMesh> ().text = "OK";
//			gameObject.GetComponent<TextMesh> ().text = gameObject.transform.parent.name;
		}
	}
}
