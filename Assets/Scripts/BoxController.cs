using UnityEngine;
using System.Collections;

public class BoxController : MonoBehaviour {
	public Vector3 velocity;
	public float dieZ;

	Vector3 initialPosition;
	float startTime;

	void Start() {
		startTime = Time.time;
		initialPosition = transform.position;
	}

	void Update () {
		transform.position = initialPosition + velocity * (Time.time - startTime);
		if (transform.position.z < -dieZ) {
			Destroy(gameObject);
		}
		else if (transform.position.z < 0.0f) {
			Material m = GetComponent<Renderer>().material;
			m.color = new Color(m.color.r, m.color.g, m.color.b, 1.0f - transform.position.z / -dieZ);
		}
	}
}
