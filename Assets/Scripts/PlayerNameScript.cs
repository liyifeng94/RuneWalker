using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerNameScript : MonoBehaviour {
	public InputField name;
	// Use this for initialization
	public void SetPlayerName(){
		GameManager.Instance.SetPlayerNameAndHighScore (name.text);
	}
}
