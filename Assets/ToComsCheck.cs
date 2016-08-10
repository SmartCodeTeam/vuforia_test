using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ToComsCheck : MonoBehaviour {

	public void ButtonPush() {

		//すでに読み込まれているコマンドイメージ名の配列から、コマンドの種類だけを抽出
		//イメージ名とコマンド名の対応付け表
		List<string> preComs=DataManager.Instance.blockCodes;
		for (int i=0; i < preComs.Count; i++) {
			//preComsないのコマンドは[1]とか余計なものを含んでいる場合があるから、それを排除し"for,5","up"などに統一
			//あとはforの回数のフォーマットとかをつくる
			string[] parts=preComs[i].Split(null);//parts=["for","(1)"]とかになってたりする。
			if(parts[0]=="if"){
				parts [0] = "if,3";				
			}
			DataManager.Instance.fixedBlockCodes.Add(parts[0]);
		}



		Application.LoadLevel("Show_Commands");
	}

}
