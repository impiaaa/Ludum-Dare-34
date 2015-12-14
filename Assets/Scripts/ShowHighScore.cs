using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowHighScore : MonoBehaviour {
	void Start () {
		Text t = GetComponent<Text>();
		t.text = string.Format(t.text, System.TimeSpan.FromSeconds(PlayerPrefs.GetFloat("HighScore")));
	}
}
