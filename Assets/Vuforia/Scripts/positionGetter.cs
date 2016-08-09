using UnityEngine;
using System.Collections;
using System.Collections.Generic;//リストよう　http://qiita.com/okjoesyaony/items/9115ee4303d413a768ff


public class positionGetter : MonoBehaviour {
	// Use this for initialization
	Vector3 pos;
	void Start () {
		pos=transform.position;

	}
	
	// Update is called once per frame
	void Update () {
//		if(Input.GetKeyDown("space")) {
//			pos=pos + new Vector3 (1.0f, 1.0f, 1.0f);
//		}
		pos=transform.position;
		DataManager.Instance.hoge = pos;
		DataManager.Instance.rawBlocks.Remove(name);
		DataManager.Instance.rawBlocks.Add(name, pos);

	}
}