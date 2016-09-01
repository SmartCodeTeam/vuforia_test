using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ToGameWithComs : MonoBehaviour {

	public void ButtonPush() {
		Application.LoadLevel(DataManager.Instance.PreActiveGameName);
	}

}
