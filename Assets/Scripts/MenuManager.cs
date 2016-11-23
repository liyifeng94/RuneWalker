using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {


	public void OnPlay () {
		Application.LoadLevel ("TestLevel");
	}

	public void OnHighScore () {
		Application.LoadLevel ("HighScore");
	}

	public void OnQuit () {
		Application.Quit ();

		#if UNITY_EDITOR

		UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}
}
