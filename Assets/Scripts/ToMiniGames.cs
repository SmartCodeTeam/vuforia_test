using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ToMiniGames: MonoBehaviour {

	public void ButtonPush() {
		if (name == "ToMaze1") {
			Application.LoadLevel("stage1");		
		}
		if (name == "ToMaze2") {
			Application.LoadLevel("stage2");		
		}
		if (name == "ToMaze3") {
			Application.LoadLevel("stage3");		
		}

	}

}
