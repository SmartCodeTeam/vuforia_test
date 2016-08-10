using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class getBlockComs : MonoBehaviour {

	static int CompareKeyValuePair(KeyValuePair<string, Vector3> a, KeyValuePair<string, Vector3> b)
	{//http://smdn.jp/programming/netfx/sorting/0_basictypes/#SortDictionary
		//
		//最終的には評価関数みたいな感じにしたい。
		//
		// Keyで比較した結果を返す
		//return string.Compare(x.Key, y.Key);
		int val=0;
		float ferr=(b. Value.y -a.Value.y)*1000;
		int err=(int)ferr;
		Debug.Log ("hyouka"+a.Value+":"+b.Value);
		if (err> 0) {
			Debug.Log ("plus"+err);
			val = err;
		}else{
			Debug.Log ("minus"+err);
			val = err;
		}
		return val;
	}

	public void ButtonPush() {
		Debug.Log("Button Push !!");
		DataManager.Instance.commandText = "";
		Dictionary<string, int> status =DataManager.Instance.blockStatus;
		List<KeyValuePair<string, Vector3>> lineUP = new List<KeyValuePair<string, Vector3>>(DataManager.Instance.cameraFrameBlocks);
		lineUP.Sort (CompareKeyValuePair);//LineUpに上から順にブロックの連想配列が入っている。
		foreach (KeyValuePair<string, Vector3> pair in lineUP) {
			Debug.Log (pair.Key+":"+pair.Value);
			if(status[pair.Key]==1){//blockのstatusが1、つまり見えてるやつだけコマンドブロックとして出力
				DataManager.Instance.commandText+=pair.Key+"\n"+pair.Value+"\n";
			}
		}//うまくいかない！//解決!

	}
}
