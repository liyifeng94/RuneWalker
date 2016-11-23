using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {


	public void OnPlay () {
		Application.LoadLevel ("TestLevel");
	}

	/*public void OnOptions () {
		Application.LoadLevel ("Option");
	}*/

	public void OnQuit () {
		Application.Quit ();
	}
}
