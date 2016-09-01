using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Continuous_Command_Get : MonoBehaviour {

	public float timeOut=0.5f;//[秒]に一回実行 //http://qiita.com/Nagitch/items/fb9157b1cb27f3d37696
	//上のタイムアウトの秒数を変えた時、変数名も変えないと反映されないのなんでだろうね。
	private float timeElapsed;
	private string[] PreCodeSnap ={};

	// Use this for initialization
	void Start () {

	}

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
//		Debug.Log ("hyouka"+a.Value+":"+b.Value);
		if (err> 0) {
//			Debug.Log ("plus"+err);
			val = err;
		}else{
//			Debug.Log ("minus"+err);
			val = err;
		}
		return val;
	}
	
	// Update is called once per frame
	void Update () {
		timeElapsed += Time.deltaTime;
		if(timeElapsed >= timeOut) {
			// Do anything
			Dictionary<string, int> status =DataManager.Instance.blockStatus;
			List<string> CodeSnap =new List<string>();
			//			CodeSnap.Add("init");
			List<KeyValuePair<string, Vector3>> lineUP = new List<KeyValuePair<string, Vector3>>(DataManager.Instance.cameraFrameBlocks);
			lineUP.Sort (CompareKeyValuePair);//LineUpに上から順にブロックの連想配列が入っている。
			foreach (KeyValuePair<string, Vector3> pair in lineUP) {
				if(status[pair.Key]==1){//blockのstatusが1、つまり見えてるやつだけコマンドブロックとして出力
					Debug.Log("Detected");
					Debug.Log (pair.Key+":"+pair.Value);
					CodeSnap.Add(pair.Key);
				}
			}
			string[] ary1;
			ary1= CodeSnap.ToArray();
			string[] ary2 = PreCodeSnap;
			//前回の取得したものと同じか判定する部分
			//結果を格納する変数
			bool isEqual = true;
			if (object.ReferenceEquals(ary1, ary2))
			{				//同一のインスタンスの時は、同じとする
				isEqual = true;
			}
			else if (ary1 == null && ary2 == null){
				//両方NULLなら同じ
				isEqual=true;
			}
			else if (ary1 == null || ary2 == null
				|| ary1.Length != ary2.Length)
			{
				//どちらかがNULLか、要素数が異なる時は、同じではない
				isEqual = false;
			}
			else
			{
				//1つ1つの要素が等しいかを調べる
				for (int i = 0; i < ary1.Length; i++)
				{
					//ary1の要素のEqualsメソッドで、ary2の要素と等しいか調べる
					if (!ary1[i].Equals(ary2[i]))
					{
						//1つでも等しくない要素があれば、同じではない
						isEqual = false;
						break;
					}
				}
			}
			//前回の取得したものと同じか判定する部分おわり


			if (isEqual) {
			} else {//前回取得のコマンドと異なったら何かをする
				
				if (ary2.Length == 0) {//前のグループがnullのとき、今のグループの最後のブロックを入れる
					DataManager.Instance.EstimatedComs.Add (ary1 [ary1.Length - 1]);
				} else {//前のグループがnullでないとき
					if (ary1.Length != 0) {//今のグループもnullじゃないなら、今のグループの最後のブロックと一個前のグループの最後のブロックを比較する
						//その際、前の最後のブロックの画面座標を取得し、新しいグループの最後のブロックがそれより上の座標なら、
						//前の最後のブロックをロストしたことなので無視。


						//
						if (ary1 [ary1.Length - 1] != ary2 [ary2.Length - 1]) {//前の最後のと新しい最後のが違う時
							DataManager.Instance.EstimatedComs.Add (ary1 [ary1.Length - 1]);
						}
					}				
				}

				DataManager.Instance.CodeSnapStorage.Add (ary1);
				Debug.Log ("STATE CHANGED");
			}
//			DataManager.Instance.blockCodes = CodeSnap;
			PreCodeSnap = ary1;
//			Debug.Log("Hey");
			timeElapsed = 0.0f;
		}
	}
}
