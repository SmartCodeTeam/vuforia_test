using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;

public class ToComsTakeFromGame : MonoBehaviour {

	public void ButtonPush() {
		DataManager.Instance.PreActiveGameName = SceneManager.GetActiveScene ().name;
		Application.LoadLevel("many_blocks");
	}

}
