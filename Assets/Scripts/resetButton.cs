using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class resetButton : MonoBehaviour {


	public void ButtonPush() {
		Debug.Log("Reset Push !!");
		DataManager.Instance.CodeStack = new List<string>();

	}

}
