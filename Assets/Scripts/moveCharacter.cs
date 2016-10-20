using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
public class moveCharacter : MonoBehaviour {

	public float speed;
	public float rotation_speed;
	public  int _currentCommandNum =  0;

	private float _currentPosX = 0;
//	private float _currentPosY = 0;
	private float _currentPosZ = 0;
	private float _currentDir = 90;

	private float _posX;
//	private float _posY;
	private float _posZ;
	private float _dir;

	private float onegrid = 3;
	private string[] commandList = {};

	private Animator animator;

	private CharacterController controller;
	 
	enum CharacterState {
		IDLE = 0,
		RIGHT = 1,
		LEFT = 2,
		UP = 3,
		DOWN = 4
	};

	enum CommandState {
		CHANGING = 0,
		READY = 1
	};

	//キャラクターの初期状態
	private CharacterState _characterState = CharacterState.IDLE;

	//コマンドの初期状態
	private CommandState _commandState = CommandState.READY;

	void Start ()
	{
		_currentPosX = transform.position.x;
		_currentPosZ = transform.position.z;

		//		commandList=ReadComFile();//テキストからコマンド読み込み
		commandList=DataManager.Instance.gameCodes;//VuforiaのやつをShowCommandsで修正したものを読み込み
		commandList=ForComUnfolding(commandList);

		animator = GetComponent<Animator> ();
		controller = GetComponent<CharacterController> ();

	}

	void Update ()
	{
		_posX = controller.transform.position.x;//x+-が右左
//		_posY = controller.transform.position.y;
		_posZ = controller.transform.position.z;

		_dir = transform.eulerAngles.y;

//		_dirVec = new Vector3 (Mathf.Cos (angleDir),0.0f , Mathf.Sin (angleDir));

		Debug.Log(_dir);


		//コマンド実行
		CommandSwitching(commandList);


		if (_characterState == CharacterState.IDLE) {
//			Debug.Log ("Character State is IDLE");
			animator.SetBool ("Run", false);

		} 
		else if (_characterState == CharacterState.RIGHT) {
//			Debug.Log ("Character State is RIGHT");
			animator.SetBool ("Run", true);
			controller.transform.Rotate(rotation_speed*Vector3.up * Time.deltaTime);
			//controller.transform.Translate (speed*Vector3.forward * Time.deltaTime);
		} 
		else if (_characterState == CharacterState.LEFT) {
//			Debug.Log ("Character State is LEFT");
			animator.SetBool ("Run", true);
			controller.transform.Rotate(-rotation_speed*Vector3.up * Time.deltaTime);
//			controller.transform.Translate (speed*Vector3.back * Time.deltaTime);
		} 
		else if (_characterState == CharacterState.UP) {
//			Debug.Log ("Character State is UP");
			animator.SetBool ("Run", true);
			controller.transform.Translate (speed*Vector3.left * Time.deltaTime);
		} 
		else if (_characterState == CharacterState.DOWN) {
//			Debug.Log ("Character State is DOWN");
			animator.SetBool ("Run", true);
			controller.transform.Translate (speed*Vector3.right * Time.deltaTime);
		}
	}

	//Command State が　CHANGINGになった時にcommand_numを更新
	public void CommandSwitching(string[] commandList){//全コマンドから一つづつ取り出す
		if(_commandState == CommandState.CHANGING){
			if (_currentCommandNum < commandList.Length) {
			_currentCommandNum += 1;
			_currentPosX = controller.transform.position.x;
//			_currentPosY = controller.transform.position.y;
			_currentPosZ = controller.transform.position.z;
			_currentDir = controller.transform.eulerAngles.y;

			_commandState = CommandState.READY;
			}
		}
		if(_commandState == CommandState.READY) {
			string currentCommand = commandList [_currentCommandNum];
			if (currentCommand == "up") {
				_characterState = CharacterState.UP;
				float targetPosZ = _currentPosZ + onegrid;
				if (_posZ >= targetPosZ) {
					_commandState = CommandState.CHANGING;
					_characterState = CharacterState.IDLE;
				}
			} else if (currentCommand == "down") {
				_characterState = CharacterState.DOWN;
				float targetPosZ = _currentPosZ - onegrid;
				if (_posZ <= targetPosZ) {
					_commandState = CommandState.CHANGING;
					_characterState = CharacterState.IDLE;
				}
			} else if (currentCommand == "right") {
				_characterState = CharacterState.RIGHT;

				float targetDir = _currentDir + 90;
				if (targetDir > 360) {
					targetDir -= 360; 
				}

				if (_dir >= targetDir) {
					_commandState = CommandState.CHANGING;
					_characterState = CharacterState.IDLE;
				}

//				float targetPosX = _currentPosX + onegrid;
//				if (_posX >= targetPosX) {
//					_commandState = CommandState.CHANGING;
//					_characterState = CharacterState.IDLE;
//				}
			} else if (currentCommand == "left") {
				_characterState = CharacterState.LEFT;

				float targetDir = _currentDir - 90;
				Debug.Log (targetDir);
				if (targetDir < 0) {
					targetDir = 360 + targetDir; 
				}

				if (_dir <= targetDir) {
					_commandState = CommandState.CHANGING;
					_characterState = CharacterState.IDLE;
				}

//				float targetPosX = _currentPosX - onegrid;
//				if (_posX <= targetPosX) {
//					_commandState = CommandState.CHANGING;
//					_characterState = CharacterState.IDLE;
//				}
			}
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Finish"))
		{
			other.gameObject.SetActive (false);
		}
		if (other.gameObject.CompareTag ("Obstacle"))
		{
			animator.SetBool ("Die", true);

		}
	}

	void DelayMethod()
	{
		Debug.Log("Delay call");
//		Stop (rb);

	}
		








	//以下並び替え用の関数なので別クラスにしたい
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
}