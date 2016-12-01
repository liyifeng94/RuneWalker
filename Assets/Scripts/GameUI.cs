using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameUI : MonoBehaviour {

	public Text score,level, scoreMultiplier, special;
	public Slider specialBar;

	// Use this for initialization
	void Start () {
		special.text = "Special: ";
	}

	
	// Update is called once per frame
	void Update () {
		score.text = "Score: " + GameManager.Instance.GetScore().ToString();
		level.text = "Level: " + GameManager.Instance.GetCurrentLevel();
		scoreMultiplier.text = "Multiplier: x" + GameManager.Instance.GetScoreMultiplier();

		int specialCurrent = GameManager.Instance.GetSpecialMeter();
		int specialMax = GameManager.Instance.GetSpecialMeterMax();
		specialCurrent = Math.Min(specialCurrent, specialMax);
		int specialMeter = (int)(((double)specialCurrent/(double)specialMax) * 100);
		//special.text = "Special: " + specialMeter.ToString() + "%";
		specialBar.value = specialMeter;
	}
}
