using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;

public class moveBall : MonoBehaviour {

	public float speed;
	public float command_change_flag;
	public int command_num=0;
	private float TargetX=0;
	private float TargetZ=0;
	private float RefX=0;
	private float RefZ=0;
	private Rigidbody rb;
	private string inter_com= "next";
	private float onegrid = 3;
	private bool half_flag=false;
	private string[] commandList = {};
	private string jump_com = "";
	private float force = 3.0f;
	private float stop_factor=0.65f;
	private string event_switch="none";
	private string event_value="red";//これもtxtデータから読み取りたい。
	private int event_num = 0;
	private string[] eventcoms={};
	void Start ()
	{
		RefX=transform.position.x;
		RefZ=transform.position.z;
//		commandList=ReadComFile();//テキストからコマンド読み込み
		commandList=DataManager.Instance.fixedBlockCodes.ToArray();//Vuforiaの方からコマンド読み込み
		commandList=ForComUnfolding(commandList);

		rb = GetComponent<Rigidbody>();
		Debug.Log(rb.transform.position);
		Debug.Log(rb.gameObject);
		Debug.Log(rb.tag);
	}

	void FixedUpdate ()
	{
		float PosX = rb.transform.position.x;//x+-が右左
		float PosY = rb.transform.position.y;
		if (PosY > 0) {//ボードの上にいるとき
			float PosZ = rb.transform.position.z;//z+-が上下
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");

			Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
			rb.AddForce (movement * speed);
			//コマンド実行
			CommandSwitching(commandList,inter_com);
			CommandControl(inter_com, PosX,PosZ,jump_com);
			//		
		}
	}

	public void CommandSwitching(string[] allcoms,string com){//全コマンドから一つづつ取り出す
		if(event_switch=="none"){
			if(com=="next"){
				if (command_num < allcoms.Length) {
					inter_com = allcoms [command_num];
					command_num += 1;
				}
			}
		}
		if(event_switch=="red"){//if内のコマンドで実行
			if(com=="next"){
				if (event_num < eventcoms.Length) {
					inter_com = eventcoms [event_num];
					event_num += 1;
				} else {//最後のコマンドが終わって"next"がきたら、イベントスイッチを切る。
					event_switch="none";
				}
			}
		}
	}
	public void CommandControl(string com,float PosX,float PosZ,string spe_com){
		if (com == "up") {
			if (PosZ >= RefZ + onegrid) {
				RefZ = RefZ + onegrid;
				inter_com = "next";
				half_flag = false;
				Stop (rb);
			}
			if (inter_com != "next") {
				if (PosZ >= RefZ + onegrid / 2.0) {
					GoUp (-stop_factor * force);
					half_flag = true;
				} else {
					GoUp (1.0f * force);
				}
			}
		} else if (com == "down") {
			if (PosZ <= RefZ - onegrid) {
				RefZ = RefZ - onegrid;
				inter_com = "next";
				half_flag = false;
				Stop (rb);
			}
			if (inter_com != "next") {
				if (PosZ <= RefZ - onegrid / 2.0) {
					GoUp (stop_factor * force);
					half_flag = true;
				} else {
					GoUp (-1.0f * force);
				}
			}
		} else if (com == "right") {
			if (PosX >= RefX + onegrid) {
				RefX = RefX + onegrid;
				inter_com = "next";
				half_flag = false;
				Stop (rb);
			}
			if (inter_com != "next") {
				if (PosX >= RefX + onegrid / 2.0) {
					GoRight (-stop_factor * force);
					half_flag = true;
				} else {
					GoRight (1.0f * force);
				}
			}
		} else if (com == "left") {
			if (PosX <= RefX - onegrid) {
				RefX = RefX - onegrid;
				inter_com = "next";
				half_flag = false;
				Stop (rb);
			}
			if (inter_com != "next") {
				if (PosX <= RefX - onegrid / 2.0) {
					GoRight (stop_factor * force);
					half_flag = true;
				} else {
					GoRight (-1.0f * force);
				}
			}
		}else if(com=="resting"){
			inter_com="resting";
		}else{
			inter_com = "next";
		}
	}


	public void GoUp(float force){//1マス分でしっかり終了するように。
		Vector3 movement = new Vector3(0.0f, 0.0f, force);
		rb.AddForce (movement * speed);
	}
	public void GoDown(){

	}
	public void GoRight(float force){
		Vector3 movement = new Vector3(force, 0.0f, 0.0f);
		rb.AddForce (movement * speed);
	}
	public void GoLeft(){

	}
	//ジャンプはどうするか。
	public void GoAbove(float force){
		Vector3 movement = new Vector3(0.0f, force, 0.0f);
		rb.AddForce (movement * speed);
	}

	public void Stop(Rigidbody rb){
		rb.transform.position = new Vector3 (RefX, 0.5f, RefZ);
		rb.velocity = new Vector3 (0.0f, 0.0f, 0.0f);
		rb.angularVelocity = new Vector3 (0.0f, 0.0f, 0.0f);
	}


	public void StopHere(Rigidbody rb){
		rb.velocity = new Vector3 (0.0f, 0.0f, 0.0f);
		rb.angularVelocity = new Vector3 (0.0f, 0.0f, 0.0f);
	}

	public void Bound(Rigidbody rb){
		rb.velocity = -0.45f*rb.velocity;
		rb.angularVelocity = new Vector3 (0.0f, 0.0f, 0.0f);
	}

		
	public string[] ReadComFile(){
		FileInfo fi = new FileInfo (Application.dataPath+"/"+"ComFile.txt");
		string line = "";
		string[] coms_txt = {};
		using(StreamReader sr = new StreamReader(fi.OpenRead(),Encoding.UTF8)){//一行ずつよみとり
			//			string com=sr.ReadToEnd();
			//			Debug.Log(com);
			while ((line = sr.ReadLine()) != null){
				//comsにlineを追加する。
				Debug.Log(line);
				string[] result = new string[coms_txt.Length + 1];
				for(int j=0;j< coms_txt.Length;j++ ){//見づらいコード。動的な配列を用いたほうがいい。
					result[j]=coms_txt[j];
				}
				result[coms_txt.Length] = line;
				coms_txt = result;
			}
		}

		return coms_txt;
	}

	public string[] ForComUnfolding(string[] commandList){
		List<string> modified = new List<string>();
		for(int i=0;i<commandList.Length;i++){
			string line = commandList[i];
			string itr_num_str;
			if (line.Contains ("for")) {
				if (line == "end_for") {
					modified.Add (line);
				} else {// for,5みたいなとき
					itr_num_str = line.Split (',') [1];
					modified.Add ("for");
					modified.Add (itr_num_str);
				}
			} else {
				modified.Add (line);
			}
		}//modifiedに {"for",5,"up","down",,,,}みたいに入った。
		while(modified.IndexOf("end_for")!=-1){
			modified=UnfoldFirstFor(modified);
		}
		commandList=modified.ToArray ();
		return commandList;
	}

	public List<string> UnfoldFirstFor(List<string> fixed_commandList){//for_endが見つかった時点でそれに合致するfor(for_endにもっとも近いfor)を定めて展開する。ひとつだけ。
		int index_end_for = fixed_commandList.IndexOf("end_for");//IndexOf ->http://dobon.net/vb/dotnet/programing/binarysearch.html
		int index_for = fixed_commandList.LastIndexOf( "for",index_end_for);
		Debug.Log ("for");
		Debug.Log (index_for);
		Debug.Log ("end_for");
		Debug.Log (index_end_for);
		//以下、forを展開する。
		//ループ数取得
		int loop_num=int.Parse(fixed_commandList[index_for+1]);
		//ループさせるエリアのコマンド配列の取得(in_forsに格納)
		List<string> in_fors = new List<string>();
		for(int i=0;i<index_end_for-index_for-1;i++){
			in_fors.Add (fixed_commandList[index_for+1+i]);
		}
		//元のコマンドを削除、ループ数倍したin_forsを挿入
		List<string> new_coms= new List<string>();
		for(int i=0;i<index_for;i++){
			new_coms.Add (fixed_commandList[i]);
		}
		for(int i=0;i<loop_num;i++){
			new_coms.AddRange (in_fors);
		}
		for(int i=0;i<fixed_commandList.Count-index_end_for-1;i++){
			new_coms.Add (fixed_commandList[index_end_for+1+i]);
		}
		fixed_commandList = new_coms;
//		//出力
		return fixed_commandList;
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Finish"))
		{
			other.gameObject.SetActive (false);
		}
		if (other.gameObject.CompareTag ("Obstacle"))
		{
			Bound (rb);
			Invoke("DelayMethod", 1.0f);
			inter_com = "resting";
		}
		if (other.gameObject.CompareTag ("Red"))
		{
			Bound (rb);
			Invoke("DelayMethodRed", 1.0f);
			inter_com = "resting";
//			event_num += 0;
		}
//		if (other.gameObject.CompareTag ("Blue"))
//		{
//			Stop (rb);
//			inter_com = "blue";
//		}
//		if (other.gameObject.CompareTag ("Green"))
//		{
//			other.gameObject.SetActive (false);
//		}

	}

	void DelayMethod()
	{
		Debug.Log("Delay call");
		Stop (rb);
		inter_com = "next";

	}

	void DelayMethodRed()
	{
		Debug.Log("Delay call");
		Stop (rb);
//		inter_com = "red";
		inter_com = "next";

	}


}