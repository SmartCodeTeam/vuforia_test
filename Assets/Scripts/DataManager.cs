using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataManager : MonoBehaviour {
	private static DataManager instance;

//	public List<string> strHoge = new List<string>();
	public Vector3 hoge;
	public Vector3 hogeInCamera;

	//これに毎フレームごとに各ブロックの生の座標が入力される。GetPosInCameraFlameから取得。
	public Dictionary<string, Vector3> rawBlocks = new Dictionary<string, Vector3> () {
		{"init num", new Vector3(1,2,3)}
	};

	//これに毎フレームごとにカメラから見た座標が入力される。GetPosInCameraFlameから取得。
	public Dictionary<string, Vector3> cameraFrameBlocks = new Dictionary<string, Vector3> () {
		{"init num", new Vector3(0.1f,0.1f,0.1f)}
	};

	//ブロックが読み取れている場合、{そのブロック名,1}という値が追加される。なくなると消去。DefaultTrackableEventHandlerで更新。
	public Dictionary<string, int> blockStatus = new Dictionary<string, int> () {
		{"init num", 0}//ロスト時は0, トラック時は1
	};
	public string NewDetected ="init";//新しくDetectしたもの
	public string NewLost ="init";//ロストしたものの名前

	public List<string> blockCodes = new List<string>();
	public List<string> fixedBlockCodes = new List<string>();
	public List<string[]>CodeSnapStorage =new List<string[]>();
	public List<string>CodeSnapDisp =new List<string>();
	public List<string>CodeStack =new List<string>();
	public List<string>EstimatedComs =new List<string>();
	//Connectionsにあるブロック名とその下のブロック名を保存(上下のどちらかが取れてればいいので、upperは割愛)
	public Dictionary<string, string> Connections = new Dictionary<string, string> () {
	};
	public int stability=0;
	public string commandText="";
	public string[] gameCodes={};


	public string PreActiveGameName="stage1";


	public static DataManager Instance{
		get{
			if( null == instance ){
				instance = (DataManager)FindObjectOfType(typeof(DataManager));
				if( null == instance ){
					Debug.Log(" DataManager Instance Error ");
				}
			}
			return instance;
		}
	}

	void Awake(){
		GameObject[] obj = GameObject.FindGameObjectsWithTag("DataManager");
		if( 1 < obj.Length ){
			// 既に存在しているなら削除
			Destroy( gameObject );
		}else{
			// シーン遷移では破棄させない
			DontDestroyOnLoad( gameObject );
		}
	}
}
