using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataManager : MonoBehaviour {
	private static DataManager instance;

//	public List<string> strHoge = new List<string>();
	public Vector3 hoge;
	public Vector3 hogeInCamera;
	public Dictionary<string, Vector3> rawBlocks = new Dictionary<string, Vector3> () {
		{"init", new Vector3(1,2,3)}
	};
	public Dictionary<string, Vector3> cameraFrameBlocks = new Dictionary<string, Vector3> () {
		{"init", new Vector3(1,2,3)}
	};
	public List<string> blockCodes = new List<string>();
	public string commandText="";

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
