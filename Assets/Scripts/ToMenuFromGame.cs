using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ToMenuFromGame : MonoBehaviour {

	public void ButtonPush() {
		Application.LoadLevel("Game_Menu");
	}

}
