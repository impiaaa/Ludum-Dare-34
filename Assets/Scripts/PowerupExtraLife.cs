using UnityEngine;
using System.Collections;

public class PowerupExtraLife : BoxController
{
	Scoring scoreboard;

	public override void Start() {
		base.Start();
		scoreboard = GameObject.FindWithTag("GameController").GetComponent<Scoring>();
	}

	public override void OnTriggerEnter(Collider other) {
		if (dieStartTime < 0.0f && other.tag == "Player") {
			scoreboard.lives++;
			dieStartTime = Time.timeSinceLevelLoad;
			audio.Play();
		}
	}
}
