using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Continuous_Command2 : MonoBehaviour {

	public int stability=0;
	// Use this for initialization
	void Start () {
		DataManager.Instance.CodeStack=new List<string>();

	}
	//並べ替えに難あり。早めの修正を行うこと！！
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
		string ND = DataManager.Instance.NewDetected;
		string NL = DataManager.Instance.NewLost;
		//何かが変化したら、安定性判定を0にし、ある時間後に1にする
		if(ND!="init" || NL!="init"){
			stability = 0;
			DataManager.Instance.NewDetected="init";
			DataManager.Instance.NewLost="init";
			StartCoroutine(Stability_On(0.5f));//この時間後にstabilityを1に

			lost_New (NL);//このとき、NLだけは強制削除(順序のねじれの防止)
		}


		DataManager.Instance.stability = stability;

		Dictionary<string, int> status =DataManager.Instance.blockStatus;
		List<string> CodeSnap =new List<string>();
		List<KeyValuePair<string, Vector3>> lineUP = new List<KeyValuePair<string, Vector3>>(DataManager.Instance.cameraFrameBlocks);
		lineUP.Sort (CompareKeyValuePair);//LineUpに上から順にブロックの連想配列が入っている。
		foreach (KeyValuePair<string, Vector3> pair in lineUP) {
			if(status[pair.Key]==1){//blockのstatusが1、つまり見えてるやつだけコマンドブロックとして出力
				Debug.Log("Detected");
				Debug.Log (pair.Key+":"+pair.Value);
				CodeSnap.Add(pair.Key);
			}
		}
		DataManager.Instance.CodeSnapDisp = CodeSnap;

		List<string> CodeStack =DataManager.Instance.CodeStack;

//		string ND=DataManager.Instance.NewDetected;
//		string NL=DataManager.Instance.NewLost;
//		if(ND!="init"){
//			StartCoroutine(DelayMethod1(0.05f,ND));
//			DataManager.Instance.NewDetected="init";
//		}
//	
//		if(NL!="init") {//新しくロストしたら
//			StartCoroutine(DelayMethod2(0.5f,NL));
//			DataManager.Instance.NewLost="init";
//		}

		//最新のSNAPとSTACKに違いがあったら、それを補正する
		//まずはSNAPとSTACKで名前が多いか少ないかをみる
		List<string> GAP_detected=new List<string>();
		List<string> GAP_lost=new List<string>();
		List<string> GAP_twist=new List<string>();

		for(int i =0;i<CodeSnap.Count;i++){
			int index=CodeStack.IndexOf(CodeSnap[i]);
			if (index < 0) {//SnapにあるのにStackにないとき
				GAP_detected.Add(CodeSnap[i]);
			}
		}
		for(int i =0;i<CodeStack.Count;i++){
			int index=CodeSnap.IndexOf(CodeStack[i]);
			if (index < 0 && CodeStack[i].Contains("(")) {//Stackにある名前で、Snapにないもののとき
				GAP_lost.Add(CodeStack[i]);
			}
		}

//		//また、順序がねじれているものをリストアップし削除
//		for(int i =0;i<CodeSnap.Count;i++){
//			int index=CodeStack.IndexOf(CodeSnap[i]);
//			if (index < 0) {//SnapにあるのにStackにないとき
//				GAP_detected.Add(CodeSnap[i]);
//			}
//		}


		if(GAP_lost.Count>0 && stability==1){ //GAPがあり、何かがdetectされてからちょっと経って安定しているとき
			//GAPが一個以上のとき、一つだけにしたいので、最初のGAPコマンド以外をCodeSnapから削除
			for (int i=0; i < GAP_lost.Count; i++) {
				lost_New (GAP_lost[i]);
			}
		}

		if(GAP_detected.Count>0 && stability==1){ //GAPがあり、何かがdetectされてからちょっと経って安定しているとき
			//GAPが一個以上のとき、一つだけにしたいので、最初のGAPコマンド以外をCodeSnapから削除
			if(GAP_detected.Count>1){
				for (int i=0; i < GAP_detected.Count-1; i++) {
					CodeSnap.Remove (GAP_detected[i+1]);
				}
			}
			detect_New(GAP_detected[0],CodeSnap);
		}
	}


	private IEnumerator Stability_On(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		stability = 1;
	}
	void detect_New (string ND,List<string> CodeSnap){
		//新しくディテクトしたら、ちょっとまって順番とりたい(ディテクトした瞬間は座標が不安定だから)
			// Do anything
			//並べ替えに難あり。早めの修正を行うこと！！  <- 原因はディテクト初期の位置不安定だった

			//DataManager.Instance.NewDetected="init";
			//以上で、今見えてるコードのデータをCodeSnapに格納
			//以下で、新しくディテクトされたものの名前NDと、CodeSnapと、コードの蓄積であるCodeStackを用いて処理を行う。
			//まず、NDがCodeSnapのどこにいるかを調べる(一番上？下？中？)
			int num = CodeSnap.IndexOf(ND);
			int com_len = CodeSnap.Count;
			List<string> stack=DataManager.Instance.CodeStack;
			if (stack.Count > 0) {//名前付きスタックがすでにあるとき
				if (CodeSnap.Count > 2) {//スナップが2こ以上のとき
					if (num == CodeSnap.Count - 1) {			//一番下のとき
						Debug.Log ("BOTTOM");
						string pre_bottom=CodeSnap[CodeSnap.Count - 2];
						int insert_index = stack.IndexOf (pre_bottom)+1;
						//一度ロストしたものかを判定(stackのinsert_indexに名前なしコマンドがあるかチェック)
					if (stack.Count > insert_index) {
						if (ND.Contains (stack [insert_index])) {//挿入するところにすでに同一コマンドあり、
							stack.RemoveAt (insert_index);//その名前なし同一コマンドを削除
							stack.Insert (insert_index, ND);
						} else {
							stack.Insert (insert_index, ND);
						}
					} else {
						stack.Insert (insert_index, ND);					
					}
					} else if (num == 0) {			//一番上のとき
						Debug.Log ("TOP");
						string pre_top=CodeSnap[1];
						int insert_index = stack.IndexOf (pre_top);
						//一度ロストしたものかを判定(stackのinsert_indexに名前なしコマンドがあるかチェック)
					if (insert_index > 0) {//上にコマンドがある時
						if (ND.Contains (stack [insert_index - 1])) {//挿入するところにすでに同一コマンドあり、
							stack.RemoveAt (insert_index - 1);//その名前なし同一コマンドを削除
							stack.Insert (insert_index-1, ND);
						}else{
							stack.Insert (insert_index, ND);
						}
					} else {//上にコマンドがないとき、つまり、消さないで挿入だけする時
						stack.Insert (0, ND);
					}
					} else {//上と下以外のとき
						Debug.Log ("MID");
						string pre=CodeSnap[num-1];
						string fol=CodeSnap[num+1];
					int pre_index = stack.IndexOf (pre);
					int fol_index = stack.IndexOf (fol);
					if(fol_index==pre_index+1){//stack内で順番続きなら
						stack.Insert (fol_index, ND);//その間にNDをいれる
					}else if(fol_index==pre_index+2){//間に一個、名前なしコマンドがすでにあるとき、そのコマンドとNDが同種なら代入、違ったら挿入できないのでエラーポップ
						//この手法で本当にいいの??
						if (ND.Contains (stack [pre_index + 1])) {//同種だったら
							stack.RemoveAt (pre_index + 1);
						} else {
						//エラーポップアップ　位置判断のための情報が足りません
						}
					}else{
						//エラーポップ　画面上の四角の色かえる的な　「もう少しコマンドを写してください」
					}
				}
				} else {//スナップが一個の時 名前つきスタックがない時点で大丈夫かな...
					Debug.Log("エラー001");
					//もしスナップが一個で、スタックが全部名前なしなら、スタックのなかにあるスナップと同一の名前なしコマンドに代入しちゃえば良いんじゃない？
					int index=stack.LastIndexOf(ND);
				if(index>=0){
					stack.RemoveAt (index);
					stack.Insert (index,ND);
				}
				else{//まったく見たことないNDならば
					stack.Add(ND);	
				}
			}
			} else {//名前付きコードスタックがまだない時
				stack.AddRange(CodeSnap);
			}
		DataManager.Instance.CodeStack=stack;
	}

	void lost_New(string NL){
		// Do anything
		//以上で、今見えてるコードのデータをCodeSnapに格納
		//以下で、新しくロストされたものの名前NLと、CodeSnapと、コードの蓄積であるCodeStackを用いて処理を行う。

		//CodeStack上の名前付き情報を削除、コマンドの一般名に置き換え
		int index=DataManager.Instance.CodeStack.IndexOf(NL);//存在しないときは-1
		List<string> stack=DataManager.Instance.CodeStack;
		if(index>=0){//NLがきちんとCodeStackのなかにある場合
			stack.RemoveAt (index);
			string generalize=NL.Split(null) [0];//generalizeはleft(3)とかの(3)を取り払ったもの。
			stack.Insert (index,generalize);
		}
		DataManager.Instance.CodeStack = stack;
	}


}
