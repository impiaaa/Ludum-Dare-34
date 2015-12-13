using UnityEngine;
using System.Collections;

public class BoxController : MonoBehaviour {
	public float dieDuration;

	protected Vector3 velocity;
	protected new AudioSource audio;
	protected float dieStartTime = -1.0f;
	protected BoxSpawner parentSpawner;

	public virtual void Start() {
		audio = GetComponent<AudioSource>();
		parentSpawner = GetComponentInParent<BoxSpawner>();
	}

	void Update () {
		transform.position += (velocity + parentSpawner.globalVelocity) * Time.deltaTime;
		if (dieStartTime >= 0.0f) {
			// in progress of death
			float deathDelta = Time.time - dieStartTime; // band name idea
			if (deathDelta > dieDuration) {
				if (!audio.isPlaying) {
					Destroy(gameObject);
				}
			}
			else {
				foreach (Material m in GetComponent<Renderer>().materials) {
					m.color = new Color(m.color.r, m.color.g, m.color.b, 1.0f - deathDelta / dieDuration);
				}
			}
		}
		else if (transform.position.z < 0.0f) {
			dieStartTime = Time.time;
		}
	}

	public virtual void OnTriggerEnter(Collider other) {
		if (dieStartTime < 0.0f && other.tag == "Player") {
			if (other.GetComponentInChildren<Controls>().Hit()) {
				dieStartTime = Time.time;
				audio.pitch = Random.Range(0.9f, 1.1f);
				audio.Play();
			}
		}
	}

	public void SetVelocity(Vector3 v) {
		velocity = v;
	}
}
