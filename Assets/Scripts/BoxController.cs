using UnityEngine;
using System.Collections;

public class BoxController : MonoBehaviour {
	public Vector3 velocity;
	public float dieDuration;

	Vector3 initialPosition;
	float spawnTime;
	new AudioSource audio;
	float dieStartTime = -1.0f;

	void Start() {
		spawnTime = Time.time;
		initialPosition = transform.position;
		audio = GetComponent<AudioSource>();
	}

	void Update () {
		transform.position = initialPosition + velocity * (Time.time - spawnTime);
		if (dieStartTime >= 0.0f) {
			// in progress of death
			float deathDelta = Time.time - dieStartTime; // band name idea
			if (deathDelta > dieDuration) {
				if (!audio.isPlaying)
					Destroy(gameObject);
			}
			else {
				Material m = GetComponent<Renderer>().material;
				m.color = new Color(m.color.r, m.color.g, m.color.b, 1.0f - deathDelta / dieDuration);
			}
		}
		else if (transform.position.z < 0.0f) {
			dieStartTime = Time.time;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (dieStartTime < 0.0f && other.tag == "Player") {
			dieStartTime = Time.time;
			audio.Play();
		}
	}
}
