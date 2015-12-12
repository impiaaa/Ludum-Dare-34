﻿using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {
	public Vector2 sensitivity;
	public Transform[] walls;
	public Vector3 bounceVelocity;
	public Vector3 movementVelocity;

	// ball size included
	public Vector3 arenaMin;
	public Vector3 arenaMax;

	float lastBounce;
	Vector3 initialPosition;
	BoxSpawner boxSpawner;
	new AudioSource audio;

	void Start() {
		SetupWallCollisions();
		lastBounce = Time.time;
		initialPosition = transform.position;
		boxSpawner = GameObject.FindWithTag("GameController").GetComponentInChildren<BoxSpawner>();
		audio = GetComponent<AudioSource>();
	}

	void SetupWallCollisions() {
		bool first = true;
		foreach (Transform wall in walls) {
			Vector3 wmin = wall.GetComponent<Collider>().bounds.min;
			Vector3 wmax = wall.GetComponent<Collider>().bounds.max;

			if (first) {
				arenaMin = wmin;
				arenaMax = wmax;
			}
			else {
				arenaMin = Vector3.Min(arenaMin, wmin);
				arenaMax = Vector3.Max(arenaMax, wmax);
			}

			first = false;
		}
		arenaMax -= GetComponent<Collider>().bounds.extents;
		arenaMin += GetComponent<Collider>().bounds.extents;
	}
	
	void Update() {
		transform.position += new Vector3(Input.GetAxis("Horizontal")*sensitivity.x, 0, 0);
		CheckWallCollisions();
		Bounce();

		// check for speedup
		boxSpawner.globalVelocity = movementVelocity;
		boxSpawner.globalVelocity += new Vector3(0, 0, Input.GetAxis("Vertical")*sensitivity.y);
		if (Input.GetAxis ("Left") > 0.0f && Input.GetAxis("Right") > 0.0f) {
			boxSpawner.globalVelocity += new Vector3(0, 0, (Input.GetAxis ("Left")+Input.GetAxis("Right"))*sensitivity.y*0.5f);
		}
		boxSpawner.globalVelocity *= -1.0f;
	}

	void CheckWallCollisions() {
		if (transform.position.x > arenaMax.x) {
			transform.position = new Vector3(arenaMax.x, transform.position.y, transform.position.z);
		}
		if (transform.position.x < arenaMin.x) {
			transform.position = new Vector3(arenaMin.x, transform.position.y, transform.position.z);
		}
		if (transform.position.y > arenaMax.y) {
			transform.position = new Vector3(transform.position.x, arenaMax.y, transform.position.z);
		}
		if (transform.position.y < arenaMin.y) {
			transform.position = new Vector3(transform.position.x, arenaMin.y, transform.position.z);
			lastBounce = Time.time;
			audio.Play();
		}
		if (transform.position.z > arenaMax.z) {
			transform.position = new Vector3(transform.position.x, transform.position.y, arenaMax.z);
		}
		if (transform.position.z < arenaMin.z) {
			transform.position = new Vector3(transform.position.x, transform.position.y, arenaMin.z);
		}
	}

	void Bounce() {
		float dt = Time.time - lastBounce;
		transform.position = bounceVelocity*dt + 0.5f*Physics.gravity*dt*dt + new Vector3(transform.position.x, initialPosition.y, transform.position.z);
	}
}
