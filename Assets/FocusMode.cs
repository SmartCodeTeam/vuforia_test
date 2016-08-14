using UnityEngine;
using System.Collections;
using Vuforia;

//http://blog.livedoor.jp/yuukiwwww/archives/46266528.html からコピー

public class FocusMode : MonoBehaviour {

	//private bool mVuforiaStarted = false;//※使用していないと変数と警告がでたのでとりあえず使わない
	// Use this for initialization
	void Start () {
		VuforiaBehaviour qcar = (VuforiaBehaviour)FindObjectOfType( typeof(VuforiaBehaviour) );
		if (qcar) {
			qcar.RegisterVuforiaStartedCallback( OnQCARStarted );
		} else {
			Debug.Log ("Failed to find QCARBehaviour in current scene");
		}
	}

	private void OnQCARStarted() {
		Debug.Log ("Vuforia has started.");
		//mVuforiaStarted = true;//※使用していないと変数と警告がでたのでとりあえず使わない
		// Enable focus mode:
		bool autofocusOK = CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
		if (autofocusOK) {
			Debug.Log ("Successfully enabled Continuous Autofocus mode");
		} else {
			// set a different focus mode (for example, FOCUS_NORMAL):
			CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_NORMAL);
			// Other possible options:
			// CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_MACRO);
			// CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_INFINITY);
		}
	}
}