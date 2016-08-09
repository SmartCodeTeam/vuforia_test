using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class getBlockComs : MonoBehaviour {

	static int CompareKeyValuePair(KeyValuePair<string, Vector3> x, KeyValuePair<string, Vector3> y)
	{//http://smdn.jp/programming/netfx/sorting/0_basictypes/#SortDictionary
		//
		//最終的には評価関数みたいな感じにしたい。
		//
		// Keyで比較した結果を返す
		//return string.Compare(x.Key, y.Key);
		int val=0;
		if (x.Value.y - y.Value.y > 0) {
			val = 1;
		} else {
			val = -1;
		}
		return val;
	}

	public void ButtonPush() {
		Debug.Log("Button Push !!");
		DataManager.Instance.commandText = "";
		List<KeyValuePair<string, Vector3>> lineUP = new List<KeyValuePair<string, Vector3>>(DataManager.Instance.cameraFrameBlocks);
		lineUP.Sort (CompareKeyValuePair);//LineUpに上から順にブロックの連想配列が入っている。
		foreach (KeyValuePair<string, Vector3> pair in lineUP) {
			Debug.Log (pair.Key+":"+pair.Value);
			DataManager.Instance.commandText+=pair.Key+"\n"+pair.Value+"\n";
		}
	}
}
