using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class contactManagement : MonoBehaviour {
	//この関数のなかで、
	//1. どのブロックが認識されているのか
	//2. その上下のブロックの認識ができてるかどうか
	//3. 上下のブロックの名称は何か
	//という情報を集めてコマンド表を作成する。continuous2の後継
	public int stability=0;
	private List<string> codestack=new List<string>();

	void Start () {
		DataManager.Instance.CodeStack=new List<string>();
	}


	void Update () {
		codestack=DataManager.Instance.CodeStack;//リセットなどの為に常にDatamanagerのCodeStackと同期しておく
		string ND = DataManager.Instance.NewDetected;
		string NL = DataManager.Instance.NewLost;
		Dictionary<string, int> status =DataManager.Instance.blockStatus;
		List<string> Visibles = new List<string> ();//今見えているブロック名をVisiblesに格納
		foreach(KeyValuePair<string,int> pair in status){
			if(pair.Value==1){
				Visibles.Add(pair.Key);
			}
		}
		//すべてのconnectionを取得
		Dictionary<string, string> allConnections =DataManager.Instance.Connections;
		Dictionary<string, string> fixedConnections =DataManager.Instance.Connections;
		//そこから画面に写ってるものだけを抽出しConnectionsに移す
		Dictionary<string, string> Connections = new Dictionary<string, string> () {
			{"upperBlockName", "lowerBlockName"}//1こ目に上のブロック名、2こ目は下
		};
		List<string> Uppers = new List<string> ();
		List<string> Lowers = new List<string> ();
		foreach(KeyValuePair<string,string> pair in allConnections){
			if(pair.Value!="none"){//コネクションが成立しているとき
				if (status [pair.Key] == 1 && status [pair.Value] == 1) {//blockのstatusが1、つまり見えてるやつだけコマンドブロックとして出力
					//				Connections[pair.Key]=pair.Value;
					Uppers.Add (pair.Key);
					Lowers.Add (pair.Value);
				} else {//見えてないやつがコネクションに含まれるならば、それを除去
					fixedConnections.Remove(pair.Key);
					Debug.Log ("removed ghost");
				}
			}
		}
		//存在しないコネクションを除去したものをconnectionに戻す
		DataManager.Instance.Connections = fixedConnections;

		//以下、codestackを変更する。
		//codestackがまだブロック名が何も入っていない場合は、NDを出てきたものを最後尾に挿入する。
		int flag=0;
		for (int i = 0; i < codestack.Count; i++) {
			if (codestack [i].Contains("(")){
				flag = 1;//名前がある！
			}
		}
		if (flag == 0) {//名前なしの場合
			if(ND!="init"){//NDが見つかった場合
				codestack.Add(ND);
				Debug.Log ("add New island");
			}
		}
		DataManager.Instance.NewDetected="init";//NDが見つかってないときは"init"としておくため代入

		//次にcodestackのなかで名前付きのものを調べて、それがUppers,Lowersに存在するかを調べる。
		//以下のコードは無限ループに陥るケースあり、アルゴリズム改変2017/1/9。for文の中でループの限界個数を変えてはいけない！！！
		//記録として残しておく。
//		for (int i = 0; i < codestack.Count; i++) {//codestackを上から走査
//			string com = codestack [i];//codestackないのコマンドor名前
//			if(com.Contains("(")){//名前付きのとき
//				//まず、VisiblesにないものがCodestack内にある、つまりcodestackにあるものが画面外に消えたときに、codestackに余計にあるものを除外
//				int visibleIndex=Visibles.IndexOf(com);
//				if (visibleIndex < 0) {//codestackにあるのにvisibleに名前がないとき、ロストと判定
//					string generalize=com.Split(null) [0];//generalizeはleft(3)とかの(3)を取り払ったもの。
//					codestack [i] = generalize;
//					Debug.Log ("LOST! "+com);
//				}
//
//				//Uppersに存在するかを調べる。
//				int comIndex=Uppers.IndexOf(com);//名前がuppersのなかの何番目か
//				if(comIndex>=0){//uppersに含まれる場合は
//					Debug.Log ("Upper Find! "+com);
//					string next_com=Lowers[comIndex];//Lowesからその下のブロック名を取得
//					if (i < codestack.Count - 1) {//最後尾(index out of range)でないときは
//						codestack[i+1]=next_com;//コマンド名を収納あるいは置き換え
//					} else {//最後尾のときは要素を増やさねばならないので
//						codestack.Add(next_com);//付け加える
//					}
//				}
//
//				//Lowersに存在するかを調べる。
//				int lowerIndex=Lowers.IndexOf(com);//名前がlowersのなかの何番目か
//				if(lowerIndex>=0){//lowersに含まれる場合は
//					Debug.Log ("Lower Find! "+com);
//					string pre_com=Uppers[lowerIndex];//Uppersからその上のブロック名を取得
//					if (i >0) {//最前(index out of range)でないときは
//						codestack[i-1]=pre_com;//コマンド名を収納あるいは置き換え
//					} else {//最前のときは要素を増やさねばならないので
//						codestack.Insert(0,pre_com);//最前に挿入
//					}
//				}				
//			}
//				
//		}
		//アルゴリズムのコメントアウトは以上 2017/1/9。

		//改変版1
		//connectionsの循環検知

		int codelen=codestack.Count;
		for (int i = 0; i < codelen; i++) {//codestackを上から走査
			string com = codestack [i];//codestackないのコマンドor名前
			if(com.Contains("(")){//名前付きのとき
				//まず、VisiblesにないものがCodestack内にある、つまりcodestackにあるものが画面外に消えたときに、codestackに余計にあるものを除外
				int visibleIndex=Visibles.IndexOf(com);
				if (visibleIndex < 0) {//codestackにあるのにvisibleに名前がないとき、ロストと判定
					string generalize=com.Split(null) [0];//generalizeはleft(3)とかの(3)を取り払ったもの。
					codestack [i] = generalize;
					Debug.Log ("LOST! "+com);
				}

				//Uppersに存在するかを調べる。
				int comIndex=Uppers.IndexOf(com);//名前がuppersのなかの何番目か
				if(comIndex>=0){//uppersに含まれる場合は
					Debug.Log ("Upper Find! "+com);
					string next_com=Lowers[comIndex];//Lowesからその下のブロック名を取得
					if (i < codestack.Count - 1) {//最後尾(index out of range)でないときは
						codestack[i+1]=next_com;//コマンド名を収納あるいは置き換え
					} else {//最後尾のときは要素を増やさねばならないので
						codestack.Add(next_com);//付け加える
					}
				}

				//Lowersに存在するかを調べる。
				int lowerIndex=Lowers.IndexOf(com);//名前がlowersのなかの何番目か
				if(lowerIndex>=0){//lowersに含まれる場合は
					Debug.Log ("Lower Find! "+com);
					string pre_com=Uppers[lowerIndex];//Uppersからその上のブロック名を取得
					if (i >0) {//最前(index out of range)でないときは
						codestack[i-1]=pre_com;//コマンド名を収納あるいは置き換え
					} else {//最前のときは要素を増やさねばならないので
						codestack.Insert(0,pre_com);//最前に挿入
					}
				}				
			}

		}



		DataManager.Instance.CodeStack=codestack;//リセットなどの為に常にDatamanagerのCodeStackと同期
	}


}
	


	