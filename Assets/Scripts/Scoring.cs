using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Scoring : MonoBehaviour {
	public Text scoreLabel, livesLabel;

	public int lives {
		get { return _lives; }
		set {
			_lives = value;
			if (value >= 0) {
				livesLabel.text = string.Format(livesFormat, value);
			}
		}
	}

	int _lives;
	string scoreFormat, livesFormat;

	// Use this for initialization
	void Start () {
		scoreFormat = scoreLabel.text;
		livesFormat = livesLabel.text;
		lives = 3;
	}
	
	// Update is called once per frame
	void Update () {
		if (lives >= 0) {
			scoreLabel.text = string.Format (scoreFormat, System.TimeSpan.FromSeconds(Time.time));
		}
	}
}
