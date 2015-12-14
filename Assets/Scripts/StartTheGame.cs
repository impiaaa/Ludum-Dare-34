using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartTheGame : MonoBehaviour {
	AsyncOperation loadasync;
	new AudioSource audio;

	void Start() {
		audio = GetComponent<AudioSource>();
	}

	public void DoClick() {
		if (loadasync == null) {
			audio.Play();
			GetComponentInChildren<Text>().text = "Loading…";
			GetComponent<Button>().enabled = false;
			loadasync = Application.LoadLevelAsync("Main");
			loadasync.allowSceneActivation = false;
		}
	}

	public void Update() {
		if (loadasync != null) {
			// started loading
			if (!audio.isPlaying) {
				loadasync.allowSceneActivation = true;
			}
		}
	}
}
