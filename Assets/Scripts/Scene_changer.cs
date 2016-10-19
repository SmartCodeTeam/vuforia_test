using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;

//http://ft-lab.ne.jp/cgi-bin-unity/wiki.cgi?page=unity_script_change_sceneを参考に
public class Scene_changer : MonoBehaviour {

//	private GUIStyle style;
//	private GUIStyleState styleState;
	[SerializeField]
	private GUISkin buttonGuiSkin;  //ボタンのスタイル設定用 http://qiita.com/Jshirius/items/4a64e1996e4acb2b4aed

	private string[] imaginary_commands = {"right","up","up"};//{"for","up","down","left","for_end","if","hole","if_end","up","down","left","up","down","left"};
	private int button_height = 70;
	private int button_width = 200;
	private string[] coms={};
	private int comLength=0;
	private int indent =0;
	private string itr_num_str="";
	// Use this for initialization
	void Start () {
		//ここでコマンドの取得と表示をする。
//		coms=ReadComFile();

		coms=DataManager.Instance.fixedBlockCodes.ToArray();
		DataManager.Instance.gameCodes=DataManager.Instance.fixedBlockCodes.ToArray();
		int button_pos = 0;
		//以上、コマンドの読み取り
//		coms = imaginary_commands;
		comLength = coms.Length;
		//coms

		//http://www.wisdomsoft.jp/262.html
//		style = new GUIStyle();
//		style.fontSize = 14;

		//http://ft-lab.ne.jp/cgi-bin-unity/wiki.cgi?page=unity_script_gui_widgets_customize
//		styleState = new GUIStyleState();
//		styleState.textColor = Color.green;   // 文字色の変更.
//		style.normal = styleState;
	}

	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate () {
		if (Input.GetKeyDown("1")) {
			Application.LoadLevel("stage1");
		}
		if (Input.GetKeyDown("2")) {
			Application.LoadLevel("stage2");
		}
		if (Input.GetKeyDown("3")) {
			Application.LoadLevel("stage3");
		}

	}

	void OnGUI () {//guiイベント用（毎フレーム呼び出される）  "GUI." available in OnGUI(){}
		//http://techacademy.jp/magazine/4289 スクリプトでのボタン表示
//		GUI.Label(new Rect(20, 40, 80, 20), "ShowCommands");
//		GUI.Button(new Rect(20, 20, 160, 30),"For");
		for(int i=0;i<comLength;i++){
//			Debug.Log(i);
//			Debug.Log(coms[i]);
			buttonSet(i,coms[i]);
		}
		indent = 0;//インデント初期化
	}

	void buttonSet(int num, string com){
		string comName="None";
		if(com.Contains("for")){// .Contains() -> http://www.atmarkit.co.jp/fdotnet/dotnettips/411contains/contains.html
			if (com == "end_for") {
				comName = "End_For";
				indent -= 40;
			} else {
				itr_num_str=com.Split(',')[1];
				comName="For"+" ["+itr_num_str+"]";

			}
		}else if(com=="left"){
			comName="Left";
		}else if(com=="right"){
			comName="Right";
		}else if(com=="up"){
			comName="Up";
		}else if(com=="down"){
			comName="Down";
		}else if(com=="walk"){
			comName="Walk";
		}else if(com=="if"){
			comName="If";
		}else if(com=="end_if"){
			comName="End_If";
			indent -= 40;
		}
		int vertical_pos = num * button_height;
//		bool buttonClick = false;
		//http://docs.unity3d.com/jp/current/ScriptReference/Rect.html
		if(GUI.Button (new Rect (20 + indent, vertical_pos, button_width, button_height), comName,buttonGuiSkin.GetStyle ("button"))){
			//ボタンが押された時の挙動
			if(com.Contains("for")){// .Contains() -> http://www.atmarkit.co.jp/fdotnet/dotnettips/411contains/contains.html
				int itr_num=0;
				if (com == "end_for") {
				} else {
					itr_num=int.Parse(com.Split(',')[1]);
					itr_num+=1;
					coms[num]="for,"+itr_num;
					DataManager.Instance.gameCodes = coms;
				}
			}
		};

		//indent
		if (com.Contains ("for")) {
			if (com == "end_for") {
			} else {
				indent += 40;
			}
		}
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
}