using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class HighScore : MonoBehaviour {

	GameManager gameManagerScript;
	public Text  name, scores,time;
	string s;
	int length = 10;

	// Use this for initialization
	void Start () {
		gameManagerScript = GameObject.Find ("GameManager(Clone)").GetComponent <GameManager>();
		name.text = "";
		scores.text = "";
		time.text = "";

	
	#region old unused code, just keeping for future reference
		/*for (int i = 0; i < length; i++) {
			scores.text += gameManagerScript.GetHighScoreBoard ().HighScoreList [i].Name.ToString () + "\t\t\t" +
			gameManagerScript.GetHighScoreBoard ().HighScoreList [i].Score.ToString () + "\t\t\t" +
			gameManagerScript.GetHighScoreBoard ().HighScoreList [i].DateTime.ToString () + "\n";
		}*/		



		/*for (int i = 0; i < length; i++) {
			s = string.Format ("{0,-10} {1,-17} {2,-22}\n", gameManagerScript.GetHighScoreBoard ().HighScoreList [i].Name,
				gameManagerScript.GetHighScoreBoard ().HighScoreList [i].Score.ToString(),
				gameManagerScript.GetHighScoreBoard ().HighScoreList [i].DateTime.ToString ());

			scores.text += s;
			 
		}*/
	#endregion

		for (int i = 0; i < length; i++) {
			name.text += gameManagerScript.GetHighScoreBoard ().HighScoreList [i].Name.ToString () +"\n";
			scores.text += gameManagerScript.GetHighScoreBoard ().HighScoreList [i].Score.ToString ()+"\n";
			time.text += gameManagerScript.GetHighScoreBoard ().HighScoreList [i].DateTime.ToString ()+"\n";

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
