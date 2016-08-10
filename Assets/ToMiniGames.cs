using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ToMiniGames: MonoBehaviour {

	public void ButtonPush() {
		if (name == "Button") {
			Application.LoadLevel("stage1");		
		}
		if (name == "Button (1)") {
			Application.LoadLevel("stage2");		
		}
		if (name == "Button (2)") {
			Application.LoadLevel("stage3");		
		}

	}

}
