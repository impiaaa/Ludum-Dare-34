using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Scoring : MonoBehaviour {
	public Text scoreLabel, livesLabel, levelLabel;
	public GameObject gameOverLabel;
	public float levelFadeoutDuration;
	public int levelNumber;

	public int lives {
		get { return _lives; }
		set {
			_lives = value;
			if (value >= 0) {
				livesLabel.text = string.Format(livesFormat, value);
			}
			else {
				GameOver ();
			}
		}
	}

	int _lives;
	string scoreFormat, livesFormat, levelFormat;

	void Start () {
		scoreFormat = scoreLabel.text;
		livesFormat = livesLabel.text;
		levelFormat = levelLabel.text;
		lives = 3;
	}
	
	void Update () {
		if (lives >= 0) {
			scoreLabel.text = string.Format (scoreFormat, System.TimeSpan.FromSeconds(Time.timeSinceLevelLoad));
		}
	}

	void GameOver() {
		if (Time.timeSinceLevelLoad > PlayerPrefs.GetFloat("HighScore")) {
			PlayerPrefs.SetFloat("HighScore", Time.timeSinceLevelLoad);
		}
		scoreLabel.text = string.Format (scoreFormat, System.TimeSpan.FromSeconds(Time.timeSinceLevelLoad));
		gameOverLabel.SetActive(true);
	}

	public void LevelSplash() {
		levelLabel.text = string.Format(levelFormat, levelNumber);
		levelLabel.canvasRenderer.SetAlpha( 1.0f );
		levelLabel.CrossFadeAlpha(0.0f, levelFadeoutDuration, false);
	}
}
