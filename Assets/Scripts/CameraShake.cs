using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
	public float shakeDuration;
	public float shakeFrequency;
	public float shakeMagnitude;

	Vector3 initialPosition;
	float lastShakeStartTime;

	void Start () {
		initialPosition = transform.position;
		lastShakeStartTime = Time.time-shakeDuration;
	}
	
	public void Update () {
		if (Time.time < lastShakeStartTime+shakeDuration) {
			float dt = Time.time - lastShakeStartTime;
			transform.position = new Vector3(Mathf.Sin(dt*shakeFrequency*2*Mathf.PI),
			                                 Mathf.Sin(dt*shakeFrequency*8*Mathf.PI),
			                                 0.0f)*shakeMagnitude*(1.0f-dt/shakeDuration)+initialPosition;
		}
	}

	public void ScreenShake() {
		lastShakeStartTime = Time.time;
	}
}
